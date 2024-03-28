

namespace ProjectCollaborationPlatform.DAL.Data.Models
{
    public class Task : BaseEntity
    {
        public string Description {  get; set; }
        public Guid FunctionalityBlockID { get; set; }
        public FunctionalityBlock FunctionalityBlock { get; set; }
    }
}
    