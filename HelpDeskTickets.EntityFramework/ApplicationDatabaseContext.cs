using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HelpDeskTickets.EntityFramework.Core.Interfaces;
using HelpDeskTickets.EntityFramework.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HelpDeskTickets.EntityFramework
{
	public class ApplicationDatabaseContext : IdentityDbContext<User>
	{
		public DbSet<Ticket> Tickets { get; set; }

		public DbSet<IssueSeverity> IssueSeverities { get; set; }

		public DbSet<IssueType> IssueTypes { get; set; }

		public DbSet<ResolutionStatus> ResolutionStatuses { get; set; }

		public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options) : base(options)
		{
		}

		#region Overrides - Save Changes

		public override int SaveChanges()
		{
			SetTimestamps();

			return base.SaveChanges();
		}

		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			SetTimestamps();

			return base.SaveChanges(acceptAllChangesOnSuccess);
		}

		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
		{
			SetTimestamps();

			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			SetTimestamps();

			return base.SaveChangesAsync(cancellationToken);
		}

		private void SetTimestamps()
		{
			var changedEntries = ChangeTracker.Entries().Where(ce => ce.State == EntityState.Added ||
																	ce.State == EntityState.Modified);

			foreach (var changedEntry in changedEntries)
			{
				var changeEntryType = changedEntry.Entity.GetType();

				var isTimestamp = changeEntryType.GetInterfaces().Any(i => i == typeof(ITimestamp));

				if (!isTimestamp) continue;

				PropertyInfo changeEntryPropertyInfo;

				if (changedEntry.State == EntityState.Added)
				{
					changeEntryPropertyInfo = changeEntryType.GetProperty(nameof(ITimestamp.CreatedAt));
					changeEntryPropertyInfo?.SetValue(changedEntry.Entity, DateTimeOffset.UtcNow, null);
				}

				changeEntryPropertyInfo = changeEntryType.GetProperty(nameof(ITimestamp.UpdatedAt));
				changeEntryPropertyInfo?.SetValue(changedEntry.Entity, DateTimeOffset.UtcNow, null);
			}
		}
		#endregion Overrides - Save Changes


	}
}
