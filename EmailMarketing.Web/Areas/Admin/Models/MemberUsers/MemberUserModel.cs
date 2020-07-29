﻿using Autofac;
using EmailMarketing.Framework.Services;
using EmailMarketing.Membership.Constants;
using EmailMarketing.Membership.Entities;
using EmailMarketing.Membership.Enums;
using EmailMarketing.Membership.Services;
using EmailMarketing.Web.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Admin.Models
{
    public class MemberUserModel :AdminBaseModel
    {
        private readonly AppSettings _appSettings;
        private readonly IApplicationUserService _applicationUserService;

        public MemberUserModel()
        {
            _applicationUserService = Startup.AutofacContainer.Resolve<IApplicationUserService>();
            _appSettings = Startup.AutofacContainer.Resolve<IOptions<AppSettings>>().Value;
        }
   
        public MemberUserModel(IApplicationUserService applicationUserService, IOptions<AppSettings> appSettings)
        {
            _applicationUserService = applicationUserService;
            _appSettings = appSettings.Value;
        }

        public async Task<object> GetAllAsync(DataTablesAjaxRequestModel tableModel)
        {
            var result = await _applicationUserService.GetAllMemberAsync(
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "UserName","Email" }),
                tableModel.PageIndex, tableModel.PageSize);

            return new
            {
                recordsTotal = result.Total,
                recordsFiltered = result.TotalFilter,
                data = (from item in result.Items
                        select new string[]
                        {
                            item.UserName,
                            item.FullName,
                            item.Email,
                            item.EmailConfirmed ? "Yes" : "No",
                            item.PhoneNumber,
                            item.IsBlocked ? "Yes" : "No",
                            item.Id.ToString()
                        }).ToArray()

            };
        }

        public async Task<string> DeleteAsync(Guid id)
        {
            var name = await _applicationUserService.DeleteAsync(id);
            return name;
        }

        public async Task<string> ResetPasswordAsync(Guid id)
        {
            var name = await _applicationUserService.ResetPasswordAsync(id, _appSettings.UserDefaultPassword);
            return name;
        }

        public async Task<(string Name, bool IsBlocked)> BlockUnblockAsync(Guid id)
        {
            var user = await _applicationUserService.BlockUnblockAsync(id);
            return user;
        }
    }
}
