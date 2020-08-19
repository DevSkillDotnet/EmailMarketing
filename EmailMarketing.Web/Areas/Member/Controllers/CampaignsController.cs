﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EmailMarketing.Web.Areas.Member.Models.Campaigns;
using Microsoft.AspNetCore.Mvc;

namespace EmailMarketing.Web.Areas.Member.Controllers
{
    [Area("Member")]
    public class CampaignsController : Controller
    {
        public IActionResult Index()
        {
            var model = new CampaignsModel();
            return View(model);
        }
        public async Task<IActionResult> AddCampaign()
        {
            var model = new CreateCampaignModel();
            model.GroupSelectList = await model.GetAllGroupForSelectAsync();
            model.EmailTemplateList = await model.GetTemplateByUserIDAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCampaign(CreateCampaignModel model)
        {
            if(ModelState.IsValid)
            {
                await model.SaveCampaignAsync();
            }
            model.GroupSelectList = await model.GetAllGroupForSelectAsync();
            model.EmailTemplateList = await model.GetTemplateByUserIDAsync();
            return View(model);
        }

        public IActionResult ViewReport()
        {
            var model = new CampaignsModel();
            return View(model);
        }
    }
}