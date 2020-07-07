using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.EntityFramework.Core.Interfaces
{
	public interface ITimestamp
	{
		DateTimeOffset CreatedAt { get; set; }

		DateTimeOffset UpdatedAt { get; set; }
	}
}
