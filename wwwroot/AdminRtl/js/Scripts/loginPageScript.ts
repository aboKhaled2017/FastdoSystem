
(function () {
    let operations = {
        _loginUrl: "/api/admin/auth/signin",
        loginForm: $('#login-form'),
        generalErrorEL: $('#generalError'),
        loginBtnSubmit: $('#loginBtnSubmit'),
        userNameInpt: $('#userNameInpt'),
        passwordInpt: $('#passwordInpt'),
        PostedFormOfSignIn: $('#PostedFormOfSignIn'),
        setCustomHtml5InputsRequiredTitle() {
            document.addEventListener("DOMContentLoaded", function () {
                var elements = document.getElementsByTagName("INPUT");
                for (var i = 0; i < elements.length; i++) {
                    (elements[i] as any).oninvalid = function (e: any) {
                        e.target.setCustomValidity("");
                        if (!e.target.validity.valid) {
                            e.target.setCustomValidity((e.target as HTMLInputElement).getAttribute('title'));
                        }
                    };
                    (elements[i] as any).oninput = function (e: any) {
                        e.target.setCustomValidity("");
                    };
                }
            })
        },
        initWork() {
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
                    _this.val("false")
            });
            this.setCustomHtml5InputsRequiredTitle();
        },
        getErrorsContainer(errors: string[]) {
            if (errors.length == 0) return $(document.createElement('span'));
            else if (errors.length == 1) {
                var el = document.createElement('span');
                el.textContent = errors[0];
                return $(el);
            }
            else {
                var container = document.createDocumentFragment();
                for (let error of errors) {
                    var el = document.createElement('span');
                    el.textContent = error;
                    container.appendChild(el);
                }
                return $(container)
            }
        },
        SignInPost(model: any, rememberMe: string) {

            const { id, name, userName, phoneNumber, prevligs } = model;
            $('#PostSign_Id').val(id);
            $('#PostSign_Name').val(name);
            $('#PostSign_UserName').val(userName);
            $('#PostSign_PhoneNumber').val(phoneNumber);
            $('#PostSign_Prevligs').val(prevligs);
            $('#PostSign_RememberMe').val(rememberMe);
            this.PostedFormOfSignIn.submit();
        },
        handleLoginForm() {
            const _this = this;
            this.userNameInpt.focus(e => {
                this.generalErrorEL.text('');
            });
            this.passwordInpt.focus(e => {
                this.generalErrorEL.text('');
            })
            this.loginForm.submit((e: any) => {
                e.preventDefault();

                let userName = this.userNameInpt.val() || "";
                let password = this.passwordInpt.val() || "";
                let rememberMe = $('#rememberMeInpt').val();
                let adminType = "Administrator";
                var data = JSON.stringify({
                    adminType,
                    userName,
                    password
                });
                this.loginBtnSubmit.setLoading('fa-sign-in');
                $.post(_this._loginUrl, data)
                    .then(data => {
                        const { user, accessToken: { token } } = data;
                        this.storeUserToken(token);
                        this.SignInPost(user, rememberMe as string);
                    })
                    .catch((e: any) => {
                        var errorObj = e.responseJSON?.errors;
                        if (errorObj) {
                            const { G, UserName, Password } = errorObj;
                            this.generalErrorEL.text(G);
                            this.userNameInpt.next().append(this.getErrorsContainer(UserName as any));
                            this.passwordInpt.next().append(this.getErrorsContainer(Password as any))
                        }
                    })
                    .always(() => {
                        this.loginBtnSubmit.removeLoading();
                    });

            })
        },
        storeUserToken(token: string) {
            localStorage.setItem('token', token)
        },
        start() {
            this.initWork();
            this.handleLoginForm();
        }
    }
    operations.start();
}());