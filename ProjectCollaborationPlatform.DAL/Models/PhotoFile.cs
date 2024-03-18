using ProjectCollaborationPlatform.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCollaborationPlatform.DAL.Models
{
    public class PhotoFile : BaseEntity
    {
        public string Path {  get; set; }
        public string Name {  get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
    }
}
