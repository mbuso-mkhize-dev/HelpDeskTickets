using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDeskTickets.Components
{
	public abstract class BaseViewComponent : ViewComponent
	{
		public new ViewViewComponentResult View<TModel>(string viewName, TModel model)
		{
			return base.View<TModel>(viewName, model);
		}

		public new ViewViewComponentResult View<TModel>(TModel model)
		{
			return base.View<TModel>(model);
		}

		public new ViewViewComponentResult View(string viewName)
		{
			return base.View(viewName);
		}
	}
}
