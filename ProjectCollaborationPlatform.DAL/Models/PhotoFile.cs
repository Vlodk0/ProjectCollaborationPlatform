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
        public Developer Developer { get; set; }
        public Guid? DeveloperId { get; set; }
        public ProjectOwner ProjectOwner { get; set; }
        public Guid? ProjectOwnerId { get; set; }
    }
}
