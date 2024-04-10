namespace ProjectCollaborationPlatform.Domain.DTOs
{
    public class PaginationDeveloperDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<DeveloperTechnologyDTO> Technologies {  get; set; }
    }
}
