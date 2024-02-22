

namespace ProjectCollaborationPlatform.DAL.Data.Models
{
    public class Developer : BaseEntity
    {
        public List<DeveloperTechnology> DeveloperTechnologies {  get; set; }
        public List<ProjectDeveloper> ProjectDevelopers { get; set; }
        public List<TeamDeveloper> TeamDevelopers { get; set; }
        
    }
}
