using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace The_Book.Models
{
    public class ValidatePictureAttribute: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            if (file != null)
            {
                var ext = Path.GetExtension(file.FileName);

                if (file.ContentLength > (1 * 1024 * 1024))
                {
                    return false;
                }
                if (ext != ".jpg" && !IsFileTypeValid(file))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsFileTypeValid(HttpPostedFileBase file)
        {
            try
            {
                using (var img = Image.FromStream(file.InputStream))
                {
                    if (IsOneOfValidFormats(img.RawFormat))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        private bool IsOneOfValidFormats(ImageFormat rawFormat)
        {
            List<ImageFormat> formats = getValidFormats();
            foreach(var format in formats)
            {
                if(rawFormat.Equals(format))
                {
                    return true;
                }
            }
            return false;
        }

        private List<ImageFormat> getValidFormats()
        {
            List<ImageFormat> formats = new List<ImageFormat>();
            //formats.Add(ImageFormat.Gif);
            formats.Add(ImageFormat.Png);
            formats.Add(ImageFormat.Jpeg);
            return formats;
        }
    }
}