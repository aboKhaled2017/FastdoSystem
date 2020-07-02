using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System_Back_End.Services;

namespace System_Back_End
{
    public static class Variables
    {
        public static string stocker = "stocker";
        public static string adminer="adminer";
        public static string pharmacier = "pharmacier";
        public static string corePolicy = "corePolicy";
        public static string AdminConfigSectionName = "AdminData";
        public static string EmailSettingSectionName = "EmailSetting";
        public static string PharmacyPolicy = "PharmacyPolicy";
        public static string StockPolicy = "StockPolicy";
        public static class ImagesPaths
        {
            public static string PharmacyLicenseImgSrc 
            {
                get 
                { 
                    return Path.Combine(RequestStaticServices.GetHostingEnv().ContentRootPath, "Images", "Pharmacies","Identity", "License");
                } 
            }
            public static string PharmacyCommertialRegImgSrc
            {
                get
                {
                    return Path.Combine(RequestStaticServices.GetHostingEnv().ContentRootPath, "Images", "Pharmacies", "Identity", "CommerialReg");
                }
            }
            public static string StockLicenseImgSrc
            {
                get
                {
                    return Path.Combine(RequestStaticServices.GetHostingEnv().ContentRootPath, "Images", "Stocks", "Identity", "License");
                }
            }
            public static string StockCommertialRegImgSrc
            {
                get
                {
                    return Path.Combine(RequestStaticServices.GetHostingEnv().ContentRootPath,"Images", "Stocks", "Identity", "CommerialReg");
                }
            }
        }
    }
}
