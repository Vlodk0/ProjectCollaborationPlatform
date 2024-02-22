
namespace ProjectCollaborationPlatform.DAL.Data.Models
{
    public class Team : BaseEntity
    {
        public List<TeamDeveloper> TeamDevelopers { get; set; }
        public int Members {  get; set; }
    }
}
