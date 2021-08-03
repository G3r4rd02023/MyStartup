using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MyStartup.Helpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAsync(IFormFile imageFile);
    }
}
