using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.ApplicationLogic.Entities.Users
{
	public class UserEntity
	{

		public string Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Address { get; set; }

		public string Username { get; set; }

		public string Email { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset UpdatedAt { get; set; }

		public UserRole UserRole { get; set; }
	}
}
