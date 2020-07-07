namespace HelpDeskTickets.ApplicationLogic.Entities.Users
{
	public class UserCreateEntity
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Address { get; set; }

		public string Email { get; set; }

		public string Username { get; set; }

		public UserRole UserRole { get; set; } // I know theres better way. Low on time
		
		public string Password { get; set; }
	}

	public class UserUpdateEntity : UserCreateEntity
	{
		public string Id { get; set; }
	}

	public enum UserRole
	{
		Admin, Customer, Technician
	}
}