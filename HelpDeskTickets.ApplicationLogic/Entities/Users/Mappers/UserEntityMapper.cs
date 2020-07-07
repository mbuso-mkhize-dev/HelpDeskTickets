using HelpDeskTickets.EntityFramework.Core.Models;

namespace HelpDeskTickets.ApplicationLogic.Entities.Users.Mappers
{
    /// <summary>
    /// User Entity Mapper
    /// </summary>
	public static class UserEntityMapper
	{
        /// <summary>
        /// Map from User Model to User Entity
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
		public static UserEntity MapToEntity(User model)
		{
			if (model == null)
				return new UserEntity();

			return new UserEntity
			{
				Id = model.Id,
				FirstName = model.FirstName,
				LastName = model.LastName,
				Address = model.Address,
				CreatedAt = model.CreatedAt,
				UpdatedAt = model.UpdatedAt,
				Email = model.Email
			};
		}

        /// <summary>
        /// Map to Userrole
        /// </summary>
        /// <param name="userRole"></param>
        /// <returns></returns>
		public static UserRole GetUserRole(EntityFramework.Core.Enums.UserRole userRole)
		{
			switch (userRole)
			{
				case EntityFramework.Core.Enums.UserRole.Admin:
					return UserRole.Admin;

				case EntityFramework.Core.Enums.UserRole.Customer:
					return UserRole.Customer;

				case EntityFramework.Core.Enums.UserRole.Technician:
					return UserRole.Technician;

				default:
					return UserRole.Customer;
			}
		}
	}
}