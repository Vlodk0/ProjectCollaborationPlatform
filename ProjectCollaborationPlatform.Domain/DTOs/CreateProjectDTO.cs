namespace ProjectCollaborationPlatform.Domain.DTOs
{
    public class CreateProjectDTO
    {
        public string Title {  get; set; }
        public string ShortInfo {  get; set; }
        public int Payment {  get; set; }
        public string Description {  get; set; }
        public string BoardName {  get; set; }
    }
}
