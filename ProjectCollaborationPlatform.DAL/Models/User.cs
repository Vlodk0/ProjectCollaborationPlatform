
namespace ProjectCollaborationPlatform.DAL.Data.Models
{
    public class User : BaseEntity
    {
        public string Name {  get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        public string RoleName { get; set; }

    }
}
