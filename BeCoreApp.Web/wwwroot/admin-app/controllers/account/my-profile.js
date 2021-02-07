var MyProfileController = function () {
    this.initialize = function () {
        registerEvents();
    }

    var registerEvents = function () {

        $('#btnMyProfile').on('click', function (e) {
            e.preventDefault();
            validateMyProfileInfo();
        });

        $('.btn-update-referral').on('click', function (e) {
            e.preventDefault();
            validateUpdateReferral(this);
        });
    }

    function validatePhone(phone) {

        var filterPhone = /^[0-9-+]+$/;

        var result = filterPhone.test(phone);

        return result;
    }


    function validateMyProfileInfo() {
        var data = {
            FullName: $('#txtFullName').val(),
            CMND: $('#txtCMND').val(),
            PhoneNumber: $('#txtPhoneNumber').val(),
        };

        var isValid = true;

        if (!data.FullName) {
            isValid = be.notify('Họ & Tên không được bỏ trống', '', 'error');
        }

        if (!data.CMND) {
            isValid = be.notify('CMND không được bỏ trống', '', 'error');
        }

        if (!data.PhoneNumber) {
            isValid = be.notify('Số Điện Thoại không được bỏ trống', '', 'error');
        }
        else {
            if (!validatePhone(data.PhoneNumber)) {
                isValid = be.notify('Số Điện Thoại không đúng định dạng', '', 'error');
            }
        }

        if (isValid) {

            $.ajax({
                type: 'POST',
                data: { profileVm: data },
                url: '/admin/profile/index',
                dataType: 'json',
                beforeSend: function () {
                    be.startLoading();
                },
                success: function (response) {

                    if (response.Success) {
                        be.success('Cập nhật thông tin', response.Message);
                    }
                    else {
                        be.error('Cập nhật thông tin', response.Message);
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

    function validateUpdateReferral(element) {
        debugger;

        var blogId = $(element).attr("data-blog-id");
        var referalLink = $('.txt-referral-link-' + blogId).val();
        var referalDescription = $('.txtarea-referral-description-' + blogId).val();

        var isValid = true;

        if (!referalLink) {
            isValid = be.notify('Liên kế Không được bỏ trống', '', 'error');
        }

        if (isValid) {

            $.ajax({
                type: 'POST',
                data: { BlogId: blogId, ReferralLink: referalLink, Description: referalDescription },
                url: '/admin/profile/updateReferral',
                dataType: 'json',
                beforeSend: function () {
                    be.startLoading();
                },
                success: function (response) {

                    if (response.Success) {
                        be.success('Cập nhật thông tin', response.Message);
                    }
                    else {
                        be.error('Cập nhật thông tin', response.Message);
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