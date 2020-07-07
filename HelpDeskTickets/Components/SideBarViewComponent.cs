using Microsoft.AspNetCore.Mvc;
using HelpDeskTickets.ApplicationLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpDeskTickets.Helpers;
using HelpDeskTickets.ApplicationLogic.Entities.Users;

namespace HelpDeskTickets.Components
{
	public class SideBarViewComponent : BaseViewComponent
	{
		private readonly IUserService _userService;

		public SideBarViewComponent(IUserService userService)
		{
			_userService = userService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var userId = HttpContext.User.GetUserId();
			var userEntity = new UserEntity();
			if (userId == null)
			{
				return View(userEntity);
			}
			var model = await _userService.GetUserRolesAsync(userId);
			if (model.Contains(UserRole.Admin))
			{
				userEntity.UserRole = UserRole.Admin;
			}

			if (model.Contains(UserRole.Customer))
			{
				userEntity.UserRole = UserRole.Customer;
			}

			if (model.Contains(UserRole.Technician))
			{
				userEntity.UserRole = UserRole.Technician;
			}
			return View(userEntity);
		}
	}
}
