/// <reference path="../plugins/DataTables/datatables.d.ts" />
/// <reference path="utiles.d.ts" />
/// <reference types="jquery.datatables" />
declare enum PharmaReqStatus {
    Pending = 0,
    Accepted = 1,
    Rejected = 2,
    Disabled = 3
}
interface IPharmaData {
    id: string;
    email: string;
    name: string;
    mgrName: string;
    ownerName: string;
    persPhone: string;
    landlinePhone: string;
    licenseImgSrc: string;
    commercialRegImgSrc: string;
    status: PharmaReqStatus;
    address: string;
    areaId: number;
    joinedStocksCount: number;
    lzDrugsCount: number;
    requestedDrugsCount: number;
}
interface IX_Pagination {
    totalCount: number;
    pageSize: number;
    currentPage: number;
    totalPages: number;
    prevPageLink: string;
    nextPageLink: string;
}
interface IPaginator {
    pageSize: number;
    pageNumber: number;
    s: string;
}
declare enum ControlActionType {
    remove = 0,
    disable = 1,
    activate = 2
}
declare const config: {
    _apiUrl: string;
    _dataTable: DataTables.DataTable;
    languageSetting: DataTables.LanguageSettings;
    _serverParams: {};
};
declare const mainOperationsOfPharmasPage: void;
//# sourceMappingURL=PharmasPageScripts.d.ts.map