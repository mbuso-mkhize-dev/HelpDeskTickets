using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using HelpDeskTickets.ApplicationLogic.Interfaces;
using HelpDeskTickets.ApplicationLogic.Services;
using HelpDeskTickets.EntityFramework.Core.Interfaces;
using HelpDeskTickets.EntityFramework.Core.Models;
using HelpDeskTickets.EntityFramework.Repositories;

namespace HelpDeskTickets.DI
{
	public static class ServiceCollectionBuilder
	{
		public static void AddServices(IServiceCollection services)
		{
			// Repositories
			services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();			
			services.AddScoped<IBaseRepository<IssueSeverity>, BaseRepository<IssueSeverity>>();
			services.AddScoped<IBaseRepository<IssueType>, BaseRepository<IssueType>>();
			services.AddScoped<IBaseRepository<ResolutionStatus>, BaseRepository<ResolutionStatus>>();
			services.AddScoped<IBaseRepository<Ticket>, BaseRepository<Ticket>>();

			// Services
			services.AddScoped<ITicketService, TicketService>();
			services.AddScoped<IUserService, UserService>();
			services.AddTransient<AspNetRoleManager<IdentityRole>>();
		}
	}
}
