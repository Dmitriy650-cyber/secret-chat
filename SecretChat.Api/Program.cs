var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(config => { }, typeof(Program).Assembly);
builder.Services.AddDbContext<DataContext>(options => options
	.UseSqlServer(builder.Configuration.GetConnectionString("Default"), sqlOptions => sqlOptions
		.EnableRetryOnFailure()));
builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer("Bearer", options =>
	{
		var key = Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:Key")!);
		var symmetricKey = new SymmetricSecurityKey(key);
		var issuer = builder.Configuration.GetValue<string>("Jwt:Issuer");
		var audience = builder.Configuration.GetValue<string>("Jwt:Audience");

		options.SaveToken = true;
		options.RequireHttpsMetadata = true;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateIssuerSigningKey = true,
			ValidateLifetime = false,
			ValidIssuer = issuer,
			ValidAudience = audience,
			IssuerSigningKey = symmetricKey
		};
	});
builder.Services.AddAuthorization();
builder.Services.AddValidation();
builder.Services.AddSignalR();

AddApiServices(builder.Services);

var app = builder.Build();

#if DEBUG
AutoMigration(app.Services);
#endif

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseMiddleware<GlobalExceptionCatcher>();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication()
   .UseAuthorization();
app.MapAuthEndpoints()
   .MapChatEndpoints()
   .MapMessageEndpoints()
   .MapPhotoEndpoints()
   .MapUserEndpoints();
app.MapHub<SecretChatHub>(AppConstants.HubPattern);
app.Run();

static void AddApiServices(IServiceCollection services)
{
	services
		.AddScoped<FileUploadingService>()
		.AddScoped<JwtTokenService>()
		.AddScoped<ValidationService>();

	services
		.AddScoped<IAuthService, AuthService>()
		.AddScoped<IChatService, ChatService>()
		.AddScoped<IMessageService, MessageService>()
		.AddScoped<IPhotoService, PhotoService>()
		.AddScoped<IUserService, UserService>();
}

static void AutoMigration(IServiceProvider sp)
{
	var scope = sp.CreateScope();
	var context = scope.ServiceProvider.GetRequiredService<DataContext>();
	if (context.Database.GetPendingMigrations().Any())
	{
		context.Database.Migrate();
	}
}
