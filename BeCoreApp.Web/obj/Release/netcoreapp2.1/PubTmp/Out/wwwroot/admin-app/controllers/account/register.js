var registerController = function () {
    this.initialize = function () {
        registerEvents();
    }

    var registerEvents = function () {

        $('#btnRegister').on('click', function (e) {
            e.preventDefault();
            validateRegisterInfo();
        });
    }

    function validatePhone(phone) {

        var filterPhone = /^[0-9-+]+$/;

        var result = filterPhone.test(phone);

        return result;
    }

    function validateEmail(email) {

        var filterEmail = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

        var result = filterEmail.test(email);

        return result;
    }

    function validateRegisterInfo() {
        var data = {
            UserName: $('#txtUserName').val(),
            FullName: $('#txtFullName').val(),
            Password: $('#txtPassword').val(),
            ConfirmPassword: $('#txtConfirmPassword').val(),
            Email: $('#txtEmail').val(),
            CMND: $('#txtCMND').val(),
            SponsorNo: $('#txtSponsorNo').val(),
            PhoneNumber: $('#txtPhoneNumber').val(),
            Agree: $('#cbAgree').prop('checked'),
        };

        var isValid = true;

        if (!data.FullName) {
            isValid = be.notify('Họ & Tên không được bỏ trống!!!', '', 'error');
        }

        if (!data.SponsorNo) {
            isValid = be.notify('Nhà tài trợ không được bỏ trống!!!', '', 'error');
        }

        if (!data.CMND) {
            isValid = be.notify('CMND không được bỏ trống!!!', '', 'error');
        }

        if (!data.UserName) {
            isValid = be.notify('Tên đăng nhập không được bỏ trống!!!', '', 'error');
        }

        if (!data.Password) {
            isValid = be.notify('Mật khẩu không được bỏ trống!!!', '', 'error');
        }

        if (data.Password != data.ConfirmPassword) {
            isValid = be.notify('Mật khẩu xác nhận không khớp!!!', '', 'error');
        }

        if (!data.PhoneNumber) {
            isValid = be.notify('Số Phone không được bỏ trống!!!', '', 'error');
        }
        else {
            if (!validatePhone(data.PhoneNumber)) {
                isValid = be.notify('Số Phone không đúng định dạng!!!', '', 'error');
            }
        }

        if (!data.Email) {
            isValid = be.notify('Email không được bỏ trống!!!', '', 'error');
        }
        else {
            data.Email = String(data.Email).toLowerCase();
            if (!validateEmail(data.Email)) {
                isValid = be.notify('Email không đúng định dạng!!!', '', 'error');
            }
        }

        if (!data.Agree) {
            isValid = be.notify('Vui lòng xác nhận "Tôi đồng ý các điều khoản và điều kiện"', '', 'error');
        }

        if (isValid) {

            $.ajax({
                type: 'POST',
                data: { registerVm: data },
                url: '/admin/account/register',
                dataType: 'json',
                beforeSend: function () {
                    be.startLoading();
                },
                success: function (response) {

                    if (response.Success) {
                        be.success('Đăng Ký Thành Công', response.Message, function () {
                            window.location.href = '/admin/account/login';
                        });
                    }
                    else {
                        be.error('Đăng Ký Thất Bại', response.Message);
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