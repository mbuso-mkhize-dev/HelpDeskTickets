using Microsoft.AspNetCore.Mvc.Filters;

namespace HelpDeskTickets.Helpers
{
	public class AiExceptionLogger : ExceptionFilterAttribute
	{
		public override void OnException(ExceptionContext context)
		{
			if (context != null && context.Exception != null)
			{
				// Track exception
				var message = context.Exception.Message;
			}
			base.OnException(context);
		}
	}
}