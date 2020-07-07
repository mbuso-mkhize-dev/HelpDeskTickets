using HelpDeskTickets.EntityFramework.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HelpDeskTickets.EntityFramework.Core.Models
{
	public class IssueType
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public string Name { get; set; }

		public virtual ISet<Ticket> Tickets { get; set; }
	}
}
