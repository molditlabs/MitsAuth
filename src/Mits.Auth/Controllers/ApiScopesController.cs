﻿using AutoMapper;
using Mits.Auth.Data.Contexts;
using Mits.Auth.ViewModels;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mits.Auth.Controllers
{
    public class ApiScopesController : Controller
    {
        protected readonly IdentityServerDbContext _dbContext;
        protected readonly IMapper _mapper;
        public ApiScopesController(IdentityServerDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetApiScopesTableData(string search, string sort, string order, int offset, int limit)
        {
            var apiScopes = _dbContext.ApiScopes.Skip(offset).Take(limit).ToList();
            var viewModel = apiScopes.Select(apires => new ApiScopeDetailsViewModel
            {
                Id = apires.Id,
                Name = apires.Name,
                DisplayName = apires.DisplayName,
            }).ToList();

            return Json(new
            {
                total = _dbContext.ApiScopes.Count(),
                rows = viewModel
            });
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var record = _dbContext.ApiScopes.FirstOrDefault(o => o.Id == id);

            var viewmodel = _mapper.Map<ApiScopeDetailsViewModel>(record);

            return View(viewmodel);
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ApiScopeAddViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var record = _mapper.Map<ApiScope>(viewModel);
                record.Enabled = true;

                await _dbContext.AddAsync(record).ConfigureAwait(false);
                var result = await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Failed");

            }

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var record = _dbContext.ApiScopes.FirstOrDefault(o => o.Id == id);

            var viewmodel = _mapper.Map<ApiScopeEditViewModel>(record);

            return View(viewmodel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApiScopeEditViewModel viewmodel)
        {
            var isRecordFound = _dbContext.ApiScopes.Any(o => o.Id == viewmodel.Id);

            if (ModelState.IsValid && isRecordFound)
            {
                var record = _mapper.Map<ApiScope>(viewmodel);
   
                _dbContext.Update(record);
                var result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Failed");
            }

            return View(viewmodel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var record = _dbContext.ApiScopes.FirstOrDefault(o => o.Id == id);

            var viewmodel = _mapper.Map<ApiScopeEditViewModel>(record);

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ApiScopeEditViewModel viewmodel)
        {
            var isRecordFound = _dbContext.ApiScopes.Any(o => o.Id == viewmodel.Id);

            if (ModelState.IsValid && isRecordFound)
            {
                var record = _mapper.Map<ApiScope>(viewmodel);
                _dbContext.Remove(record);
                var result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Failed");
            }

            return View(viewmodel);
        }
    }
}
