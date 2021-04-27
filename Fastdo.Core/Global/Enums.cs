using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.Core.Enums
{
    public enum RegisterType
    {
        AsPharmacy,
        AsStock
    }
    public enum EUserType
    {
        Pharmacy=1,
        Stock=2,
        Admin=3
    }
    public enum PharmacyRequestStatus
    {
        Pending ,
        Accepted ,
        Rejected ,
        Disabled 
    }
    public enum StockRequestStatus
    {
        Pending ,
        Accepted ,
        Rejected ,
        Disabled 
    }
    
    public enum LzDrugPriceState
    {
        oldP,newP
    }
    public enum LzDrugConsumeType
    {
        burning,
        exchanging
    }
    public enum LzDrugUnitType
    {
        shareet,
        elba,
        capsole,
        cartoon,
        unit
    }
    public enum LzDrugRequestStatus
    {
        Pending,
        Accepted,
        Rejected,
        Completed,
        AtNegotioation,
        AcceptedForAnotherOne
    }
    public enum StkDrugPackageRequestStatus
    {
        Pending,
        Accepted,
        Rejected,
        Completed,
        CanceledFromStk,
        CanceledFromPharma,
        AtNegotioation
    }
    public enum UserPropertyType
    {
        phone,
        email,
        userName
    }
    public enum UserType
    {
        pharmacier,
        stocker
    }
    public enum ModelType
    {
        add,
        update
    }
    public enum AdminerPreviligs
    {
        HaveFullControl,
        HaveControlOnAdminersPage,
        HaveControlOnPharmaciesPage,
        HaveControlOnStocksPage,
        HaveControlOnVStockPage,
        HaveControlOnDrugsREquestsPage
    }
}
