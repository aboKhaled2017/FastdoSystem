"use strict";
(function () {
    var $tableLoadingEl = $('.adminsPageSection .ContainerOverlay').eq(0);
    var $tableDomEl = $('#AdminsTable');
    var addNewAdminOperObj = {
        dtExecuterContext: {},
        form: $('#addNewAdminForm'),
        clearFormDataBtn: $('#clearFormDataBtn'),
        priviligsErrorEl: $('#priviligsErrorOfAdd'),
        generalErrorOfAddEl: $('#generalErrorOfAdd'),
        addNewAdminLoadingEl: $('#addNewAdminLoadingEl'),
        dataModel: {},
        _addNewAdminModal: $('#addNewAdminModal'),
        setDataModelData: function () {
            var text = "";
            $('#addNewAdminForm input[type=checkbox]:checked').each(function (i, v) {
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
        handleFormControls: function () {
            var _this_1 = this;
            var _this = this;
            this.form.find('input[type="text"],input[type="password"],input[type="tel"]').keyup(function (e) {
                var $inp = $(this);
                $inp.removeClass('is-invalid').next().text('');
                _this.dataModel[$inp.attr('name')] = $inp.val().trim();
            });
            $('#addNewAdminForm input[type="checkbox"]').change(function (e) {
                _this.priviligsErrorEl.text('');
            });
            $('#priviligControll_All_AddInp').change(function () {
                var $this = $(this);
                if ($this.is(':checked')) {
                    $this.parent('label').siblings().find('input').attr('disabled', 'disabled').addClass('n');
                }
                else {
                    $this.parent('label').siblings().find('input').removeAttr('disabled').removeClass('n');
                }
            });
            this.clearFormDataBtn.click(function (e) { _this_1.clearFormFields(); });
        },
        addNewAdminAsync: function (executeOnErrors, executOnDone, executeAlwys) {
            if (executOnDone === void 0) { executOnDone = undefined; }
            if (executeAlwys === void 0) { executeAlwys = undefined; }
            $.post("/api/admins", JSON.stringify(this.dataModel))
                .done(function (newEl) {
                console.log(newEl);
                if (executOnDone)
                    executOnDone();
            })
                .catch(function (e) {
                if (!e.responseJSON)
                    helperFunctions.alertServerError(helperFunctions.getGeneralErrorFromHttpErrorMess(e));
                var errors = e.responseJSON.errors;
                executeOnErrors(errors);
            })
                .always(function () {
                executeAlwys();
            });
        },
        validateForm: function () {
            var isValide = true;
            if (!this.dataModel.priviligs.trim()) {
                isValide = false;
                this.priviligsErrorEl.text(this.priviligsErrorEl.data('title'));
            }
            if (this.dataModel.name.length < 5) {
                isValide = false;
                var nameInp = $('#nameAddInpt');
                nameInp.addClass('is-invalid').next().text(nameInp.data('title'));
            }
            if (this.dataModel.password.length < 6) {
                isValide = false;
                var passInp = $('#passwordAddInpt');
                passInp.addClass('is-invalid').next().text(passInp.data('title'));
            }
            if (this.dataModel.password != this.dataModel.confirmPassword) {
                isValide = false;
                var passInp = $('#confirmPasswordAddInpt');
                passInp.addClass('is-invalid').next().text(passInp.data('title'));
            }
            if (this.dataModel.phoneNumber.length != 11) {
                isValide = false;
                var passInp = $('#PhoneNumberAddInpt');
                passInp.addClass('is-invalid').next().text(passInp.data('title'));
            }
            return isValide;
        },
        handleOnSubmit: function () {
            var _this_1 = this;
            this.form.submit(function (e) {
                e.preventDefault();
                _this_1.setDataModelData();
                if (!_this_1.validateForm())
                    return;
                _this_1.addNewAdminLoadingEl.toggleClass('d-none');
                _this_1.addNewAdminAsync(function (errors) {
                    $('#addNewAdminForm').find('input[type=text],input[type=password],input[type=tel]')
                        .next().each(function (i, el) {
                        var $el = $(el);
                        var attrib = $el.prev().data('error');
                        if (errors[attrib]) {
                            $el.prev().addClass('is-invalid');
                            $(el).text(helperFunctions.getInputError(errors[attrib]));
                        }
                    });
                    if (errors.G)
                        _this_1.generalErrorOfAddEl.text(errors.G);
                    if (errors.Priviligs)
                        _this_1.priviligsErrorEl.text(helperFunctions.getInputError(errors.Priviligs));
                }, function () {
                    alert('تمت الاضافة بنجاح');
                    _this_1.dtExecuterContext.refreshTableData();
                    _this_1.clearFormFields();
                }, function () {
                    _this_1.addNewAdminLoadingEl.toggleClass('d-none');
                    //(this._addNewAdminModal as any).modal('hide');
                });
            });
        },
        clearFormErrors: function () {
            $('#addNewAdminForm').find('input[type=text],input[type=password],input[type=tel]')
                .next().each(function (i, el) {
                $(el).text('');
            });
            this.generalErrorOfAddEl.text('');
            this.priviligsErrorEl.text('');
        },
        clearFormFields: function () {
            $('#addNewAdminForm').find('input[type=text],input[type=password],input[type=tel]')
                .each(function (i, el) {
                $(el).val('');
            });
            this.generalErrorOfAddEl.text('');
            $('#addNewAdminForm').find('input[type=checkbox]:checked').removeAttr('disabled').click();
        },
        handleOnAddNewAdmin: function () {
            var _this_1 = this;
            this._addNewAdminerBtn.on('click', function (e) {
                _this_1._addNewAdminModal.modal('show');
            });
        },
        start: function (dtExecuterContext) {
            this.dtExecuterContext = dtExecuterContext;
            this.handleOnSubmit();
            this.handleFormControls();
            this.handleOnAddNewAdmin();
        }
    };
    var updateAdminOperObj = {
        dtExecuterContext: {},
        form: $('#updateAdminForm'),
        id: "",
        priviligsErrorEl: $('#priviligsErrorOfUpdate'),
        generalErrorOfUpdateEl: $('#generalErrorOfUpdate'),
        updateAdminLoadingEl: $('#updateAdminLoadingEl'),
        dataModel: {},
        oldDataModel: {},
        _updateAdminModal: $('#updateAdminModal'),
        nameInp: $('#nameUpdateInpt'),
        setDataModelData: function () {
            this.dataModel.name = this.nameInp.val();
            var text = "";
            $('#updateAdminForm input[type=checkbox]:checked').each(function (i, v) {
                if ($(v).not(':disabled'))
                    text += $(v).attr('name') + ',';
            });
            this.dataModel.priviligs = text.substr(0, text.length - 1);
            if (text.indexOf('HaveFullControl') != -1) {
                this.dataModel.priviligs = "HaveFullControl";
            }
        },
        _updateAdminModalBody: $('#updateAdminModal .modal-body'),
        handleFormControls: function () {
            var _this = this;
            this.nameInp.keyup(function (e) {
                var $inp = $(this);
                $inp.removeClass('is-invalid').next().text('');
                _this.dataModel[$inp.attr('name')] = $inp.val().trim();
            });
            $('#updateAdminForm input[type="checkbox"]').change(function (e) {
                _this.priviligsErrorEl.text('');
            });
            $('#priviligControll_All_UpdateInp').change(function () {
                var $this = $(this);
                if ($this.is(':checked')) {
                    $this.parent('label').siblings().find('input').attr('disabled', 'disabled');
                }
                else {
                    $this.parent('label').siblings().find('input').removeAttr('disabled');
                }
            });
        },
        updateAdminAsync: function (id, executeOnErrors, executOnDone, executeAlwys) {
            if (executOnDone === void 0) { executOnDone = undefined; }
            if (executeAlwys === void 0) { executeAlwys = undefined; }
            $.ajax({
                url: _Config.makeUrle(id),
                method: "PUT",
                data: JSON.stringify(this.dataModel)
            })
                .done(function (tokenObj) {
                if (executOnDone)
                    executOnDone();
                if (helperFunctions.checkIfThat_ID_IsOf_TheCurrentUser(id)) {
                    helperFunctions.updateUserizationToken(tokenObj);
                }
                location.reload();
            })
                .catch(function (e) {
                if (!e.responseJSON)
                    helperFunctions.alertServerError(helperFunctions.getGeneralErrorFromHttpErrorMess(e));
                var errors = e.responseJSON.errors;
                executeOnErrors(errors);
            })
                .always(function () {
                executeAlwys();
            });
        },
        validateForm: function () {
            var isValide = true;
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
        setOldData: function (oldData) {
            this.oldDataModel.name = oldData.name;
            this.oldDataModel.priviligs = oldData.priviligs;
            this.nameInp.val(oldData.name);
            var priviligsArr = oldData.priviligs.split(',');
            this.form.find('input[type=checkbox]:checked').removeAttr('disabled').click();
            this.form.find('input[type=checkbox]').each(function (i, el) {
                var $el = $(el);
                var nameAttrib = $el.attr('name');
                if (priviligsArr.some(function (att) { return att == nameAttrib; }))
                    $el.click();
            });
        },
        handleOnSubmit: function () {
            var _this_1 = this;
            this.form.submit(function (e) {
                e.preventDefault();
                _this_1.setDataModelData();
                if (!_this_1.validateForm())
                    return;
                _this_1.updateAdminLoadingEl.toggleClass('d-none');
                _this_1.updateAdminAsync(_this_1.id, function (errors) {
                    var attrib = _this_1.nameInp.prev().data('error');
                    if (errors[attrib]) {
                        _this_1.nameInp.addClass('is-invalid');
                        _this_1.nameInp.next().text(helperFunctions.getInputError(errors[attrib]));
                    }
                    if (errors.G)
                        _this_1.generalErrorOfUpdateEl.text(errors.G);
                    if (errors.Priviligs)
                        _this_1.priviligsErrorEl.text(helperFunctions.getInputError(errors.Priviligs));
                }, function () {
                    alert('تمت التعديل بنجاح');
                    _this_1.dtExecuterContext.refreshTableData();
                }, function () {
                    _this_1.updateAdminLoadingEl.toggleClass('d-none');
                    //(this._addNewAdminModal as any).modal('hide');
                });
            });
        },
        clearFormErrors: function () {
            this.nameInp.next().text('');
            this.generalErrorOfUpdateEl.text('');
            this.priviligsErrorEl.text('');
        },
        openModal: function (_modelData) {
            this.id = _modelData.id;
            this.setOldData(_modelData);
            this._updateAdminModal.modal('show');
        },
        start: function (dtExecuterContext) {
            this.dtExecuterContext = dtExecuterContext;
            this.handleOnSubmit();
            this.handleFormControls();
        }
    };
    var updateAdminUsernameOperObj = {
        dtExecuterContext: {},
        form: $('#updateUserNameAdminForm'),
        id: "",
        generalErrorOfUpdateEl: $('#generalErrorOfUpdateUserName'),
        updateAdminLoadingEl: $('#updateUserNameAdminLoadingEl'),
        dataModel: {},
        oldDataModel: {},
        _updateAdminModal: $('#updateUserNameAdminModal'),
        userNameInp: $('#userNameUpdateInpt'),
        setDataModelData: function () {
            this.dataModel.newUserName = this.userNameInp.val();
        },
        _updateAdminModalBody: $('#updateUserNameAdminModal .modal-body'),
        handleFormControls: function () {
            var _this = this;
            this.userNameInp.keyup(function (e) {
                var $inp = $(this);
                $inp.removeClass('is-invalid').next().text('');
                _this.dataModel[$inp.attr('name')] = $inp.val().trim();
            });
        },
        updateAdminUserNameAsync: function (id, executeOnErrors, executOnDone, executeAlwys) {
            if (executOnDone === void 0) { executOnDone = undefined; }
            if (executeAlwys === void 0) { executeAlwys = undefined; }
            $.ajax({
                url: _Config.makeUrle("username/" + id),
                method: "PUT",
                data: JSON.stringify(this.dataModel)
            })
                .done(function (tokenObj) {
                if (executOnDone)
                    executOnDone();
                if (helperFunctions.checkIfThat_ID_IsOf_TheCurrentUser(id)) {
                    helperFunctions.updateUserizationToken(tokenObj);
                    location.reload();
                }
            })
                .catch(function (e) {
                if (!e.responseJSON)
                    helperFunctions.alertServerError(helperFunctions.getGeneralErrorFromHttpErrorMess(e));
                var errors = e.responseJSON.errors;
                executeOnErrors(errors);
            })
                .always(function () {
                executeAlwys();
            });
        },
        validateForm: function () {
            var isValide = true;
            if (this.dataModel.newUserName.length < 3) {
                isValide = false;
                this.userNameInp.addClass('is-invalid').next().text(this.userNameInp.data('title'));
            }
            return isValide;
        },
        setOldData: function (oldData) {
            this.oldDataModel.userName = oldData.userName;
            this.userNameInp.val(oldData.userName);
        },
        handleOnSubmit: function () {
            var _this_1 = this;
            this.form.submit(function (e) {
                e.preventDefault();
                _this_1.setDataModelData();
                if (!_this_1.validateForm())
                    return;
                _this_1.updateAdminLoadingEl.toggleClass('d-none');
                _this_1.updateAdminUserNameAsync(_this_1.id, function (errors) {
                    var attrib = _this_1.userNameInp.data('error');
                    if (errors[attrib]) {
                        _this_1.userNameInp.addClass('is-invalid');
                        _this_1.userNameInp.next().text(helperFunctions.getInputError(errors[attrib]));
                    }
                    if (errors.G)
                        _this_1.generalErrorOfUpdateEl.text(errors.G);
                }, function () {
                    alert('تمت التعديل بنجاح');
                    _this_1.dtExecuterContext.refreshTableData();
                }, function () {
                    _this_1.updateAdminLoadingEl.toggleClass('d-none');
                    //(this._addNewAdminModal as any).modal('hide');
                });
            });
        },
        clearFormErrors: function () {
            this.userNameInp.next().text('');
            this.generalErrorOfUpdateEl.text('');
        },
        openModal: function (_modelData) {
            this.id = _modelData.id;
            this.setOldData(_modelData);
            this._updateAdminModal.modal('show');
        },
        start: function (dtExecuterContext) {
            this.dtExecuterContext = dtExecuterContext;
            this.handleOnSubmit();
            this.handleFormControls();
        }
    };
    var updateAdminPhoneOperObj = {
        dtExecuterContext: {},
        form: $('#updatePhoneAdminForm'),
        id: "",
        generalErrorOfUpdateEl: $('#generalErrorOfUpdatePhone'),
        updateAdminLoadingEl: $('#updateUserNameAdminLoadingEl'),
        dataModel: {},
        oldDataModel: {},
        _updateAdminModal: $('#updatePhoneAdminModal'),
        phoneNumberInp: $('#phoneNumberUpdateInpt'),
        setDataModelData: function () {
            this.dataModel.phoneNumber = this.phoneNumberInp.val();
        },
        _updateAdminModalBody: $('#updatePhoneAdminModal .modal-body'),
        handleFormControls: function () {
            var _this = this;
            this.phoneNumberInp.keyup(function (e) {
                var $inp = $(this);
                $inp.removeClass('is-invalid').next().text('');
                _this.dataModel[$inp.attr('name')] = $inp.val().trim();
            });
        },
        updateAdminPhoneNumberAsync: function (id, executeOnErrors, executOnDone, executeAlwys) {
            if (executOnDone === void 0) { executOnDone = undefined; }
            if (executeAlwys === void 0) { executeAlwys = undefined; }
            $.ajax({
                url: _Config.makeUrle("phone/" + id),
                method: "PUT",
                data: JSON.stringify(this.dataModel)
            })
                .done(function (tokenObj) {
                if (executOnDone)
                    executOnDone();
                if (helperFunctions.checkIfThat_ID_IsOf_TheCurrentUser(id)) {
                    helperFunctions.updateUserizationToken(tokenObj);
                    location.reload();
                }
            })
                .catch(function (e) {
                if (!e.responseJSON)
                    helperFunctions.alertServerError(helperFunctions.getGeneralErrorFromHttpErrorMess(e));
                var errors = e.responseJSON.errors;
                executeOnErrors(errors);
            })
                .always(function () {
                executeAlwys();
            });
        },
        validateForm: function () {
            var validity = this.phoneNumberInp.get(0).validity;
            if (validity.valid)
                return true;
            if (validity.patternMismatch) {
                this.phoneNumberInp.addClass('is-invalid').next().text("رقم الهاتف غير صالح");
                return false;
            }
        },
        setOldData: function (oldData) {
            this.oldDataModel.phoneNumber = oldData.phoneNumber;
            this.phoneNumberInp.val(oldData.phoneNumber);
        },
        handleOnSubmit: function () {
            var _this_1 = this;
            this.form.submit(function (e) {
                e.preventDefault();
                _this_1.setDataModelData();
                if (!_this_1.validateForm())
                    return;
                _this_1.updateAdminLoadingEl.toggleClass('d-none');
                _this_1.updateAdminPhoneNumberAsync(_this_1.id, function (errors) {
                    var attrib = _this_1.phoneNumberInp.data('error');
                    if (errors[attrib]) {
                        _this_1.phoneNumberInp.addClass('is-invalid');
                        _this_1.phoneNumberInp.next().text(helperFunctions.getInputError(errors[attrib]));
                    }
                    if (errors.G)
                        _this_1.generalErrorOfUpdateEl.text(errors.G);
                }, function () {
                    alert('تمت التعديل بنجاح');
                    _this_1.dtExecuterContext.refreshTableData();
                }, function () {
                    _this_1.updateAdminLoadingEl.toggleClass('d-none');
                    //(this._addNewAdminModal as any).modal('hide');
                });
            });
        },
        clearFormErrors: function () {
            this.phoneNumberInp.next().text('');
            this.generalErrorOfUpdateEl.text('');
        },
        openModal: function (_modelData) {
            this.id = _modelData.id;
            this.setOldData(_modelData);
            this._updateAdminModal.modal('show');
        },
        start: function (dtExecuterContext) {
            this.dtExecuterContext = dtExecuterContext;
            this.handleOnSubmit();
            this.handleFormControls();
        }
    };
    var updateAdminPasswordOperObj = {
        dtExecuterContext: {},
        form: $('#updatePasswordAdminForm'),
        id: "",
        generalErrorOfUpdateEl: $('#generalErrorOfUpdatePassword'),
        updateAdminLoadingEl: $('#updatePasswordAdminLoadingEl'),
        dataModel: {},
        oldDataModel: {},
        _updateAdminModal: $('#updatePasswordAdminModal'),
        passwordInp: $('#newPasswordUpdateInpt'),
        confirmPasswordInp: $('#confirmPasswordUpdateInpt'),
        _updateAdminModalBody: $('#updatePasswordAdminModal .modal-body'),
        handleFormControls: function () {
            var _this = this;
            this.form.find('input[type=password]').keyup(function (e) {
                var $inp = $(this);
                $inp.removeClass('is-invalid').next().text('');
                _this.dataModel[$inp.attr('name')] = $inp.val().trim();
            });
        },
        clearForm: function () {
            this.passwordInp.val('');
            this.confirmPasswordInp.val('');
        },
        updateAdminPasswordAsync: function (id, executeOnErrors, executOnDone, executeAlwys) {
            if (executOnDone === void 0) { executOnDone = undefined; }
            if (executeAlwys === void 0) { executeAlwys = undefined; }
            $.ajax({
                url: _Config.makeUrle("password/" + id),
                method: "PUT",
                data: JSON.stringify(this.dataModel)
            })
                .done(function (newEl) {
                if (executOnDone)
                    executOnDone();
            })
                .catch(function (e) {
                if (!e.responseJSON)
                    helperFunctions.alertServerError(helperFunctions.getGeneralErrorFromHttpErrorMess(e));
                var errors = e.responseJSON.errors;
                executeOnErrors(errors);
            })
                .always(function () {
                executeAlwys();
            });
        },
        validateForm: function () {
            var isValide = true;
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
        handleOnSubmit: function () {
            var _this_1 = this;
            this.form.submit(function (e) {
                e.preventDefault();
                if (!_this_1.validateForm())
                    return;
                _this_1.updateAdminLoadingEl.toggleClass('d-none');
                _this_1.updateAdminPasswordAsync(_this_1.id, function (errors) {
                    var attrib1 = _this_1.passwordInp.data('error');
                    if (errors[attrib1]) {
                        _this_1.passwordInp.addClass('is-invalid');
                        _this_1.passwordInp.next().text(helperFunctions.getInputError(errors[attrib1]));
                    }
                    var attrib2 = _this_1.confirmPasswordInp.data('error');
                    if (errors[attrib2]) {
                        _this_1.confirmPasswordInp.addClass('is-invalid');
                        _this_1.confirmPasswordInp.next().text(helperFunctions.getInputError(errors[attrib2]));
                    }
                    if (errors.G)
                        _this_1.generalErrorOfUpdateEl.text(errors.G);
                }, function () {
                    alert('تمت التعديل بنجاح');
                    _this_1.clearForm();
                    _this_1.dtExecuterContext.refreshTableData();
                }, function () {
                    _this_1.updateAdminLoadingEl.toggleClass('d-none');
                    //(this._addNewAdminModal as any).modal('hide');
                });
            });
        },
        clearFormErrors: function () {
            this.passwordInp.next().text('');
            this.confirmPasswordInp.next().text('');
            this.generalErrorOfUpdateEl.text('');
        },
        openModal: function (id) {
            this.id = id;
            this._updateAdminModal.modal('show');
        },
        start: function (dtExecuterContext) {
            this.dtExecuterContext = dtExecuterContext;
            this.handleOnSubmit();
            this.handleFormControls();
        }
    };
    var _Config = {
        _innerTable: $('#innerTable'),
        _tableCounter: 0,
        _modal: $('#modal'),
        _modalTitle: $('#modal .modal-title'),
        _modalBody: $('#modal .modal-body'),
        _confirmModal: $('#confirmModal'),
        _confirmModalTitle: $('#confirmModal .modal-title'),
        _confirmModalBody: $('#confirmModal .modal-body'),
        _dataTable: null,
        getNewInnerTable: function (data) {
            this._tableCounter += 1;
            var newTb = this._innerTable.clone();
            newTb.removeClass('d-none');
            newTb.attr('id', "vstockTable_" + this._tableCounter);
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
                        render: function (val) {
                            return drugHelperFunctions.getDiscount(val);
                        }
                    },
                    {
                        targets: 3,
                        data: 'valideDate',
                        className: 'text-nowrap',
                        render: function (val) {
                            val = new Date(val);
                            return val.getMonth() + " / " + val.getFullYear();
                        }
                    },
                    {
                        targets: 4,
                        data: 'priceType',
                        className: 'text-nowrap',
                        render: function (val) {
                            return drugHelperFunctions.getPriceType(val);
                        }
                    },
                    {
                        targets: 5,
                        data: 'desc'
                    }
                ]
            });
            return newTb.get(0).outerHTML;
        },
        get _columnDefs() {
            var _this = this;
            return [
                {
                    targets: 0,
                    data: null,
                    render: function (dataRow) {
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
                    render: function (Prevligs) {
                        return _this.makePriviligsColumn(Prevligs);
                    }
                }
            ];
        },
        buildModalForPriviligsColumn: function (userName, priviligs) {
            this._modalTitle.text("\u0635\u0644\u0627\u062D\u064A\u0627\u062A / " + userName);
            var _container = $();
            priviligs.forEach(function (privilig, index) {
                _container = _container.add("\n                  <ol class=\"breadcrumb\">\n                    <li class=\"breadcrumb-item\"><span class=\"badge badge-info\">" + (index + 1) + "</span> -" + privilig + "</li>\n                  </ol>\n                ");
            });
            this._modalBody.empty().append(_container);
            this._modal.modal('show');
        },
        handlPriviligsColumn: function (dt) {
            var _this = this;
            $(document).on('click', '.showPriviligsBtn', function () {
                var $btn = $(this);
                var tr = $btn.closest('tr');
                var row = dt.row(tr);
                var rowData = row.data();
                _this.buildModalForPriviligsColumn(rowData.name, AdminsHeperFunctions.getAdminPriviligs(rowData.priviligs));
            });
        },
        makePriviligsColumn: function (Prevligs) {
            var showInfoBtn = $("<button class=\"btn btn-info showPriviligsBtn\">\n                                      <i class=\"fa fa-info fa-lg\"></i>\n                                      \u0639\u0631\u0636 \u0627\u0644\u0635\u0644\u0627\u062D\u064A\u0627\u062A\n                                   </button>");
            return showInfoBtn.get(0).outerHTML;
        },
        makeControlsActionsBtnsColumn: function (dataRow) {
            var isMainAdmin = dataRow.superId == "null" || dataRow.superId == null;
            var classText = isMainAdmin ? 'disabled' : "";
            var container = $("<div class=\"btn-group btn-group-toggle\" data-toggle=\"buttons\">                          \n                           </div>");
            var updateAdminBtn = $("<button class=\"btn btn-primary updateAdminBtn " + classText + "\" " + classText + "> \n                                       <i class=\"fa fa-edit\"></i> \n                                      \u062A\u0639\u062F\u064A\u0644\n                                    </button>");
            var updateAdminPhoneBtn = $("<button class=\"btn btn-primary updateAdminPhoneBtn " + classText + "\" " + classText + "> \n                                       <i class=\"fa fa-edit\"></i> \n                                      \u0631\u0642\u0645 \u0627\u0644\u0647\u0627\u062A\u0641\n                                    </button>");
            var updateAdminUserNameBtn = $("<button class=\"btn btn-primary updateAdminUserNameBtn " + classText + "\" " + classText + "> \n                                       <i class=\"fa fa-edit\"></i> \n                                      \u0627\u0633\u0645 \u0627\u0644\u0645\u0633\u062A\u062E\u062F\u0645\n                                    </button>");
            var updateAdminPasswordBtn = $("<button class=\"btn btn-primary updateAdminPasswordBtn " + classText + "\" " + classText + "> \n                                       <i class=\"fa fa-edit\"></i> \n                                      \u0643\u0644\u0645\u0629 \u0627\u0644\u0645\u0631\u0648\u0631\n                                    </button>");
            var deleteAdminBtn = $("<button class=\"btn btn-danger deleteAdminBtn " + classText + "\" " + classText + ">\n                                       <i class=\"fa fa-remove\"></i> \n                                      \u062D\u062F\u0641\n                                    </button>");
            container.append([updateAdminBtn, updateAdminPasswordBtn, updateAdminPhoneBtn, updateAdminUserNameBtn, deleteAdminBtn]);
            return container.get(0).outerHTML;
        },
        onRemovePharmacyConfirmed: function () { },
        handleConfirmModal: function () {
            var _this_1 = this;
            this._confirmModal.data('agree', false);
            this._confirmModal.find('.modal-footer .cancel').eq(0).click(function (e) {
                _this_1._confirmModal.data('agree', false);
                _this_1.onRemovePharmacyConfirmed();
            });
            this._confirmModal.find('.modal-footer .agree').eq(0).click(function (e) {
                _this_1._confirmModal.data('agree', true).modal('hide');
                _this_1.onRemovePharmacyConfirmed();
            });
        },
        deleteAdminAsync: function (rowData, afterDelete, alwysExecute) {
            if (afterDelete === void 0) { afterDelete = undefined; }
            if (alwysExecute === void 0) { alwysExecute = undefined; }
            $.ajax({
                url: this.makeUrle(rowData.id),
                method: "DELETE"
            })
                .done(function () {
                if (afterDelete)
                    afterDelete();
            })
                .catch(function (e) {
                helperFunctions.alertServerError(helperFunctions.getGeneralErrorFromHttpErrorMess(e));
            })
                .always(function () {
                if (alwysExecute)
                    alwysExecute();
            });
        },
        handControlsActionsColumn: function (dt, context) {
            var _this = this;
            $(document).on('click', '.updateAdminBtn', function () {
                var $btn = $(this);
                var tr = $btn.closest('tr');
                var row = dt.row(tr);
                var rowData = row.data();
                updateAdminOperObj.openModal(rowData);
            });
            $(document).on('click', '.updateAdminUserNameBtn', function () {
                var $btn = $(this);
                var tr = $btn.closest('tr');
                var row = dt.row(tr);
                var rowData = row.data();
                updateAdminUsernameOperObj.openModal(rowData);
            });
            $(document).on('click', '.updateAdminPhoneBtn', function () {
                var $btn = $(this);
                var tr = $btn.closest('tr');
                var row = dt.row(tr);
                var rowData = row.data();
                updateAdminPhoneOperObj.openModal(rowData);
            });
            $(document).on('click', '.updateAdminPasswordBtn', function () {
                var $btn = $(this);
                var tr = $btn.closest('tr');
                var row = dt.row(tr);
                var rowData = row.data();
                updateAdminPasswordOperObj.openModal(rowData.id);
            });
            $(document).on('click', '.deleteAdminBtn', function () {
                var $btn = $(this);
                var tr = $btn.closest('tr');
                var row = dt.row(tr);
                var rowData = row.data();
                _this._confirmModal.modal('show');
                _this.onRemovePharmacyConfirmed = function () {
                    var isAgree = _this._confirmModal.data('agree');
                    _this._confirmModal.data('agree', false);
                    if (isAgree) {
                        $btn.setLoading('fa-remove');
                        _this.deleteAdminAsync(rowData, function () {
                            alert("تم حذف الحساب بنجاح");
                            context.refreshTableData();
                        }, function () { $btn.removeLoading(); });
                    }
                };
            });
        },
        baseUrl: '/api/admins/all',
        makeUrle: function (id) {
            return "/api/admins/" + id;
        },
    };
    new DatatableExecuter($tableDomEl, _Config.baseUrl, _Config._dataTable, _Config._columnDefs, 4, $tableLoadingEl, undefined, function (dt, context) {
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
//# sourceMappingURL=AdminsPageScripts.js.map