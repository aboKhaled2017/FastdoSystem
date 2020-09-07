interface I_LzDrugReq_Paginator extends IPaginator {
    status: LzDrugsReqsStatus;
}
interface ILzDrugReqData {
    id: string;
    lzDrugId: string;
    requesterPhram_Id: string;
    requesterPhram_Name: string;
    owenerPh_Id: string;
    owenerPh_Name: string;
    status: LzDrugsReqsStatus;
    lzDrugName: string;
}
declare enum LzDrugsReqsStatus {
    Pending = 0,
    Accepted = 1,
    Rejected = 2,
    Completed = 3,
    AtNegotioation = 4,
    AcceptedForAnotherOne = 5
}
//# sourceMappingURL=LzDrugsReqsPageScripts.d.ts.map