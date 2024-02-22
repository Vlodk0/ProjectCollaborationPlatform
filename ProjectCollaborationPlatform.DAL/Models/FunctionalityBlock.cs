

namespace ProjectCollaborationPlatform.DAL.Data.Models
{
    public class FunctionalityBlock : BaseEntity
    {
        public string Name { get; set; }
        public List<Models.Task> Tasks { get; set; }    
        public Guid BoardID { get; set; }
        public Board Board { get; set; }    
    }
}
