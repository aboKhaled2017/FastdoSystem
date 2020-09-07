(function () {
    const $tableLoadingEl = $('.vstockPageSection .ContainerOverlay').eq(0);
    const $tableDomEl = $('#vstockTable');


    const vstockConfig = {
        _modal: $('#modal'),
        _innerTable: $('#innerTable'),
        _tableCounter: 0,
        _modalTitle: $('#modal .modal-title'),
        _modalBody: $('#modal .modal-body'),
        _dataTable: null as any as DataTables.DataTable,
        getNewInnerTable(data: IVStockDrug) {
            this._tableCounter += 1;
            var newTb = this._innerTable.clone();
            newTb.removeClass('d-none');
            newTb.attr('id', `vstockTable_${this._tableCounter}`);
            newTb.DataTable({
                data: data.products,
                columnDefs: [
                    {
                        targets: 0,
                        data: "pharmacyName",
                        className: 'text-nowrap'
                    },
                    {
                        targets: 1,
                        data: 'quantity',
                        className: 'text-nowrap'
                    },
                    {
                        targets: 2,
                        data: 'discount',
                        className: 'text-nowrap',
                        render(val) {
                            return drugHelperFunctions.getDiscount(val)
                        }
                    },
                    {
                        targets: 3,
                        data: 'valideDate',
                        className: 'text-nowrap',
                        render(val: Date) {
                            val = new Date(val);
                            return `${val.getMonth()} / ${val.getFullYear()}`
                        }
                    },
                    {
                        targets: 4,
                        data: 'priceType',
                        className: 'text-nowrap',
                        render(val: LzDrugPriceType) {
                            return drugHelperFunctions.getPriceType(val);
                        }
                    },
                    {
                        targets: 5,
                        data: 'desc'
                    }
                ]
            })
            return newTb.get(0).outerHTML;
        },
        get _columnDefs() {
            const _this = this;
            return [
                {
                    targets: 0,
                    data: 'name'
                },
                {
                    orderable: false,
                    targets: 1,
                    data: 'type'
                },
                {
                    orderable: false,
                    targets: 2,
                    data: null,
                    render(val, t, rowData: IVStockDrug) {
                        return vStockHelperFunctions.getTotalPriceInStringFormate(rowData);
                    }
                },
                {
                    orderable: false,
                    targets: 3,
                    data: null,
                    render(val, t, rowData: IVStockDrug) {
                        return `${rowData.products.length} صيدلية`;
                    }
                },
                {
                    orderable: false,
                    targets: 4,
                    data: null,
                    render(row: IVStockDrug, type: string, d, meta) {
                        return _this.makeDetailsColumn(row, meta.row);
                    }
                }
            ] as DataTables.ColumnDefsSettings[]
        },
        handleDetailsColumnActions(dt: DataTables.DataTable) {
            const _this = this;
            $(document).on('click', '.showVstockDetailsBtn', function () {
                const $btn = $(this);

                var tr = $btn.closest('tr');
                var row = dt.row(tr);
                var rowData = row.data() as IVStockDrug;
                console.log(rowData)
                if (row.child.isShown()) {
                    // This row is already open - close it
                    row.child.hide();
                    tr.removeClass('shown');
                }
                else {
                    // Open this row
                    row.child(_this.getNewInnerTable(rowData)).show();
                    tr.addClass('shown');
                }


                $btn.toggleClass('fa-plus-circle').toggleClass('fa-minus-circle');

            });
        },
        makeDetailsColumn(row: IVStockDrug, index: number) {

            const showInfoBtn = $(`<i class="fa fa-plus-circle  showVstockDetailsBtn"></i>`);
            return showInfoBtn.get(0).outerHTML;
        },
        tableCreatedRow(row: Node, data: IVStockDrug, dataIndex: number) {



        }
    }


    const dtExecuter = new DatatableExecuter(
        $tableDomEl,
        Urls.getVstockApiUrl,
        vstockConfig._dataTable,
        vstockConfig._columnDefs,
        4,
        $tableLoadingEl,
        vstockConfig.tableCreatedRow as any,
        (dt) => {
            vstockConfig.handleDetailsColumnActions(dt);
        }).start();


}());