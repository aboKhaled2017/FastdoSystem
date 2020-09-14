enum LzDrugUnitType {
    shareet,
    elba,
    capsole,
    cartoon,
    unit
}
enum LzDrugPriceType {
    oldP,
    newP
}
enum PharmaReqStatus {
    Pending = 0,
    Accepted = 1,
    Rejected = 2,
    Disabled = 3
}
enum AdminPriviligType {
    HaveFullControl = "HaveFullControl",
    HaveControlOnAdminersPage = "HaveControlOnAdminersPage",
    HaveControlOnPharmaciesPage = "HaveControlOnPharmaciesPage",
    HaveControlOnStocksPage = "HaveControlOnStocksPage",
    HaveControlOnVStockPage = "HaveControlOnVStockPage",
    HaveControlOnDrugsREquestsPage = "HaveControlOnDrugsREquestsPage"
}
interface JQuery {
    setLoading: (icon: string) => JQuery<HTMLElement>
    removeLoading: () => JQuery<HTMLElement>
}
const _JqueyExtenionsObj = {
    setLoading: function (icon: string) {
        var el = (this as any as JQuery<HTMLElement>);
        el.attr('disabled', 'disabled');
        var i = el.find('i');
        i.removeClass(icon).addClass('fa-circle-o-notch fa-spin');
        i.data('icon', icon);
        return el;
    },
    removeLoading: function () {
        var el = (this as JQuery<HTMLElement>);
        el.removeAttr('disabled');
        var i = el.find('i');
        var icon = i.data('icon');
        i.addClass(icon).removeClass('fa-circle-o-notch fa-spin');
        return el;
    }
}



const helperFunctions = {
    delay:function delay(callback: Function, ms: number) {
        var timer = 0;
        return function (this: (e: any) => void) {
            var context = this, args = arguments;
            clearTimeout(timer);
            timer = setTimeout(function () {
                callback.apply(context, args);
            }, ms || 0);
        };
    },
    convertValToTextIfZero(val: number) {
        if (val == 0)
            return 'لايوجد';
        return val;
    },
    alertServerError(message: string = "حدثت مشكلة فى الخادم اثناء معالجة الطلب") {
        alert(message);
    },
    getGeneralErrorFromHttpErrorMess(e: JQuery.jqXHR<any>) {
        if (!e.responseJSON)
            return "خطأ ما حدث اثناء معالجت طلبك";
        return e.responseJSON.errors.G;
    },
    getInputError(error: string | string[]) {
        if (typeof error == "string")
            return error;
        else return error[0];
    },
    checkIfThat_ID_IsOf_TheCurrentUser(id: string) {
        return id == localStorage.getItem('uid');
    },
    setAutherizations() {
        const token = localStorage.getItem('token');
        $.ajaxSetup({
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', `bearer ${token}`);
            },
            contentType: 'application/json'
        });
    },
    updateUserizationToken(data: any) {
        const { user: { name }, accessToken: { token } } = data;
        localStorage.setItem('token', token);
        localStorage.name = name;
        this.setAutherizations();
    },
}
const drugHelperFunctions = {
    getPriceType(priceType: LzDrugPriceType) {
        return priceType == LzDrugPriceType.newP ? "سعر جديد" : "سعر قديم";
    },
    getUnitType(unitType: LzDrugUnitType) {
        return unitType == LzDrugUnitType.capsole
            ? "كبسول"
            : unitType == LzDrugUnitType.cartoon
                ? "كرتونة"
                : unitType == LzDrugUnitType.elba
                    ? "علبة"
                    : unitType == LzDrugUnitType.shareet
                        ? "شريط"
                        : unitType == LzDrugUnitType.unit
                            ? "وحدة" : "علبة";

    },
    getDiscount(discount: number) {
        if (discount == 0)
            return 'لا يوجد خصم';
        return `${discount} %`;
    }
}
const vStockHelperFunctions = {
    getTotalPriceInStringFormate(drg: IVStockDrug) {
        const obj: any = {}; let text = "";
        drg.products.forEach(prod => {
            obj[prod.unitType] = obj[prod.unitType] || 0;
            obj[prod.unitType] += prod.quantity;
        });
        for (let prop in obj) {
            text += `${obj[prop]}:${drugHelperFunctions.getUnitType(prop as any)} , `;
        }
        return text.substr(0, text.length - 3);
    }
}
const PharmaHelperFunctions={
    getPharmaStatus(status: PharmaReqStatus) {
        return status == PharmaReqStatus.Accepted
            ? "نشطة الان"
            : status == PharmaReqStatus.Disabled
                ? "موقوفة الان"
                : status == PharmaReqStatus.Pending
                    ? "لم يتم معالجة الطلب من المسؤل حتى الان"
                    : status == PharmaReqStatus.Rejected
                        ? "تم رفض الطلب" : "نشطة الان";
    },
}
const AdminsHeperFunctions = {
    getPriviligMapping(privilig: AdminPriviligType) {
        if (privilig == AdminPriviligType.HaveFullControl)
            return "تحكم كامل فى بلوحة التحكم";
        if (privilig == AdminPriviligType.HaveControlOnAdminersPage)
            return "تحكم فى صفحة المسؤلين";
        if (privilig == AdminPriviligType.HaveControlOnDrugsREquestsPage)
            return "تحكم فى صفحة طلبات الرواكد";
        if (privilig == AdminPriviligType.HaveControlOnPharmaciesPage)
            return "تحكم فى صفحة الصيادلة";
        if (privilig == AdminPriviligType.HaveControlOnStocksPage)
            return "تحكم فى صفحة المخازن";
        if (privilig == AdminPriviligType.HaveControlOnVStockPage)
            return "تحكم فى صفحة المخازن الافتراضية";
        return "تحكم كامل فى بلوحة التحكم"; 
    },
    getAdminPriviligs(priviligs: string) {
       return priviligs.split(',').map(privilig => this.getPriviligMapping(privilig as AdminPriviligType));      
    }
}

const Urls = {
    apiUrl: '/api/admins/drgsReq',
    getDrugInfoUrl: '/api/admins/drgs',
    getPharmaInfoUrl: '/api/admins/pharmacies',
    getVstockApiUrl:'/api/admins/vstock'
}

const initalOperationsObj = {
    addJqueryExtensions() {
        jQuery.fn.extend(_JqueyExtenionsObj);
    },  
    poliyFillRequireAttrOfHtml5Inputs() {
        document.addEventListener("DOMContentLoaded", function () {
            var elements = document.getElementsByTagName("INPUT");
            for (var i = 0; i < elements.length; i++) {
                (elements[i] as any).oninvalid = function (e: any) {
                    e.target.setCustomValidity("");
                    if (!e.target.validity.valid) {
                        e.target.setCustomValidity((e.target as HTMLInputElement).getAttribute('title'));
                    }
                };
                (elements[i] as any).oninput = function (e: any) {
                    e.target.setCustomValidity("");
                };
            }
        })
    },
    start() {
        helperFunctions.setAutherizations();
        this.addJqueryExtensions();
        this.poliyFillRequireAttrOfHtml5Inputs();
    }
}.start();
class DatatableStaticProps {
    static languageSettings: DataTables.LanguageSettings = {
        "sProcessing": "جارٍ التحميل...",
        "sLengthMenu": "أظهر _MENU_ مدخلات",
        "sZeroRecords": "لم يعثر على أية سجلات",
        "sInfo": "إظهار _START_ إلى _END_ من أصل _TOTAL_ مدخل",
        "sInfoEmpty": "يعرض 0 إلى 0 من أصل 0 سجل",
        "sInfoFiltered": "(منتقاة من مجموع _MAX_ مُدخل)",
        "sInfoPostFix": "",
        "sSearch": "ابحث:",
        "sUrl": "",
        "oPaginate": {
            "sFirst": "الأول",
            "sPrevious": "السابق",
            "sNext": "التالي",
            "sLast": "الأخير"
        }
    } as any
}
class DatatableExecuter {
    paginator: IPaginator = {
        pageNumber: 1,
        pageSize: 4,
        s: undefined as any
    }
    paginationUI = $('#paginationUI')
    pageSizeSelectInp = $('#pageSizeSelectInp')
    dtSearchInp = $('#dtSearchInp')  
    private _currentPagination:any
    constructor(
        private _tableDomEl: JQuery<HTMLElement>,
        private _baseLoadingDataUrl: string = "",
        private _dataTable: DataTables.DataTable,
        private columnDef:  DataTables.ColumnDefsSettings[],
        pageSize: number = 4,
        private _tableLoadingEl: JQuery<HTMLElement>,
        private _tableCreatedRow: DataTables.FunctionCreateRow = () => { },
        private afterStart: Function = () => { })
    {
        this.paginator.pageSize = pageSize;
    }
    start() {
        this.handleMainTable(() => { this.afterStart(this._dataTable,this)});       
        this.handleOnPaging();
        this.handleSearch();
        this.handleSelectPageSize();
    }
    handleSearch() {
        var _this = this;
        this.dtSearchInp.keyup(helperFunctions.delay(function (e: any) {
            var text = (_this.dtSearchInp.val() as string).trim();
            _this.paginator.pageNumber = 1;
            _this.paginator.s = text;
            _this.refreshTableData();
        }, 700) as any);
    }
    handleSelectPageSize() {
        this.pageSizeSelectInp.change(e => {
            let pageSize = this.pageSizeSelectInp.val() as number;
            this.paginator.pageNumber = 1;
            this.paginator.pageSize = pageSize;
            this.refreshTableData();
        });
    }
    generateUrlWithPaginator(baseUrl: string) {
        baseUrl += '?';
        for (let prop in this.paginator) {
            if ((this.paginator as any)[prop]) {
                baseUrl += `${prop}=${(this.paginator as any)[prop]}&`;
            }
        }
        return baseUrl.substr(0, baseUrl.length - 1);
    }
    updatePaginiationUi(pagingObj: IX_Pagination) {
        this.paginationUI.empty();

        let container = $();
        const firstItem = $(`<li data-page="0" class="page-item ${!pagingObj.prevPageLink ? "disabled" : ""}"><a class="page-link" href="#">«</a></li>`);

        container = container.add(firstItem);

        for (let i = 1; i <= pagingObj.totalPages; i++) {
            var item = $(`<li data-page="${i}" class="page-item ${pagingObj.currentPage == i ? "active" : ""}"><a class="page-link" href="#">${i}</a></li>`);
            container = container.add(item);
        }
        const lastItem = $(`<li data-page="-1" class="page-item ${!pagingObj.nextPageLink ? "disabled" : ""}"> <a class="page-link" href = "#" >»</a></li >`);
        container = container.add(lastItem);
        if (pagingObj.totalPages == 0) {
            this.paginationUI.empty();
            return;
        }
        this.paginationUI.append(container);
    }
    handleOnPaging() {
        const _this = this;
        $(document).on('click', '#paginationUI .page-item', function (e) {
            const el = $(this);
            if (el.attr('disabled'))
                return;
            var pageNumber = el.data('page');
            if (pageNumber == 0)
                _this.paginator.pageNumber -= 1;
            else if (pageNumber == -1)
                _this.paginator.pageNumber += 1;
            else
                _this.paginator.pageNumber = pageNumber;

            _this.refreshTableData();
        })
    }
    refreshTableData() {
        this._tableLoadingEl.toggleClass('d-none');
        $.get(this.generateUrlWithPaginator(this._baseLoadingDataUrl), {}, (d, f, req) => {
            this._currentPagination = JSON.parse(req.getResponseHeader('X-Pagination') as string) as IX_Pagination;
            this.updatePaginiationUi(this._currentPagination);
        })
            .done((data: ILzDrugReqData[]) => {
              
                this._dataTable.clear().draw();
                this._dataTable.rows.add(data);
                this._dataTable.columns.adjust().draw();

            })
            .catch(e => {
                alert('problem as getting data from server');
            })
            .always(() => {
                this._tableLoadingEl.toggleClass('d-none');
            });
    }
    handleMainTable(executeOnLoadTable: Function) {
        const _this = this;
        $.get(this.generateUrlWithPaginator(this._baseLoadingDataUrl), {}, (d, f, req) => {
            this._currentPagination = JSON.parse(req.getResponseHeader('X-Pagination') as string) as IX_Pagination;
            this.updatePaginiationUi(this._currentPagination);
        })
            .done((data: ILzDrugReqData[]) => {
                _this._dataTable = _this._tableDomEl.DataTable({
                    data: data,
                    columnDefs: _this.columnDef,
                    createdRow: _this._tableCreatedRow,
                    language: DatatableStaticProps.languageSettings,
                    paging: false,
                    searching: false,
                    scrollX: true,
                    autoWidth: true
                });
                executeOnLoadTable();
            })
            .catch(e => {
                alert('problem as getting data from server');
            })
            .always(() => {
                _this._tableLoadingEl.toggleClass('d-none');
            })
    }
    getDatatable() {
        return this._dataTable;
    }
}
