﻿using EmailMarketing.Data;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailMarketing.Framework.Repositories.Contacts
{
    public class DownloadQueueSubEntityRepository : Repository<DownloadQueueSubEntity, int, FrameworkContext>, IDownloadQueueSubEntityRepository
    {
        public DownloadQueueSubEntityRepository(FrameworkContext dbContext)
           : base(dbContext)
        {

        }
    }
}
