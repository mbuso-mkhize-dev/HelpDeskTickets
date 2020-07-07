using HelpDeskTickets.ApplicationLogic.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDeskTickets.Models.Users.Mappers
{
	public static class UserViewModelMapper
	{
		public static UserCreateEntity MapToEntity(RegisterViewModel model)
		{
			return new UserCreateEntity
			{
				Email = model.Email,
				Address = model.Address,
				Password = model.Password,
				FirstName = model.FirstName,
				LastName = model.LastName
			};
		}
	}
}
