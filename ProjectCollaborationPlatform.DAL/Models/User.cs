﻿
namespace ProjectCollaborationPlatform.DAL.Data.Models
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
        public string RoleName { get; set; }
    }
}
