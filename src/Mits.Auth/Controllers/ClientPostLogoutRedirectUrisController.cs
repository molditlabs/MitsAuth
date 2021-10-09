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
    public class ClientPostLogoutRedirectUrisController : Controller
    {
        protected readonly IdentityServerDbContext _dbContext;
        protected readonly IMapper _mapper;
        public ClientPostLogoutRedirectUrisController(IdentityServerDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        [HttpGet]
        public JsonResult GetClientPostLogoutRedirectUrisTableData(int clientid, string search, string sort, string order, int offset, int limit)
        {
            var query = _dbContext.ClientPostLogoutRedirectUris.Where(o => o.ClientId == clientid);

            var data = query.Skip(offset).Take(limit).ToList();
            var viewModel = data.Select(uri => new ClientPostLogoutRedirectUriDetailsViewModel
            {
                Id = uri.Id,
                PostLogoutRedirectUri = uri.PostLogoutRedirectUri,
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
            var record = _dbContext.ClientPostLogoutRedirectUris.FirstOrDefault(o => o.Id == id);

            var viewmodel = _mapper.Map<ClientPostLogoutRedirectUriDetailsViewModel>(record);

            return View(viewmodel);
        }


        [HttpGet]
        public IActionResult Add(int id)
        {
            var viewmodel = new ClientPostLogoutRedirectUriAddViewModel
            {
                ClientId = id,
            };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ClientPostLogoutRedirectUriAddViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                var record = _mapper.Map<ClientPostLogoutRedirectUri>(viewmodel);
                record.Id = 0;
                _dbContext.ClientPostLogoutRedirectUris.Add(record);

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
            var record = _dbContext.ClientPostLogoutRedirectUris.FirstOrDefault(o => o.Id == id);

            var viewmodel = _mapper.Map<ClientPostLogoutRedirectUriEditViewModel>(record);

            return View(viewmodel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClientPostLogoutRedirectUriEditViewModel viewmodel)
        {
            var isRecordFound = _dbContext.ClientPostLogoutRedirectUris.Any(o => o.Id == viewmodel.Id);

            if (ModelState.IsValid && isRecordFound)
            {
                var record = _mapper.Map<ClientPostLogoutRedirectUri>(viewmodel);

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
            var record = _dbContext.ClientPostLogoutRedirectUris.FirstOrDefault(o => o.Id == id);

            var viewmodel = _mapper.Map<ClientPostLogoutRedirectUriEditViewModel>(record);

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ClientPostLogoutRedirectUriEditViewModel viewmodel)
        {
            var isRecordFound = _dbContext.ClientPostLogoutRedirectUris.Any(o => o.Id == viewmodel.Id);

            if (ModelState.IsValid && isRecordFound)
            {
                var record = _mapper.Map<ClientPostLogoutRedirectUri>(viewmodel);
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
