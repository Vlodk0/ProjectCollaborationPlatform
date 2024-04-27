using Microsoft.AspNetCore.Http;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public interface IPhotoManageService
    {
        Task<string> UploadFile(IFormFile _formfile, Guid userId);
        Task<(byte[], string, string)> DownloadFile(string FileName);
    }
}
