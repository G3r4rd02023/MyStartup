using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MyStartup.Helpers
{
    public class ImageHelper : IImageHelper
        {
            public async Task<string> UploadImageAsync(IFormFile imageFile)
            {
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot\\images\\App",
                    file);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return $"~/images/App/{file}";
            }
        }

}
