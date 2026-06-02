namespace SecretChat.Mobile.ViewModels
{
	public partial class ContactsViewModel(IConnectivity connectivity, IUserApi userApi, RealtimeUpdateService realtimeUpdateService, AuthService authService) : BaseViewModel(connectivity, authService)
	{
		[ObservableProperty]
		private bool _isSearchMode = false;

		public ObservableCollection<CollectionViewItemLoggedInUser> SortUsers { get; set; } = [];
		public ObservableCollection<CollectionViewGroupLoggedInUser> Contacts { get; set; } = [];
		private ObservableCollection<LoggedInUserDto> _allUsers { get; set; } = [];
		private ObservableCollection<LoggedInUserDto> _userContacts { get; set; } = [];

		public override async Task InitializeViewModel()
		{
			await MakeFirstApiRequestAsync(async () =>
			{
				var response = await userApi.GetUserContactsAsync();
				var response2 = await userApi.GetAllUsersAsync();

				if (!response.IsSuccess)
				{
					await ShowErrorAlertAsync(response.Message);
					return;
				}

				if (!response2.IsSuccess)
				{
					await ShowErrorAlertAsync(response2.Message);
					return;
				}

				_allUsers.AddRange(response2.Data);
				var currentUser = _allUsers.FirstOrDefault(n => n.Id == AuthService.User!.Id);
				if (currentUser is { })
				{
					_allUsers.Remove(currentUser);
				}
				_userContacts.AddRange(response.Data);

				RefreshContacts(response.Data);
			});
		}

		[RelayCommand]
		private async Task GoToDetailsPageAsync(LoggedInUserDto dto)
		{
			var isUserContact = IsItInTheContacts(dto);
			var homeViewModel = ServiceHelper.GetService<HomeViewModel>();
			var isHaveChat = homeViewModel!.IsItInTheChat(dto);

			await NavigateAsync(nameof(DetailsPage));

			WeakReferenceMessenger.Default.Send(new FromContactToDetailsMessage
			{
				User = dto,
				IsUserContact = isUserContact,
				IsHaveChat = isHaveChat,
			});
		}

		public bool IsItInTheContacts(LoggedInUserDto dto)
		{
			foreach(var contacts in Contacts)
			{
				if (contacts.Any(n => n.Id == dto.Id))
				{
					return true;
				}
			}

			return false;
		}

		#region CreateGroupMethods

		private static readonly Dictionary<string, int> GroupOrder = new()
		{
			["en"] = 0,
			["ru"] = 1,
			["digit"] = 2,
			["symbol"] = 3,
			["!"] = 4
		};

		private static (string GroupKey, string GroupTitle) GetGroupKey(LoggedInUserDto user)
		{
			if (string.IsNullOrWhiteSpace(user.Name))
				return ("!", "#");

			var firstChar = user.Name[0];
			var lower = char.ToLowerInvariant(firstChar);

			if (char.IsLetter(firstChar))
			{
				if (lower is >= 'a' and <= 'z')
					return ("en", "A–Z");
				if (lower is >= 'а' and <= 'я')
					return ("ru", "А–Я");
			}

			if (char.IsDigit(firstChar))
				return ("digit", "0–9");

			return ("symbol", "Symbols");
		}

		public void RefreshContacts(IEnumerable<LoggedInUserDto> allUsersFromApi)
		{
			// 1. сначала сортируем весь список имён (по Name, без учёта регистра)
			var sortedUsers = allUsersFromApi
				.OrderBy(user => user.Name ?? string.Empty, StringComparer.OrdinalIgnoreCase)
				.ToList();

			// 2. группируем по нашим правилам (en, ru, digit, symbol, !)
			var grouped = sortedUsers
				.GroupBy(GetGroupKey)
				.OrderBy(g => GroupOrder[g.Key.GroupKey])
				.ToList();

			// 3. формируем ObservableCollection<CollectionViewGroupLoggedInUser>
			var result = new List<CollectionViewGroupLoggedInUser>();

			int globalIndex = 0;
			int totalItems = sortedUsers.Count;

			foreach (var group in grouped)
			{
				var groupList = group.ToList();

				// если группа пустая → не включаем (в данном случае не случится, но логика та же)
				if (!groupList.Any())
					continue;

				// порядок внутри группы тоже по Name (уже отсортирован в sortedUsers)
				var groupSorted = groupList
					.OrderBy(user => user.Name ?? string.Empty, StringComparer.OrdinalIgnoreCase)
					.ToList();

				var groupHeaderTitle = group.Key.GroupTitle;

				var groupViewModel = new CollectionViewGroupLoggedInUser(
					groupTitle: groupHeaderTitle,
					index: globalIndex,                    // первый элемент группы
					count: totalItems,
					items: groupSorted
				);

				result.Add(groupViewModel);

				globalIndex += groupSorted.Count;
			}

			// 4. обновляем привязку в ObservableCollection
			Contacts.Clear();
			Contacts.AddRange(result);
		}

		#endregion

		#region SortingMethods

		private static string CleanPhoneNumber(string? phone)
		{
			if (string.IsNullOrWhiteSpace(phone))
				return "";

			return new string(phone
				.Where(c => c is >= '0' and <= '9')
				.ToArray());
		}

		private bool IsMatch(LoggedInUserDto user, string? query)
		{
			if (string.IsNullOrWhiteSpace(query))
				return true;

			query = query.Trim().ToLowerInvariant();

			var firstCharFromName = string.IsNullOrEmpty(user.Name)
				? ""
				: user.Name[0].ToString().ToLowerInvariant();

			var idStr = user.Id.ToString();
			var cleanPhone = CleanPhoneNumber(user.TelephoneNumber);

			return
				firstCharFromName.Contains(query) ||
				idStr.Contains(query) ||
				cleanPhone.Contains(query);
		}

		public void UpdateSortUsers(string? searchQuery = null)
		{
			var filtered = _allUsers
				.Where(user => IsMatch(user, searchQuery))
				.OrderBy(user => user.Name ?? string.Empty, StringComparer.OrdinalIgnoreCase)
				.ToList();

			SortUsers.Clear();

			int total = filtered.Count;

			for (int i = 0; i < filtered.Count; i++)
			{
				SortUsers.Add(new CollectionViewItemLoggedInUser
				{
					Item = filtered[i],
					ItemIndex = i,
					ItemCount = total
				});
			}
		} 

		#endregion

		public void ConfigureRealtimeUpdates()
		{
			realtimeUpdateService.AddUserCreatedOrUpdatedHandler(nameof(ContactsViewModel), OnUserCreatedOrUpdated);
			realtimeUpdateService.AddUserDeletedHandler(nameof(ContactsViewModel), OnUserDeleted);
			realtimeUpdateService.AddContactAddedHandler(nameof(ContactsViewModel), OnContactAdded);
			realtimeUpdateService.AddContactDeletedHandler(nameof(ContactsViewModel), OnContactDeleted);
		}

		private void OnContactDeleted(int id)
		{
			var contact = _userContacts.FirstOrDefault(n => n.Id == id);
			if (contact is null)
				return;

			_userContacts.Remove(contact);

			RefreshContacts(_userContacts);
		}

		private void OnContactAdded(int id)
		{
			var user = _allUsers.FirstOrDefault(n => n.Id == id);
			if (user is null)
				return;

			_userContacts.Add(user);

			RefreshContacts(_userContacts);
		}

		private void OnUserDeleted(int id)
		{
			var user = _allUsers.FirstOrDefault(n => n.Id == id);
			if (user is null)
				return;

			_allUsers.Remove(user);

			var userContact = _userContacts.FirstOrDefault(n => n.Id == id);
			if (userContact is not null)
			{
				_userContacts.Remove(userContact);
				RefreshContacts(_userContacts);
			}
		}

		private void OnUserCreatedOrUpdated(LoggedInUserDto dto)
		{
			var user = _allUsers.FirstOrDefault(n => n.Id == dto.Id);
			if (user is not null)
			{
				_allUsers.Remove(user);
			}

			_allUsers.Add(dto);

			var userContact = _userContacts.FirstOrDefault(n => n.Id == dto.Id);
			if (userContact is not null)
			{
				_userContacts.Remove(userContact);
				_userContacts.Add(dto);
				RefreshContacts(_userContacts);
			}
		}
	}
}
