using System.Collections.Generic;

namespace TestFlow.DevSoftware.Authentication.Data
{
    internal class UserInfo
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public UserGroup UserGroup { get; set; }

        public string CreationTime { get; set; }
        
    }
}