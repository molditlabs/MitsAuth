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
    public class ClientGrantTypesController : Controller
    {
        protected readonly IdentityServerDbContext _dbContext;
        protected readonly IMapper _mapper;
        public ClientGrantTypesController(IdentityServerDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public JsonResult GetClientGrantTypesTableData(int clientid, string search, string sort, string order, int offset, int limit)
        {
            var query = _dbContext.ClientGrantTypes.Where(o => o.ClientId == clientid);

            var data = query.Skip(offset).Take(limit).ToList();
            var viewModel = data.Select(granttype => new ClientGrantTypeDetailsViewModel
            {
                Id = granttype.Id,
                GrantType = granttype.GrantType,
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
            var record = _dbContext.ClientGrantTypes.FirstOrDefault(o => o.Id == id);

            var viewmodel = _mapper.Map<ClientGrantTypeDetailsViewModel>(record);

            return View(viewmodel);
        }


        [HttpGet]
        public IActionResult Add(int id)
        {
            var viewmodel = new ClientGrantTypeAddViewModel
            {
                ClientId = id,
            };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ClientGrantTypeAddViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                var record = _mapper.Map<ClientGrantType>(viewmodel);
                record.Id = 0;
                _dbContext.ClientGrantTypes.Add(record);

                var result = await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                if (result > 0)
                {
                    return RedirectToAction("Details", "Clients", new { id = viewmodel.ClientId });
                }

                ModelState.AddModelError("", "Failed");

            }

            return View(viewmodel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var record = _dbContext.ClientGrantTypes.FirstOrDefault(o => o.Id == id);

            var viewmodel = _mapper.Map<ClientGrantTypeEditViewModel>(record);

            return View(viewmodel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClientGrantTypeEditViewModel viewmodel)
        {
            var isRecordFound = _dbContext.ClientGrantTypes.Any(o => o.Id == viewmodel.Id);

            if (ModelState.IsValid && isRecordFound)
            {
                var record = _mapper.Map<ClientGrantType>(viewmodel);

                _dbContext.Update(record);
                var result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    return RedirectToAction("Details", "Clients", new { id = viewmodel.ClientId });
                }

                ModelState.AddModelError("", "Failed");
            }

            return View(viewmodel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var record = _dbContext.ClientGrantTypes.FirstOrDefault(o => o.Id == id);

            var viewmodel = _mapper.Map<ClientGrantTypeEditViewModel>(record);

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ClientGrantTypeEditViewModel viewmodel)
        {
            var isRecordFound = _dbContext.ClientGrantTypes.Any(o => o.Id == viewmodel.Id);

            if (ModelState.IsValid && isRecordFound)
            {
                var record = _mapper.Map<ClientGrantType>(viewmodel);
                _dbContext.Remove(record);
                var result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    return RedirectToAction("Details", "Clients", new { id = viewmodel.ClientId });
                }

                ModelState.AddModelError("", "Failed");
            }

            return View(viewmodel);
        }
    }
}
