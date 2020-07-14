using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Models;
using System.Threading.Tasks;

namespace System_Back_End.Services
{
    public class HandlingImgResponse
    {
        public bool Status { get; set; } = false;
        public string errorMess { get; set; }
        public string LicenseImgPath { get; set; }
        public string CommertialRegImgPath { get; set; }

    }
    public class HandlingProofImgsServices
    {
        private static readonly IHostingEnvironment _env = RequestStaticServices.GetHostingEnv();
        public HandlingImgResponse SavePharmacyProofImgs(IFormFile licenseImg,IFormFile commerialRegImg,object Id)
        {
            var response = new HandlingImgResponse();
            if (licenseImg == null || commerialRegImg == null || licenseImg.Length==0 || commerialRegImg.Length == 0)
            {
                return response;
            }
            string licenseImgExt = Path.GetExtension(licenseImg.FileName);
            string commerialRegImgExt = Path.GetExtension(commerialRegImg.FileName);
            var supportedTypes = new string[] { "png", "jpg", "jpeg", "gif", "PNG", "JPG", "GIF", "JPEG" };
            //not valid extension
            if (!supportedTypes.Contains(licenseImgExt.Replace(".", string.Empty))
                ||
                !supportedTypes.Contains(commerialRegImgExt.Replace(".", string.Empty))
                ) return response;
            try
            { //delete old image if exists
                var file1 = licenseImg.OpenReadStream();
                if (file1.Length > 0)
                {
                    response.LicenseImgPath = Variables.ImagesPaths.PharmacyLicenseImgSrc+ $@"/{Id}.{licenseImgExt}";
                    if (File.Exists(response.LicenseImgPath))
                       File.Delete(response.LicenseImgPath);
                    using (FileStream fs = System.IO.File.Create(response.LicenseImgPath))
                    {
                        file1.CopyTo(fs);
                        fs.Flush();

                    }
                }
                var file2 = commerialRegImg.OpenReadStream();
                if (file2.Length > 0)
                {
                    response.CommertialRegImgPath = Variables.ImagesPaths.PharmacyCommertialRegImgSrc + $@"/{Id}.{licenseImgExt}";
                    if (File.Exists(response.CommertialRegImgPath))
                        File.Delete(response.CommertialRegImgPath);
                    using (FileStream fs = System.IO.File.Create(response.CommertialRegImgPath))
                    {
                        file2.CopyTo(fs);
                        fs.Flush();

                    }
                }
            }
            catch (Exception ex)
            {
                response.errorMess = ex.Message;
                return response;
            }
            response.Status = true;
            return response;
        }
        public HandlingImgResponse SaveStockProofImgs(IFormFile licenseImg, IFormFile commerialRegImg, object Id)
        {
            var response = new HandlingImgResponse();
            if (licenseImg == null || commerialRegImg == null || licenseImg.Length == 0 || commerialRegImg.Length == 0)
            {
                response.errorMess = "in valid request";
                return response;
            }
            string licenseImgExt = Path.GetExtension(licenseImg.FileName);
            string commerialRegImgExt = Path.GetExtension(commerialRegImg.FileName);
            var supportedTypes = new string[] { "png", "jpg", "jpeg", "gif", "PNG", "JPG", "GIF", "JPEG" };
            //not valid extension
            if (!supportedTypes.Contains(licenseImgExt.Replace(".", string.Empty))
                ||
                !supportedTypes.Contains(commerialRegImgExt.Replace(".", string.Empty))
                )
            {
                response.errorMess = "not supported extension";
                return response;
            }
            try
            { //delete old image if exists
                var file1 = licenseImg.OpenReadStream();
                if (file1.Length > 0)
                {
                    response.LicenseImgPath = Variables.ImagesPaths.StockLicenseImgSrc + $@"/{Id}{licenseImgExt}";
                    if (File.Exists(response.LicenseImgPath))
                        File.Delete(response.LicenseImgPath);
                    using (FileStream fs = System.IO.File.Create(response.LicenseImgPath))
                    {
                        file1.CopyTo(fs);
                        fs.Flush();

                    }
                }
                var file2 = commerialRegImg.OpenReadStream();
                if (file2.Length > 0)
                {
                    response.CommertialRegImgPath = Variables.ImagesPaths.StockCommertialRegImgSrc + $@"/{Id}{licenseImgExt}";
                    if (File.Exists(response.CommertialRegImgPath))
                        File.Delete(response.CommertialRegImgPath);
                    using (FileStream fs = System.IO.File.Create(response.CommertialRegImgPath))
                    {
                        file2.CopyTo(fs);
                        fs.Flush();

                    }
                }
            }
            catch (Exception ex)
            {
                response.errorMess = ex.Message;
                return response;
            }
            response.Status = true;
            return response;
        }
        public void DeleteImg(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }
        public void DeleteImgs(params string[] paths)
        {
            Array.ForEach(paths, path =>
            {
                if (File.Exists(path))
                    File.Delete(path);
            });
        }
        public bool EditImg(IFormFile imgFile,string oldPath)
        {
            if (imgFile == null || imgFile.Length == 0)
            {
                return false;
            }
            string imgFileExt = Path.GetExtension(imgFile.FileName);
            var supportedTypes = new string[] { "png", "jpg", "jpeg", "gif", "PNG", "JPG", "GIF", "JPEG" };
            //not valid extension
            if (!supportedTypes.Contains(imgFileExt.Replace(".", string.Empty))) return false;
            try
            { //delete old image if exists
                var file = imgFile.OpenReadStream();
                if (file.Length > 0)
                {
                    string newPath = oldPath.Split('.')[0] + $".{imgFileExt}";
                    if (File.Exists(newPath))
                        File.Delete(newPath);
                    using (FileStream fs = System.IO.File.Create(newPath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();

                    }
                }
                
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

    }
}
