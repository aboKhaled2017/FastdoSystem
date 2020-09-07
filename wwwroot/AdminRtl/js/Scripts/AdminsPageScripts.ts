
interface IAddNewAdminModel {
    name: string
    phoneNumber: string
    userName: string
    password: string
    confirmPassword: string
    priviligs: string
}
interface IUpdateAdminModel {
    name: string
    priviligs: string
}
interface IUpdateUserNameAdminModel {
    newUserName: string
}
interface IUpdatePhoneAdminModel {
    phoneNumber: string
}
interface IUpdatePasswordAdminModel {
    newPassword: string
    confirmPassword
}


interface IAddNewAdminErrors {
    Name: string[]
    UserName: string[]
    Password: string[],
    ConfirmPassword: string[]
    PhoneNumber: string[]
    Priviligs: string[]
    G: string
    [key: string]: any
}
interface IUpdateAdminErrors {
    Name: string[]
    Priviligs: string[]
    G: string
    [key: string]: any
}
interface IUpdateUserNameAdminErrors {
    NewUserName: string[]
    G: string
    [key: string]: any
}
interface IUpdatePhoneAdminErrors {
    PhoneNumber: string[]
    G: string
    [key: string]: any
}
interface IUpdatePasswordAdminErrors {
    NewPassword: string[],
    ConfirmPassword: string[]
    G: string
    [key: string]: any
}


(function () {
    const $tableLoadingEl = $('.adminsPageSection .ContainerOverlay').eq(0);
    const $tableDomEl = $('#AdminsTable');

    const addNewAdminOperObj = {
        dtExecuterContext: {} as DatatableExecuter,
        form: $('#addNewAdminForm'),
        clearFormDataBtn: $('#clearFormDataBtn'),
        priviligsErrorEl: $('#priviligsErrorOfAdd'),
        generalErrorOfAddEl: $('#generalErrorOfAdd'),
        addNewAdminLoadingEl: $('#addNewAdminLoadingEl'),
        dataModel: {} as IAddNewAdminModel,
        _addNewAdminModal: $('#addNewAdminModal'),
        setDataModelData() {
            let text = "";
            $('#addNewAdminForm input[type=checkbox]:checked').each((i, v) => {
                if ($(v).not(':disabled'))
                    text += $(v).attr('name') + ',';
            });

            this.dataModel.priviligs = text.substr(0, text.length - 1);
            if (text.indexOf('HaveFullControl') != -1) {
                this.dataModel.priviligs = "HaveFullControl";
            }
            console.log(this.dataModel);
        },
        _addNewAdminModalBody: $('#addNewAdminModal .modal-body'),
        _addNewAdminerBtn: $('#addNewAdminerBtn'),
        handleFormControls() {
            const _this = this;
            this.form.find('input[type="text"],input[type="password"],input[type="tel"]').keyup(function (e) {
                const $inp = $(this);
                $inp.removeClass('is-invalid').next().text('');
                _this.dataModel[$inp.attr('name') as string] = ($inp.val() as string).trim();
            });

            $('#addNewAdminForm input[type="checkbox"]').change(function (e) {
                _this.priviligsErrorEl.text('');
            });

            $('#priviligControll_All_AddInp').change(function () {
                const $this = $(this);
                if ($this.is(':checked')) {
                    $this.parent('label').siblings().find('input').attr('disabled', 'disabled').addClass('n');
                }
                else {
                    $this.parent('label').siblings().find('input').removeAttr('disabled').removeClass('n');
                }
            });
            this.clearFormDataBtn.click(e => { this.clearFormFields(); });
        },
        addNewAdminAsync(
            executeOnErrors: (errors: IAddNewAdminErrors) => void,
            executOnDone: Function = undefined as any,
            executeAlwys: Function = undefined as any) {
            $.post("/api/admins", JSON.stringify(this.dataModel))
                .done(newEl => {
                    console.log(newEl);
                    if (executOnDone) executOnDone();
                })
                .catch(e => {
                    if (!e.responseJSON)
                        helperFunctions.alertServerError(helperFunctions.getGeneralErrorFromHttpErrorMess(e));
                    const errors = e.responseJSON.errors as IAddNewAdminErrors;
                    executeOnErrors(errors);
                })
                .always(() => {
                    executeAlwys();
                })
        },
        validateForm() {
            let isValide = true;
            if (!this.dataModel.priviligs.trim()) {
                isValide = false;
                this.priviligsErrorEl.text(this.priviligsErrorEl.data('title'));
            }
            if (this.dataModel.name.length < 5) {
                isValide = false;
                let nameInp = $('#nameAddInpt');
                nameInp.addClass('is-invalid').next().text(nameInp.data('title'));
            }
            if (this.dataModel.password.length < 6) {
                isValide = false;
                let passInp = $('#passwordAddInpt');
                passInp.addClass('is-invalid').next().text(passInp.data('title'));
            }
            if (this.dataModel.password != this.dataModel.confirmPassword) {
                isValide = false;
                let passInp = $('#confirmPasswordAddInpt');
                passInp.addClass('is-invalid').next().text(passInp.data('title'));
            }
            if (this.dataModel.phoneNumber.length != 11) {
                isValide = false;
                let passInp = $('#PhoneNumberAddInpt');
                passInp.addClass('is-invalid').next().text(passInp.data('title'));
            }
            return isValide;
        },
        handleOnSubmit() {
            this.form.submit(e => {
                e.preventDefault();
                this.setDataModelData();
                if (!this.validateForm()) return;
                this.addNewAdminLoadingEl.toggleClass('d-none');
                this.addNewAdminAsync(
                    (errors) => {
                        $('#addNewAdminForm').find('input[type=text],input[type=password],input[type=tel]')
                            .next().each((i, el) => {
                                var $el = $(el);
                                var attrib = $el.prev().data('error') as string;
                                if (errors[attrib]) {
                                    $el.prev().addClass('is-invalid');
                                    $(el).text(helperFunctions.getInputError(errors[attrib]))
                                }

                            });
                        if (errors.G)
                            this.generalErrorOfAddEl.text(errors.G);
                        if (errors.Priviligs)
                            this.priviligsErrorEl.text(helperFunctions.getInputError(errors.Priviligs));
                    },
                    () => {
                        alert('تمت الاضافة بنجاح');
                        this.dtExecuterContext.refreshTableData();
                        this.clearFormFields();
                    },
                    () => {
                        this.addNewAdminLoadingEl.toggleClass('d-none');
                        //(this._addNewAdminModal as any).modal('hide');
                    })
            })
        },
        clearFormErrors() {
            $('#addNewAdminForm').find('input[type=text],input[type=password],input[type=tel]')
                .next().each((i, el) => {
                    $(el).text('');
                });
            this.generalErrorOfAddEl.text('');
            this.priviligsErrorEl.text('');
        },
        clearFormFields() {
            $('#addNewAdminForm').find('input[type=text],input[type=password],input[type=tel]')
                .each((i, el) => {
                    $(el).val('');
                });
            this.generalErrorOfAddEl.text('');
            $('#addNewAdminForm').find('input[type=checkbox]:checked').removeAttr('disabled').click();
        },
        handleOnAddNewAdmin() {
            this._addNewAdminerBtn.on('click', e => {
                (this._addNewAdminModal as any).modal('show');
            });
        },
        start(dtExecuterContext: DatatableExecuter) {
            this.dtExecuterContext = dtExecuterContext;
            this.handleOnSubmit();
            this.handleFormControls();
            this.handleOnAddNewAdmin();
        }
    }

    const updateAdminOperObj = {
        dtExecuterContext: {} as DatatableExecuter,
        form: $('#updateAdminForm'),
        id: "" as string,
        priviligsErrorEl: $('#priviligsErrorOfUpdate'),
        generalErrorOfUpdateEl: $('#generalErrorOfUpdate'),
        updateAdminLoadingEl: $('#updateAdminLoadingEl'),
        dataModel: {} as IUpdateAdminModel,
        oldDataModel: {} as { name: string, priviligs: string },
        _updateAdminModal: $('#updateAdminModal'),
        nameInp: $('#nameUpdateInpt'),
        setDataModelData() {
            this.dataModel.name = this.nameInp.val() as string;
            let text = "";
            $('#updateAdminForm input[type=checkbox]:checked').each((i, v) => {
                if ($(v).not(':disabled'))
                    text += $(v).attr('name') + ',';
            });

            this.dataModel.priviligs = text.substr(0, text.length - 1);
            if (text.indexOf('HaveFullControl') != -1) {
                this.dataModel.priviligs = "HaveFullControl";
            }
        },
        _updateAdminModalBody: $('#updateAdminModal .modal-body'),
        handleFormControls() {
            const _this = this;
            this.nameInp.keyup(function (e) {
                const $inp = $(this);
                $inp.removeClass('is-invalid').next().text('');
                _this.dataModel[$inp.attr('name') as string] = ($inp.val() as string).trim();
            });

            $('#updateAdminForm input[type="checkbox"]').change(function (e) {
                _this.priviligsErrorEl.text('');
            });

            $('#priviligControll_All_UpdateInp').change(function () {
                const $this = $(this);
                if ($this.is(':checked')) {
                    $this.parent('label').siblings().find('input').attr('disabled', 'disabled');
                }
                else {
                    $this.parent('label').siblings().find('input').removeAttr('disabled');
                }
            });

        },
        updateAdminAsync(
            id: string,
            executeOnErrors: (errors: IUpdateAdminErrors) => void,
            executOnDone: Function = undefined as any,
            executeAlwys: Function = undefined as any) {           
            $.ajax({
                url: _Config.makeUrle(id),
                method: "PUT",
                data: JSON.stringify(this.dataModel)
            })
                .done(tokenObj => {
                    if (executOnDone) executOnDone();
                    if (helperFunctions.checkIfThat_ID_IsOf_TheCurrentUser(id)) {
                        helperFunctions.updateUserizationToken(tokenObj);
                    }                   
                    location.reload();
                })
                .catch(e => {
                    if (!e.responseJSON)
                        helperFunctions.alertServerError(helperFunctions.getGeneralErrorFromHttpErrorMess(e));
                    const errors = e.responseJSON.errors as IUpdateAdminErrors;
                    executeOnErrors(errors);
                })
                .always(() => {
                    executeAlwys();
                })
        },
        validateForm() {
            let isValide = true;
            if (!this.dataModel.priviligs.trim()) {
                isValide = false;
                this.priviligsErrorEl.text(this.priviligsErrorEl.data('title'));
            }
            if (this.dataModel.name.length < 5) {
                isValide = false;
                this.nameInp.addClass('is-invalid').next().text(this.nameInp.data('title'));
            }
            return isValide;
        },
        setOldData(oldData: IAdmininstartorInfo) {
            this.oldDataModel.name = oldData.name;
            this.oldDataModel.priviligs = oldData.priviligs;
            this.nameInp.val(oldData.name);
            const priviligsArr = oldData.priviligs.split(',');
            this.form.find('input[type=checkbox]:checked').removeAttr('disabled').click();
            this.form.find('input[type=checkbox]').each((i, el) => {
                var $el = $(el);
                var nameAttrib = $el.attr('name');
                if (priviligsArr.some(att => att == nameAttrib))
                    $el.click();
            })
        },
        handleOnSubmit() {
            this.form.submit(e => {
                e.preventDefault();
                this.setDataModelData();
                if (!this.validateForm()) return;
                this.updateAdminLoadingEl.toggleClass('d-none');
                this.updateAdminAsync(
                    this.id,
                    (errors) => {
                        var attrib = this.nameInp.prev().data('error') as string;
                        if (errors[attrib]) {
                            this.nameInp.addClass('is-invalid');
                            this.nameInp.next().text(helperFunctions.getInputError(errors[attrib]))
                        }
                        if (errors.G)
                            this.generalErrorOfUpdateEl.text(errors.G);
                        if (errors.Priviligs)
                            this.priviligsErrorEl.text(helperFunctions.getInputError(errors.Priviligs));
                    },
                    () => {
                        alert('تمت التعديل بنجاح');
                        this.dtExecuterContext.refreshTableData();
                    },
                    () => {
                        this.updateAdminLoadingEl.toggleClass('d-none');
                        //(this._addNewAdminModal as any).modal('hide');
                    })
            })
        },
        clearFormErrors() {

            this.nameInp.next().text('');
            this.generalErrorOfUpdateEl.text('');
            this.priviligsErrorEl.text('');
        },
        openModal(_modelData: IAdmininstartorInfo) {
            this.id = _modelData.id;
            this.setOldData(_modelData);
            (this._updateAdminModal as any).modal('show');
        },

        start(dtExecuterContext: DatatableExecuter) {
            this.dtExecuterContext = dtExecuterContext;
            this.handleOnSubmit();
            this.handleFormControls();
        }
    }

    const updateAdminUsernameOperObj = {
        dtExecuterContext: {} as DatatableExecuter,
        form: $('#updateUserNameAdminForm'),
        id: "" as string,        
        generalErrorOfUpdateEl: $('#generalErrorOfUpdateUserName'),
        updateAdminLoadingEl: $('#updateUserNameAdminLoadingEl'),
        dataModel: {} as IUpdateUserNameAdminModel,
        oldDataModel: {} as { userName: string},
        _updateAdminModal: $('#updateUserNameAdminModal'),
        userNameInp: $('#userNameUpdateInpt'),
        setDataModelData() {
            this.dataModel.newUserName = this.userNameInp.val() as string;
        },
        _updateAdminModalBody: $('#updateUserNameAdminModal .modal-body'),
        handleFormControls() {
            const _this = this;
            this.userNameInp.keyup(function (e) {
                const $inp = $(this);
                $inp.removeClass('is-invalid').next().text('');
                _this.dataModel[$inp.attr('name') as string] = ($inp.val() as string).trim();
            });

        },
        updateAdminUserNameAsync(
            id: string,
            executeOnErrors: (errors: IUpdateAdminErrors) => void,
            executOnDone: Function = undefined as any,
            executeAlwys: Function = undefined as any) {
            $.ajax({
                url: _Config.makeUrle(`username/${id}`),
                method: "PUT",
                data: JSON.stringify(this.dataModel)
            })
                .done(tokenObj => {
                    if (executOnDone) executOnDone();
                    if (helperFunctions.checkIfThat_ID_IsOf_TheCurrentUser(id)) {
                        helperFunctions.updateUserizationToken(tokenObj);
                        location.reload();
                    }  
                })
                .catch(e => {
                    if (!e.responseJSON)
                        helperFunctions.alertServerError(helperFunctions.getGeneralErrorFromHttpErrorMess(e));
                    const errors = e.responseJSON.errors as IUpdateAdminErrors;
                    executeOnErrors(errors);
                })
                .always(() => {
                    executeAlwys();
                })
        },
        validateForm() {
            let isValide = true;

            if (this.dataModel.newUserName.length <3) {
                isValide = false;
                this.userNameInp.addClass('is-invalid').next().text(this.userNameInp.data('title'));
            }
            return isValide;
        },
        setOldData(oldData: IAdmininstartorInfo) {
            this.oldDataModel.userName = oldData.userName;
            this.userNameInp.val(oldData.userName);           
        },
        handleOnSubmit() {
            this.form.submit(e => {
                e.preventDefault();
                this.setDataModelData();
                if (!this.validateForm()) return;
                this.updateAdminLoadingEl.toggleClass('d-none');
                this.updateAdminUserNameAsync(
                    this.id,
                    (errors) => {
                        var attrib = this.userNameInp.data('error') as string;
                        if (errors[attrib]) {
                            this.userNameInp.addClass('is-invalid');
                            this.userNameInp.next().text(helperFunctions.getInputError(errors[attrib]))
                        }
                        if (errors.G)
                            this.generalErrorOfUpdateEl.text(errors.G);
                    },
                    () => {
                        alert('تمت التعديل بنجاح');
                        this.dtExecuterContext.refreshTableData();
                    },
                    () => {
                        this.updateAdminLoadingEl.toggleClass('d-none');
                        //(this._addNewAdminModal as any).modal('hide');
                    })
            })
        },
        clearFormErrors() {

            this.userNameInp.next().text('');
            this.generalErrorOfUpdateEl.text('');
        },
        openModal(_modelData: IAdmininstartorInfo) {
            this.id = _modelData.id;
            this.setOldData(_modelData);
            (this._updateAdminModal as any).modal('show');
        },
        start(dtExecuterContext: DatatableExecuter) {
            this.dtExecuterContext = dtExecuterContext;
            this.handleOnSubmit();
            this.handleFormControls();
        }
    }

    const updateAdminPhoneOperObj = {
        dtExecuterContext: {} as DatatableExecuter,
        form: $('#updatePhoneAdminForm'),
        id: "" as string,
        generalErrorOfUpdateEl: $('#generalErrorOfUpdatePhone'),
        updateAdminLoadingEl: $('#updateUserNameAdminLoadingEl'),
        dataModel: {} as IUpdatePhoneAdminModel,
        oldDataModel: {} as { phoneNumber: string },
        _updateAdminModal: $('#updatePhoneAdminModal'),
        phoneNumberInp: $('#phoneNumberUpdateInpt'),
        setDataModelData() {
            this.dataModel.phoneNumber = this.phoneNumberInp.val() as string;
        },
        _updateAdminModalBody: $('#updatePhoneAdminModal .modal-body'),
        handleFormControls() {
            const _this = this;
            this.phoneNumberInp.keyup(function (e) {
                const $inp = $(this);
                $inp.removeClass('is-invalid').next().text('');
                _this.dataModel[$inp.attr('name') as string] = ($inp.val() as string).trim();
            });

        },
        updateAdminPhoneNumberAsync(
            id: string,
            executeOnErrors: (errors: IUpdateAdminErrors) => void,
            executOnDone: Function = undefined as any,
            executeAlwys: Function = undefined as any) {
            $.ajax({
                url: _Config.makeUrle(`phone/${id}`),
                method: "PUT",
                data: JSON.stringify(this.dataModel)
            })
                .done(tokenObj => {
                    if (executOnDone) executOnDone();
                    if (helperFunctions.checkIfThat_ID_IsOf_TheCurrentUser(id)) {
                        helperFunctions.updateUserizationToken(tokenObj);
                        location.reload();
                    }  
                })
                .catch(e => {
                    if (!e.responseJSON)
                        helperFunctions.alertServerError(helperFunctions.getGeneralErrorFromHttpErrorMess(e));
                    const errors = e.responseJSON.errors as IUpdateAdminErrors;
                    executeOnErrors(errors);
                })
                .always(() => {
                    executeAlwys();
                })
        },
        validateForm() {
            let validity = (this.phoneNumberInp.get(0) as HTMLInputElement).validity;
            if (validity.valid) return true;
            if (validity.patternMismatch) {
                this.phoneNumberInp.addClass('is-invalid').next().text("رقم الهاتف غير صالح");
                return false;
            }
        },
        setOldData(oldData: IAdmininstartorInfo) {
            this.oldDataModel.phoneNumber = oldData.phoneNumber;
            this.phoneNumberInp.val(oldData.phoneNumber);
        },
        handleOnSubmit() {
            this.form.submit(e => {
                e.preventDefault();
                this.setDataModelData();
                if (!this.validateForm()) return;
                this.updateAdminLoadingEl.toggleClass('d-none');
                this.updateAdminPhoneNumberAsync(
                    this.id,
                    (errors) => {
                        var attrib = this.phoneNumberInp.data('error') as string;
                        if (errors[attrib]) {
                            this.phoneNumberInp.addClass('is-invalid');
                            this.phoneNumberInp.next().text(helperFunctions.getInputError(errors[attrib]))
                        }
                        if (errors.G)
                            this.generalErrorOfUpdateEl.text(errors.G);
                    },
                    () => {
                        alert('تمت التعديل بنجاح');
                        this.dtExecuterContext.refreshTableData();
                    },
                    () => {
                        this.updateAdminLoadingEl.toggleClass('d-none');
                        //(this._addNewAdminModal as any).modal('hide');
                    })
            })
        },
        clearFormErrors() {

            this.phoneNumberInp.next().text('');
            this.generalErrorOfUpdateEl.text('');
        },
        openModal(_modelData: IAdmininstartorInfo) {
            this.id = _modelData.id;
            this.setOldData(_modelData);
            (this._updateAdminModal as any).modal('show');
        },
        start(dtExecuterContext: DatatableExecuter) {
            this.dtExecuterContext = dtExecuterContext;
            this.handleOnSubmit();
            this.handleFormControls();
        }
    }

    const updateAdminPasswordOperObj = {
        dtExecuterContext: {} as DatatableExecuter,
        form: $('#updatePasswordAdminForm'),
        id: "" as string,
        generalErrorOfUpdateEl: $('#generalErrorOfUpdatePassword'),
        updateAdminLoadingEl: $('#updatePasswordAdminLoadingEl'),
        dataModel: {} as IUpdatePasswordAdminModel,
        oldDataModel: {} as { phoneNumber: string },
        _updateAdminModal: $('#updatePasswordAdminModal'),
        passwordInp: $('#newPasswordUpdateInpt'),
        confirmPasswordInp: $('#confirmPasswordUpdateInpt'),       
        _updateAdminModalBody: $('#updatePasswordAdminModal .modal-body'),
        handleFormControls() {
            const _this = this;
            this.form.find('input[type=password]').keyup(function (e) {
                const $inp = $(this);
                $inp.removeClass('is-invalid').next().text('');
                _this.dataModel[$inp.attr('name') as string] = ($inp.val() as string).trim();
            });

        },
        clearForm() {
            this.passwordInp.val('');
            this.confirmPasswordInp.val('');
        },
        updateAdminPasswordAsync(
            id: string,
            executeOnErrors: (errors: IUpdatePasswordAdminErrors) => void,
            executOnDone: Function = undefined as any,
            executeAlwys: Function = undefined as any) {
            $.ajax({
                url: _Config.makeUrle(`password/${id}`),
                method: "PUT",
                data: JSON.stringify(this.dataModel)
            })
                .done(newEl => {
                    if (executOnDone) executOnDone();
                })
                .catch(e => {
                    if (!e.responseJSON)
                        helperFunctions.alertServerError(helperFunctions.getGeneralErrorFromHttpErrorMess(e));
                    const errors = e.responseJSON.errors as IUpdatePasswordAdminErrors;
                    executeOnErrors(errors);
                })
                .always(() => {
                    executeAlwys();
                })
        },
        validateForm() {
            let isValide = true;
            if (this.dataModel.newPassword.length < 6) {
                isValide = false;
                this.passwordInp.addClass('is-invalid').next().text(this.passwordInp.data('title'));
            }
            if (this.dataModel.newPassword != this.dataModel.confirmPassword) {
                isValide = false;
                this.confirmPasswordInp.addClass('is-invalid').next().text('كلمتى المرور غير متطابقتان');
            }
            return isValide;
        },
        handleOnSubmit() {
            this.form.submit(e => {
                e.preventDefault();
                if (!this.validateForm()) return;
                this.updateAdminLoadingEl.toggleClass('d-none');
                this.updateAdminPasswordAsync(
                    this.id,
                    (errors) => {
                        var attrib1 = this.passwordInp.data('error') as string;
                        if (errors[attrib1]) {
                            this.passwordInp.addClass('is-invalid');
                            this.passwordInp.next().text(helperFunctions.getInputError(errors[attrib1]))
                        }
                        var attrib2 = this.confirmPasswordInp.data('error') as string;
                        if (errors[attrib2]) {
                            this.confirmPasswordInp.addClass('is-invalid');
                            this.confirmPasswordInp.next().text(helperFunctions.getInputError(errors[attrib2]))
                        }
                        if (errors.G)
                            this.generalErrorOfUpdateEl.text(errors.G);
                    },
                    () => {
                        alert('تمت التعديل بنجاح');
                        this.clearForm();
                        this.dtExecuterContext.refreshTableData();
                    },
                    () => {
                        this.updateAdminLoadingEl.toggleClass('d-none');
                        //(this._addNewAdminModal as any).modal('hide');
                    })
            })
        },
        clearFormErrors() {
            this.passwordInp.next().text('');
            this.confirmPasswordInp.next().text('');
            this.generalErrorOfUpdateEl.text('');
        },
        openModal(id: string) {
            this.id =id;
            (this._updateAdminModal as any).modal('show');
        },
        start(dtExecuterContext: DatatableExecuter) {
            this.dtExecuterContext = dtExecuterContext;
            this.handleOnSubmit();
            this.handleFormControls();
        }
    }

    const _Config = {
        _innerTable: $('#innerTable'),
        _tableCounter: 0,
        _modal: $('#modal'),
        _modalTitle: $('#modal .modal-title'),
        _modalBody: $('#modal .modal-body'),
        _confirmModal: $('#confirmModal'),
        _confirmModalTitle: $('#confirmModal .modal-title'),
        _confirmModalBody: $('#confirmModal .modal-body'),
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
                    data: null,
                    render(dataRow: IAdmininstartorInfo) {
                        return _this.makeControlsActionsBtnsColumn(dataRow);
                    }
                },
                {
                    orderable: false,
                    targets: 1,
                    data: 'name'
                },
                {
                    orderable: false,
                    targets: 2,
                    className: "notArabicFont text-nowrap",
                    data: 'userName'
                },
                {
                    orderable: false,
                    targets: 3,
                    data: 'phoneNumber'
                },
                {
                    orderable: false,
                    targets: 4,
                    data: 'type'
                },
                {
                    orderable: false,
                    targets: 5,
                    data: 'prevligs',
                    render(Prevligs: string) {
                        return _this.makePriviligsColumn(Prevligs);
                    }
                }
            ] as DataTables.ColumnDefsSettings[]
        },
        buildModalForPriviligsColumn(userName: string, priviligs: string[]) {
            this._modalTitle.text(`صلاحيات / ${userName}`);
            let _container = $();
            priviligs.forEach((privilig, index) => {
                _container = _container.add(`
                  <ol class="breadcrumb">
                    <li class="breadcrumb-item"><span class="badge badge-info">${index + 1}</span> -${privilig}</li>
                  </ol>
                `)
            });
            this._modalBody.empty().append(_container);
            (this._modal as any).modal('show');
        },
        handlPriviligsColumn(dt: DataTables.DataTable) {
            const _this = this;
            $(document).on('click', '.showPriviligsBtn', function () {
                const $btn = $(this);

                var tr = $btn.closest('tr');
                var row = dt.row(tr);
                var rowData = row.data() as IAdmininstartorInfo;
                _this.buildModalForPriviligsColumn(rowData.name, AdminsHeperFunctions.getAdminPriviligs(rowData.priviligs))

            });
        },
        makePriviligsColumn(Prevligs: string) {

            const showInfoBtn = $(`<button class="btn btn-info showPriviligsBtn">
                                      <i class="fa fa-info fa-lg"></i>
                                      عرض الصلاحيات
                                   </button>`);
            return showInfoBtn.get(0).outerHTML;
        },
        makeControlsActionsBtnsColumn(dataRow: IAdmininstartorInfo) {
            let isMainAdmin = dataRow.superId == "null" || dataRow.superId==null;
            let classText = isMainAdmin ? 'disabled' : "";
            let container = $(`<div class="btn-group btn-group-toggle" data-toggle="buttons">                          
                           </div>`);
            const updateAdminBtn = $(`<button class="btn btn-primary updateAdminBtn ${classText}" ${classText}> 
                                       <i class="fa fa-edit"></i> 
                                      تعديل
                                    </button>`);
            const updateAdminPhoneBtn = $(`<button class="btn btn-primary updateAdminPhoneBtn ${classText}" ${classText}> 
                                       <i class="fa fa-edit"></i> 
                                      رقم الهاتف
                                    </button>`);
            const updateAdminUserNameBtn = $(`<button class="btn btn-primary updateAdminUserNameBtn ${classText}" ${classText}> 
                                       <i class="fa fa-edit"></i> 
                                      اسم المستخدم
                                    </button>`);
            const updateAdminPasswordBtn = $(`<button class="btn btn-primary updateAdminPasswordBtn ${classText}" ${classText}> 
                                       <i class="fa fa-edit"></i> 
                                      كلمة المرور
                                    </button>`);
            const deleteAdminBtn = $(`<button class="btn btn-danger deleteAdminBtn ${classText}" ${classText}>
                                       <i class="fa fa-remove"></i> 
                                      حدف
                                    </button>`);

            container.append([updateAdminBtn, updateAdminPasswordBtn, updateAdminPhoneBtn, updateAdminUserNameBtn, deleteAdminBtn])
            return container.get(0).outerHTML;
        },
        onRemovePharmacyConfirmed: () => { },
        handleConfirmModal() {
            this._confirmModal.data('agree', false);
            this._confirmModal.find('.modal-footer .cancel').eq(0).click(e => {
                this._confirmModal.data('agree', false);
                this.onRemovePharmacyConfirmed();
            });
            this._confirmModal.find('.modal-footer .agree').eq(0).click(e => {
                (this._confirmModal.data('agree', true) as any).modal('hide');
                this.onRemovePharmacyConfirmed();
            });
        },
        deleteAdminAsync(
            rowData: IAdmininstartorInfo,
            afterDelete: Function = undefined as any,
            alwysExecute: Function = undefined as any) {
            $.ajax({
                url: this.makeUrle(rowData.id),
                method: "DELETE"
            })
                .done(() => {
                    if (afterDelete) afterDelete();
                })
                .catch(e => {
                    helperFunctions.alertServerError(helperFunctions.getGeneralErrorFromHttpErrorMess(e));
                })
                .always(() => {
                    if (alwysExecute) alwysExecute();
                });
        },
        handControlsActionsColumn(dt: DataTables.DataTable, context: DatatableExecuter) {
            const _this = this;
            $(document).on('click', '.updateAdminBtn', function () {
                const $btn = $(this);

                var tr = $btn.closest('tr');
                var row = dt.row(tr);
                var rowData = row.data() as IAdmininstartorInfo;
                updateAdminOperObj.openModal(rowData);

            });
            $(document).on('click', '.updateAdminUserNameBtn', function () {
                const $btn = $(this);

                var tr = $btn.closest('tr');
                var row = dt.row(tr);
                var rowData = row.data() as IAdmininstartorInfo;
                updateAdminUsernameOperObj.openModal(rowData);

            }); 
            $(document).on('click', '.updateAdminPhoneBtn', function () {
                const $btn = $(this);

                var tr = $btn.closest('tr');
                var row = dt.row(tr);
                var rowData = row.data() as IAdmininstartorInfo;
                updateAdminPhoneOperObj.openModal(rowData);

            });
            $(document).on('click', '.updateAdminPasswordBtn', function () {
                const $btn = $(this);

                var tr = $btn.closest('tr');
                var row = dt.row(tr);
                var rowData = row.data() as IAdmininstartorInfo;
                updateAdminPasswordOperObj.openModal(rowData.id);

            });
            $(document).on('click', '.deleteAdminBtn', function () {
                const $btn = $(this);
                const tr = $btn.closest('tr');
                const row = dt.row(tr);
                const rowData = row.data() as IAdmininstartorInfo;
                (_this._confirmModal as any).modal('show');
                _this.onRemovePharmacyConfirmed = () => {
                    var isAgree = _this._confirmModal.data('agree');
                    _this._confirmModal.data('agree', false);
                    if (isAgree) {
                        $btn.setLoading('fa-remove');
                        _this.deleteAdminAsync(
                            rowData,
                            () => {
                                alert("تم حذف الحساب بنجاح");
                                context.refreshTableData();
                            },
                            () => { $btn.removeLoading(); });
                    }
                }


            });
        },
        baseUrl: '/api/admins/all',
        makeUrle(id: string) {
            return `/api/admins/${id}`;
        },
    }



    new DatatableExecuter(
        $tableDomEl,
        _Config.baseUrl,
        _Config._dataTable,
        _Config._columnDefs,
        4,
        $tableLoadingEl,
        undefined,
        (dt: DataTables.DataTable, context: DatatableExecuter) => {
            _Config.handlPriviligsColumn(dt);
            _Config.handControlsActionsColumn(dt, context);
            _Config.handleConfirmModal();
            addNewAdminOperObj.start(context);
            updateAdminOperObj.start(context);
            updateAdminUsernameOperObj.start(context);
            updateAdminPhoneOperObj.start(context);
            updateAdminPasswordOperObj.start(context);
        }).start();
}());

