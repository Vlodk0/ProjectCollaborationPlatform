namespace ProjectCollaborationPlatform.Domain.DTOs
{
    public class PaginationDeveloperDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<DeveloperTechnologyDTO> Technologies {  get; set; }
    }
}
