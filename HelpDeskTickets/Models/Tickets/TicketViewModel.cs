using System;

namespace HelpDeskTickets.Models.Tickets
{
	public class TicketViewModel
	{
		public Guid Id { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public Guid IssueSeverity { get; set; }

		public Guid IssueType { get; set; }

		public Guid ResolutionStatus { get; set; }

		public string ResolutionSteps { get; set; }

		public string Comments { get; set; }

		public string Customer { get; set; }

		public string TechnicianId { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset UpdatedAt { get; set; }
	}

	public class MyTicketViewModel
	{
		public string Tickets { get; set; }
	}
}