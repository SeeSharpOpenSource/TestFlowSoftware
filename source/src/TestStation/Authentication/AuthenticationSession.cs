using System;
using System.Collections.Generic;

namespace TestStation.Authentication
{
    public sealed class AuthenticationSession
    {
        public string UserName { get; }

        public UserGroup UserGroup { get; }

        internal string CreationTime { get; }

        public IList<string> Authorities { get; internal set; }

        public DateTime LogInTime { get; set; }

        public DateTime LogOutTime { get; set; }

        internal AuthenticationSession(string userName, UserGroup userGroup, string creationTime)
        {
            this.UserGroup = userGroup;
            this.UserName = userName;
            this.CreationTime = creationTime;
        }

        public void CheckAuthority(string authorityName)
        {
            if (!Authorities.Contains(authorityName))
            {
                throw new AuthenticationException($"Authority <{authorityName}> has not been authorized to current user.");
            }
        }

        public bool HasAuthority(string authorityName)
        {
            return Authorities.Contains(authorityName);
        }
    }
}
