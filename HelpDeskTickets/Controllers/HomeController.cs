using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HelpDeskTickets.ApplicationLogic.Entities.Users;
using HelpDeskTickets.ApplicationLogic.Interfaces;
using HelpDeskTickets.Helpers;
using HelpDeskTickets.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HelpDeskTickets.Controllers
{
	[AiExceptionLogger]
	public class HomeController : Controller
	{
		private readonly IUserService _userService;

		public HomeController(IUserService userService)
		{
			_userService = userService;
		}

		[Authorize]
		[AllowAnonymous]
		public async Task<IActionResult> Index()
		{
			var userId = User.GetUserId();
			if (userId == null)
			{
				return Redirect("/Account/Login");
			}

			var userRoles = await _userService.GetUserRolesAsync(userId);
			if (userRoles.Contains(UserRole.Technician) || userRoles.Contains(UserRole.Admin))
			{
				return Redirect("/Tickets/GetLoggedTickets");
			}
			else if (userRoles.Contains(UserRole.Customer))
			{
				return Redirect("/Tickets/MyOustandingTickets");
			}

			return Redirect("/Account/Login");
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}