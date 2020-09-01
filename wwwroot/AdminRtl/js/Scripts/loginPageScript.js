"use strict";
var operations = {
    loginForm: $('#login-form'),
    initWork: function () {
        $('.login-content [data-toggle="flip"]').click(function () {
            $('.login-box').toggleClass('flipped');
            return false;
        });
    },
    handleLoginForm: function () {
        this.loginForm.submit(function (e) {
            e.preventDefault();
            var userName = $('#userNameInpt').val();
            var password = $('#password').val();
            alert(userName + " " + password);
            $.post('https://www.backend.fastdo.co/api/admin/auth/signin', {
                adminType: "Administartor",
                userName: "MahmoudAnwar"
            })
                .then(function (d) {
                alert(JSON.stringify(d));
            })
                .fail(function (e) {
                alert(e);
            })
                .catch(function (e) {
                alert('catched');
                alert(e);
            });
        });
    },
    start: function () {
        this.initWork();
        this.handleLoginForm();
    }
};
operations.start();
//# sourceMappingURL=loginPageScript.js.map