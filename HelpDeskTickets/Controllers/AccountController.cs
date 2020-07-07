using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HelpDeskTickets.ApplicationLogic.Entities.Users;
using HelpDeskTickets.ApplicationLogic.Interfaces;
using HelpDeskTickets.EntityFramework.Core.Models;
using HelpDeskTickets.Helpers;
using HelpDeskTickets.Models.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDeskTickets.Controllers
{
	[AiExceptionLogger]
	public class AccountController : Controller
	{
		private readonly SignInManager<User> _signInManager;
		private readonly IEmailSender _emailSender;
		private readonly IUserService _userService;

		public AccountController(
		   SignInManager<User> signInManager,
		   IEmailSender emailSender,
		   IUserService userService)
		{
			_signInManager = signInManager;
			_emailSender = emailSender;
			_userService = userService;
		}

		/// <summary>
		/// Login page
		/// </summary>
		/// <param name="returnUrl"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Login(string returnUrl = null)
		{
			// Clear the existing external cookie to ensure a clean login process
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			return View();
		}

		/// <summary>
		/// Login page info
		/// </summary>
		/// <param name="model"></param>
		/// <param name="returnUrl"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
		{
			if (ModelState.IsValid)
			{
				//need to activate email first
				bool isEmailActivated = _userService.IsUserActivated(model.Email);
				if (!isEmailActivated)
				{
					ModelState.AddModelError("", "You need to confirm your email.");
					ViewData["error"] = "You need to register first or You need to confirm your email.";
					return View(model);
				}

				var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
				if (result.Succeeded)
				{
					var user = _userService.GetUserByEmail(model.Email);
					
					var userRoles =await  _userService.GetUserRolesAsync(user.Id);
					if (userRoles.Contains(UserRole.Technician) || userRoles.Contains(UserRole.Admin))
					{
						return Redirect("/Tickets/GetLoggedTickets");
						
					}
					else if (userRoles.Contains(UserRole.Customer))
					{
						return Redirect("/Tickets/MyOustandingTickets");
					}
				}
			}

			return View(model);
		}

		/// <summary>
		/// Lockout View
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		public IActionResult Lockout()
		{
			return View();
		}

		/// <summary>
		/// Success View
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		public IActionResult RegisterSuccess()
		{
			return View();
		}

		/// <summary>
		/// Register page
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		public IActionResult Register()
		{
			return View();
		}

		/// <summary>
		/// Regist page info
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				// Get callback url template
				var baseCallbackUrl = Url.EmailConfirmationLink(Request.Scheme);
				await _userService.AddUser(new UserCreateEntity
				{
					FirstName = model.FirstName,
					LastName = model.LastName,
					Address = model.Address,
					Email = model.Email,
					UserRole = UserRole.Customer,
					Password = model.Password,
					Username = model.Email
				}, baseCallbackUrl);

				return RedirectToAction("login");
			}

			return View(model);
		}

		/// <summary>
		/// User Role Management page
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UserRoles()
		{
			ViewBag.Users = (await _userService.GetAllUsersAsync()).Select(x =>
						new SelectListItem
						{
							Value = x.Id.ToString(),
							Text = x.Email
						}).ToList(); ;
			ViewBag.Roles = Enum.GetValues(typeof(UserRole)).Cast<UserRole>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
			return View();
		}

		/// <summary>
		/// UserRole Management page info
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UserRoles(UserUpdateViewModel model)
		{
			if (ModelState.IsValid)
			{
				await _userService.AssignUserRole(model.UserId, model.UserRole);
			}

			return await UserRoles();
		}

		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("login");
		}

		/// <summary>
		/// Confirm email view
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> ConfirmEmail(string userId, string code)
		{
			if (userId == null || code == null)
			{
				return RedirectToAction("login");
			}

			await _userService.ConfirmEmail(userId, code);

			return RedirectToAction("login");
		}

		/// <summary>
		/// Forgot password page
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		public IActionResult ForgotPassword()
		{
			return View();
		}

		/// <summary>
		/// Access Denied view
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult AccessDenied()
		{
			return View();
		}

		#region Helpers

		private IActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return Redirect("/");
			}
		}

		#endregion Helpers
	}
}