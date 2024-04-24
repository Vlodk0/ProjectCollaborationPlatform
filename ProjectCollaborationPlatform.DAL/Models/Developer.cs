

using ProjectCollaborationPlatform.DAL.Models;

namespace ProjectCollaborationPlatform.DAL.Data.Models
{
    public class Developer : BaseEntity
    {
        public string FirstName {  get; set; }
        public string LastName {  get; set; }
        public bool IsDeleted { get; set; }
        public string Email {  get; set; }
        public PhotoFile? PhotoFile { get; set; }
        public Guid? PhotoFileId { get; set; }
        public List<DeveloperTechnology> DeveloperTechnologies {  get; set; }
        public List<ProjectDeveloper> ProjectDevelopers { get; set; }
        public List<Feedback> Feedbacks { get; set; }
        
    }
}
