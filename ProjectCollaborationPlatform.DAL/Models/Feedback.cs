using ProjectCollaborationPlatform.DAL.Data.Models;

namespace ProjectCollaborationPlatform.DAL.Models
{
    public class Feedback : BaseEntity
    {
        public string Content {  get; set; }
        public Guid ProjectOwnerID { get; set; }
        public ProjectOwner ProjectOwner { get; set; }
        public Guid DeveloperId { get; set; }
        public Developer Developer { get; set; }
    }
}
