using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System_Back_End.Models;

namespace System_Back_End.Repositories
{
    public interface ILzDrg_Search_Repository:IMainRepository
    {
        Task<PagedList<LzDrugCard_Info_BM>> Get_All_LzDrug_Cards_BMs(LzDrg_Card_Info_BM_ResourceParameters _params);
    }
}
