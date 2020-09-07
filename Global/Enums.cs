using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys
{
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
