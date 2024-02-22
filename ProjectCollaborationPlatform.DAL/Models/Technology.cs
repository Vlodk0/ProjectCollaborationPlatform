

namespace ProjectCollaborationPlatform.DAL.Data.Models
{
    public class Technology : BaseEntity
    {
        public string Framework { get; set; }
        public string Language { get; set; }
        public List<DeveloperTechnology> DeveloperTechnologies { get; set; }
        public List<ProjectTechnology> ProjectTechnologies { get; set; }
    }
}
