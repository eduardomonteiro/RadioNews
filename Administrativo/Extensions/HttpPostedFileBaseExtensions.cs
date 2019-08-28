using System;
using System.IO;
using System.Linq;
using System.Web;

namespace System.Web.Mvc.Html
{
    public static class HttpPostedFileBaseExtensions
    {
        public static bool IsImage(this HttpPostedFileBase file)
        {
            if (file == null || string.IsNullOrEmpty(file.FileName))
                return false;

            return file.IsExtension(".jpg", ".jpeg", ".png", ".gif");
        }

        public static bool IsExtension(this HttpPostedFileBase file, params string[] extensions)
        {
            if (file == null || string.IsNullOrEmpty(file.FileName))
                return false;

            return extensions.Any(extension => file.FileName.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase));
        }

        public static string SaveAsDefaultName(this HttpPostedFileBase imageFile, string clientPath)
        {
            var exists = false;

            var filename = DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(imageFile.FileName);
            var path = Path.Combine(clientPath, filename);

            do
            {
                if (exists)
                {
                    filename = DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(imageFile.FileName);
                    path = Path.Combine(clientPath, filename);
                }

                exists = File.Exists(path);

            } while (exists);

            imageFile.SaveAs(path);

            return filename;
        }
    }
}
