declare enum StkControlActionType {
    remove = 0,
    disable = 1,
    activate = 2
}
declare enum StockReqStatus {
    Pending = 0,
    Accepted = 1,
    Rejected = 2,
    Disabled = 3
}
interface IStockData {
    id: string;
    email: string;
    name: string;
    mgrName: string;
    ownerName: string;
    persPhone: string;
    landlinePhone: string;
    licenseImgSrc: string;
    commercialRegImgSrc: string;
    status: StockReqStatus;
    address: string;
    areaId: number;
    joinedPharmesCount: number;
    drugsCount: number;
}
//# sourceMappingURL=StocksPageScripts.d.ts.map