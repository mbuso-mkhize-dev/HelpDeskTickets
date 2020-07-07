using Microsoft.AspNetCore.Identity;
using HelpDeskTickets.EntityFramework.Core.Enums;
using HelpDeskTickets.EntityFramework.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.EntityFramework.Core.Models
{
	public class User : IdentityUser, ITimestamp
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Address { get; set; }
				
		public DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset UpdatedAt { get; set; }

		public virtual ISet<Ticket> Tickets { get; set; }
	}
}
