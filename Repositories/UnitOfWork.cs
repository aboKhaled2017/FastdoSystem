
using Fastdo.Core;
using Fastdo.Core.Models;
using Fastdo.Core.Repositories;
using Fastdo.Core.Services;

namespace Fastdo.API.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SysDbContext context;
        private readonly IpropertyMappingService _propertMappingService;

        public UnitOfWork(IpropertyMappingService propertMappingService, SysDbContext context)
        {
            _propertMappingService = propertMappingService;
            this.context = context;
        }

        private AdminRepository _AdminRepository;
        private IAreaRepository _AreaRepository;
        private IComplainsRepository _ComplainsRepository;
        private ILzDrgRequestsRepository _LzDrgRequestsRepository;
        private ILzDrugRepository _LzDrugRepository;
        private ILzDrg_Search_Repository _LzDrg_Search_Repository;
        private IPharmacyInStkClassRepository _PharmacyInStkClassRepository;
        private IPharmacyInStkRepository _PharmacyInStkRepository;
        private IPharmacyRepository _PharmacyRepository;
        private IStkDrgInPackagesReqsRepository _StkDrgInPackagesReqsRepository;
        private IStkDrugPackgesReqsRepository _StkDrugPackgesReqsRepository;
        private IStkDrugsRepository _StkDrugsRepository;
        private IStockRepository _StockRepository;
        private IStockWithClassRepository _StockWithClassRepository;
        private ITechSupportQRepository _TechSupportQRepository;

        public IAdminRepository AdminRepository
        {
            get
            {
                return _AdminRepository ?? (_AdminRepository = new AdminRepository(context,PharmacyRepository));
            }
        }

        public IAreaRepository AreaRepository
        {
            get
            {
                return _AreaRepository ?? (_AreaRepository = new AreaRepository(context));
            }
        }

        public IComplainsRepository ComplainsRepository
        {
            get
            {
                return _ComplainsRepository ?? (_ComplainsRepository = new ComplainsRepository(context));
            }
        }

        public ILzDrgRequestsRepository LzDrgRequestsRepository
        {
            get
            {
                return _LzDrgRequestsRepository ?? (_LzDrgRequestsRepository = new LzDrgRequestsRepository(context,LzDrugRepository));
            }
        }

        public ILzDrugRepository LzDrugRepository
        {
            get
            {
                return _LzDrugRepository ?? (_LzDrugRepository = new LzDrugRepository(context));
            }
        }

        public ILzDrg_Search_Repository LzDrg_Search_Repository
        {
            get
            {
                return _LzDrg_Search_Repository ?? (_LzDrg_Search_Repository = new LzDrg_Search_Repository(context, _propertMappingService));
            }
        }

        public IPharmacyInStkClassRepository PharmacyInStkClassRepository
        {
            get
            {
                return _PharmacyInStkClassRepository ?? (_PharmacyInStkClassRepository = new PharmacyInStkClassRepository(context));
            }
        }

        public IPharmacyInStkRepository PharmacyInStkRepository
        {
            get
            {
                return _PharmacyInStkRepository ?? (_PharmacyInStkRepository = new PharmacyInStkRepository(context));
            }
        }

        public IPharmacyRepository PharmacyRepository
        {
            get
            {
                return _PharmacyRepository ?? (_PharmacyRepository=new PharmacyRepository(context));
            }
        }

        public IStkDrgInPackagesReqsRepository StkDrgInPackagesReqsRepository
        {
            get
            {
                return _StkDrgInPackagesReqsRepository ?? (_StkDrgInPackagesReqsRepository=new StkDrgInPackagesReqs(context));
            }
        }

        public IStkDrugPackgesReqsRepository StkDrugPackgesReqsRepository
        {
            get
            {
                return _StkDrugPackgesReqsRepository ?? (_StkDrugPackgesReqsRepository=new StkDrugPackagesReqsRepository(context));
            }
        }

        public IStkDrugsRepository StkDrugsRepository
        {
            get
            {
                return _StkDrugsRepository ?? (_StkDrugsRepository=new StkDrugsRepository(context,StkDrugPackgesReqsRepository,StkDrgInPackagesReqsRepository));
            }
        }

        public IStockRepository StockRepository
        {
            get
            {
                return _StockRepository ?? (_StockRepository=new StockRepository(context,PharmacyInStkRepository,StockWithClassRepository,PharmacyInStkClassRepository,StkDrgInPackagesReqsRepository,StkDrugsRepository));
            }
        }

        public IStockWithClassRepository StockWithClassRepository
        {
            get
            {
                return _StockWithClassRepository ?? (_StockWithClassRepository = new StockWithClassRepository(context));
            }
        }

        public ITechSupportQRepository TechSupportQRepository
        {
            get
            {
                return _TechSupportQRepository ?? (_TechSupportQRepository = new TechSupportQRepository(context));
            }
        }

        public bool Save()
        {
            return context.SaveChanges() > 0;
        }
    }
}
