

enum StkControlActionType {
    remove,
    disable,
    activate
}
enum StockReqStatus {
    Pending,
    Accepted,
    Rejected,
    Disabled
}
interface IStockData {
    id: string
    email: string
    name: string
    mgrName: string
    ownerName: string
    persPhone: string
    landlinePhone: string
    licenseImgSrc: string
    commercialRegImgSrc: string
    status: StockReqStatus
    address: string
    areaId: number
    joinedPharmesCount: number
    drugsCount:number
}
(() => {
    const stkConfig = {
        _target: 'STOCK',
        _apiUrl: '/api/admins/stocks',
        _dataTable: null as any as DataTables.DataTable,
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
        } as DataTables.LanguageSettings,

    }
    const mainOperationsOfStockPage = {
        _mainTable: $('#StockTable'),
        _paginationUI: $('#paginationUI'),
        _modal: $('#modal'),
        _modalTitle: $('#modal .modal-title'),
        _modalBody: $('#modal .modal-body'),
        _confirmModal: $('#confirmModal'),
        _tableLoading: $('.stocksPageSection .ContainerOverlay').eq(0),
        _confirmModalLoading: $('#confirmModal .ContainerOverlay').eq(0),
        _stockSearchInp: $('#dtSearchInp'),
        _pageSizeSelectInp: $('#pageSizeSelectInp'),
        _paginator: {
            pageNumber: 1,
            pageSize: 5,
        } as IPaginator,
        initWork() {
            this.handleConfirmModal();
            //this.handleColumnControlsBtns();
        },
        onRemoveStockConfirmed() { },
        handleConfirmModal() {
            this._confirmModal.data('agree', false);
            this._confirmModal.on('hide.bs.modal', e => {

            });
            this._confirmModal.find('.modal-footer .cancel').eq(0).click(e => {
                this._confirmModal.data('agree', false);
                this.onRemoveStockConfirmed();
            });
            this._confirmModal.find('.modal-footer .agree').eq(0).click(e => {
                this._confirmModal.data('agree', true);
                this.onRemoveStockConfirmed();
            });
        },
        _currentPagination: {} as IX_Pagination,
        _getStkStatus(status: StockReqStatus) {
            return status == StockReqStatus.Accepted
                ? "نشطة الان"
                : status == StockReqStatus.Disabled
                    ? "موقوفة الان"
                    : status == StockReqStatus.Pending
                        ? "لم يتم معالجة الطلب من المسؤل حتى الان"
                        : status == StockReqStatus.Rejected
                            ? "تم رفض الطلب" : "نشطة الان";
        },
        handleSearch() {
            const _this = this;
            this._stockSearchInp.keyup(helperFunctions.delay(function (e: any) {
                var text = (_this._stockSearchInp.val() as string).trim();
                _this._paginator.pageNumber = 1;
                _this._paginator.s = text;
                _this.refreshTableData();
            }, 700) as any);
        },
        handleSelectPageSize() {
            this._pageSizeSelectInp.change(e => {
                let pageSize = this._pageSizeSelectInp.val() as number;
                this._paginator.pageNumber = 1;
                this._paginator.pageSize = pageSize;
                this.refreshTableData();
            });
        },
        handleDetailsColumnActions() {
            const _this = this;
            $(document).on('click', '.tbShowLicenseImgBtn', function () {
                _this._modalTitle.text("صورة الترخيص");
                const $btn = $(this);
                const index = $btn.parents('tr').index();
                const rowData = stkConfig._dataTable.row(index).data() as IStockData;
                const imgEl = $(`<img src="${rowData.licenseImgSrc}" class="modalImageInBody"/>`);
                _this._modalBody.empty().append(imgEl);
                (_this._modal as any).modal('show');
            });
            $(document).on('click', '.tbShowCommerImgBtn', function () {
                _this._modalTitle.text("صورة التسجيل التجارى");
                const $btn = $(this);
                const index = $btn.parents('tr').index();
                const rowData = stkConfig._dataTable.row(index).data() as IStockData;
                const imgEl = $(`<img src="${rowData.commercialRegImgSrc}" class="modalImageInBody"/>`);
                _this._modalBody.empty().append(imgEl);
                (_this._modal as any).modal('show');
            });
            $(document).on('click', '.tbShowAllInfoBtn', function () {
                const $btn = $(this);
                const index = $btn.parents('tr').index();
                const rowData = stkConfig._dataTable.row(index).data() as IStockData;
                _this._modalTitle.text(`كل بيانات المخزن/${rowData.name}`);
                let _container = $();
                const _elements = $(`
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
                <li class="breadcrumb-item"><span class="badge badge-info"> الحالة </span>: ${_this._getStkStatus(rowData.status)}</li>
              </ol>
              <ol class="breadcrumb">
                <li cla<li class="breadcrumb-item"><span class="badge badge-info"> الصيدليات المنضمة اليها </span>: ${helperFunctions.convertValToTextIfZero(rowData.joinedPharmesCount)}</li>
              </ol>
              <ol class="breadcrumb">
                <li cla<li class="breadcrumb-item"><span class="badge badge-info">عدد منتجات العقاقير المتوفرة</span>: ${helperFunctions.convertValToTextIfZero(rowData.drugsCount)}</li>
              </ol>
            `);
                _container = _container.add(_elements);
                _this._modalBody.empty().append(_container);
                (_this._modal as any).modal('show');
            })
        },
        handleTableControlsAction() {
            const _this = this;
            $(document).on('click', '.handleStockReqBtn', function () {
                const $btn = $(this);
                let actionType: StkControlActionType = StkControlActionType.activate;
                const index = $btn.parents('tr').index();
                const rowData = stkConfig._dataTable.row(index).data() as IStockData;
                let newStatus = StockReqStatus.Pending;
                if ($btn.data('icon') == 'fa-remove') {
                    newStatus = StockReqStatus.Rejected;
                    actionType = StkControlActionType.remove;
                }
                else if ($btn.data('icon') == 'fa-pause') {
                    newStatus = StockReqStatus.Disabled;
                    actionType = StkControlActionType.disable;
                }
                else if ($btn.data('icon') == 'fa-check') {
                    newStatus = StockReqStatus.Accepted;
                    actionType = StkControlActionType.activate;
                }
                const execute = () => {
                    $btn.setLoading($btn.data('icon'));
                    $.ajax({
                        url: `${stkConfig._apiUrl}/${rowData.id}`,
                        method: 'PATCH',
                        data: JSON.stringify([{ "path": "/Status", "op": "replace", "value": newStatus }])
                    }).done(() => {
                        rowData.status = newStatus;

                        (stkConfig._dataTable).row(index).data(rowData).draw();
                    }).catch(e => {
                        alert('مشكلة اثناء معالجة البيانات فى السرفر')
                    }).always(() => {
                        $btn.removeLoading();
                    })
                };
                const deletePharmacyRecord = () => {
                    $btn.setLoading($btn.data('icon'));
                    _this._confirmModalLoading.toggleClass('d-none');
                    $.ajax({
                        url: `${stkConfig._apiUrl}/${rowData.id}`,
                        method: 'DELETE'
                    }).done(() => {
                        _this.refreshTableData();

                    }).catch(e => {
                        alert('مشكلة اثناء معالجة البيانات فى السرفر')
                    }).always(() => {
                        $btn.removeLoading();
                        _this._confirmModalLoading.toggleClass('d-none');
                        (_this._confirmModal as any).modal('hide');
                    })
                };
                if (actionType == StkControlActionType.remove) {
                    (_this._confirmModal as any).modal('show');
                    _this.onRemoveStockConfirmed = () => {
                        var isAgree = _this._confirmModal.data('agree');
                        _this._confirmModal.data('agree', false);
                        if (isAgree) {
                            deletePharmacyRecord();
                        }
                    }
                }
                else execute();
            });
        },
        generateUrlWithPaginator(baseUrl: string, paginator: IPaginator) {
            baseUrl += '?';
            for (let prop in paginator) {
                if ((paginator as any)[prop]) {
                    baseUrl += `${prop}=${(paginator as any)[prop]}&`;
                }
            }
            return baseUrl.substr(0, baseUrl.length - 1);
        },
        makeBtnGroupBadedOnStatus(row: IStockData) {
            const { status } = row;
            let container = $(`<div class="btn-group btn-group-toggle" data-toggle="buttons">                          
                           </div>`);
            const executeRejectBtn = $(`<button class="btn btn-danger handleStockReqBtn" data-icon="fa-remove" ${status == StockReqStatus.Rejected ? "disabled" : ""}>
                                       <i class="fa fa-remove"></i> 
                                      رفض
                                    </button>`);
            const executeDisableBtn = $(`<button class="btn btn-warning handleStockReqBtn" data-icon="fa-pause" ${status == StockReqStatus.Disabled ? "disabled" : ""}> 
                                       <i class="fa fa-pause"></i> 
                                      تعطيل
                                    </button>`);
            const executeActiveBtn = $(`<button class="btn btn-success handleStockReqBtn" data-icon="fa-check" ${status == StockReqStatus.Accepted ? "disabled" : ""}>
                                       <i class="fa fa-check"></i> 
                                      تفعيل
                                    </button>`);

            container.append([executeActiveBtn, executeDisableBtn, executeRejectBtn])
            return container.get(0).outerHTML;
        },
        makeDetailsColumn(row: IStockData, index: number) {
            let container = $(`<div class="btn-group btn-group-toggle" data-toggle="buttons">                          
                           </div>`);
            container.data('row', row);
            const showLicenseImgBtn = $(`<button class="btn btn-primary tbShowLicenseImgBtn"}>
                                       <i class="fa fa-picture-o"></i> 
                                      صورةالترخيص
                                    </button>`);
            const showCommertialImgBtn = $(`<button class="btn btn-primary tbShowCommerImgBtn"}> 
                                       <i class="fa fa-picture-o"></i> 
                                      صورة التسجيل التجارى
                                    </button>`);
            showCommertialImgBtn.css({ marginRight: 2 });
            const showAllInfoBtn = $(`<button class="btn btn-info tbShowAllInfoBtn"}>
                                       <i class="fa fa-info"></i> 
                                      كل البيانات
                                    </button>`);
            showAllInfoBtn.css({ marginRight: 2 });

            container.append([showLicenseImgBtn, showCommertialImgBtn, showAllInfoBtn])
            return container.get(0).outerHTML;
        },
        handleMainTable() {
            const _this = this;
            $.get(this.generateUrlWithPaginator(stkConfig._apiUrl, this._paginator), {}, (d, f, req) => {
                this._currentPagination = JSON.parse(req.getResponseHeader('X-Pagination') as string) as IX_Pagination;
                this.updatePaginiationUi(this._currentPagination);
            })
                .done((data: IStockData[]) => {
                    stkConfig._dataTable = this._mainTable.DataTable({
                        data: data,
                        columnDefs: [
                            {
                                targets: 0,
                                data: null,
                                orderable: false,
                                render(value: any, type: string, row: IStockData, meta) {

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
                                render(val, type, row: IStockData) {
                                    const text = row.status == StockReqStatus.Accepted
                                        ? "مفعل"
                                        : row.status == StockReqStatus.Pending ? "منتظر"
                                            : row.status == StockReqStatus.Disabled ? "موقوف"
                                                : "مرفوض";
                                    const _class = row.status == StockReqStatus.Accepted
                                        ? "success"
                                        : row.status == StockReqStatus.Pending ? "secondary"
                                            : row.status == StockReqStatus.Disabled ? "warning"
                                                : "danger";
                                    const container = $(`<div><span class="badge badge-${_class}">${text}</span></div>`);
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

                                render(row: IStockData, type: string, d, meta) {
                                    return _this.makeDetailsColumn(row, meta.row);
                                }
                            }
                        ] as DataTables.ColumnDefsSettings[],
                        //createdRow: _this.OnCreatedRow,
                        language: stkConfig.languageSetting,
                        pageLength: _this._currentPagination.pageSize,
                        paging: false,
                        searching: false,
                        scrollX: true
                    })
                })
                .catch(e => {
                    alert('problem as getting data from server');
                })
                .always(() => {
                    _this._tableLoading.toggleClass('d-none');
                })
        },
        updatePaginiationUi(pagingObj: IX_Pagination) {
            this._paginationUI.empty();

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
                this._paginationUI.empty();
                return;
            }
            this._paginationUI.append(container);
        },
        refreshTableData() {
            const _this = this;
            this._tableLoading.toggleClass('d-none');
            $.get(this.generateUrlWithPaginator(stkConfig._apiUrl, this._paginator), {}, (d, f, req) => {
                this._currentPagination = JSON.parse(req.getResponseHeader('X-Pagination') as string) as IX_Pagination;
                this.updatePaginiationUi(this._currentPagination);
            })
                .done((data: IStockData[]) => {
                    stkConfig._dataTable.clear().draw();
                    stkConfig._dataTable.rows.add(data);
                    stkConfig._dataTable.columns.adjust().draw();

                })
                .catch(e => {
                    alert('problem as getting data from server');
                })
                .always(() => {
                    _this._tableLoading.toggleClass('d-none');
                });
        },
        handleOnPaging() {
            const _this = this;
            $(document).on('click', '#paginationUI .page-item', function (e) {
                const el = $(this);
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
            })
        },
        start() {
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