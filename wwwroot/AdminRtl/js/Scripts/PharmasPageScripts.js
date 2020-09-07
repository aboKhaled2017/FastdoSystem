"use strict";
var PharmaControlActionType;
(function (PharmaControlActionType) {
    PharmaControlActionType[PharmaControlActionType["remove"] = 0] = "remove";
    PharmaControlActionType[PharmaControlActionType["disable"] = 1] = "disable";
    PharmaControlActionType[PharmaControlActionType["activate"] = 2] = "activate";
})(PharmaControlActionType || (PharmaControlActionType = {}));
(function () {
    var pharmaConfig = {
        _apiUrl: '/api/admins/pharmacies',
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
    var mainOperationsOfPharmasPage = {
        _mainTable: $('#PharmaciesTable'),
        _paginationUI: $('#paginationUI'),
        _pageSizeSelectInp: $('#pageSizeSelectInp'),
        _modal: $('#modal'),
        _modalTitle: $('#modal .modal-title'),
        _modalBody: $('#modal .modal-body'),
        _confirmModal: $('#confirmModal'),
        _tableLoading: $('.pharmaciesPageSection .ContainerOverlay').eq(0),
        _confirmModalLoading: $('#confirmModal .ContainerOverlay').eq(0),
        _pharmacySearchInp: $('#dtSearchInp'),
        _paginator: {
            pageNumber: 1,
            pageSize: 5,
        },
        initWork: function () {
            this.handleConfirmModal();
            //this.handleColumnControlsBtns();
        },
        onRemovePharmacyConfirmed: function () { },
        handleConfirmModal: function () {
            var _this_1 = this;
            this._confirmModal.data('agree', false);
            this._confirmModal.on('hide.bs.modal', function (e) {
            });
            this._confirmModal.find('.modal-footer .cancel').eq(0).click(function (e) {
                _this_1._confirmModal.data('agree', false);
                _this_1.onRemovePharmacyConfirmed();
            });
            this._confirmModal.find('.modal-footer .agree').eq(0).click(function (e) {
                _this_1._confirmModal.data('agree', true);
                _this_1.onRemovePharmacyConfirmed();
            });
        },
        _currentPagination: {},
        handleSearch: function () {
            var _this = this;
            this._pharmacySearchInp.keyup(helperFunctions.delay(function (e) {
                var text = _this._pharmacySearchInp.val().trim();
                _this._paginator.pageNumber = 1;
                _this._paginator.s = text;
                _this.refreshTableData();
            }, 700));
            /*this._pharmacySearchInp.keyup(e => {
                var text = (this._pharmacySearchInp.val() as string).trim();
                this._paginator.s = text;
                this.refreshTableData();
            });*/
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
        handleDetailsColumnActions: function () {
            var _this = this;
            $(document).on('click', '.tbShowLicenseImgBtn', function () {
                _this._modalTitle.text("صورة الترخيص");
                var $btn = $(this);
                var index = $btn.parents('tr').index();
                var rowData = pharmaConfig._dataTable.row(index).data();
                var imgEl = $("<img src=\"" + rowData.licenseImgSrc + "\" class=\"modalImageInBody\"/>");
                _this._modalBody.empty().append(imgEl);
                _this._modal.modal('show');
            });
            $(document).on('click', '.tbShowCommerImgBtn', function () {
                _this._modalTitle.text("صورة التسجيل التجارى");
                var $btn = $(this);
                var index = $btn.parents('tr').index();
                var rowData = pharmaConfig._dataTable.row(index).data();
                var imgEl = $("<img src=\"" + rowData.commercialRegImgSrc + "\" class=\"modalImageInBody\"/>");
                _this._modalBody.empty().append(imgEl);
                _this._modal.modal('show');
            });
            $(document).on('click', '.tbShowAllInfoBtn', function () {
                var $btn = $(this);
                var index = $btn.parents('tr').index();
                var rowData = pharmaConfig._dataTable.row(index).data();
                _this._modalTitle.text("\u0643\u0644 \u0628\u064A\u0627\u0646\u0627\u062A \u0635\u064A\u062F\u0644\u064A\u0629/" + rowData.name);
                var _container = $();
                var _elements = $("\n              <ol class=\"breadcrumb\">\n                <li class=\"breadcrumb-item\"><span class=\"badge badge-info\">\u0627\u0644\u0627\u0633\u0645</span> : " + rowData.name + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li class=\"breadcrumb-item\"><span class=\"badge badge-info\">\u0627\u0644\u0628\u0631\u064A\u062F \u0627\u0644\u0627\u0644\u0643\u062A\u0631\u0648\u0646\u0649</span> : " + rowData.email + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li class=\"breadcrumb-item\"><span class=\"badge badge-info\">\u0627\u0633\u0645 \u0627\u0644\u0645\u0627\u0644\u0643 </span>: " + rowData.ownerName + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li class=\"breadcrumb-item\"><span class=\"badge badge-info\">\u0627\u0633\u0645 \u0627\u0644\u0645\u062F\u064A\u0631 </span>: " + rowData.mgrName + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li cla<li class=\"breadcrumb-item\"><span class=\"badge badge-info\">\u0627\u0644\u0639\u0646\u0648\u0627\u0646</span>: " + rowData.address + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li class=\"breadcrumb-item\"><span class=\"badge badge-info\"> \u0627\u0644\u062A\u0644\u064A\u0641\u0648\u0646 \u0627\u0644\u0627\u0631\u0636\u0649 </span>: " + rowData.landlinePhone + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li class=\"breadcrumb-item\"><span class=\"badge badge-info\"> \u0627\u0644\u062A\u0644\u064A\u0641\u0648\u0646 \u0627\u0644\u0645\u062D\u0645\u0648\u0644 </span>: " + rowData.persPhone + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li class=\"breadcrumb-item\"><span class=\"badge badge-info\"> \u0627\u0644\u062D\u0627\u0644\u0629 </span>: " + PharmaHelperFunctions.getPharmaStatus(rowData.status) + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li cla<li class=\"breadcrumb-item\"><span class=\"badge badge-info\"> \u0627\u0644\u0645\u062E\u0627\u0632\u0646 \u0627\u0644\u0645\u0646\u0636\u0645\u0629 \u0627\u0644\u064A\u0647\u0627 </span>: " + helperFunctions.convertValToTextIfZero(rowData.joinedStocksCount) + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li class=\"breadcrumb-item\"><span class=\"badge badge-info\"> \u0639\u062F\u062F \u0639\u0631\u0648\u0636 \u0627\u0644\u0631\u0648\u0627\u0643\u062F </span>: " + helperFunctions.convertValToTextIfZero(rowData.lzDrugsCount) + "</li>\n              </ol>\n              <ol class=\"breadcrumb\">\n                <li cla<li class=\"breadcrumb-item\"><span class=\"badge badge-info\"> \u0639\u062F\u062F \u0627\u0644\u0637\u0644\u0628\u0627\u062A \u0627\u0644\u062A\u0649 \u0642\u0627\u0645\u062A \u0628\u0647\u0627 </span>: " + helperFunctions.convertValToTextIfZero(rowData.requestedDrugsCount) + "</li>\n              </ol>\n            ");
                _container = _container.add(_elements);
                _this._modalBody.empty().append(_container);
                _this._modal.modal('show');
            });
        },
        handleTableControlsAction: function () {
            var _this = this;
            $(document).on('click', '.handlePharmaReqBtn', function () {
                var $btn = $(this);
                var actionType = PharmaControlActionType.activate;
                var index = $btn.parents('tr').index();
                var rowData = pharmaConfig._dataTable.row(index).data();
                var newStatus = PharmaReqStatus.Pending;
                if ($btn.data('icon') == 'fa-remove') {
                    newStatus = PharmaReqStatus.Rejected;
                    actionType = PharmaControlActionType.remove;
                }
                else if ($btn.data('icon') == 'fa-pause') {
                    newStatus = PharmaReqStatus.Disabled;
                    actionType = PharmaControlActionType.disable;
                }
                else if ($btn.data('icon') == 'fa-check') {
                    newStatus = PharmaReqStatus.Accepted;
                    actionType = PharmaControlActionType.activate;
                }
                var execute = function () {
                    $btn.setLoading($btn.data('icon'));
                    $.ajax({
                        url: pharmaConfig._apiUrl + "/" + rowData.id,
                        method: 'PATCH',
                        data: JSON.stringify([{ "path": "/Status", "op": "replace", "value": newStatus }])
                    }).done(function () {
                        rowData.status = newStatus;
                        (pharmaConfig._dataTable).row(index).data(rowData).draw();
                    }).catch(function (e) {
                        alert('مشكلة اثناء معالجة البيانات فى السرفر');
                    }).always(function () {
                        $btn.removeLoading();
                    });
                };
                var deletePharmacyRecord = function () {
                    $btn.setLoading($btn.data('icon'));
                    _this._confirmModalLoading.toggleClass('d-none');
                    $.ajax({
                        url: pharmaConfig._apiUrl + "/" + rowData.id,
                        method: 'DELETE'
                    }).done(function () {
                        _this.refreshTableData();
                    }).catch(function (e) {
                        alert('مشكلة اثناء معالجة البيانات فى السرفر');
                    }).always(function () {
                        $btn.removeLoading();
                        _this._confirmModalLoading.toggleClass('d-none');
                        _this._confirmModal.modal('hide');
                    });
                };
                if (actionType == PharmaControlActionType.remove) {
                    _this._confirmModal.modal('show');
                    _this.onRemovePharmacyConfirmed = function () {
                        var isAgree = _this._confirmModal.data('agree');
                        _this._confirmModal.data('agree', false);
                        if (isAgree) {
                            deletePharmacyRecord();
                        }
                    };
                }
                else
                    execute();
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
        makeBtnGroupBadedOnStatus: function (row) {
            var status = row.status;
            var container = $("<div class=\"btn-group btn-group-toggle\" data-toggle=\"buttons\">                          \n                           </div>");
            var executeRejectBtn = $("<button class=\"btn btn-danger handlePharmaReqBtn\" data-icon=\"fa-remove\" " + (status == PharmaReqStatus.Rejected ? "disabled" : "") + ">\n                                       <i class=\"fa fa-remove\"></i> \n                                      \u0631\u0641\u0636\n                                    </button>");
            var executeDisableBtn = $("<button class=\"btn btn-warning handlePharmaReqBtn\" data-icon=\"fa-pause\" " + (status == PharmaReqStatus.Disabled ? "disabled" : "") + "> \n                                       <i class=\"fa fa-pause\"></i> \n                                      \u062A\u0639\u0637\u064A\u0644\n                                    </button>");
            var executeActiveBtn = $("<button class=\"btn btn-success handlePharmaReqBtn\" data-icon=\"fa-check\" " + (status == PharmaReqStatus.Accepted ? "disabled" : "") + ">\n                                       <i class=\"fa fa-check\"></i> \n                                      \u062A\u0641\u0639\u064A\u0644\n                                    </button>");
            container.append([executeActiveBtn, executeDisableBtn, executeRejectBtn]);
            return container.get(0).outerHTML;
        },
        makeDetailsColumn: function (row, index) {
            var container = $("<div class=\"btn-group btn-group-toggle\" data-toggle=\"buttons\">                          \n                           </div>");
            container.data('row', row);
            var showLicenseImgBtn = $("<button class=\"btn btn-primary tbShowLicenseImgBtn\"}>\n                                       <i class=\"fa fa-picture-o\"></i> \n                                      \u0635\u0648\u0631\u0629\u0627\u0644\u062A\u0631\u062E\u064A\u0635\n                                    </button>");
            var showCommertialImgBtn = $("<button class=\"btn btn-primary tbShowCommerImgBtn\"}> \n                                       <i class=\"fa fa-picture-o\"></i> \n                                      \u0635\u0648\u0631\u0629 \u0627\u0644\u062A\u0633\u062C\u064A\u0644 \u0627\u0644\u062A\u062C\u0627\u0631\u0649\n                                    </button>");
            showCommertialImgBtn.css({ marginRight: 2 });
            var showAllInfoBtn = $("<button class=\"btn btn-info tbShowAllInfoBtn\"}>\n                                       <i class=\"fa fa-info\"></i> \n                                      \u0643\u0644 \u0627\u0644\u0628\u064A\u0627\u0646\u0627\u062A\n                                    </button>");
            showAllInfoBtn.css({ marginRight: 2 });
            container.append([showLicenseImgBtn, showCommertialImgBtn, showAllInfoBtn]);
            return container.get(0).outerHTML;
        },
        handleMainTable: function () {
            var _this_1 = this;
            var _this = this;
            $.get(this.generateUrlWithPaginator(pharmaConfig._apiUrl, this._paginator), {}, function (d, f, req) {
                _this_1._currentPagination = JSON.parse(req.getResponseHeader('X-Pagination'));
                _this_1.updatePaginiationUi(_this_1._currentPagination);
            })
                .done(function (data) {
                pharmaConfig._dataTable = _this_1._mainTable.DataTable({
                    data: data,
                    columnDefs: [
                        {
                            targets: 0,
                            data: null,
                            orderable: false,
                            render: function (value, type, row, meta) {
                                return _this.makeBtnGroupBadedOnStatus(row);
                            }
                        },
                        {
                            targets: 1,
                            data: 'name'
                        },
                        {
                            orderable: false,
                            targets: 2,
                            data: 'persPhone'
                        },
                        {
                            orderable: false,
                            targets: 3,
                            data: "status",
                            render: function (val, type, row) {
                                var text = row.status == PharmaReqStatus.Accepted
                                    ? "مفعل"
                                    : row.status == PharmaReqStatus.Pending ? "منتظر"
                                        : row.status == PharmaReqStatus.Disabled ? "موقوف"
                                            : "مرفوض";
                                var _class = row.status == PharmaReqStatus.Accepted
                                    ? "success"
                                    : row.status == PharmaReqStatus.Pending ? "secondary"
                                        : row.status == PharmaReqStatus.Disabled ? "warning"
                                            : "danger";
                                var container = $("<div><span class=\"badge badge-" + _class + "\">" + text + "</span></div>");
                                return container.get(0).outerHTML;
                            }
                        },
                        {
                            orderable: false,
                            targets: 4,
                            data: 'address',
                            className: "noWrap"
                        },
                        {
                            orderable: false,
                            targets: 5,
                            data: null,
                            render: function (row, type, d, meta) {
                                return _this.makeDetailsColumn(row, meta.row);
                            }
                        }
                    ],
                    //createdRow: _this.OnCreatedRow,
                    language: pharmaConfig.languageSetting,
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
            $.get(this.generateUrlWithPaginator(pharmaConfig._apiUrl, this._paginator), {}, function (d, f, req) {
                _this_1._currentPagination = JSON.parse(req.getResponseHeader('X-Pagination'));
                _this_1.updatePaginiationUi(_this_1._currentPagination);
            })
                .done(function (data) {
                pharmaConfig._dataTable.clear().draw();
                pharmaConfig._dataTable.rows.add(data);
                pharmaConfig._dataTable.columns.adjust().draw();
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
            this.initWork();
            this.handleMainTable();
            this.handleOnPaging();
            this.handleDetailsColumnActions();
            this.handleTableControlsAction();
            this.handleSearch();
            this.handleSelectPageSize();
        }
    }.start();
})();
//# sourceMappingURL=PharmasPageScripts.js.map