/*Interfaces*/
interface IPharmaData {
    id: string
    email: string
    name: string
    mgrName: string
    ownerName: string
    persPhone: string
    landlinePhone: string
    licenseImgSrc: string
    commercialRegImgSrc: string
    status: PharmaReqStatus
    address: string
    areaId: number
    joinedStocksCount: number
    lzDrugsCount: number
    requestedDrugsCount: number
}
interface IX_Pagination {
    totalCount: number
    pageSize: number
    currentPage: number
    totalPages: number
    prevPageLink: string
    nextPageLink: string
}
interface IPaginator {
    pageSize: number
    pageNumber: number,
    s: string
}
interface IDrugInfo {
    id :string
    name :string
    type :string
    quantity :number
    price :number
    discount :number
    valideDate :string
    priceType: LzDrugPriceType
    unitType: LzDrugUnitType
    desc :string
    requestCount :number
}
interface IVstockProduct{
    drugId: string
    pharmacyId: string
    pharmacyName:string
    type: string
    quantity: number
    price: number
    consumeType:number
    discount: number
    valideDate: string
    priceType: LzDrugPriceType
    unitType: LzDrugUnitType
    desc: string
}
interface IVStockDrug{
    name: string 
    type: string
    products: IVstockProduct[]
}
interface IAdmininstartorInfo {
    id: string 
    name: string 
    userName: string
    phoneNumber: string
    superId: string 
    type: "Administartor"|"Representative"|"Supervisor" 
    priviligs:string
}


/*Enums*/
