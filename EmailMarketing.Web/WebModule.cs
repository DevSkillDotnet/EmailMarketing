﻿using Autofac;
using EmailMarketing.Membership.Services;
using EmailMarketing.Web.Areas.Admin.Models;
using EmailMarketing.Web.Areas.Member.Models.Groups;
using EmailMarketing.Web.Areas.Member.Models;
using EmailMarketing.Web.Areas.Member.Models.ProfileModels;

using EmailMarketing.Web.Areas.Admin.Models.AdminUsers;
using EmailMarketing.Web.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailMarketing.Web.Areas.Member.Models.Smtp;
using EmailMarketing.Web.Areas.Member.Models.Contacts;
using EmailMarketing.Web.Areas.Member.Models.Campaigns;

namespace EmailMarketing.Web
{
    public class WebModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public WebModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AdminUsersModel>();
            builder.RegisterType<MemberUserModel>();
            builder.RegisterType<GroupModel>();
            builder.RegisterType<CampaignsModel>();
            builder.RegisterType<CampaignBaseModel>();
            builder.RegisterType<ContactsModel>();
            builder.RegisterType<FieldMapModel>();
            builder.RegisterType<SMTPModel>();
            builder.RegisterType<ChangeDefaultPasswordViewModel>();
            builder.RegisterType<ContactUploadModel>();
            builder.RegisterType<ViewContactUploadModel>();
            builder.RegisterType<ContactsBaseModel>();
            builder.RegisterType<MemberBaseModel>();

            base.Load(builder);
        }
    }
}  
