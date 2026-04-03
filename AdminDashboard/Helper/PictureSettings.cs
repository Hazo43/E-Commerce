namespace AdminDashboard.Helper
{
    public class PictureSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            // folderPath دا بيكون ال
            // معناها انو اشتغل علي الابلكيشن دا او المشروع دا GetCurrentDirectory دي
            //wwwroot جوا ال images اسمو folder هتعمل "wwwroot\\images" دي
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", folderName);

            // لو موجود اشتغل عليه Create مش موجود روح اعملو folderPath لو ال
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Guid.NewGuid() يكون مش متكرر عشان كدا استخدمنا fileName دي بتعمل
            var fileName = Guid.NewGuid() + file.FileName;

            // wwwroot و هيظهر في ال fileName مع ال folderPath هجمع ال filePath عشان اكون ال
            var filePath = Path.Combine(folderPath, fileName);

            var fs = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fs);

            return Path.Combine("images//products", fileName);
        }

        // هنمسح الصوره بتاعتو product دي عشان لو هنمسح ال
        public static void DeleteFile(string folderName, string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", folderName, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);

        }
    }
}
