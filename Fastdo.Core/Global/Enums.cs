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
}
