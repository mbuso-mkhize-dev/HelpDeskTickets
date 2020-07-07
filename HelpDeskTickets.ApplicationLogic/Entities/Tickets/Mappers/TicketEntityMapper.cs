using HelpDeskTickets.ApplicationLogic.Entities.Users;
using HelpDeskTickets.EntityFramework.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.ApplicationLogic.Entities.Tickets.Mappers
{
	public static class TicketEntityMapper
	{
		public static TicketEntity MapToEntity(Ticket model)
		{
			return new TicketEntity
			{
				Id = model.Id,
				Title = model.Title,
				Description = model.Description,
				IssueSeverityEntity = MapToEntity(model.IssueSeverity),
				IssueTypeEntity = MapToEntity(model.IssueType),
				ResolutionStatusEntity = MapToEntity(model.ResolutionStatus),
				ResolutionSteps = model.ResolutionSteps,
				Comments = model.Comments,
				CreatedAt = model.CreatedAt,
				UpdatedAt = model.UpdatedAt,
				Customer = new UserEntity
				{
					Id = model.Customer.Id,
					FirstName = model.Customer.FirstName,
					LastName = model.Customer.LastName,
					Address = model.Customer.Address
				},
				TechnicianId = model.TechnicianId
			};
		}

		public static IssueSeverityEntity MapToEntity(IssueSeverity model)
		{
			return new IssueSeverityEntity
			{
				Id = model.Id,
				Name = model.Name,
				Color = model.Color
			};
		}

		public static IssueTypeEntity MapToEntity(IssueType model)
		{
			return new IssueTypeEntity
			{
				Id = model.Id,
				Name = model.Name
			};
		}


		public static ResolutionStatusEntity MapToEntity(ResolutionStatus model)
		{
			return new ResolutionStatusEntity
			{
				Id = model.Id,
				Name = model.Name,
				Color = model.Color
			};
		}
	}
}
