"use strict";
(function () {
    var operations = {
        _loginUrl: "/api/admin/auth/signin",
        loginForm: $('#login-form'),
        generalErrorEL: $('#generalError'),
        loginBtnSubmit: $('#loginBtnSubmit'),
        userNameInpt: $('#userNameInpt'),
        passwordInpt: $('#passwordInpt'),
        PostedFormOfSignIn: $('#PostedFormOfSignIn'),
        setCustomHtml5InputsRequiredTitle: function () {
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
        initWork: function () {
            $.ajaxSettings.contentType = "application/json";
            $('.login-content [data-toggle="flip"]').click(function () {
                $('.login-box').toggleClass('flipped');
                return false;
            });
            $('#rememberMeInpt').click(function (e) {
                var _this = $(this);
                if (_this.val() == "false")
                    _this.val("true");
                else
                    _this.val("false");
            });
            this.setCustomHtml5InputsRequiredTitle();
        },
        getErrorsContainer: function (errors) {
            if (errors.length == 0)
                return $(document.createElement('span'));
            else if (errors.length == 1) {
                var el = document.createElement('span');
                el.textContent = errors[0];
                return $(el);
            }
            else {
                var container = document.createDocumentFragment();
                for (var _i = 0, errors_1 = errors; _i < errors_1.length; _i++) {
                    var error = errors_1[_i];
                    var el = document.createElement('span');
                    el.textContent = error;
                    container.appendChild(el);
                }
                return $(container);
            }
        },
        SignInPost: function (model, rememberMe) {
            var id = model.id, name = model.name, userName = model.userName, phoneNumber = model.phoneNumber, prevligs = model.prevligs;
            $('#PostSign_Id').val(id);
            $('#PostSign_Name').val(name);
            $('#PostSign_UserName').val(userName);
            $('#PostSign_PhoneNumber').val(phoneNumber);
            $('#PostSign_Prevligs').val(prevligs);
            $('#PostSign_RememberMe').val(rememberMe);
            this.PostedFormOfSignIn.submit();
        },
        handleLoginForm: function () {
            var _this_1 = this;
            var _this = this;
            this.userNameInpt.focus(function (e) {
                _this_1.generalErrorEL.text('');
            });
            this.passwordInpt.focus(function (e) {
                _this_1.generalErrorEL.text('');
            });
            this.loginForm.submit(function (e) {
                e.preventDefault();
                var userName = _this_1.userNameInpt.val() || "";
                var password = _this_1.passwordInpt.val() || "";
                var rememberMe = $('#rememberMeInpt').val();
                var adminType = "Administrator";
                var data = JSON.stringify({
                    adminType: adminType,
                    userName: userName,
                    password: password
                });
                _this_1.loginBtnSubmit.setLoading('fa-sign-in');
                $.post(_this._loginUrl, data)
                    .then(function (data) {
                    var user = data.user, token = data.accessToken.token;
                    _this_1.storeUserToken(token);
                    _this_1.SignInPost(user, rememberMe);
                })
                    .catch(function (e) {
                    var _a;
                    var errorObj = (_a = e.responseJSON) === null || _a === void 0 ? void 0 : _a.errors;
                    if (errorObj) {
                        var G = errorObj.G, UserName = errorObj.UserName, Password = errorObj.Password;
                        _this_1.generalErrorEL.text(G);
                        _this_1.userNameInpt.next().append(_this_1.getErrorsContainer(UserName));
                        _this_1.passwordInpt.next().append(_this_1.getErrorsContainer(Password));
                    }
                })
                    .always(function () {
                    _this_1.loginBtnSubmit.removeLoading();
                });
            });
        },
        storeUserToken: function (token) {
            localStorage.setItem('token', token);
        },
        start: function () {
            this.initWork();
            this.handleLoginForm();
        }
    };
    operations.start();
}());
//# sourceMappingURL=loginPageScript.js.map