using Microsoft.EntityFrameworkCore;
using HelpDeskTickets.ApplicationLogic.Entities.Tickets;
using HelpDeskTickets.ApplicationLogic.Entities.Tickets.Mappers;
using HelpDeskTickets.ApplicationLogic.Interfaces;
using HelpDeskTickets.EntityFramework.Core.Interfaces;
using HelpDeskTickets.EntityFramework.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDeskTickets.ApplicationLogic.Services
{
	public class TicketService : ITicketService
	{
		private readonly IBaseRepository<Ticket> _ticketRepository;
		private readonly IBaseRepository<IssueSeverity> _issueSeverityRepository;
		private readonly IBaseRepository<IssueType> _issueTypeRepository;
		private readonly IBaseRepository<ResolutionStatus> _resolutionStatusRepository;
		private readonly IBaseRepository<User> _userRepository;

		public TicketService(
			IBaseRepository<Ticket> ticketRepository,
			IBaseRepository<IssueSeverity> issueSeverityRepository,
			IBaseRepository<IssueType> issueTypeRepository,
			IBaseRepository<ResolutionStatus> resolutionStatusRepository,
			IBaseRepository<User> userRepository)
		{
			_ticketRepository = ticketRepository;
			_issueSeverityRepository = issueSeverityRepository;
			_issueTypeRepository = issueTypeRepository;
			_resolutionStatusRepository = resolutionStatusRepository;
			_userRepository = userRepository;
		}

		#region IssueServerity

		public async Task<IssueSeverityEntity> GetIssueServerity(Guid id)
		{
			var model = await _issueSeverityRepository.FindAsync(id);

			return TicketEntityMapper.MapToEntity(model);
		}

		public async Task<List<IssueSeverityEntity>> GetAllIssueServerities()
		{
			var models = await _issueSeverityRepository.AllAsync();

			return models.Select(model => TicketEntityMapper.MapToEntity(model)).ToList();
		}

		public async Task AddIssueSeverity(IssueSeverityEntity entity)
		{
			_issueSeverityRepository.Add(new IssueSeverity
			{
				Name = entity.Name,
				Color = entity.Color
			});

			await _issueSeverityRepository.SaveAsync();
		}

		public async Task DeleteIssueServerity(Guid id)
		{
			var model = await _issueSeverityRepository.FindAsync(id);

			if (model != null)
			{
				_issueSeverityRepository.Delete(model);
				await _issueSeverityRepository.SaveAsync();
			}
		}

		#endregion IssueServerity

		#region IssueType

		public async Task<IssueTypeEntity> GetIssueType(Guid id)
		{
			var model = await _issueTypeRepository.FindAsync(id);

			return TicketEntityMapper.MapToEntity(model);
		}

		public async Task<List<IssueTypeEntity>> GetAllIssueTypes()
		{
			var models = await _issueTypeRepository.AllAsync();

			return models.Select(model => TicketEntityMapper.MapToEntity(model)).ToList();
		}

		public async Task AddIssueType(IssueTypeEntity entity)
		{
			_issueTypeRepository.Add(new IssueType
			{
				Name = entity.Name
			});

			await _issueTypeRepository.SaveAsync();
		}

		public async Task DeleteIssueType(Guid id)
		{
			var model = await _issueTypeRepository.FindAsync(id);

			if (model != null)
			{
				_issueTypeRepository.Delete(model);
				await _issueTypeRepository.SaveAsync();
			}
		}

		#endregion IssueType

		#region ResolutionStatus

		public async Task<ResolutionStatusEntity> GetResolutionStatus(Guid id)
		{
			var model = await _resolutionStatusRepository.FindAsync(id);

			return TicketEntityMapper.MapToEntity(model);
		}

		public async Task<List<ResolutionStatusEntity>> GetAllResolutionStatuses()
		{
			var models = await _resolutionStatusRepository.AllAsync();

			return models.Select(model => TicketEntityMapper.MapToEntity(model)).ToList();
		}

		public async Task AddResolutionStatus(ResolutionStatusEntity entity)
		{
			_resolutionStatusRepository.Add(new ResolutionStatus
			{
				Name = entity.Name,
				Color = entity.Color
			});

			await _resolutionStatusRepository.SaveAsync();
		}

		public async Task DeleteResolutionStatus(Guid id)
		{
			var model = await _resolutionStatusRepository.FindAsync(id);

			if (model != null)
			{
				_resolutionStatusRepository.Delete(model);
				await _resolutionStatusRepository.SaveAsync();
			}
		}

		#endregion ResolutionStatus

		#region Tickets

		public TicketEntity GetTicket(Guid id)
		{
			var model = _ticketRepository.Where(t => t.Id == id)
				.Include(i => i.ResolutionStatus)
				.Include(i => i.IssueType)
				.Include(i => i.IssueSeverity)
				.Include(i => i.Customer)
				.FirstOrDefault();

			return TicketEntityMapper.MapToEntity(model);
		}

		public List<TicketEntity> Search(string searchText)
		{
			var models = _ticketRepository.Where(c => 
			//c.ResolutionStatus.Name == "Resolved" && 
			(c.Title.Contains(searchText) || 
			c.Description.Contains(searchText) || 
			c.ResolutionSteps.Contains(searchText) ||
			c.Comments.Contains(searchText)))
			.Include(i =>i.ResolutionStatus)
			.Include(i => i.IssueSeverity)
			.Include(i => i.IssueType)
			.Include(i => i.Customer);

			return models.Select(model => TicketEntityMapper.MapToEntity(model)).ToList();
		}

		public List<TicketEntity> GetCustomerTickets(string userId, bool outstanding)
		{
			List<Ticket> models;
			if (outstanding)
			{
				models = _ticketRepository
				.Where(t => t.Customer.Id == userId && t.ResolutionStatus.Name == "Logged")
				.Include(i => i.ResolutionStatus)
				.Include(i => i.IssueType)
				.Include(i => i.IssueSeverity)
				.Include(i => i.Customer)
				.ToList();
			}
			else
			{
				models = _ticketRepository.Where(t => t.Customer.Id == userId)
				.Include(i => i.ResolutionStatus)
				.Include(i => i.IssueType)
				.Include(i => i.IssueSeverity)
				.Include(i => i.Customer)
				.ToList();
			}

			return models.Select(model => TicketEntityMapper.MapToEntity(model)).ToList();
		}

		public List<TicketEntity> GetLoggedTickets()
		{
			var models = _ticketRepository
				.Where(t => t.ResolutionStatus.Name == "Logged")
				.Include(i => i.ResolutionStatus)
				.Include(i => i.IssueType)
				.Include(i => i.IssueSeverity)
				.Include(i => i.Customer)
				.ToList();

			return models.Select(model => TicketEntityMapper.MapToEntity(model)).ToList();
		}

		public async Task AddTicket(TicketCreateEntity entity)
		{
			var issueType = await _issueTypeRepository.FindAsync(entity.IssueType);
			var issueSeverity = await _issueSeverityRepository.FindAsync(entity.IssueSeverity);
			var resolutionStatus = await _resolutionStatusRepository.FindAsync(entity.ResolutionStatus);
			var customer = await _userRepository.FindAsync(entity.Customer);

			_ticketRepository.Add(new Ticket
			{
				IssueSeverity = issueSeverity,
				IssueType = issueType,
				ResolutionStatus = resolutionStatus,
				Title = entity.Title,
				Description = entity.Description,
				Customer = customer
			});

			await _ticketRepository.SaveAsync();
		}

		public async Task UpdateTicket(TicketUpdateEntity entity)
		{
			var model = await _ticketRepository.FindAsync(entity.Id);
			var issueType = await _issueTypeRepository.FindAsync(entity.IssueType);
			var issueSeverity = await _issueSeverityRepository.FindAsync(entity.IssueSeverity);
			var resolutionStatus = await _resolutionStatusRepository.FindAsync(entity.ResolutionStatus);
			var customer = await _userRepository.FindAsync(entity.Customer);

			model.IssueSeverity = issueSeverity;
			model.IssueType = issueType;
			model.ResolutionStatus = resolutionStatus;
			model.Title = entity.Title;
			model.Description = entity.Description;
			model.Customer = customer;
			model.TechnicianId = entity.TechnicianId;
			model.ResolutionSteps = entity.ResolutionSteps;
			model.Comments = entity.Comments;

			_ticketRepository.Update(model);

			await _ticketRepository.SaveAsync();
		}

		public async Task DeleteTicket(Guid id)
		{
			var model = await _ticketRepository.FindAsync(id);

			if (model != null)
			{
				_ticketRepository.Delete(model);
				await _ticketRepository.SaveAsync();
			}
		}

		#endregion Tickets
	}
}