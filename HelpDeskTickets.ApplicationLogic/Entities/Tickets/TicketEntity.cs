using HelpDeskTickets.ApplicationLogic.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.ApplicationLogic.Entities.Tickets
{
	public class TicketEntity
	{
		public Guid Id { get; set; }
		
		public string Title { get; set; }

		public string Description { get; set; }

		public IssueSeverityEntity IssueSeverityEntity { get; set; }

		public IssueTypeEntity IssueTypeEntity { get; set; }

		public ResolutionStatusEntity ResolutionStatusEntity { get; set; }

		public string ResolutionSteps { get; set; }

		public string Comments { get; set; }

		public UserEntity Customer { get; set; }

		public string TechnicianId { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset UpdatedAt { get; set; }
	}
}
