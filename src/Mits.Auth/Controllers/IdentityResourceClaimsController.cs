﻿using AutoMapper;
using Mits.Auth.Data.Contexts;
using Mits.Auth.ViewModels;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mits.Auth.Controllers
{
    [Authorize]
    public class IdentityResourceClaimsController : Controller
    {
        protected readonly IdentityServerDbContext _dbContext;
        protected readonly IMapper _mapper;
        public IdentityResourceClaimsController(IdentityServerDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public JsonResult GetIdentityResourceClaimsTableData(int identityresourceid, string search, string sort, string order, int offset, int limit)
        {
            var query = _dbContext.IdentityResourceClaims.Where(o => o.IdentityResourceId == identityresourceid);

            var data = query.Skip(offset).Take(limit).ToList();
            var viewModel = data.Select(Identityres => new IdentityResourceClaimDetailsViewModel
            {
                Id = Identityres.Id,
                Type = Identityres.Type,
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
            var record = _dbContext.IdentityResourceClaims.FirstOrDefault(o => o.Id == id);

            var viewmodel = _mapper.Map<IdentityResourceClaimDetailsViewModel>(record);

            return View(viewmodel);
        }


        [HttpGet]
        public IActionResult Add(int id)
        {
            var viewmodel = new IdentityResourceClaimAddViewModel
            {
                IdentityResourceId = id,
            };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(IdentityResourceClaimAddViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                var record = _mapper.Map<IdentityResourceClaim>(viewmodel);
                _dbContext.IdentityResourceClaims.Add(record);

                var result = await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                if (result > 0)
                {
                    return RedirectToAction("Details", "IdentityResources", new { id = viewmodel.IdentityResourceId });
                }

                ModelState.AddModelError("", "Failed");

            }

            return View(viewmodel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var record = _dbContext.IdentityResourceClaims.FirstOrDefault(o => o.Id == id);

            var viewmodel = _mapper.Map<IdentityResourceClaimEditViewModel>(record);

            return View(viewmodel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IdentityResourceClaimEditViewModel viewmodel)
        {
            var isRecordFound = _dbContext.IdentityResourceClaims.Any(o => o.Id == viewmodel.Id);

            if (ModelState.IsValid && isRecordFound)
            {
                var record = _mapper.Map<IdentityResourceClaim>(viewmodel);

                _dbContext.Update(record);
                var result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    return RedirectToAction("Details", "IdentityResources", new { id = viewmodel.IdentityResourceId });
                }

                ModelState.AddModelError("", "Failed");
            }

            return View(viewmodel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var record = _dbContext.IdentityResourceClaims.FirstOrDefault(o => o.Id == id);

            var viewmodel = _mapper.Map<IdentityResourceClaimEditViewModel>(record);

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(IdentityResourceClaimEditViewModel viewmodel)
        {
            var isRecordFound = _dbContext.IdentityResourceClaims.Any(o => o.Id == viewmodel.Id);

            if (ModelState.IsValid && isRecordFound)
            {
                var record = _mapper.Map<IdentityResourceClaim>(viewmodel);
                _dbContext.Remove(record);
                var result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    return RedirectToAction("Details", "IdentityResources", new { id = viewmodel.IdentityResourceId });
                }

                ModelState.AddModelError("", "Failed");
            }

            return View(viewmodel);
        }

    }
}
