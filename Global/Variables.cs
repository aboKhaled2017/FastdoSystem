using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.backendsys.Services;

namespace Fastdo.backendsys
{
    public static class Variables
    {
        public static string AdminSchemaOfAdminSite = "AdminSchemaOfAdminSite";
        public static string AdminPanelCookiePath = "AdminPanel";
        public static string stocker = "stocker";
        public static string adminer="adminer";
        public static string pharmacier = "pharmacier";
        public static string corePolicy = "corePolicy";
        public static string AdminerConfigSectionName = "Administrator";
        public static string EmailSettingSectionName = "EmailSetting";
        public static string PharmacyPolicy = "PharmacyPolicy";
        public static string StockPolicy = "StockPolicy";       
        public static string Stock_Or_PharmacyPolicy = "Stock_Or_PharmacyPolicy";
        public static string AdministratorPolicy = "AdministratorPolicy";

        public static string X_PaginationHeader = "X-Pagination";
        public static class AdminPolicies
        {
            public static string HaveFullControlPolicy = "HaveFullControlPolicy";
            public static string ControlOnPharmaciesPagePolicy = "ControlOnPharmaciesPagePolicy";
            public static string ControlOnStocksPagePolicy = "ControlOnStocksPagePolicy";
            public static string ControlOnDrugsRequestsPagePolicy = "ControlOnDrugsRequestsPagePolicy";
            public static string ControlOnAdministratorsPagePolicy = "ControlOnAdministratorsPagePolicy";
            public static string ControlOnVStockPagePolicy = "ControlOnVStockPagePolicy";
        }
        public static class AdminPanelPolicies
        {
            public static string AdminPanelAuthPolicy = "AdminPanelAuthPolicy";
            public static string AdminPanel_ForPharmaciesPage_AuthPolicy = "AdminPanel_ForPharmaciesPage_AuthPolicy";
            public static string AdminPanel_ForStocksPage_AuthPolicy = "AdminPanel_ForStocksPage_AuthPolicy";
            public static string AdminPanel_ForVStockPage_AuthPolicy = "AdminPanel_ForVStockPage_AuthPolicy";
            public static string AdminPanel_ForAdminsPage_AuthPolicy = "AdminPanel_ForAdminsPage_AuthPolicy";
            public static string AdminPanel_ForDrugsReqsPage_AuthPolicy = "AdminPanel_ForDrugsReqsPage_AuthPolicy";
        }
        public static class AdminClaimsTypes 
        {
            public static string AdminType = "AdminType";
            public static string Priviligs = "Priviligs";

        }
        public static class UserClaimsTypes
        {
            public static string UserName = "UserName";
            public static string Phone = "Phone";
            public static string UserId = "UserId";
        }
        public static class StockUserClaimsTypes
        {
            public static string PharmasClasses = "PharmasClasses";
        }
        public static class ExcelPaths
        {
            public static string StockDrugsReportFilePath
            {
                get
                {
                    return Path.Combine(RequestStaticServices.GetHostingEnv().WebRootPath, "Sheets", "Stocks", "Orders");
                }
            }
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
