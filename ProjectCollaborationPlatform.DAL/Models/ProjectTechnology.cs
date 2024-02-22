
namespace ProjectCollaborationPlatform.DAL.Data.Models
{
    public class ProjectTechnology
    {
        public Guid ProjectID {  get; set; }
        public Project Project { get; set; }
        public Guid TechnologyID { get; set; }
        public Technology Technology { get; set;}
    }
}
