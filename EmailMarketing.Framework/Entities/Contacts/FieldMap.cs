﻿using EmailMarketing.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmailMarketing.Framework.Entities.Contacts
{
    public class FieldMap : IAuditableEntity<int>
    {
        public Guid? UserId { get; set; }
        public string DisplayName { get; set; }
        public bool IsStandard { get; set; }
    }
}
