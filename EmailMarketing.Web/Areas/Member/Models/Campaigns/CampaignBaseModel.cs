﻿using Autofac;
using EmailMarketing.Common.Services;
using EmailMarketing.Framework.Services.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Member.Models.Campaigns
{
    public class CampaignBaseModel : MemberBaseModel, IDisposable
    {
        protected readonly ICampaignReportExportService _campaignService;
        protected readonly ICurrentUserService _currentUserService;
        public CampaignBaseModel(ICampaignReportExportService campaignService,
            ICurrentUserService currentUserService)
        {
            _campaignService = campaignService;
            _currentUserService = currentUserService;
        }

        public CampaignBaseModel()
        {
            _campaignService = Startup.AutofacContainer.Resolve<ICampaignReportExportService>();
            _currentUserService = Startup.AutofacContainer.Resolve<ICurrentUserService>();
        }
        public void Dispose()
        {
            _campaignService?.Dispose();
        }
    }
}
