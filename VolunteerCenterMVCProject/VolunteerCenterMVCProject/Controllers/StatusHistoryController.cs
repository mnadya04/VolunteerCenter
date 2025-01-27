using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Linq.Expressions;
using VolunteerCenterMVCProject.Common;
using VolunteerCenterMVCProject.Services.Interfaces;
using VolunteerCenterMVCProject.ViewModels.Shared;
using VolunteerCenterMVCProject.ViewModels.StatusHistory;

namespace VolunteerCenterMVCProject.Controllers
{
	[Authorize(Roles = Constants.AdminRole)]
	public class StatusHistoryController : Controller
	{
		private readonly IStatusHistoryService service;

		public StatusHistoryController(IStatusHistoryService service)
		{
			this.service = service;
		}

		[HttpGet]
		public async Task<IActionResult> Index(IndexVM model)
		{

			model.Pager = new PagerVM();

			model.Pager.Page = model.Pager.Page <= 0 ? 1 : model.Pager.Page;

			model.Pager.ItemsPerPage = model.Pager.ItemsPerPage <= 0 ? 10 : model.Pager.ItemsPerPage;

			model.Pager.PagesCount = (int)Math.Ceiling(service.Count() / (double)model.Pager.ItemsPerPage);

			IndexVM result = await service.GetAllChangesAsync(model.Pager.Page, model.Pager.ItemsPerPage, model.Pager.PagesCount);

			model.Changes = result.Changes;

			return View(model);
		}
	}
}
