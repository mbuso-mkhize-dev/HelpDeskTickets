using HelpDeskTickets.ApplicationLogic.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskTickets.ApplicationLogic.Interfaces
{
    /// <summary>
    /// Userservice interface
    /// </summary>
 	public interface IUserService
	{
        /// <summary>
        /// Get User By email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
		UserEntity GetUserByEmail(string email);

        /// <summary>
        /// Check if User is activated
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
		bool IsUserActivated(string email);

        /// <summary>
        /// Confirm email 2fa
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
		Task ConfirmEmail(string userId, string code);

        /// <summary>
        /// Update User details
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
		Task UpdateUser(UserUpdateEntity entity);

        /// <summary>
        /// Add User
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="callBackUrl"></param>
        /// <returns></returns>
		Task AddUser(UserCreateEntity entity, string callBackUrl);

        /// <summary>
        /// Get User by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		Task<UserEntity> GetUser(string id);

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
		Task<List<UserEntity>> GetAllUsersAsync();

		/// <summary>
		/// Get role
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		Task<UserRole> GetUserRole(string userId);

		/// <summary>
		/// Assign User Role
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="role"></param>
		/// <returns></returns>
		Task AssignUserRole(string userId, string role);

		/// <summary>
		/// Get User Roles
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		Task<List<UserRole>> GetUserRolesAsync(string userId);

	}
}
