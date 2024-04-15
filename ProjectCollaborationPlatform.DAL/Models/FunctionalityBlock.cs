

namespace ProjectCollaborationPlatform.DAL.Data.Models
{
    public class FunctionalityBlock : BaseEntity
    {
        public string Task  { get; set; }
        public Guid BoardID { get; set; }
        public Board Board { get; set; }    
    }
}
