let operations = {
    loginForm:$('#login-form'),
    initWork() {
        $('.login-content [data-toggle="flip"]').click(function () {
            $('.login-box').toggleClass('flipped');
            return false;
        });
    },
    handleLoginForm() {
        this.loginForm.submit(e => {
            e.preventDefault();
            let userName = $('#userNameInpt').val();
            let password = $('#password').val();
            alert(userName + " " + password)
            $.post('https://www.backend.fastdo.co/api/admin/auth/signin', {
                adminType: "Administartor",
                userName:"MahmoudAnwar"
            })
                .then(d => {
                    alert(JSON.stringify(d))
                })
                .fail(e => {
                    alert(e)
                })
                .catch(e => {
                    alert('catched');
                    alert(e)
                })
        })
    },
    start() {
        this.initWork();
        this.handleLoginForm();
    }
}
operations.start();