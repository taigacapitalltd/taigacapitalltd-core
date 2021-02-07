var LoginController = function () {
    this.initialize = function () {
        registerEvents();
    }

    var registerEvents = function () {

        $('#btnLogin').on('click', function (e) {
            e.preventDefault();
            validateLoginInfo();
        });
    }
    function validateLoginInfo() {

        var data = {
            UserName: $('#txtUserName').val(),
            Password: $('#txtPassword').val(),
        };

        var isValid = true;

        if (!data.UserName) {
            isValid = be.notify('Vui lòng nhập tên đăng nhập!!!', "", 'error');
        }

        if (!data.Password) {
            isValid = be.notify('Vui lòng nhập mật khẩu!!!', "", 'error');
        }

        if (isValid) {

            $.ajax({
                type: 'POST',
                data: { loginVm: data },
                dataType: 'json',
                beforeSend: function () {
                    be.startLoading();
                },
                url: '/admin/account/login',
                success: function (response) {
                    
                    if (response.Success) {
                        be.notify('Đăng nhập thành công', '', 'success');
                        window.location.href = '/admin/home/index';
                    }
                    else {
                        be.notify(response.Message, '', 'error');
                    }

                    be.stopLoading();
                },
                error: function (message) {

                    be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                    be.stopLoading();
                },
            });
        }
    }
}