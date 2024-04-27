namespace ProjectCollaborationPlatform.Domain.DTOs
{
    public class GetProjectDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Payment { get; set; }
        public string Description { get; set; }
        public Guid BoardId { get; set; }
        public List<DeveloperTechnologyDTO> Technologies { get; set; }
        public List<DeveloperDTO> Developers { get; set; }
    }
}
