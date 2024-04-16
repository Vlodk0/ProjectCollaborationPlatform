using ProjectCollaborationPlatform.Domain.Enums;

namespace ProjectCollaborationPlatform.Domain.DTOs
{
    public class FunctionalityBlockDTO
    {
        public Guid Id { get; set; }
        public string Task {  get; set; }
        public StatusEnum Status { get; set; }
    }
}
