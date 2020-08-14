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
        public static string AdminerConfigSectionName = "Administrator";
        public static string EmailSettingSectionName = "EmailSetting";
        public static string PharmacyPolicy = "PharmacyPolicy";
        public static string StockPolicy = "StockPolicy";
        public static string Stock_Or_PharmacyPolicy = "Stock_Or_PharmacyPolicy";
        public static string AdminPolicy = "AdminPolicy";
        public static string FullControlOnSubAdminsPolicy = "FullControlOnSubAdminsPolicy";
        public static string ViewAnySubAdminPolicy = "ViewAnySubAdminPolicy";
        public static string RepresentativePolicy = "RepresentativePolicy";
        public static string CanAddSubAdminPolicy = "CanAddSubAdminPolicy";
        public static string CanUpdateSubAdminPolicy = "CanUpdateSubAdminPolicy";
        public static string CanDeleteSubAdminPolicy = "CanDeleteSubAdminPolicy";
        public static string CanAddNewRepresentativePolicy = "CanAddNewRepresentativePolicy";
        public static string X_PaginationHeader = "X-Pagination";
        public static class AdminClaimsTypes 
        {
            public static string AdminType = "AdminType";
            public static string Previligs = "Previligs";

        }
        public static class ImagesPaths
        {
            public static string PharmacyLicenseImgSrc 
            {
                get 
                { 
                    return Path.Combine(RequestStaticServices.GetHostingEnv().WebRootPath, "Images", "Pharmacies","Identity", "License");
                } 
            }
            public static string PharmacyCommertialRegImgSrc
            {
                get
                {
                    return Path.Combine(RequestStaticServices.GetHostingEnv().WebRootPath, "Images", "Pharmacies", "Identity", "CommerialReg");
                }
            }
            public static string StockLicenseImgSrc
            {
                get
                {
                    return Path.Combine(RequestStaticServices.GetHostingEnv().WebRootPath, "Images", "Stocks", "Identity", "License");
                }
            }
            public static string StockCommertialRegImgSrc
            {
                get
                {
                    return Path.Combine(RequestStaticServices.GetHostingEnv().WebRootPath,"Images", "Stocks", "Identity", "CommerialReg");
                }
            }
        }
    }
}
