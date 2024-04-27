

using ProjectCollaborationPlatform.Domain.Enums;

namespace ProjectCollaborationPlatform.DAL.Data.Models
{
    public class FunctionalityBlock : BaseEntity
    {
        public string Task  { get; set; }
        public StatusEnum Status {  get; set; }
        public Guid BoardID { get; set; }
        public Board Board { get; set; }    
    }
}
