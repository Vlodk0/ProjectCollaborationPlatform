

using ProjectCollaborationPlatform.DAL.Models;

namespace ProjectCollaborationPlatform.DAL.Data.Models
{
    public class ProjectOwner : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDeleted { get; set; }
        public string Email { get; set; }
        public PhotoFile PhotoFile { get; set; }
        public List<Project> Projects { get; set; }
        public List<Feedback> Feedbacks { get; set; }
    }
}
