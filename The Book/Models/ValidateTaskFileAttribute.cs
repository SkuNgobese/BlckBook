using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace The_Book.Models
{
    public class ValidateTaskFileAttribute: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            if (file != null)
            {
                string ext = Path.GetExtension(file.FileName);
                var allowedExts = new[] { ".pdf", ".docx", ".txt", ".pptx", ".vsdx", ".ppt", ".xls" };
                if (file.ContentLength <= (5 * 1024 * 1024) && (allowedExts.Contains(ext)))
                {
                    return true;
                }
            }
            if(file == null)
            {
                return true;
            }
            return false;
        }
    }
}