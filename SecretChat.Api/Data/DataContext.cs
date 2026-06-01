namespace SecretChat.Api.Data
{
	public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
	{
		public DbSet<User> Users { get; set; }
		public DbSet<UserContact> Contacts { get; set; }
		public DbSet<Chat> Chats { get; set; }
		public DbSet<Message> Messages { get; set; }
		public DbSet<Photo> Photos { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder
				.Entity<UserContact>()
				.HasKey(n => new { n.UserId, n.ContactId });
			modelBuilder
				.Entity<UserContact>()
				.HasOne(n => n.User)
				.WithMany(n => n.Contacts)        
				.HasForeignKey(n => n.UserId)
				.OnDelete(DeleteBehavior.Restrict);
			modelBuilder
				.Entity<UserContact>()
				.HasOne(n => n.Contact)
				.WithMany()                       
				.HasForeignKey(n => n.ContactId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder
				.Entity<Chat>()
				.HasOne(n => n.FirstUser)
				.WithMany(n => n.Chats)
				.HasForeignKey(n => n.FirstUserId)
				.OnDelete(DeleteBehavior.Restrict);
			modelBuilder
				.Entity<Chat>()
				.HasOne(n => n.SecondUser)
				.WithMany()
				.HasForeignKey(n => n.SecondUserId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder
				.Entity<Message>()
				.Property(n => n.SendOn)
				.HasDefaultValueSql("GETUTCDATE()");
			modelBuilder
				.Entity<Message>()
				.HasOne(n => n.Chat)
				.WithMany(n => n.Messages)
				.HasForeignKey(n => n.ChatId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder
				.Entity<Chat>()
				.Property(n => n.CreatedAt)
				.HasDefaultValueSql("GETUTCDATE()");

			modelBuilder
				.Entity<User>()
				.HasAlternateKey(n => n.Email);
		}
	}
}
