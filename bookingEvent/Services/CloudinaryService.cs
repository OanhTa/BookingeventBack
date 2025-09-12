using bookingEvent.Data;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;


namespace bookingEvent.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ApplicationDbContext _context;

        public CloudinaryService(IConfiguration config, ApplicationDbContext context)
        {
            var account = new Account(
                config["Cloudinary:CloudName"],
                config["Cloudinary:ApiKey"],
                config["Cloudinary:ApiSecret"]
            );
            _cloudinary = new Cloudinary(account);
            _context = context;
        }

        public async Task<bool> UploadImageAsync(IFormFile file, Guid userId)
        {
            if (file == null || file.Length == 0)
                return false;

            //if (!string.IsNullOrEmpty(user.AvatarUrl))
            //{
            //    // Lấy public_id từ URL
            //    var publicId = GetPublicIdFromUrl(user.AvatarUrl);
            //    var deletionParams = new DeletionParams(publicId);
            //    await _cloudinary.DestroyAsync(deletionParams);
            //}

            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "uploads"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                return false; 

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false; 

            user.AvatarUrl = uploadResult.SecureUrl.ToString();
            await _context.SaveChangesAsync();

            return true;
        }
        private string GetPublicIdFromUrl(string url)
        {
            var uri = new Uri(url);
            var segments = uri.AbsolutePath.Split('/');
            var folder = segments[segments.Length - 2];
            var filename = Path.GetFileNameWithoutExtension(segments[^1]);
            return $"{folder}/{filename}";
        }


    }
}
