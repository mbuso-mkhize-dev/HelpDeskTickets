using Microsoft.AspNetCore.Mvc;
using HelpDeskTickets.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDeskTickets.Helpers
{
	public static class UrlHelperExtensions
	{
		public static string EmailConfirmationLink(this IUrlHelper urlHelper, string scheme)
		{
			return urlHelper.Action(
				action: nameof(AccountController.ConfirmEmail),
				controller: "Account",
				values: null,
				protocol: scheme);
		}
		
	}
}
