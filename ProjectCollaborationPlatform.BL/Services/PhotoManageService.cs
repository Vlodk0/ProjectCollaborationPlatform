using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Models;
using ProjectCollaborationPlatform.Domain.Helpers;

namespace ProjectCollaborationPlatform.BL.Services
{
    public class PhotoManageService : IPhotoManageService
    {
        private readonly ProjectPlatformContext _context;
        public PhotoManageService(ProjectPlatformContext context)
        {
            _context = context;
        }

        public async Task<string> UploadFile(IFormFile _formfile, Guid userId)
        {
            try
            {
                FileInfo _fileInfo = new FileInfo(_formfile.FileName);
                string FileName = Path.GetFileNameWithoutExtension(_formfile.FileName) + "_" + DateTime.Now.Ticks.ToString() + _fileInfo.Extension;
                var _getFilePath = await GetFilePath(FileName);
                using var fileStream = new FileStream(_getFilePath, FileMode.Create);
                await _formfile.CopyToAsync(fileStream);
                await CreateFile(FileName, _getFilePath, userId);
                return FileName;
            }
            catch (Exception)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Title = "Can't upload image",
                    Detail = "Error occured while uploading file on server"
                };
            }
        }

        public async Task<(byte[], string, string)> DownloadFile(string FileName)
        {
            try
            {
                var file = await _context.PhotoFiles.Where(f => f.Name == FileName).FirstOrDefaultAsync();

                var getFilePath = file.Path;
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(getFilePath, out var contentType))
                {
                    contentType = "application/octet-stream";
                }

                var readAllBytesAsync = await File.ReadAllBytesAsync(getFilePath);
                return (readAllBytesAsync, contentType, Path.Combine(getFilePath));
            }
            catch (Exception)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Title = "Can't download file",
                    Detail = "Error occured while downloading file from server"
                };
            }
        }

        private async Task<bool> CreateFile(string fileName, string _getFilePath, Guid userId)
        {
            var photo = new PhotoFile
            {
                Name = fileName,
                Path = _getFilePath,
                UserId = userId
            };
            await _context.PhotoFiles.AddAsync(photo);
            var isSaved = await SaveAsync();
            if (isSaved)
                return true;
            else
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Title = "Can't save photo",
                    Detail = "Error occured while creating photo on server"
                };
            }
        }
        private async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        private string GetStaticContentDirectory()
        {
            var result = "D:\\Exoft\\Images\\";
            if (!Directory.Exists(result))
            {
                Directory.CreateDirectory(result);
            }

            return result;
        }
        private async Task<string> GetFilePath(string fileName)
        {
            var getStaticContentDirectory = GetStaticContentDirectory();
            return Path.Combine(getStaticContentDirectory, fileName);
        }
    }
}
