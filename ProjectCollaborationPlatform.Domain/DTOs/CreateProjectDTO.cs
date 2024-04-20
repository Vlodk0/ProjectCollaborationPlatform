namespace ProjectCollaborationPlatform.Domain.DTOs
{
    public class ProjectFullInfoDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Payment { get; set; }
        public string Description { get; set; }
        public string ShortInfo {  get; set; }
        public string BoardName {  get; set; }
        public List<DeveloperTechnologyDTO> Technologies { get; set; }
    }
}
