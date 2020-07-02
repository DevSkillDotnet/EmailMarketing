﻿using EmailMarketing.Membership.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailMarketing.Membership.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ImageUrl { get; set; }

        public string LastPassword { get; set; }
        public DateTime? LastPassChangeDate { get; set; }
        public int PasswordChangedCount { get; set; }
        public EnumApplicationUserStatus Status { get; set; }

        public Guid? CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public IList<UserRole> UserRoles { get; set; }

        public ApplicationUser()
                    : base()
        {
            this.IsActive = true;
            this.IsDeleted = false;
            this.UserRoles = new List<UserRole>();
        }

        internal ApplicationUser(string userName)
            : base(userName)
        {

        }

        public ApplicationUser(string userName, string mobileNumber, string email)
            : base(userName)
        {
            Email = email;
        }

        public ApplicationUser(string userName, string fullName, string mobileNumber, string email)
            : this(userName, mobileNumber, email)
        {
            FullName = fullName;
        }

        public ApplicationUser(string userName, string fullName, string mobileNumber, string phone, string email)
            : this(userName, fullName, mobileNumber, email)
        {
            PhoneNumber = phone;
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            var hashCode = -582740416;
            hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(UserName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Email);
            return hashCode;
        }
    }
}