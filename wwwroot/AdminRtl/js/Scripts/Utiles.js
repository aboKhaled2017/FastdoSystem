"use strict";
var LzDrugUnitType;
(function (LzDrugUnitType) {
    LzDrugUnitType[LzDrugUnitType["shareet"] = 0] = "shareet";
    LzDrugUnitType[LzDrugUnitType["elba"] = 1] = "elba";
    LzDrugUnitType[LzDrugUnitType["capsole"] = 2] = "capsole";
    LzDrugUnitType[LzDrugUnitType["cartoon"] = 3] = "cartoon";
    LzDrugUnitType[LzDrugUnitType["unit"] = 4] = "unit";
})(LzDrugUnitType || (LzDrugUnitType = {}));
var LzDrugPriceType;
(function (LzDrugPriceType) {
    LzDrugPriceType[LzDrugPriceType["oldP"] = 0] = "oldP";
    LzDrugPriceType[LzDrugPriceType["newP"] = 1] = "newP";
})(LzDrugPriceType || (LzDrugPriceType = {}));
var PharmaReqStatus;
(function (PharmaReqStatus) {
    PharmaReqStatus[PharmaReqStatus["Pending"] = 0] = "Pending";
    PharmaReqStatus[PharmaReqStatus["Accepted"] = 1] = "Accepted";
    PharmaReqStatus[PharmaReqStatus["Rejected"] = 2] = "Rejected";
    PharmaReqStatus[PharmaReqStatus["Disabled"] = 3] = "Disabled";
})(PharmaReqStatus || (PharmaReqStatus = {}));
var AdminPriviligType;
(function (AdminPriviligType) {
    AdminPriviligType["HaveFullControl"] = "HaveFullControl";
    AdminPriviligType["HaveControlOnAdminersPage"] = "HaveControlOnAdminersPage";
    AdminPriviligType["HaveControlOnPharmaciesPage"] = "HaveControlOnPharmaciesPage";
    AdminPriviligType["HaveControlOnStocksPage"] = "HaveControlOnStocksPage";
    AdminPriviligType["HaveControlOnVStockPage"] = "HaveControlOnVStockPage";
    AdminPriviligType["HaveControlOnDrugsREquestsPage"] = "HaveControlOnDrugsREquestsPage";
})(AdminPriviligType || (AdminPriviligType = {}));
var _JqueyExtenionsObj = {
    setLoading: function (icon) {
        var el = this;
        el.attr('disabled', 'disabled');
        var i = el.find('i');
        i.removeClass(icon).addClass('fa-circle-o-notch fa-spin');
        i.data('icon', icon);
        return el;
    },
    removeLoading: function () {
        var el = this;
        el.removeAttr('disabled');
        var i = el.find('i');
        var icon = i.data('icon');
        i.addClass(icon).removeClass('fa-circle-o-notch fa-spin');
        return el;
    }
};
var helperFunctions = {
    delay: function delay(callback, ms) {
        var timer = 0;
        return function () {
            var context = this, args = arguments;
            clearTimeout(timer);
            timer = setTimeout(function () {
                callback.apply(context, args);
            }, ms || 0);
        };
    },
    convertValToTextIfZero: function (val) {
        if (val == 0)
            return 'لايوجد';
        return val;
    },
    alertServerError: function (message) {
        if (message === void 0) { message = "حدثت مشكلة فى الخادم اثناء معالجة الطلب"; }
        alert(message);
    },
    getGeneralErrorFromHttpErrorMess: function (e) {
        if (!e.responseJSON)
            return "خطأ ما حدث اثناء معالجت طلبك";
        return e.responseJSON.errors.G;
    },
    getInputError: function (error) {
        if (typeof error == "string")
            return error;
        else
            return error[0];
    },
    checkIfThat_ID_IsOf_TheCurrentUser: function (id) {
        return id == localStorage.getItem('uid');
    },
    setAutherizations: function () {
        var token = localStorage.getItem('token');
        $.ajaxSetup({
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', "bearer " + token);
            },
            contentType: 'application/json'
        });
    },
    updateUserizationToken: function (data) {
        var name = data.user.name, token = data.accessToken.token;
        localStorage.setItem('token', token);
        localStorage.name = name;
        this.setAutherizations();
    },
};
var drugHelperFunctions = {
    getPriceType: function (priceType) {
        return priceType == LzDrugPriceType.newP ? "سعر جديد" : "سعر قديم";
    },
    getUnitType: function (unitType) {
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
    getDiscount: function (discount) {
        if (discount == 0)
            return 'لا يوجد خصم';
        return discount + " %";
    }
};
var vStockHelperFunctions = {
    getTotalPriceInStringFormate: function (drg) {
        var obj = {};
        var text = "";
        drg.products.forEach(function (prod) {
            obj[prod.unitType] = obj[prod.unitType] || 0;
            obj[prod.unitType] += prod.quantity;
        });
        for (var prop in obj) {
            text += obj[prop] + ":" + drugHelperFunctions.getUnitType(prop) + " , ";
        }
        return text.substr(0, text.length - 3);
    }
};
var PharmaHelperFunctions = {
    getPharmaStatus: function (status) {
        return status == PharmaReqStatus.Accepted
            ? "نشطة الان"
            : status == PharmaReqStatus.Disabled
                ? "موقوفة الان"
                : status == PharmaReqStatus.Pending
                    ? "لم يتم معالجة الطلب من المسؤل حتى الان"
                    : status == PharmaReqStatus.Rejected
                        ? "تم رفض الطلب" : "نشطة الان";
    },
};
var AdminsHeperFunctions = {
    getPriviligMapping: function (privilig) {
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
    getAdminPriviligs: function (priviligs) {
        var _this = this;
        return priviligs.split(',').map(function (privilig) { return _this.getPriviligMapping(privilig); });
    }
};
var Urls = {
    apiUrl: '/api/admins/drgsReq',
    getDrugInfoUrl: '/api/admins/drgs',
    getPharmaInfoUrl: '/api/admins/pharmacies',
    getVstockApiUrl: '/api/admins/vstock'
};
var initalOperationsObj = {
    addJqueryExtensions: function () {
        jQuery.fn.extend(_JqueyExtenionsObj);
    },
    poliyFillRequireAttrOfHtml5Inputs: function () {
        document.addEventListener("DOMContentLoaded", function () {
            var elements = document.getElementsByTagName("INPUT");
            for (var i = 0; i < elements.length; i++) {
                elements[i].oninvalid = function (e) {
                    e.target.setCustomValidity("");
                    if (!e.target.validity.valid) {
                        e.target.setCustomValidity(e.target.getAttribute('title'));
                    }
                };
                elements[i].oninput = function (e) {
                    e.target.setCustomValidity("");
                };
            }
        });
    },
    start: function () {
        helperFunctions.setAutherizations();
        this.addJqueryExtensions();
        this.poliyFillRequireAttrOfHtml5Inputs();
    }
}.start();
var DatatableStaticProps = /** @class */ (function () {
    function DatatableStaticProps() {
    }
    DatatableStaticProps.languageSettings = {
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
    };
    return DatatableStaticProps;
}());
var DatatableExecuter = /** @class */ (function () {
    function DatatableExecuter(_tableDomEl, _baseLoadingDataUrl, _dataTable, columnDef, pageSize, _tableLoadingEl, _tableCreatedRow, afterStart) {
        if (_baseLoadingDataUrl === void 0) { _baseLoadingDataUrl = ""; }
        if (pageSize === void 0) { pageSize = 4; }
        if (_tableCreatedRow === void 0) { _tableCreatedRow = function () { }; }
        if (afterStart === void 0) { afterStart = function () { }; }
        this._tableDomEl = _tableDomEl;
        this._baseLoadingDataUrl = _baseLoadingDataUrl;
        this._dataTable = _dataTable;
        this.columnDef = columnDef;
        this._tableLoadingEl = _tableLoadingEl;
        this._tableCreatedRow = _tableCreatedRow;
        this.afterStart = afterStart;
        this.paginator = {
            pageNumber: 1,
            pageSize: 4,
            s: undefined
        };
        this.paginationUI = $('#paginationUI');
        this.pageSizeSelectInp = $('#pageSizeSelectInp');
        this.dtSearchInp = $('#dtSearchInp');
        this.paginator.pageSize = pageSize;
    }
    DatatableExecuter.prototype.start = function () {
        var _this = this;
        this.handleMainTable(function () { _this.afterStart(_this._dataTable, _this); });
        this.handleOnPaging();
        this.handleSearch();
        this.handleSelectPageSize();
    };
    DatatableExecuter.prototype.handleSearch = function () {
        var _this = this;
        this.dtSearchInp.keyup(helperFunctions.delay(function (e) {
            var text = _this.dtSearchInp.val().trim();
            _this.paginator.pageNumber = 1;
            _this.paginator.s = text;
            _this.refreshTableData();
        }, 700));
    };
    DatatableExecuter.prototype.handleSelectPageSize = function () {
        var _this = this;
        this.pageSizeSelectInp.change(function (e) {
            var pageSize = _this.pageSizeSelectInp.val();
            _this.paginator.pageNumber = 1;
            _this.paginator.pageSize = pageSize;
            _this.refreshTableData();
        });
    };
    DatatableExecuter.prototype.generateUrlWithPaginator = function (baseUrl) {
        baseUrl += '?';
        for (var prop in this.paginator) {
            if (this.paginator[prop]) {
                baseUrl += prop + "=" + this.paginator[prop] + "&";
            }
        }
        return baseUrl.substr(0, baseUrl.length - 1);
    };
    DatatableExecuter.prototype.updatePaginiationUi = function (pagingObj) {
        this.paginationUI.empty();
        var container = $();
        var firstItem = $("<li data-page=\"0\" class=\"page-item " + (!pagingObj.prevPageLink ? "disabled" : "") + "\"><a class=\"page-link\" href=\"#\">\u00AB</a></li>");
        container = container.add(firstItem);
        for (var i = 1; i <= pagingObj.totalPages; i++) {
            var item = $("<li data-page=\"" + i + "\" class=\"page-item " + (pagingObj.currentPage == i ? "active" : "") + "\"><a class=\"page-link\" href=\"#\">" + i + "</a></li>");
            container = container.add(item);
        }
        var lastItem = $("<li data-page=\"-1\" class=\"page-item " + (!pagingObj.nextPageLink ? "disabled" : "") + "\"> <a class=\"page-link\" href = \"#\" >\u00BB</a></li >");
        container = container.add(lastItem);
        if (pagingObj.totalPages == 0) {
            this.paginationUI.empty();
            return;
        }
        this.paginationUI.append(container);
    };
    DatatableExecuter.prototype.handleOnPaging = function () {
        var _this = this;
        $(document).on('click', '#paginationUI .page-item', function (e) {
            var el = $(this);
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
        });
    };
    DatatableExecuter.prototype.refreshTableData = function () {
        var _this = this;
        this._tableLoadingEl.toggleClass('d-none');
        $.get(this.generateUrlWithPaginator(this._baseLoadingDataUrl), {}, function (d, f, req) {
            _this._currentPagination = JSON.parse(req.getResponseHeader('X-Pagination'));
            _this.updatePaginiationUi(_this._currentPagination);
        })
            .done(function (data) {
            _this._dataTable.clear().draw();
            _this._dataTable.rows.add(data);
            _this._dataTable.columns.adjust().draw();
        })
            .catch(function (e) {
            alert('problem as getting data from server');
        })
            .always(function () {
            _this._tableLoadingEl.toggleClass('d-none');
        });
    };
    DatatableExecuter.prototype.handleMainTable = function (executeOnLoadTable) {
        var _this = this;
        var _this = this;
        $.get(this.generateUrlWithPaginator(this._baseLoadingDataUrl), {}, function (d, f, req) {
            _this._currentPagination = JSON.parse(req.getResponseHeader('X-Pagination'));
            _this.updatePaginiationUi(_this._currentPagination);
        })
            .done(function (data) {
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
            .catch(function (e) {
            alert('problem as getting data from server');
        })
            .always(function () {
            _this._tableLoadingEl.toggleClass('d-none');
        });
    };
    DatatableExecuter.prototype.getDatatable = function () {
        return this._dataTable;
    };
    return DatatableExecuter;
}());
//# sourceMappingURL=Utiles.js.map