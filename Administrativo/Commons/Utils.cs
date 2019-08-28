using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Omu.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Administrativo.Commons
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new
                 RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
        }
    }

    public static class Utils
    {
        public static string GenerateSlug(this string phrase)
        {
            string str = phrase.RemoveAccent().ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        public static string SaveFile(string path, HttpPostedFile file, string mantemNome = null)
        {
            string pathSave = path;
            string fileName = Path.GetFileNameWithoutExtension(file.FileName).GenerateSlug() + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
            string ext = Path.GetExtension(file.FileName);

            fileName = string.IsNullOrEmpty(mantemNome) ? GetFileName(fileName + ext, pathSave) : mantemNome;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            file.SaveAs(pathSave + fileName);
            return fileName;
            //try
            //{
            //    file.SaveAs(pathSave + fileName);
            //    return fileName;
            //}
            //catch
            //{
            //    return "";
            //}

        }

        public static string SaveFileBase(string path, HttpPostedFileBase file, string mantemNome = null)
        {
            string pathSave = path;
            string fileName = (Path.GetFileNameWithoutExtension(file.FileName).GenerateSlug() + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss")).Substring(0,20);
            string ext = Path.GetExtension(file.FileName);

            fileName = string.IsNullOrEmpty(mantemNome) ? GetFileName(fileName + ext, pathSave) : mantemNome;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            file.SaveAs(pathSave + fileName);
            return fileName;
            //try
            //{
            //    file.SaveAs(pathSave + fileName);
            //    return fileName;
            //}
            //catch
            //{
            //    return "";
            //}

        }

        public static string SaveAndCropColunista(HttpPostedFileBase image, string path, int cropX = 0, int cropY = 0, int cropWidth = 0, int cropHeight = 0, bool keepFileName = false, string fileNameKeep = "")
        {
            var fileName = keepFileName ? (!string.IsNullOrEmpty(fileNameKeep) ? fileNameKeep : GetFileName(image.FileName, path)) : GetFileName(image.FileName, path);
            var mimeType = MimeMapping.GetMimeMapping(fileName);
            using (var origem = Image.FromStream(image.InputStream, true, true))
            {
                //var media = Imager.Resize(origem, 620, 200, true);
                Image media = null;

                int startX = cropX;
                int startY = cropY;

                media = Imager.Crop(origem, new Rectangle(startX, startY, cropWidth, cropHeight));

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var imageCodecInfo = Imager.GetEncoderInfo(mimeType);

                // Imager.SaveJpeg(path + fileName, media != null ? media : origem);
                Imager.Save(path + fileName, media != null ? media : origem, imageCodecInfo);

            }

            return fileName;

        }

        public static string SaveAndCropImage(HttpPostedFileBase image, string path, int cropX = 0, int cropY = 0, int cropWidth = 0, int cropHeight = 0, bool keepFileName = false, string fileNameKeep = "")
        {
            var fileName = keepFileName ? (!string.IsNullOrEmpty(fileNameKeep) ? fileNameKeep : GetFileName(image.FileName, path)) : GetFileName(image.FileName, path);
            var mimeType = MimeMapping.GetMimeMapping(fileName);
            using (var origem = Image.FromStream(image.InputStream, true, true))
            {
                //var media = Imager.Resize(origem, 620, 200, true);
                Image media = null;


                double croppedHeightToWidth = (double)cropHeight / cropWidth;
                double croppedWidthToHeight = (double)cropWidth / cropHeight;
                int startX = cropX;
                int startY = cropY;


                #region
                if (origem.Width > cropWidth && origem.Height > cropHeight)
                {
                    if (cropWidth > 0 && cropHeight > 0)
                    {

                        //try to get the center of image
                        #region GetCenter
                        if (origem.Width > origem.Height)
                        {

                            cropWidth = (int)(Math.Round(origem.Height * croppedWidthToHeight));

                            if (cropWidth < origem.Width)
                            {
                                cropHeight = origem.Height;
                                startX = (origem.Width - cropWidth) / 2;
                            }
                            else
                            {

                                cropHeight = (int)Math.Round(origem.Height * ((double)origem.Width / cropWidth));
                                cropWidth = origem.Width;
                                startY = (origem.Height - cropHeight) / 2;
                            }

                        }
                        else
                        {
                            cropHeight = (int)(Math.Round(origem.Width * croppedHeightToWidth));
                            if (cropHeight < origem.Height)
                            {
                                cropWidth = origem.Width;
                                startY = (origem.Height - cropHeight) / 2;
                            }
                            else
                            {
                                cropWidth = (int)Math.Round(origem.Width * ((double)origem.Height / cropHeight));
                                cropHeight = origem.Height;
                                startX = (origem.Width - cropWidth) / 2;
                            }
                        }
                        #endregion


                    }
                    media = Imager.Crop(origem, new Rectangle(startX, startY, cropWidth, cropHeight));
                }
                #endregion


                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var imageCodecInfo = Imager.GetEncoderInfo(mimeType);

                // Imager.SaveJpeg(path + fileName, media != null ? media : origem);
                Imager.Save(path + fileName, media != null ? media : origem, imageCodecInfo);

            }

            return fileName;

        }

        public static string resizeImageAndSave(string imagePath, int largura, int altura, string newPath = "")
        {
            Image fullSizeImg = Image.FromFile(imagePath);
            var thumbnailImg = new Bitmap(largura, altura);

           // var mimeType = MimeMapping.GetMimeMapping(imagePath);

            var thumbGraph = Graphics.FromImage(thumbnailImg);
            thumbGraph.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            thumbGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            thumbGraph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            var imageRectangle = new Rectangle(0, 0, largura, altura);
            thumbGraph.DrawImage(fullSizeImg, imageRectangle);

            string targetPath = imagePath.Replace(Path.GetFileNameWithoutExtension(imagePath), Path.GetFileNameWithoutExtension(imagePath));

            targetPath = string.IsNullOrEmpty(newPath) ? targetPath : newPath + Path.GetFileName(imagePath);

            thumbnailImg.Save(Path.GetFullPath(targetPath), ImageFormat.Jpeg);
            thumbnailImg.Dispose();
            return Path.GetFileName(targetPath);
        }
        
        public static string resizeImageAndSave3(string imagePath, int largura, int altura, string newPath = "")
        {
            var fullSizeImg = Image.FromFile(imagePath);
            var thumbnailImg = new Bitmap(largura, altura);
            var ext = Path.GetExtension(imagePath);
            var encoder = GetEncoder(ImageFormat.Jpeg);
            if (newPath.Contains("Banners"))
            {
                encoder = GetEncoder(ImageFormat.Png);
                newPath = newPath.Replace(".jpg", ".png");
                imagePath = imagePath.Replace(".jpg", ".png");
            }
            var myEncoder = Encoder.Quality;
            string targetPath;
            using (var myEncoderParameters = new EncoderParameters(1))
            {
                var myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                thumbGraph.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                thumbGraph.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                var imageRectangle = new Rectangle(0, 0, largura, altura);
                thumbGraph.DrawImage(fullSizeImg, imageRectangle);
                targetPath = imagePath.Replace(Path.GetFileNameWithoutExtension(imagePath), Path.GetFileNameWithoutExtension(imagePath));
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                targetPath = string.IsNullOrEmpty(newPath) ? targetPath : newPath + Path.GetFileName(imagePath);
                thumbnailImg.Save(Path.GetFullPath(targetPath), encoder, myEncoderParameters);
            }
            thumbnailImg.Dispose();
            return Path.GetFileName(targetPath);
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }

        public static string resizeImageAndSave2(HttpPostedFileBase Imagem, int width, int heigth, string path)
        {
            using (var origem = Image.FromStream(Imagem.InputStream, true, true))
            {
                var mimeType = MimeMapping.GetMimeMapping(Imagem.FileName);

                var media = Imager.Resize(origem, width, heigth, true);

                Imager.Save(path + Imagem.FileName, media != null ? media : origem, Imager.GetEncoderInfo(mimeType));

                return Imagem.FileName;

            }
        }

        private static string GetFileName(string fileName, string path)
        {
            string nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            string ext = Path.GetExtension(fileName);
            string ret = File.Exists(path + fileName) ? GetFileName(nameWithoutExt + "_" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") + ext, path) : nameWithoutExt + ext;

            return ret;
        }

        public static bool VerifyExtensions(List<string> extensionsPermits, HttpPostedFile file)
        {
            string extFile = Path.GetExtension(file.FileName);

            return extensionsPermits.Contains(extFile.Replace(".", "")) ? true : false;
        }

        public static bool VerifyExtensions(List<string> extensionsPermits, string file)
        {
            string extFile = Path.GetExtension(file);

            return extensionsPermits.Contains(extFile.Replace(".", "")) ? true : false;
        }

        public static ImageFormat getImageFormat(string extension)
        {
            switch(extension.ToLower())
            {
                case ".jpeg":
                case ".jpg":
                    return ImageFormat.Jpeg;
                case ".png":
                    return ImageFormat.Png;
                case ".gif":
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Jpeg;
            }
            
        }

        public static List<string> ExtractZipFile(string archiveFilenameIn, string outFolder, List<FolderGalery> folders = null)
        {
            ZipFile zf = null;
            List<string> files = new List<string>();
            try
            {
                FileStream fs = File.OpenRead(archiveFilenameIn);
                zf = new ZipFile(fs);

                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue;           // Ignore directories
                    }

                    string fileName = Path.GetFileNameWithoutExtension(zipEntry.Name).GenerateSlug() + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
                    string ext = Path.GetExtension(zipEntry.Name);

                    System.String entryFileName = fileName + ext;
                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.

                    byte[] buffer = new byte[4096];     // 4K is optimum
                    Stream zipStream = zf.GetInputStream(zipEntry);

                    // Manipulate the output filename here as desired.
                    System.String fullZipToPath = Path.Combine(outFolder, entryFileName);
                    string directoryName = Path.GetDirectoryName(fullZipToPath);
                    if (directoryName.Length > 0)
                        Directory.CreateDirectory(directoryName);

                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    // The "using" will close the stream even if an exception occurs.
                    using (FileStream streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                    string folderFullPath;
                    foreach (FolderGalery folder in folders)
                    {
                        folderFullPath = folder.FolderPath;

                        if (!Directory.Exists(folderFullPath))
                        {
                            Directory.CreateDirectory(folderFullPath);
                        }

                        //só redimensiono se for mesmo uma uma imagem.
                        List<string> extensoes = new List<string>();
                        extensoes.Add("jpg");
                        extensoes.Add("jpeg");
                        extensoes.Add("png");

                        if (Utils.VerifyExtensions(extensoes, entryFileName))
                        {

                            AMImageLIB.Imagem amlib = new AMImageLIB.Imagem(outFolder + entryFileName);
                            amlib.redimensionar(folder.Width, folder.Height, true, true);
                            amlib.salvar(folderFullPath + entryFileName);
                            // WebRocket.Images.Manipulation.Resize(outFolder + entryFileName, folderFullPath + entryFileName, folder.Width, folder.Height, false);
                            files.Add(entryFileName);
                        }

                    }

                }
                return files;
            }
            catch
            {
                throw new ApplicationException("Ocorreu um erro com o zip, tente novamente.");
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }
        }

        public static string GetActive(string currentController)
        {
            var controllers = currentController.Split(',');
            return controllers.Contains(HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString()) ?
                "active" : "";
        }

        public static string GetActive(string currentController, string currentActions)
        {
            var actions = currentController.Split(',');
            return currentController.Contains(HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString()) ?
                "active" : "";
        }



        public static string WeekDayName(this int weekDay)
        {
            var diaExtenso = new CultureInfo("pt-BR").DateTimeFormat.GetDayName((DayOfWeek)weekDay);
            return diaExtenso;
        }

        public static string MonthName(this int weekMonth)
        {
            var mesExtenso = new CultureInfo("pt-BR").DateTimeFormat.GetAbbreviatedMonthName(weekMonth);
            return mesExtenso;
        }


        public static string WeekDayName(this DayOfWeek weekDay)
        {
            var diaExtenso = new CultureInfo("pt-BR").DateTimeFormat.GetDayName(weekDay);
            return diaExtenso;
        }

        public static MvcHtmlString CutString(this HtmlHelper html, string text, int length, string label)
        {
            return new MvcHtmlString(text.Length > length ? text.Remove(length) + label : text);
        }


    }
}