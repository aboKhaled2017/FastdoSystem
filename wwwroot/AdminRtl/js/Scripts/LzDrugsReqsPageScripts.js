"use strict";
var LzDrugsReqsStatus;
(function (LzDrugsReqsStatus) {
    LzDrugsReqsStatus[LzDrugsReqsStatus["Pending"] = 0] = "Pending";
    LzDrugsReqsStatus[LzDrugsReqsStatus["Accepted"] = 1] = "Accepted";
    LzDrugsReqsStatus[LzDrugsReqsStatus["Rejected"] = 2] = "Rejected";
    LzDrugsReqsStatus[LzDrugsReqsStatus["Completed"] = 3] = "Completed";
    LzDrugsReqsStatus[LzDrugsReqsStatus["AtNegotioation"] = 4] = "AtNegotioation";
    LzDrugsReqsStatus[LzDrugsReqsStatus["AcceptedForAnotherOne"] = 5] = "AcceptedForAnotherOne";
})(LzDrugsReqsStatus || (LzDrugsReqsStatus = {}));
(function () {
    var LzDrgReqConfig = {
        _apiUrl: '/api/admins/drgsReq',
        _getDrugInfoUrl: '/api/admins/drgs',
        _getPharmaInfoUrl: '/api/admins/pharmacies',
        _dataTable: null,
        languageSetting: {
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
        },
        _serverParams: {}
    };
    var mainOperationsOfLzDrgsReqsPage = {
        _mainTable: $('#lzDrugsReqsTable'),
        _paginationUI: $('#paginationUI'),
        _pageSizeSelectInp: $('#pageSizeSelectInp'),
        _reqStatusSelectInp: $('#reqStatusSelectInp'),
        _modal: $('#modal'),
        _modalTitle: $('#modal .modal-title'),
        _modalBody: $('#modal .modal-body'),
        _confirmModal: $('#confirmModal'),
        _tableLoading: $('.lzDrugsReqsPageSection .ContainerOverlay').eq(0),
        _dtSearchInp: $('#dtSearchInp'),
        _paginator: {
            pageNumber: 1,
            pageSize: 4,
        },
        _currentPagination: {},
        _getPharmaStatus: function (status) {
            return status == LzDrugsReqsStatus.Accepted
                ? "تم قبول الطلب"
                : status == LzDrugsReqsStatus.Rejected
                    ? "تم رفض الطلب"
                    : status == LzDrugsReqsStatus.Pending
                        ? "لم يتم الرد على الطلب"
                        : status == LzDrugsReqsStatus.Completed
                            ? "اكتملت عملية التبادل"
                            : status == LzDrugsReqsStatus.AtNegotioation
                                ? "مازالت فى التفاوض"
                                : status == LzDrugsReqsStatus.AcceptedForAnotherOne
                                    ? "تم الطلب من قبل صيدلية اخرى" : "لم يتم الرد على الطلب";
        },
        handleSearch: function () {
            var _this = this;
            this._dtSearchInp.keyup(helperFunctions.delay(function (e) {
                var text = _this._dtSearchInp.val().trim();
                _this._paginator.pageNumber = 1;
                _this._paginator.s = text;
                _this.refreshTableData();
            }, 700));
            /*this._dtSearchInp.keyup(e => {
                var text = (this._dtSearchInp.val() as string).trim();
                this._paginator.s = text;
                this.refreshTableData();
            });*/
        },
        handleOnSelectReqStatus: function () {
            var _this_1 = this;
            this._reqStatusSelectInp.change(function (e) {
                var status = _this_1._reqStatusSelectInp.val();
                _this_1._paginator.pageNumber = 1;
                _this_1._paginator.status = status;
                _this_1.refreshTableData();
            });
        },
        handleSelectPageSize: function () {
            var _this_1 = this;
            this._pageSizeSelectInp.change(function (e) {
                var pageSize = _this_1._pageSizeSelectInp.val();
                _this_1._paginator.pageNumber = 1;
                _this_1._paginator.pageSize = pageSize;
                _this_1.refreshTableData();
            });
        },
        getModalBodyHtmlForDrugInfo: function (rowData) {
            var _container = $();
            var _elements = $("\n              <ol class=\"breadcrumb\">\n                <li class=\"breadcrumb-item\"><span class=\"badge badge-info\">\u0627\u0644\u0627\u0633\u0645</span> : " + rowData.name + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li class=\"breadcrumb-item\"><span class=\"badge badge-info\">\u0627\u0644\u062E\u0635\u0645</span> : " + drugHelperFunctions.getDiscount(rowData.discount) + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li class=\"breadcrumb-item\"><span class=\"badge badge-info\">\u0627\u0644\u0633\u0639\u0631</span> : " + rowData.price + " \u062C\u0646\u064A\u0629</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li class=\"breadcrumb-item\"><span class=\"badge badge-info\">\u0646\u0648\u0639 \u0627\u0644\u0633\u0639\u0631 </span> : " + drugHelperFunctions.getPriceType(rowData.priceType) + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li cla<li class=\"breadcrumb-item\"><span class=\"badge badge-info\">\u0627\u0644\u0643\u0645\u064A\u0629</span> : " + rowData.quantity + " " + drugHelperFunctions.getUnitType(rowData.unitType) + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li class=\"breadcrumb-item\"><span class=\"badge badge-info\">\u0627\u0644\u0646\u0648\u0639 </span> : " + rowData.type + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li class=\"breadcrumb-item\"><span class=\"badge badge-info\"> \u0639\u062F\u062F \u0627\u0644\u0637\u0644\u0628 \u0639\u0644\u0649 \u0627\u0644\u0631\u0627\u0643\u062F </span> : " + helperFunctions.convertValToTextIfZero(rowData.requestCount) + "</li>\n              </ol>             \n            ");
            _container = _container.add(_elements);
            this._modalBody.empty().append(_container);
            this._modal.modal('show');
        },
        getModalBodyHtmlForRequesterPharmaInfo: function (rowData) {
            var _container = $();
            var _elements = $("\n              <ol class=\"breadcrumb\">\n                <li class=\"breadcrumb-item\"><span class=\"badge badge-info\">\u0627\u0644\u0627\u0633\u0645</span> : " + rowData.name + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li class=\"breadcrumb-item\"><span class=\"badge badge-info\">\u0627\u0644\u0628\u0631\u064A\u062F \u0627\u0644\u0627\u0644\u0643\u062A\u0631\u0648\u0646\u0649</span> : " + rowData.email + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li class=\"breadcrumb-item\"><span class=\"badge badge-info\">\u0627\u0633\u0645 \u0627\u0644\u0645\u0627\u0644\u0643 </span>: " + rowData.ownerName + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li class=\"breadcrumb-item\"><span class=\"badge badge-info\">\u0627\u0633\u0645 \u0627\u0644\u0645\u062F\u064A\u0631 </span>: " + rowData.mgrName + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li cla<li class=\"breadcrumb-item\"><span class=\"badge badge-info\">\u0627\u0644\u0639\u0646\u0648\u0627\u0646</span>: " + rowData.address + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li class=\"breadcrumb-item\"><span class=\"badge badge-info\"> \u0627\u0644\u062A\u0644\u064A\u0641\u0648\u0646 \u0627\u0644\u0627\u0631\u0636\u0649 </span>: " + rowData.landlinePhone + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li class=\"breadcrumb-item\"><span class=\"badge badge-info\"> \u0627\u0644\u062A\u0644\u064A\u0641\u0648\u0646 \u0627\u0644\u0645\u062D\u0645\u0648\u0644 </span>: " + rowData.persPhone + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li class=\"breadcrumb-item\"><span class=\"badge badge-info\"> \u0627\u0644\u062D\u0627\u0644\u0629 </span>: " + PharmaHelperFunctions.getPharmaStatus(rowData.status) + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li cla<li class=\"breadcrumb-item\"><span class=\"badge badge-info\"> \u0627\u0644\u0645\u062E\u0627\u0632\u0646 \u0627\u0644\u0645\u0646\u0636\u0645\u0629 \u0627\u0644\u064A\u0647\u0627 </span>: " + helperFunctions.convertValToTextIfZero(rowData.joinedStocksCount) + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li class=\"breadcrumb-item\"><span class=\"badge badge-info\"> \u0639\u062F\u062F \u0639\u0631\u0648\u0636 \u0627\u0644\u0631\u0648\u0627\u0643\u062F </span>: " + helperFunctions.convertValToTextIfZero(rowData.lzDrugsCount) + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li cla<li class=\"breadcrumb-item\"><span class=\"badge badge-info\"> \u0639\u062F\u062F \u0627\u0644\u0637\u0644\u0628\u0627\u062A \u0627\u0644\u062A\u0649 \u0642\u0627\u0645\u062A \u0628\u0647\u0627 </span>: " + helperFunctions.convertValToTextIfZero(rowData.requestedDrugsCount) + "</li>\n              </ol>             \n            ");
            _container = _container.add(_elements);
            this._modalBody.empty().append(_container);
            this._modal.modal('show');
        },
        handleDetailsColumnActions: function () {
            var _this = this;
            $(document).on('click', '.tbShowDrgInfoBtn', function () {
                var $btn = $(this);
                var index = $btn.parents('tr').index();
                var rowData = LzDrgReqConfig._dataTable.row(index).data();
                _this._modalTitle.text("\u0643\u0644 \u0628\u064A\u0627\u0646\u0627\u062A /" + rowData.lzDrugName);
                var data = $btn.data('drug');
                if (!data) {
                    $btn.setLoading('fa-info');
                    $.get(LzDrgReqConfig._getDrugInfoUrl + "/" + rowData.lzDrugId + "/details")
                        .done(function (_data) {
                        _this.getModalBodyHtmlForDrugInfo(_data);
                        $btn.data('drug', _data);
                    })
                        .catch(function (e) {
                        helperFunctions.alertServerError();
                    })
                        .always(function () {
                        $btn.removeLoading();
                    });
                }
                else {
                    _this.getModalBodyHtmlForDrugInfo(data);
                }
            });
            $(document).on('click', '.tbShowRequisterPharmaInfoBtn', function () {
                var $btn = $(this);
                var index = $btn.parents('tr').index();
                var rowData = LzDrgReqConfig._dataTable.row(index).data();
                _this._modalTitle.text("\u0643\u0644 \u0628\u064A\u0627\u0646\u0627\u062A /" + rowData.requesterPhram_Name);
                var data = $btn.data('pharma');
                if (!data) {
                    $btn.setLoading('fa-info');
                    $.get(LzDrgReqConfig._getPharmaInfoUrl + "/" + rowData.requesterPhram_Id)
                        .done(function (_data) {
                        _this.getModalBodyHtmlForRequesterPharmaInfo(_data);
                        $btn.data('pharma', _data);
                    })
                        .catch(function (e) {
                        helperFunctions.alertServerError();
                    })
                        .always(function () {
                        $btn.removeLoading();
                    });
                }
                else {
                    _this.getModalBodyHtmlForDrugInfo(data);
                }
            });
            $(document).on('click', '.tbShowOwnerPharmaInfoBtn', function () {
                var $btn = $(this);
                var index = $btn.parents('tr').index();
                var rowData = LzDrgReqConfig._dataTable.row(index).data();
                _this._modalTitle.text("\u0643\u0644 \u0628\u064A\u0627\u0646\u0627\u062A /" + rowData.owenerPh_Name);
                var data = $btn.data('pharma');
                if (!data) {
                    $btn.setLoading('fa-info');
                    $.get(LzDrgReqConfig._getPharmaInfoUrl + "/" + rowData.owenerPh_Id)
                        .done(function (_data) {
                        _this.getModalBodyHtmlForRequesterPharmaInfo(_data);
                        $btn.data('pharma', _data);
                    })
                        .catch(function (e) {
                        helperFunctions.alertServerError();
                    })
                        .always(function () {
                        $btn.removeLoading();
                    });
                }
                else {
                    _this.getModalBodyHtmlForDrugInfo(data);
                }
            });
            $(document).on('click', '.tbShowAllReqInfoBtn', function () {
                var $btn = $(this);
                var index = $btn.parents('tr').index();
                var rowData = LzDrgReqConfig._dataTable.row(index).data();
                _this._modalTitle.text("\u0643\u0644 \u0628\u064A\u0627\u0646\u0627\u062A /" + rowData.lzDrugId);
                var _container = $();
                var _elements = $( /*`
              <ol class="breadcrumb">
                <li class="breadcrumb-item"><span class="badge badge-info">الاسم</span> : ${rowData.name}</li>
              </ol>
              <ol class="breadcrumb">
                <li class="breadcrumb-item"><span class="badge badge-info">البريد الالكترونى</span> : ${rowData.email}</li>
              </ol>
              <ol class="breadcrumb">
                <li class="breadcrumb-item"><span class="badge badge-info">اسم المالك </span>: ${rowData.ownerName}</li>
              </ol>
              <ol class="breadcrumb">
                <li class="breadcrumb-item"><span class="badge badge-info">اسم المدير </span>: ${rowData.mgrName}</li>
              </ol>
              <ol class="breadcrumb">
                <li cla<li class="breadcrumb-item"><span class="badge badge-info">العنوان</span>: ${rowData.address}</li>
              </ol>
              <ol class="breadcrumb">
                <li class="breadcrumb-item"><span class="badge badge-info"> التليفون الارضى </span>: ${rowData.landlinePhone}</li>
              </ol>
              <ol class="breadcrumb">
                <li class="breadcrumb-item"><span class="badge badge-info"> التليفون المحمول </span>: ${rowData.persPhone}</li>
              </ol>
              <ol class="breadcrumb">
                <li class="breadcrumb-item"><span class="badge badge-info"> الحالة </span>: ${_this._getPharmaStatus(rowData.status)}</li>
              </ol>
              <ol class="breadcrumb">
                <li cla<li class="breadcrumb-item"><span class="badge badge-info"> المخازن المنضمة اليها </span>: ${_this._expressIntToText(rowData.joinedStocksCount)}</li>
              </ol>
              <ol class="breadcrumb">
                <li class="breadcrumb-item"><span class="badge badge-info"> عدد عروض الرواكد </span>: ${_this._expressIntToText(rowData.lzDrugsCount)}</li>
              </ol>
              <ol class="breadcrumb">
                <li cla<li class="breadcrumb-item"><span class="badge badge-info"> عدد الطلبات التى قامت بها </span>: ${_this._expressIntToText(rowData.requestedDrugsCount)}</li>
              </ol>
            `*/);
                _container = _container.add(_elements);
                _this._modalBody.empty().append(_container);
                _this._modal.modal('show');
            });
        },
        generateUrlWithPaginator: function (baseUrl, paginator) {
            baseUrl += '?';
            for (var prop in paginator) {
                if (paginator[prop]) {
                    baseUrl += prop + "=" + paginator[prop] + "&";
                }
            }
            return baseUrl.substr(0, baseUrl.length - 1);
        },
        makeDetailsColumn: function (row, index) {
            var container = $("<div class=\"btn-group btn-group-toggle\" data-toggle=\"buttons\">                          \n                           </div>");
            container.data('row', row);
            var showDrugInfoBtn = $("<button class=\"btn btn-info tbShowDrgInfoBtn\"}>\n                                       <i class=\"fa fa-info\"></i> \n                                      \u0628\u064A\u0627\u0646\u0627\u062A \u0627\u0644\u0631\u0627\u0643\u062F\n                                    </button>");
            var showRequesterPharmaBtn = $("<button class=\"btn btn-info tbShowRequisterPharmaInfoBtn\"}> \n                                       <i class=\"fa fa-info\"></i> \n                                      \u0628\u064A\u0627\u0646\u0627\u062A \u0627\u0644\u0635\u064A\u062F\u0644\u0629(\u0627\u0644\u0637\u0627\u0644\u0628\u0629)\n                                    </button>");
            showRequesterPharmaBtn.css({ marginRight: 2 });
            var showOwnerPharmaBtn = $("<button class=\"btn btn-info tbShowOwnerPharmaInfoBtn\"}>\n                                       <i class=\"fa fa-info\"></i> \n                                      \u0628\u064A\u0627\u0646\u0627\u062A \u0627\u0644\u0635\u064A\u062F\u0644\u064A\u0629(\u062D\u0627\u0648\u064A\u0629 \u0627\u0644\u0631\u0627\u0643\u062F)\n                                    </button>");
            showOwnerPharmaBtn.css({ marginRight: 2 });
            var showAllReqInfoBtnBtn = $("<button class=\"btn btn-info d-none  tbShowAllReqInfoBtn\"}>\n                                       <i class=\"fa fa-info\"></i> \n                                      \u0628\u064A\u0627\u0646\u0627\u062A \u0639\u0646 \u0647\u0630\u0627 \u0627\u0644\u0637\u0644\u0628\n                                    </button>");
            showAllReqInfoBtnBtn.css({ marginRight: 2 });
            container.append([showDrugInfoBtn, showRequesterPharmaBtn, showOwnerPharmaBtn, showAllReqInfoBtnBtn]);
            return container.get(0).outerHTML;
        },
        handleMainTable: function () {
            var _this_1 = this;
            var _this = this;
            $.get(this.generateUrlWithPaginator(LzDrgReqConfig._apiUrl, this._paginator), {}, function (d, f, req) {
                _this_1._currentPagination = JSON.parse(req.getResponseHeader('X-Pagination'));
                _this_1.updatePaginiationUi(_this_1._currentPagination);
            })
                .done(function (data) {
                LzDrgReqConfig._dataTable = _this_1._mainTable.DataTable({
                    data: data,
                    columnDefs: [
                        {
                            targets: 0,
                            data: 'lzDrugName'
                        },
                        {
                            orderable: false,
                            targets: 1,
                            data: 'owenerPh_Name'
                        },
                        {
                            orderable: false,
                            targets: 2,
                            data: 'requesterPhram_Name'
                        },
                        {
                            orderable: false,
                            targets: 3,
                            data: "status",
                            render: function (status, type, row) {
                                var _class = status == LzDrugsReqsStatus.Accepted
                                    ? "primary"
                                    : status == LzDrugsReqsStatus.Pending ? "secondary"
                                        : status == LzDrugsReqsStatus.AcceptedForAnotherOne ? "danger"
                                            : status == LzDrugsReqsStatus.AtNegotioation
                                                ? "info"
                                                : status == LzDrugsReqsStatus.Rejected
                                                    ? "danger"
                                                    : status == LzDrugsReqsStatus.Completed
                                                        ? "success" : "secondary";
                                var container = $("<div><span class=\"badge badge-" + _class + "\">" + _this._getPharmaStatus(status) + "</span></div>");
                                return container.get(0).outerHTML;
                            }
                        },
                        {
                            orderable: false,
                            targets: 4,
                            data: null,
                            render: function (row, type, d, meta) {
                                return _this.makeDetailsColumn(row, meta.row);
                            }
                        }
                    ],
                    //createdRow: _this.OnCreatedRow,
                    language: LzDrgReqConfig.languageSetting,
                    paging: false,
                    searching: false,
                    scrollX: true
                });
            })
                .catch(function (e) {
                alert('problem as getting data from server');
            })
                .always(function () {
                _this._tableLoading.toggleClass('d-none');
            });
        },
        updatePaginiationUi: function (pagingObj) {
            this._paginationUI.empty();
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
                this._paginationUI.empty();
                return;
            }
            this._paginationUI.append(container);
        },
        refreshTableData: function () {
            var _this_1 = this;
            var _this = this;
            this._tableLoading.toggleClass('d-none');
            $.get(this.generateUrlWithPaginator(LzDrgReqConfig._apiUrl, this._paginator), {}, function (d, f, req) {
                _this_1._currentPagination = JSON.parse(req.getResponseHeader('X-Pagination'));
                _this_1.updatePaginiationUi(_this_1._currentPagination);
            })
                .done(function (data) {
                LzDrgReqConfig._dataTable.clear().draw();
                LzDrgReqConfig._dataTable.rows.add(data);
                LzDrgReqConfig._dataTable.columns.adjust().draw();
            })
                .catch(function (e) {
                alert('problem as getting data from server');
            })
                .always(function () {
                _this._tableLoading.toggleClass('d-none');
            });
        },
        handleOnPaging: function () {
            var _this = this;
            $(document).on('click', '#paginationUI .page-item', function (e) {
                var el = $(this);
                if (el.attr('disabled'))
                    return;
                var pageNumber = el.data('page');
                if (pageNumber == 0)
                    _this._paginator.pageNumber -= 1;
                else if (pageNumber == -1)
                    _this._paginator.pageNumber += 1;
                else
                    _this._paginator.pageNumber = pageNumber;
                _this.refreshTableData();
            });
        },
        start: function () {
            this.handleMainTable();
            this.handleOnPaging();
            this.handleDetailsColumnActions();
            this.handleOnSelectReqStatus();
            this.handleSelectPageSize();
        }
    }.start();
})();
//# sourceMappingURL=LzDrugsReqsPageScripts.js.map