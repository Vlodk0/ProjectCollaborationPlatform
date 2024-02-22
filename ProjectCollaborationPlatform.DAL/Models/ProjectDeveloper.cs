

namespace ProjectCollaborationPlatform.DAL.Data.Models
{
    public class ProjectDeveloper
    {
        public Guid ProjectID { get; set; }
        public Project Project { get; set; }
        public Guid DeveloperID { get; set; }
        public Developer Developer { get; set; }
    }
}
