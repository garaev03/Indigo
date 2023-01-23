using Indigo.Business.Services.Interfaces;

namespace Indigo.Business.Services.Implementations
{
    public class ImageService : IImageService
    {
        public void CheckImage(IFormFile file)
        {
            if (!file.ContentType.Contains("image"))
                throw new Exception("not image");
        }

        public void CheckSize(IFormFile file)
        {
            if (file.Length/1024/1024>2)
                throw new Exception("image larger than 2MB."); 
        }

        public async Task<string> CreateImageAsync(string RootPath, string FolderPath, IFormFile file)
        {
            string ImageName=Guid.NewGuid()+file.FileName;
            string FullPath = RootPath + "/" + FolderPath + ImageName;
            using (FileStream reader = new(FullPath,FileMode.Create))
            {
                await file.CopyToAsync(reader);
            }
            return ImageName;
        }
    }
}
