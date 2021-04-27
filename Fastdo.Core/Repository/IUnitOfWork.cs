
using Fastdo.Core.Repositories;

namespace Fastdo.Core
{
    public interface IUnitOfWork
    {
        IAdminRepository AdminRepository { get; }
        IAreaRepository AreaRepository { get; }
        IComplainsRepository ComplainsRepository { get; }
        ILzDrgRequestsRepository LzDrgRequestsRepository { get; }
        ILzDrugRepository LzDrugRepository { get; }
        ILzDrg_Search_Repository LzDrg_Search_Repository { get; }
        IPharmacyInStkClassRepository PharmacyInStkClassRepository { get; }
        IPharmacyInStkRepository PharmacyInStkRepository { get; }
        IPharmacyRepository PharmacyRepository { get; }
        IStkDrgInPackagesReqsRepository StkDrgInPackagesReqsRepository { get; }
        IStkDrugPackgesReqsRepository StkDrugPackgesReqsRepository { get; }
        IStkDrugsRepository StkDrugsRepository { get; }
        IStockRepository StockRepository { get; }
        IStockWithClassRepository StockWithClassRepository { get; }
        ITechSupportQRepository TechSupportQRepository { get; }
        bool Save();
    }
}
