﻿using EmailMarketing.Framework.Context;
using EmailMarketing.Data;
using EmailMarketing.Framework.Repositories.Campaigns;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmailMarketing.Framework.UnitOfWorks.Campaigns
{
    public class CampaignUnitOfWork : UnitOfWork, ICampaignUnitOfWork
    {

        public ICampaignRepository CampaignRepository { get; set; }
        public ICampaignReportRepository CampaignReportRepository { get; set; }

        public IEmailTemplateRepository EmailTemplateRepository { get; set; }
        public CampaignUnitOfWork(FrameworkContext dbContext, ICampaignReportRepository campaignReportRepository, ICampaignRepository campaignRepository,
            IEmailTemplateRepository emailTemplateRepository)
            : base(dbContext)
        {
            CampaignRepository = campaignRepository;
            EmailTemplateRepository = emailTemplateRepository;
            CampaignReportRepository = campaignReportRepository;

        }

        
    }
}
