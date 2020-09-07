/// <reference types="jquery" />
/// <reference types="jquery.datatables" />
declare enum LzDrugUnitType {
    shareet = 0,
    elba = 1,
    capsole = 2,
    cartoon = 3,
    unit = 4
}
declare enum LzDrugPriceType {
    oldP = 0,
    newP = 1
}
declare enum PharmaReqStatus {
    Pending = 0,
    Accepted = 1,
    Rejected = 2,
    Disabled = 3
}
declare enum AdminPriviligType {
    HaveFullControl = "HaveFullControl",
    HaveControlOnAdminersPage = "HaveControlOnAdminersPage",
    HaveControlOnPharmaciesPage = "HaveControlOnPharmaciesPage",
    HaveControlOnStocksPage = "HaveControlOnStocksPage",
    HaveControlOnVStockPage = "HaveControlOnVStockPage",
    HaveControlOnDrugsREquestsPage = "HaveControlOnDrugsREquestsPage"
}
interface JQuery {
    setLoading: (icon: string) => JQuery<HTMLElement>;
    removeLoading: () => JQuery<HTMLElement>;
}
declare const _JqueyExtenionsObj: {
    setLoading: (icon: string) => JQuery<HTMLElement>;
    removeLoading: () => JQuery<HTMLElement>;
};
declare const helperFunctions: {
    delay: (callback: Function, ms: number) => (this: (e: any) => void) => void;
    convertValToTextIfZero(val: number): number | "لايوجد";
    alertServerError(message?: string): void;
    getGeneralErrorFromHttpErrorMess(e: JQuery.jqXHR<any>): any;
    getInputError(error: string | string[]): string;
    checkIfThat_ID_IsOf_TheCurrentUser(id: string): boolean;
    setAutherizations(): void;
    updateUserizationToken(data: any): void;
};
declare const drugHelperFunctions: {
    getPriceType(priceType: LzDrugPriceType): "سعر جديد" | "سعر قديم";
    getUnitType(unitType: LzDrugUnitType): "كبسول" | "كرتونة" | "علبة" | "شريط" | "وحدة";
    getDiscount(discount: number): string;
};
declare const vStockHelperFunctions: {
    getTotalPriceInStringFormate(drg: IVStockDrug): string;
};
declare const PharmaHelperFunctions: {
    getPharmaStatus(status: PharmaReqStatus): "نشطة الان" | "موقوفة الان" | "لم يتم معالجة الطلب من المسؤل حتى الان" | "تم رفض الطلب";
};
declare const AdminsHeperFunctions: {
    getPriviligMapping(privilig: AdminPriviligType): "تحكم كامل فى بلوحة التحكم" | "تحكم فى صفحة المسؤلين" | "تحكم فى صفحة طلبات الرواكد" | "تحكم فى صفحة الصيادلة" | "تحكم فى صفحة المخازن" | "تحكم فى صفحة المخازن الافتراضية";
    getAdminPriviligs(priviligs: string): ("تحكم كامل فى بلوحة التحكم" | "تحكم فى صفحة المسؤلين" | "تحكم فى صفحة طلبات الرواكد" | "تحكم فى صفحة الصيادلة" | "تحكم فى صفحة المخازن" | "تحكم فى صفحة المخازن الافتراضية")[];
};
declare const Urls: {
    apiUrl: string;
    getDrugInfoUrl: string;
    getPharmaInfoUrl: string;
    getVstockApiUrl: string;
};
declare const initalOperationsObj: void;
declare class DatatableStaticProps {
    static languageSettings: DataTables.LanguageSettings;
}
declare class DatatableExecuter {
    private _tableDomEl;
    private _baseLoadingDataUrl;
    private _dataTable;
    private columnDef;
    private _tableLoadingEl;
    private _tableCreatedRow;
    private afterStart;
    paginator: IPaginator;
    paginationUI: JQuery<HTMLElement>;
    pageSizeSelectInp: JQuery<HTMLElement>;
    dtSearchInp: JQuery<HTMLElement>;
    private _currentPagination;
    constructor(_tableDomEl: JQuery<HTMLElement>, _baseLoadingDataUrl: string, _dataTable: DataTables.DataTable, columnDef: DataTables.ColumnDefsSettings[], pageSize: number | undefined, _tableLoadingEl: JQuery<HTMLElement>, _tableCreatedRow?: DataTables.FunctionCreateRow, afterStart?: Function);
    start(): void;
    handleSearch(): void;
    handleSelectPageSize(): void;
    generateUrlWithPaginator(baseUrl: string): string;
    updatePaginiationUi(pagingObj: IX_Pagination): void;
    handleOnPaging(): void;
    refreshTableData(): void;
    handleMainTable(executeOnLoadTable: Function): void;
    getDatatable(): DataTables.DataTable;
}
//# sourceMappingURL=Utiles.d.ts.map