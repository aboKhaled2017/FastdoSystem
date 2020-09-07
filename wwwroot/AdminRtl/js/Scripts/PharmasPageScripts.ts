

enum PharmaControlActionType {
    remove,
    disable,
    activate
}
(() => {
    const pharmaConfig = {
        _apiUrl: '/api/admins/pharmacies',
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
        _serverParams: {

        }
    }
    const mainOperationsOfPharmasPage = {
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
        } as IPaginator,
        initWork() {
            this.handleConfirmModal();
            //this.handleColumnControlsBtns();
        },
        onRemovePharmacyConfirmed() { },
        handleConfirmModal() {
            this._confirmModal.data('agree', false);
            this._confirmModal.on('hide.bs.modal', e => {

            });
            this._confirmModal.find('.modal-footer .cancel').eq(0).click(e => {
                this._confirmModal.data('agree', false);
                this.onRemovePharmacyConfirmed();
            });
            this._confirmModal.find('.modal-footer .agree').eq(0).click(e => {
                this._confirmModal.data('agree', true);
                this.onRemovePharmacyConfirmed();
            });
        },
        _currentPagination: {} as IX_Pagination,

        handleSearch() {
            const _this = this;
            this._pharmacySearchInp.keyup(helperFunctions.delay(function (e: any) {
                var text = (_this._pharmacySearchInp.val() as string).trim();
                _this._paginator.pageNumber = 1;
                _this._paginator.s = text;
                _this.refreshTableData();
            }, 700) as any);
            /*this._pharmacySearchInp.keyup(e => {
                var text = (this._pharmacySearchInp.val() as string).trim();
                this._paginator.s = text;
                this.refreshTableData();
            });*/
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
                const rowData = pharmaConfig._dataTable.row(index).data() as IPharmaData;
                const imgEl = $(`<img src="${rowData.licenseImgSrc}" class="modalImageInBody"/>`);
                _this._modalBody.empty().append(imgEl);
                (_this._modal as any).modal('show');
            });
            $(document).on('click', '.tbShowCommerImgBtn', function () {
                _this._modalTitle.text("صورة التسجيل التجارى");
                const $btn = $(this);
                const index = $btn.parents('tr').index();
                const rowData = pharmaConfig._dataTable.row(index).data() as IPharmaData;
                const imgEl = $(`<img src="${rowData.commercialRegImgSrc}" class="modalImageInBody"/>`);
                _this._modalBody.empty().append(imgEl);
                (_this._modal as any).modal('show');
            });
            $(document).on('click', '.tbShowAllInfoBtn', function () {
                const $btn = $(this);
                const index = $btn.parents('tr').index();
                const rowData = pharmaConfig._dataTable.row(index).data() as IPharmaData;
                _this._modalTitle.text(`كل بيانات صيدلية/${rowData.name}`);
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
                <li class="breadcrumb-item"><span class="badge badge-info"> الحالة </span>: ${PharmaHelperFunctions.getPharmaStatus(rowData.status)}</li>
              </ol>
              <ol class="breadcrumb">
                <li cla<li class="breadcrumb-item"><span class="badge badge-info"> المخازن المنضمة اليها </span>: ${helperFunctions.convertValToTextIfZero(rowData.joinedStocksCount)}</li>
              </ol>
              <ol class="breadcrumb">
                <li class="breadcrumb-item"><span class="badge badge-info"> عدد عروض الرواكد </span>: ${helperFunctions.convertValToTextIfZero(rowData.lzDrugsCount)}</li>
              </ol>
              <ol class="breadcrumb">
                <li cla<li class="breadcrumb-item"><span class="badge badge-info"> عدد الطلبات التى قامت بها </span>: ${helperFunctions.convertValToTextIfZero(rowData.requestedDrugsCount)}</li>
              </ol>
            `);
                _container = _container.add(_elements);
                _this._modalBody.empty().append(_container);
                (_this._modal as any).modal('show');
            })
        },
        handleTableControlsAction() {
            const _this = this;
            $(document).on('click', '.handlePharmaReqBtn', function () {
                const $btn = $(this);
                let actionType: PharmaControlActionType = PharmaControlActionType.activate;
                const index = $btn.parents('tr').index();
                const rowData = pharmaConfig._dataTable.row(index).data() as IPharmaData;
                let newStatus = PharmaReqStatus.Pending;
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
                const execute = () => {
                    $btn.setLoading($btn.data('icon'));
                    $.ajax({
                        url: `${pharmaConfig._apiUrl}/${rowData.id}`,
                        method: 'PATCH',
                        data: JSON.stringify([{ "path": "/Status", "op": "replace", "value": newStatus }])
                    }).done(() => {
                        rowData.status = newStatus;

                        (pharmaConfig._dataTable).row(index).data(rowData).draw();
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
                        url: `${pharmaConfig._apiUrl}/${rowData.id}`,
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
                if (actionType == PharmaControlActionType.remove) {
                    (_this._confirmModal as any).modal('show');
                    _this.onRemovePharmacyConfirmed = () => {
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
        makeBtnGroupBadedOnStatus(row: IPharmaData) {
            const { status } = row;
            let container = $(`<div class="btn-group btn-group-toggle" data-toggle="buttons">                          
                           </div>`);
            const executeRejectBtn = $(`<button class="btn btn-danger handlePharmaReqBtn" data-icon="fa-remove" ${status == PharmaReqStatus.Rejected ? "disabled" : ""}>
                                       <i class="fa fa-remove"></i> 
                                      رفض
                                    </button>`);
            const executeDisableBtn = $(`<button class="btn btn-warning handlePharmaReqBtn" data-icon="fa-pause" ${status == PharmaReqStatus.Disabled ? "disabled" : ""}> 
                                       <i class="fa fa-pause"></i> 
                                      تعطيل
                                    </button>`);
            const executeActiveBtn = $(`<button class="btn btn-success handlePharmaReqBtn" data-icon="fa-check" ${status == PharmaReqStatus.Accepted ? "disabled" : ""}>
                                       <i class="fa fa-check"></i> 
                                      تفعيل
                                    </button>`);

            container.append([executeActiveBtn, executeDisableBtn, executeRejectBtn])
            return container.get(0).outerHTML;
        },
        makeDetailsColumn(row: IPharmaData, index: number) {
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
            $.get(this.generateUrlWithPaginator(pharmaConfig._apiUrl, this._paginator), {}, (d, f, req) => {
                this._currentPagination = JSON.parse(req.getResponseHeader('X-Pagination') as string) as IX_Pagination;
                this.updatePaginiationUi(this._currentPagination);
            })
                .done((data: IPharmaData[]) => {
                    pharmaConfig._dataTable = this._mainTable.DataTable({
                        data: data,
                        columnDefs: [
                            {
                                targets: 0,
                                data: null,
                                orderable: false,
                                render(value: any, type: string, row: IPharmaData, meta) {

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
                                render(val, type, row: IPharmaData) {
                                    const text = row.status == PharmaReqStatus.Accepted
                                        ? "مفعل"
                                        : row.status == PharmaReqStatus.Pending ? "منتظر"
                                            : row.status == PharmaReqStatus.Disabled ? "موقوف"
                                                : "مرفوض";
                                    const _class = row.status == PharmaReqStatus.Accepted
                                        ? "success"
                                        : row.status == PharmaReqStatus.Pending ? "secondary"
                                            : row.status == PharmaReqStatus.Disabled ? "warning"
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

                                render(row: IPharmaData, type: string, d, meta) {
                                    return _this.makeDetailsColumn(row, meta.row);
                                }
                            }
                        ] as DataTables.ColumnDefsSettings[],
                        //createdRow: _this.OnCreatedRow,
                        language: pharmaConfig.languageSetting,
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
            $.get(this.generateUrlWithPaginator(pharmaConfig._apiUrl, this._paginator), {}, (d, f, req) => {
                this._currentPagination = JSON.parse(req.getResponseHeader('X-Pagination') as string) as IX_Pagination;
                this.updatePaginiationUi(this._currentPagination);
            })
                .done((data: IPharmaData[]) => {
                    pharmaConfig._dataTable.clear().draw();
                    pharmaConfig._dataTable.rows.add(data);
                    pharmaConfig._dataTable.columns.adjust().draw();

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