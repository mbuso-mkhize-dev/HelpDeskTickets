using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using HelpDeskTickets.ApplicationLogic.Entities.Tickets;
using HelpDeskTickets.ApplicationLogic.Interfaces;
using HelpDeskTickets.Helpers;
using HelpDeskTickets.Models.Tickets;

namespace HelpDeskTickets.Controllers
{
    /// <summary>
    /// Tickets
    /// </summary>
	[Authorize]
	[AiExceptionLogger]
	public class TicketsController : Controller
    {
		private readonly ITicketService _ticketService;
        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="ticketService"></param>
		public TicketsController(
			ITicketService ticketService)
		{
			_ticketService = ticketService;
		}
		
        /// <summary>
        /// Create Ticket View
        /// </summary>
        /// <returns></returns>
		public async Task<IActionResult> CreateTicket()
		{

			var resoSelectlist = (await _ticketService.GetAllResolutionStatuses())
				.Select(x =>
						new SelectListItem
						{
							Value = x.Id.ToString(),
							Text = x.Name
						}).ToList();

			var typeSelectlist = (await _ticketService.GetAllIssueTypes())
				.Select(x =>
						new SelectListItem
						{
							Value = x.Id.ToString(),
							Text = x.Name
						}).ToList();


			var severeSelectlist = (await _ticketService.GetAllIssueServerities())
				.Select(x =>
						new SelectListItem
						{
							Value = x.Id.ToString(),
							Text = x.Name
						}).ToList();

			ViewBag.Reso = resoSelectlist;
			ViewBag.Types = typeSelectlist;
			ViewBag.Severe = severeSelectlist;

			return View();
		}

        /// <summary>
        /// Create Ticktet
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> CreateTicket(TicketViewModel model)
		{
			// For Customers
			await _ticketService.AddTicket(new TicketCreateEntity
			{
				Customer = User.GetUserId(),
				Description = model.Description,
				Title = model.Title,
				IssueSeverity = model.IssueSeverity,
				IssueType = model.IssueType,
				ResolutionStatus = Guid.Parse("55b3e135-05d4-48cd-893b-08d752a6561a") // Logged Status

			});

			// Maybe later add implementation for Technicians

			// Return to My Tickets listing
			return RedirectToAction("MyTickets");
		}

		/// <summary>
        /// Update Ticket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		public async Task<IActionResult> UpdateTicket(Guid id)
		{
			var resoSelectlist = (await _ticketService.GetAllResolutionStatuses())
				.Select(x =>
						new SelectListItem
						{
							Value = x.Id.ToString(),
							Text = x.Name
						}).ToList();

			var typeSelectlist = (await _ticketService.GetAllIssueTypes())
				.Select(x =>
						new SelectListItem
						{
							Value = x.Id.ToString(),
							Text = x.Name
						}).ToList();


			var severeSelectlist = (await _ticketService.GetAllIssueServerities())
				.Select(x =>
						new SelectListItem
						{
							Value = x.Id.ToString(),
							Text = x.Name
						}).ToList();

			ViewBag.Reso = resoSelectlist;
			ViewBag.Types = typeSelectlist;
			ViewBag.Severe = severeSelectlist;

			var model = _ticketService.GetTicket(id);
			return View(new TicketViewModel
			{
				Id = model.Id,
				Title = model.Title,
				Description = model.Description,
				Comments = model.Comments,
				ResolutionSteps = model.ResolutionSteps,
				ResolutionStatus = model.ResolutionStatusEntity.Id,
				IssueSeverity = model.IssueSeverityEntity.Id,
				IssueType = model.IssueTypeEntity.Id,
				CreatedAt = model.CreatedAt,
				Customer = model.Customer.Id
			});
		}

        /// <summary>
        /// Update Ticket
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> UpdateTicket(TicketUpdateEntity model)
		{
			await _ticketService.UpdateTicket(model);
            return RedirectToAction("GetLoggedTickets");
		}

		public async Task<IActionResult> GetTicket(Guid id)
		{
			var resoSelectlist = (await _ticketService.GetAllResolutionStatuses())
				   .Select(x =>
						   new SelectListItem
						   {
							   Value = x.Id.ToString(),
							   Text = x.Name
						   }).ToList();

			var typeSelectlist = (await _ticketService.GetAllIssueTypes())
				.Select(x =>
						new SelectListItem
						{
							Value = x.Id.ToString(),
							Text = x.Name
						}).ToList();


			var severeSelectlist = (await _ticketService.GetAllIssueServerities())
				.Select(x =>
						new SelectListItem
						{
							Value = x.Id.ToString(),
							Text = x.Name
						}).ToList();

			ViewBag.Reso = resoSelectlist;
			ViewBag.Types = typeSelectlist;
			ViewBag.Severe = severeSelectlist;

			var model = _ticketService.GetTicket(id);
			return View(new TicketViewModel
			{
				Id = model.Id,
				Title = model.Title,
				Description = model.Description,
				Comments = model.Comments,
				ResolutionSteps = model.ResolutionSteps,
				ResolutionStatus = model.ResolutionStatusEntity.Id,
				IssueSeverity = model.IssueSeverityEntity.Id,
				IssueType = model.IssueTypeEntity.Id,
				CreatedAt = model.CreatedAt,
				Customer = model.Customer.Id
			});
		}

        /// <summary>
        /// My Oustanding Tickets
        /// </summary>
        /// <returns></returns>
		public IActionResult MyOustandingTickets()
		{
			var models = _ticketService.GetCustomerTickets(User.GetUserId(), true);
			var model = new MyTicketViewModel
			{
				Tickets = JsonConvert.SerializeObject(models.Select(c =>
					new
					{
						Id = c.Id,
						Title = c.Title,
						Status = c.ResolutionStatusEntity.Name,
						Createdat = c.CreatedAt.ToString("yyyy/MM/dd HH:mm"),
						IssueType = c.IssueTypeEntity.Name,
						IssueSeverity = c.IssueSeverityEntity.Name

					}))
			};
			return View(model);
		}

        /// <summary>
        /// My Tickets
        /// </summary>
        /// <returns></returns>
		public IActionResult MyTickets()
		{
			var models = _ticketService.GetCustomerTickets(User.GetUserId(), false);
			var model = new MyTicketViewModel
			{
				Tickets = JsonConvert.SerializeObject(models.Select(c =>
					new
					{
						Id = c.Id,
						Title = c.Title,
						Status = c.ResolutionStatusEntity.Name,
						Createdat = c.CreatedAt.ToString("yyyy/MM/dd HH:mm"),
						IssueType = c.IssueTypeEntity.Name,
						IssueSeverity = c.IssueSeverityEntity.Name

					}))
			};
			return View(model);
		}

        /// <summary>
        /// Get Logged Tickets
        /// </summary>
        /// <returns></returns>
		public IActionResult GetLoggedTickets()
		{
			var models = _ticketService.GetLoggedTickets();
			var model = new MyTicketViewModel
			{
				Tickets = JsonConvert.SerializeObject(models.Select(c =>
					new
					{
						Id = c.Id,
						Title = c.Title,
						Status = c.ResolutionStatusEntity.Name,
						Createdat = c.CreatedAt.ToString("yyyy/MM/dd HH:mm"),
						IssueType = c.IssueTypeEntity.Name,
						IssueSeverity = c.IssueSeverityEntity.Name

					}))
			};
			return View(model);
		}

        /// <summary>
        /// Search Tickets for suggestions
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
		[Produces("application/json")]
		public IActionResult SearchTickets(string searchText)
		{
			var models = _ticketService.Search(searchText);
			return Ok(JsonConvert.SerializeObject(models.Select(c =>
				   new
				   {
					   Id = c.Id,
					   Title = c.Title,
					   Status = c.ResolutionStatusEntity.Name,
					   Createdat = c.CreatedAt.ToString("yyyy/MM/dd HH:mm"),
					   IssueType = c.IssueTypeEntity.Name,
					   IssueSeverity = c.IssueSeverityEntity.Name

				   })));
		}

        /// <summary>
        /// Delete Ticket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpDelete]
		public async Task<IActionResult> DeleteTicket(Guid id)
		{
			await _ticketService.DeleteTicket(id);
			return View();
		}

        /// <summary>
        /// Create IssueType
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost]
		[Produces("application/json")]
		public async Task<IActionResult> CreateIssueType(IssueTypeEntity model)
		{
			await _ticketService.AddIssueType(model);
			return Ok();
		}

        /// <summary>
        /// Create IssueSeverity
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
		[Produces("application/json")]
		public async Task<IActionResult> CreateIssueSeverity(IssueSeverityEntity model)
		{
			await _ticketService.AddIssueSeverity(model);
			return Ok();
		}

        /// <summary>
        /// Create Resolution
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
		[HttpPost]
		[Produces("application/json")]
		public async Task<IActionResult> CreateResolution(ResolutionStatusEntity model)
		{
			await _ticketService.AddResolutionStatus(model);
			return Ok();
		}

	}
}