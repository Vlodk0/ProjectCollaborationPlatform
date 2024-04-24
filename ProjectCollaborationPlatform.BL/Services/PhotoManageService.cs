using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Data.Models;
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
                await GetUser(userId, FileName, _getFilePath);
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

                if (file == null)
                {
                    throw new CustomApiException()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Title = "File Not found",
                        Detail = "File is null"
                    };
                }

                var getFilePath = file.Path;
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(getFilePath, out var contentType))
                {
                    contentType = "application/octet-stream";
                }

                var readAllBytesAsync = await File.ReadAllBytesAsync(getFilePath);
                return (readAllBytesAsync, contentType, Path.Combine(getFilePath));
            }
            catch (Exception e)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Title = "Can't download file",
                    Detail = "Error occured while downloading file from server"
                };
            }
        }

        private async Task<bool> GetUser(Guid id, string fileName, string _getFilePath)
        {
            var dev = await _context.Developers.FirstOrDefaultAsync(d => d.Id == id);

            if (dev == null)
            {
                var projectOwner = await _context.ProjectOwners.FirstOrDefaultAsync(p => p.Id == id);

                if (projectOwner == null)
                {
                    throw new CustomApiException
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Title = "Entity not found",
                        Detail = $"No Developer or ProjectOwner found with ID: {id}"
                    };
                }
                else
                {
                    return await CreateFile(fileName, _getFilePath, projectOwner);
                }
            }

            else
            {
               return await CreateFile(fileName, _getFilePath, dev);
            }

        }

        private async Task<bool> CreateFile(string fileName, string _getFilePath, Developer dev)
        {
            var photo = new PhotoFile
            {
                Name = fileName,
                Path = _getFilePath,
            };
            await _context.PhotoFiles.AddAsync(photo);
            var isSaved = await SaveAsync();
            if (isSaved)
            {
                var img = await _context.PhotoFiles.Where(n => n.Name == photo.Name).FirstOrDefaultAsync();
                dev.PhotoFileId = img.Id;
                return await SaveAsync();
            }
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

        private async Task<bool> CreateFile(string fileName, string _getFilePath, ProjectOwner projOwner)
        {
            var photo = new PhotoFile
            {
                Name = fileName,
                Path = _getFilePath,
            };
            await _context.PhotoFiles.AddAsync(photo);
            var isSaved = await SaveAsync();
            if (isSaved)
            {
                var img = await _context.PhotoFiles.Where(n => n.Name == photo.Name).FirstOrDefaultAsync();
                projOwner.PhotoFileId = img.Id;
                
                return await SaveAsync();
            }
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
