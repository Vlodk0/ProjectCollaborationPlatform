

namespace ProjectCollaborationPlatform.DAL.Data.Models
{
    public class Project : BaseEntity
    {
        public string Title { get; set; }
        public int Payment {  get; set; }
        public Board Board { get; set; }
        public Guid ProjectDetailID {  get; set; }
        public ProjectDetail ProjectDetail {  get; set; }
        public List<ProjectDeveloper> ProjectDevelopers { get; set; }
        public List<ProjectTechnology> ProjectTechnologies { get; set; }
        public Guid ProjectOwnerID { get; set; }
        public ProjectOwner ProjectOwner { get; set; }
    }
}
