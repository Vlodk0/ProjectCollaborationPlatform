

namespace ProjectCollaborationPlatform.DAL.Data.Models
{
    public class TeamDeveloper
    {
        public Guid DeveloperID { get; set; }
        public Developer Developer { get; set; }
        public Guid TeamID { get; set; }
        public Team Team { get; set; }
    }
}
