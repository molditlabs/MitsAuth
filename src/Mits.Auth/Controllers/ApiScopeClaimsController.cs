﻿using AutoMapper;
using Mits.Auth.Data.Contexts;
using Mits.Auth.ViewModels;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mits.Auth.Controllers
{
    public class ApiScopeClaimsController : Controller
    {
        protected readonly IdentityServerDbContext _dbContext;
        protected readonly IMapper _mapper;
        public ApiScopeClaimsController(IdentityServerDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public JsonResult GetApiScopeClaimsTableData(int apiScopeid, string search, string sort, string order, int offset, int limit)
        {
            var query= _dbContext.ApiScopeClaims.Where(o => o.ScopeId == apiScopeid);

            var data = query.Skip(offset).Take(limit).ToList();
            var viewModel = data.Select(apires => new ApiScopeClaimDetailsViewModel
            {
                Id = apires.Id,
                Type = apires.Type,
            }).ToList();

            return Json(new
            {
                total = query.Count(),
                rows = viewModel
            });
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var record = _dbContext.ApiScopeClaims.FirstOrDefault(o => o.Id == id);

            var viewmodel = _mapper.Map<ApiScopeClaimDetailsViewModel>(record);

            return View(viewmodel);
        }


        [HttpGet]
        public IActionResult Add(int id)
        {
            var viewmodel = new ApiScopeClaimAddViewModel 
            {
                ScopeId = id,
            };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ApiScopeClaimAddViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                var record = _mapper.Map<ApiScopeClaim>(viewmodel);
                _dbContext.ApiScopeClaims.Add(record);

                var result = await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                if (result > 0)
                {
                    return RedirectToAction("Details", "ApiScopes", new { id= viewmodel.ScopeId});
                }

                ModelState.AddModelError("", "Failed");

            }

            return View(viewmodel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var record = _dbContext.ApiScopeClaims.FirstOrDefault(o => o.Id == id);

            var viewmodel = _mapper.Map<ApiScopeClaimEditViewModel>(record);

            return View(viewmodel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApiScopeClaimEditViewModel viewmodel)
        {
            var isRecordFound = _dbContext.ApiScopeClaims.Any(o => o.Id == viewmodel.Id);

            if (ModelState.IsValid && isRecordFound)
            {
                var record = _mapper.Map<ApiScopeClaim>(viewmodel);
        
                _dbContext.Update(record);
                var result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    return RedirectToAction("Details", "ApiScopes", new { id = viewmodel.ScopeId });
                }

                ModelState.AddModelError("", "Failed");
            }

            return View(viewmodel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var record = _dbContext.ApiScopeClaims.FirstOrDefault(o => o.Id == id);

            var viewmodel = _mapper.Map<ApiScopeClaimEditViewModel>(record);

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ApiScopeClaimEditViewModel viewmodel)
        {
            var isRecordFound = _dbContext.ApiScopeClaims.Any(o => o.Id == viewmodel.Id);

            if (ModelState.IsValid && isRecordFound)
            {
                var record = _mapper.Map<ApiScopeClaim>(viewmodel);
                _dbContext.Remove(record);
                var result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    return RedirectToAction("Details", "ApiScopes", new { id = viewmodel.ScopeId });
                }

                ModelState.AddModelError("", "Failed");
            }

            return View(viewmodel);
        }
    }
}
