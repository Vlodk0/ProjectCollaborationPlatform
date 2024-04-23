

namespace ProjectCollaborationPlatform.Domain.DTOs
{
    public class DeveloperDTO
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public Guid Id { get; set; }
        public string Email { get; set; }
        public bool IsDeleted {  get; set; }
        public string RoleName {  get; set; }
    }
}
