using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.ApplicationLogic.Entities.Tickets
{
	public class IssueSeverityEntity
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Color { get; set; }
	}
}
