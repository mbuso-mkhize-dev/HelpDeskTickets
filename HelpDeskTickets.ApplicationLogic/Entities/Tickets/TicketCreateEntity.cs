using System;

namespace HelpDeskTickets.ApplicationLogic.Entities.Tickets
{
	public class TicketCreateEntity
	{
		public string Title { get; set; }

		public string Description { get; set; }

		public Guid IssueSeverity { get; set; }

		public Guid IssueType { get; set; }

		public Guid ResolutionStatus { get; set; }

		public string Customer { get; set; }

	}

	public class TicketUpdateEntity : TicketCreateEntity
	{
		public Guid Id { get; set; }

		public string TechnicianId { get; set; }

		public string ResolutionSteps { get; set; }

		public string Comments { get; set; }
	}
}