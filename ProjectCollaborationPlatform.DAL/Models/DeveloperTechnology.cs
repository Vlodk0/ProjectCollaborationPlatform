

namespace ProjectCollaborationPlatform.DAL.Data.Models
{
    public class DeveloperTechnology
    {
        public Guid DeveloperID { get; set; }
        public Developer Developer { get; set; }
        public Guid TechnologyID { get; set; }
        public Technology Technology { get; set;}
    }
}
