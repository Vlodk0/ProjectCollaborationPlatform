

namespace ProjectCollaborationPlatform.DAL.Data.Models
{
    public class Task : BaseEntity
    {
        public string Descripion {  get; set; }
        public Guid FunctionalityBlockID { get; set; }
        public FunctionalityBlock FunctionalityBlock { get; set; }
    }
}
    