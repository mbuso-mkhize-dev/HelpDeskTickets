using HelpDeskTickets.EntityFramework.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HelpDeskTickets.EntityFramework.Core.Models
{
	public class Ticket : ITimestamp
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public int TicketNumber { get; set; }

		[Required]
		public string Title { get; set; }

		public string Description { get; set; }

		[Required]
		public IssueSeverity IssueSeverity { get; set; }

		[Required]
		public IssueType IssueType { get; set; }

		[Required]
		public ResolutionStatus ResolutionStatus { get; set; }

		public string ResolutionSteps { get; set; }

		public string Comments { get; set; }

		public User Customer { get; set; }
		
		public string TechnicianId { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset UpdatedAt { get; set; }
	}
}
