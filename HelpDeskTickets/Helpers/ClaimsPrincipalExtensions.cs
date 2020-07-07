using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HelpDeskTickets.Helpers
{
	public static class ClaimsPrincipalExtensions
	{
		public static string GetUserId(this ClaimsPrincipal principal)
		{
			return principal?.FindFirstValue(ClaimTypes.NameIdentifier);
		}
	}
}
