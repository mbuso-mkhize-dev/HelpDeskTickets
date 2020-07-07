using HelpDeskTickets.ApplicationLogic.Entities.Tickets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpDeskTickets.ApplicationLogic.Interfaces
{
	public interface ITicketService
	{
		Task AddTicket(TicketCreateEntity entity);

		List<TicketEntity> GetLoggedTickets();

		List<TicketEntity> GetCustomerTickets(string userId, bool outstanding);

		TicketEntity GetTicket(Guid id);

		Task DeleteTicket(Guid id);

		Task UpdateTicket(TicketUpdateEntity entity);

		Task AddIssueSeverity(IssueSeverityEntity entity);

		Task AddIssueType(IssueTypeEntity entity);

		Task AddResolutionStatus(ResolutionStatusEntity entity);

		List<TicketEntity> Search(string searchText);

		Task<List<IssueSeverityEntity>> GetAllIssueServerities();

		Task<List<IssueTypeEntity>> GetAllIssueTypes();

		Task<List<ResolutionStatusEntity>> GetAllResolutionStatuses();
	}
}