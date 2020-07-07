using Microsoft.AspNetCore.Identity;
using HelpDeskTickets.ApplicationLogic.Entities.Users;
using HelpDeskTickets.ApplicationLogic.Entities.Users.Mappers;
using HelpDeskTickets.ApplicationLogic.Interfaces;
using HelpDeskTickets.EntityFramework.Core.Interfaces;
using HelpDeskTickets.EntityFramework.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HelpDeskTickets.ApplicationLogic.Services
{
    /// <summary>
    /// Userservice for user management
    /// </summary>
	public class UserService : IUserService
	{
		private readonly IBaseRepository<User> _userRepository;
		private readonly UserManager<User> _userManager;
		private readonly IEmailSender _emailSender;
		private readonly AspNetRoleManager<IdentityRole> _aspNetRoleManager;


		/// <summary>
		/// CTOR
		/// </summary>
		/// <param name="userRepository"></param>
		/// <param name="userManager"></param>
		/// <param name="emailSender"></param>
		public UserService(
			IBaseRepository<User> userRepository,
			UserManager<User> userManager,
			IEmailSender emailSender,
			AspNetRoleManager<IdentityRole> aspNetRoleManager)
		{
			_userRepository = userRepository;
			_userManager = userManager;
			_emailSender = emailSender;
			_aspNetRoleManager = aspNetRoleManager;
		}

        /// <summary>
        /// Get User by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		public async Task<UserEntity> GetUser(string id)
		{
			var model = await _userRepository.FindAsync(id);

			return UserEntityMapper.MapToEntity(model);
		}

        /// <summary>
        /// Get User by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
		public UserEntity GetUserByEmail(string email)
		{
			var model = _userRepository.Where(u => u.Email == email).FirstOrDefault();

			return UserEntityMapper.MapToEntity(model);
		}

        /// <summary>
        /// Confirma email 2FA
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
		public async Task ConfirmEmail(string userId, string code)
		{
			var model = await _userRepository.FindAsync(userId);
			if (model == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{userId}'.");
			}
			var result = await _userManager.ConfirmEmailAsync(model, code);
		}

        /// <summary>
        /// Check if User is Activated
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
		public bool IsUserActivated(string email)
		{
			var response = _userRepository.Where(u => u.Email == email && u.EmailConfirmed).Any();

			return response;
		}

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns></returns>
		public async Task<List<UserEntity>> GetAllUsersAsync()
		{
			var models = await _userRepository.AllAsync();

			return models.Select(model => UserEntityMapper.MapToEntity(model)).ToList();
		}

        /// <summary>
        /// Add User
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="baseCallbackUrl"></param>
        /// <returns></returns>
		public async Task AddUser(UserCreateEntity entity, string baseCallbackUrl)
		{
			var user = new User
			{
				FirstName = entity.FirstName,
				LastName = entity.LastName,
				Email = entity.Email,
				UserName = entity.Email,
				Address = entity.Address,
			};
			//Email not working yet (This skips the confirm part)
			user.EmailConfirmed = true;

			var result = await _userManager.CreateAsync(user, entity.Password);
			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, entity.UserRole.ToString());

				var confirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

				var callbackUrl = BuildCallbackUrlWithParameters(baseCallbackUrl, user.Id, confirmToken);

				//await _emailSender.SendEmailAsync(entity.Email, "Reset Password",
				   //$"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");				

			}

		}

        /// <summary>
        /// Update User details
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
		public async Task UpdateUser(UserUpdateEntity entity)
		{
			var model = await _userRepository.FindAsync(entity.Id);

			model.FirstName = entity.FirstName;
			model.LastName = entity.LastName;
			model.UserName = entity.Username ?? entity.Email;
			model.Address = entity.Address;

			_userRepository.Update(model);

			await _userRepository.SaveAsync();
		}

        /// <summary>
        /// Delete User by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		public async Task DeleteUser(string id)
		{
			var model = await _userRepository.FindAsync(id);

			if (model != null)
			{
				_userRepository.Delete(model);
				await _userRepository.SaveAsync();
			}
		}

		/// <summary>
		/// Get User Role
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public async Task<UserRole> GetUserRole(string userId)
		{
			var user = await _userRepository.FindAsync(userId);
			var roles = await _userManager.GetRolesAsync(user);
			Enum.TryParse(roles.FirstOrDefault(), out EntityFramework.Core.Enums.UserRole userRole);
			return UserEntityMapper.GetUserRole(userRole);
		}

		public async Task<List<UserRole>> GetUserRolesAsync(string userId)
		{
			var user = await _userRepository.FindAsync(userId);
			var roles = await _userManager.GetRolesAsync(user);
			var userRoles = new List<UserRole>();
			foreach(var role in roles)
			{
				Enum.TryParse(role, out UserRole userRole);
				userRoles.Add(userRole);
			}
			return userRoles;
		}

		public async Task AssignUserRole(string userId, string roleNum)
		{
			var user = await _userRepository.FindAsync(userId);
			var roleEnum = (UserRole)Convert.ToInt16(roleNum);

			// For cases of singular role
			//await _userManager.RemoveFromRoleAsync(user, UserRole.Admin.ToString());
			//await _userManager.RemoveFromRoleAsync(user, UserRole.Customer.ToString());
			//await _userManager.RemoveFromRoleAsync(user, UserRole.Technician.ToString());

			await _userManager.AddToRoleAsync(user, roleEnum.ToString().ToUpperInvariant());
		}

		// Append the Callback url
		private string BuildCallbackUrlWithParameters(string baseCallbackUrl, string userId, string code)
		{
			var uriBuilder = new UriBuilder(baseCallbackUrl);

			var query = HttpUtility.ParseQueryString(uriBuilder.Query);

			query["userId"] = userId;
			query["code"] = code;

			uriBuilder.Query = query.ToString();

			return uriBuilder.ToString();
		}

        /// <summary>
        /// Map to User role
        /// </summary>
        /// <param name="userRole"></param>
        /// <returns></returns>
		private static EntityFramework.Core.Enums.UserRole GetUserRole(UserRole userRole)
		{
			switch (userRole)
			{
				case UserRole.Admin:
					return EntityFramework.Core.Enums.UserRole.Admin;

				case UserRole.Customer:
					return EntityFramework.Core.Enums.UserRole.Customer;

				case UserRole.Technician:
					return EntityFramework.Core.Enums.UserRole.Technician;

				default:
					return EntityFramework.Core.Enums.UserRole.Customer;
			}
		}
	}
}