namespace Indigo.Business.Services.Interfaces
{
    public interface IImageService
    {
        public void CheckImage(IFormFile file);
        public void CheckSize(IFormFile file);
        public Task<string> CreateImageAsync(string RootPath, string FolderPath, IFormFile file);
    }
}
