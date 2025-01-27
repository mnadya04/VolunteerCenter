
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using VolunteerCenterMVCProject.Common;
using VolunteerCenterMVCProject.Services;
using VolunteerCenterMVCProject.Services.Interfaces;
using VolunteerCenterMVCProject.ViewModels.Categories;
using VolunteerCenterMVCProject.ViewModels.Events;
using VolunteerCenterMVCProject.ViewModels.Shared;

namespace VolunteerCenterMVCProject.Controllers
{
	
	public class CategoriesController : Controller
    {
        private readonly ICategoriesService service;
        private readonly IEventsService eventsService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.service = categoriesService;
            this.eventsService = eventsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(IndexVM model)
        {
            model.Pager ??= new PagerVM();

            model.Pager.Page = model.Pager.Page <= 0
                                        ? 1
                                        : model.Pager.Page;

            model.Pager.ItemsPerPage = model.Pager.ItemsPerPage <= 0
                                        ? 10
                                        : model.Pager.ItemsPerPage;

            Expression<Func<CategoryVM, bool>> filter =  i => 
            (String.IsNullOrEmpty(model.Name) || i.Name.Contains(model.Name)) &&
            (String.IsNullOrEmpty(model.Description) || i.Description.Contains(model.Description));

			model.Pager.PagesCount =
			  (int)Math.Ceiling(service.Count(filter) / (double)model.Pager.ItemsPerPage);

			IndexVM result = await service.GetAllAsync(filter, model.Pager.Page, model.Pager.ItemsPerPage, model.Pager.PagesCount);


            model.Categories = result.Categories;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create ()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await service.CreateAsync(model);

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            CategoryVM model = await service.GetCategoryAsync(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await service.UpdateAsync(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await service.DeleteByIdAsync(id);

            return RedirectToAction("Index");
        }

        // GET: Categories/Events
        public async Task<IActionResult> Events(string categoryId)
        {
            if (string.IsNullOrEmpty(categoryId))
            {
                return NotFound();
            }

            var categoryWithEvents = await this.service.GetCategoryWithEventsAsync(categoryId);

            if (categoryWithEvents == null)
            {
                return NotFound();
            }

            return View(categoryWithEvents);
        }


    }
}
