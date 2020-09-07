
interface I_LzDrugReq_Paginator extends IPaginator {
    status: LzDrugsReqsStatus
}
interface ILzDrugReqData {
    id: string
    lzDrugId:string
    requesterPhram_Id :string
    requesterPhram_Name :string
    owenerPh_Id :string
    owenerPh_Name:string
    status: LzDrugsReqsStatus
    lzDrugName:string 
}
enum LzDrugsReqsStatus {
    Pending,
    Accepted,
    Rejected,
    Completed,
    AtNegotioation,
    AcceptedForAnotherOne
}

(() => {
    const LzDrgReqConfig = {
        _apiUrl: '/api/admins/drgsReq',
        _getDrugInfoUrl: '/api/admins/drgs',
        _getPharmaInfoUrl: '/api/admins/pharmacies',
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
    const mainOperationsOfLzDrgsReqsPage = {
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
        } as I_LzDrugReq_Paginator,
        _currentPagination: {} as IX_Pagination,
        _getPharmaStatus(status: LzDrugsReqsStatus) {
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
        handleSearch() {
            const _this = this;
            this._dtSearchInp.keyup(helperFunctions.delay(function (e: any) {
                var text = (_this._dtSearchInp.val() as string).trim();
                _this._paginator.pageNumber = 1;
                _this._paginator.s = text;
                _this.refreshTableData();
            }, 700) as any);
            /*this._dtSearchInp.keyup(e => {
                var text = (this._dtSearchInp.val() as string).trim();
                this._paginator.s = text;
                this.refreshTableData();
            });*/
        },
        handleOnSelectReqStatus() {
            this._reqStatusSelectInp.change(e => {
                let status = this._reqStatusSelectInp.val() as LzDrugsReqsStatus;
                this._paginator.pageNumber = 1;
                this._paginator.status = status;
                this.refreshTableData();
            });
        },
        handleSelectPageSize() {
            this._pageSizeSelectInp.change(e => {
                let pageSize = this._pageSizeSelectInp.val() as number;
                this._paginator.pageNumber = 1;
                this._paginator.pageSize = pageSize;
                this.refreshTableData();
            });
        },
        getModalBodyHtmlForDrugInfo(rowData: IDrugInfo) {
            let _container = $();
            const _elements = $(`
              <ol class="breadcrumb">
                <li class="breadcrumb-item"><span class="badge badge-info">الاسم</span> : ${rowData.name}</li>
              </ol>
              <ol class="breadcrumb">
                <li class="breadcrumb-item"><span class="badge badge-info">الخصم</span> : ${drugHelperFunctions.getDiscount(rowData.discount)}</li>
              </ol>
              <ol class="breadcrumb">
                <li class="breadcrumb-item"><span class="badge badge-info">السعر</span> : ${rowData.price} جنية</li>
              </ol>
              <ol class="breadcrumb">
                <li class="breadcrumb-item"><span class="badge badge-info">نوع السعر </span> : ${drugHelperFunctions.getPriceType(rowData.priceType)}</li>
              </ol>
              <ol class="breadcrumb">
                <li cla<li class="breadcrumb-item"><span class="badge badge-info">الكمية</span> : ${rowData.quantity} ${drugHelperFunctions.getUnitType(rowData.unitType)}</li>
              </ol>
              <ol class="breadcrumb">
                <li class="breadcrumb-item"><span class="badge badge-info">النوع </span> : ${rowData.type}</li>
              </ol>
              <ol class="breadcrumb">
                <li class="breadcrumb-item"><span class="badge badge-info"> عدد الطلب على الراكد </span> : ${helperFunctions.convertValToTextIfZero(rowData.requestCount)}</li>
              </ol>             
            `);
            _container = _container.add(_elements);
            this._modalBody.empty().append(_container);
            (this._modal as any).modal('show');
        },
        getModalBodyHtmlForRequesterPharmaInfo(rowData: IPharmaData) {
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
            this._modalBody.empty().append(_container);
            (this._modal as any).modal('show');
        },
        handleDetailsColumnActions() {
            const _this = this;
            $(document).on('click', '.tbShowDrgInfoBtn', function () {
                const $btn = $(this);
                const index = $btn.parents('tr').index();
                const rowData = LzDrgReqConfig._dataTable.row(index).data() as ILzDrugReqData;
                _this._modalTitle.text(`كل بيانات /${rowData.lzDrugName}`);
                const data = $btn.data('drug') as IDrugInfo;
                if (!data) {
                    $btn.setLoading('fa-info');
                    $.get(`${LzDrgReqConfig._getDrugInfoUrl}/${rowData.lzDrugId}/details`)
                        .done(_data => {
                            _this.getModalBodyHtmlForDrugInfo(_data);
                            $btn.data('drug', _data);
                        })
                        .catch(e => {
                            helperFunctions.alertServerError();
                        })
                        .always(() => {
                            $btn.removeLoading();
                        })
                }
                else {
                    _this.getModalBodyHtmlForDrugInfo(data);
                }
            });
            $(document).on('click', '.tbShowRequisterPharmaInfoBtn', function () {
                const $btn = $(this);
                const index = $btn.parents('tr').index();
                const rowData = LzDrgReqConfig._dataTable.row(index).data() as ILzDrugReqData;
                _this._modalTitle.text(`كل بيانات /${rowData.requesterPhram_Name}`);
                const data = $btn.data('pharma') as IDrugInfo;
                if (!data) {
                    $btn.setLoading('fa-info');
                    $.get(`${LzDrgReqConfig._getPharmaInfoUrl}/${rowData.requesterPhram_Id}`)
                        .done(_data => {
                            _this.getModalBodyHtmlForRequesterPharmaInfo(_data);
                            $btn.data('pharma', _data);
                        })
                        .catch(e => {
                            helperFunctions.alertServerError();
                        })
                        .always(() => {
                            $btn.removeLoading();
                        })
                }
                else {
                    _this.getModalBodyHtmlForDrugInfo(data);
                }
            });
            $(document).on('click', '.tbShowOwnerPharmaInfoBtn', function () {
                const $btn = $(this);
                const index = $btn.parents('tr').index();
                const rowData = LzDrgReqConfig._dataTable.row(index).data() as ILzDrugReqData;
                _this._modalTitle.text(`كل بيانات /${rowData.owenerPh_Name}`);
                const data = $btn.data('pharma') as IDrugInfo;
                if (!data) {
                    $btn.setLoading('fa-info');
                    $.get(`${LzDrgReqConfig._getPharmaInfoUrl}/${rowData.owenerPh_Id}`)
                        .done(_data => {
                            _this.getModalBodyHtmlForRequesterPharmaInfo(_data);
                            $btn.data('pharma', _data);
                        })
                        .catch(e => {
                            helperFunctions.alertServerError();
                        })
                        .always(() => {
                            $btn.removeLoading();
                        })
                }
                else {
                    _this.getModalBodyHtmlForDrugInfo(data);
                }
            });
            $(document).on('click', '.tbShowAllReqInfoBtn', function () {
                const $btn = $(this);
                const index = $btn.parents('tr').index();
                const rowData = LzDrgReqConfig._dataTable.row(index).data() as ILzDrugReqData;
                _this._modalTitle.text(`كل بيانات /${rowData.lzDrugId}`);
                let _container = $();
                const _elements = $(/*`
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
                (_this._modal as any).modal('show');
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
        makeDetailsColumn(row: ILzDrugReqData, index: number) {
            let container = $(`<div class="btn-group btn-group-toggle" data-toggle="buttons">                          
                           </div>`);
            container.data('row', row);
            const showDrugInfoBtn = $(`<button class="btn btn-info tbShowDrgInfoBtn"}>
                                       <i class="fa fa-info"></i> 
                                      بيانات الراكد
                                    </button>`);
            const showRequesterPharmaBtn = $(`<button class="btn btn-info tbShowRequisterPharmaInfoBtn"}> 
                                       <i class="fa fa-info"></i> 
                                      بيانات الصيدلة(الطالبة)
                                    </button>`);
            showRequesterPharmaBtn.css({ marginRight: 2 });
            const showOwnerPharmaBtn = $(`<button class="btn btn-info tbShowOwnerPharmaInfoBtn"}>
                                       <i class="fa fa-info"></i> 
                                      بيانات الصيدلية(حاوية الراكد)
                                    </button>`);
            showOwnerPharmaBtn.css({ marginRight: 2 });
            const showAllReqInfoBtnBtn = $(`<button class="btn btn-info d-none  tbShowAllReqInfoBtn"}>
                                       <i class="fa fa-info"></i> 
                                      بيانات عن هذا الطلب
                                    </button>`);
            showAllReqInfoBtnBtn.css({ marginRight: 2 });

            container.append([showDrugInfoBtn, showRequesterPharmaBtn, showOwnerPharmaBtn, showAllReqInfoBtnBtn])
            return container.get(0).outerHTML;
        },
        handleMainTable() {
            const _this = this;
            $.get(this.generateUrlWithPaginator(LzDrgReqConfig._apiUrl, this._paginator), {}, (d, f, req) => {
                this._currentPagination = JSON.parse(req.getResponseHeader('X-Pagination') as string) as IX_Pagination;
                this.updatePaginiationUi(this._currentPagination);
            })
                .done((data: ILzDrugReqData[]) => {
                    LzDrgReqConfig._dataTable = this._mainTable.DataTable({
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
                                render(status, type, row: ILzDrugReqData) {
                                    const _class = status == LzDrugsReqsStatus.Accepted
                                        ? "primary"
                                        : status == LzDrugsReqsStatus.Pending ? "secondary"
                                            : status == LzDrugsReqsStatus.AcceptedForAnotherOne ? "danger"
                                                : status == LzDrugsReqsStatus.AtNegotioation
                                                    ? "info"
                                                    : status == LzDrugsReqsStatus.Rejected
                                                        ? "danger"
                                                        : status == LzDrugsReqsStatus.Completed
                                                            ? "success" : "secondary";

                                    const container = $(`<div><span class="badge badge-${_class}">${_this._getPharmaStatus(status)}</span></div>`);
                                    return container.get(0).outerHTML;
                                }
                            },

                            {
                                orderable: false,
                                targets: 4,
                                data: null,
                                render(row: ILzDrugReqData, type: string, d, meta) {
                                    return _this.makeDetailsColumn(row, meta.row);
                                }
                            }
                        ] as DataTables.ColumnDefsSettings[],
                        //createdRow: _this.OnCreatedRow,
                        language: LzDrgReqConfig.languageSetting,
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
            $.get(this.generateUrlWithPaginator(LzDrgReqConfig._apiUrl, this._paginator), {}, (d, f, req) => {
                this._currentPagination = JSON.parse(req.getResponseHeader('X-Pagination') as string) as IX_Pagination;
                this.updatePaginiationUi(this._currentPagination);
            })
                .done((data: ILzDrugReqData[]) => {
                    LzDrgReqConfig._dataTable.clear().draw();
                    LzDrgReqConfig._dataTable.rows.add(data);
                    LzDrgReqConfig._dataTable.columns.adjust().draw();

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
            this.handleMainTable();
            this.handleOnPaging();
            this.handleDetailsColumnActions();
            this.handleOnSelectReqStatus();
            this.handleSelectPageSize();
        }
    }.start();
})();