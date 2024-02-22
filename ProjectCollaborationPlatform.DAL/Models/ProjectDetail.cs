

namespace ProjectCollaborationPlatform.DAL.Data.Models
{
    public class ProjectDetail : BaseEntity
    {
        public Guid ProjectID { get; set; }
        public string Description { get; set; }
        public Project Project { get; set; }
    }
}
