var KYCController = function () {
    this.initialize = function () {
        be.startLoading();
        loadData();
        be.stopLoading();
        registerEvents();
    }

    var dataModel = null;
    function validateData() {
        dataModel = {
            CMNDImage: $('#txtCMNDImage').val(),
            BankCardImage: $('#txtBankCardImage').val(),
            BankBillImage: $('#txtBankBillImage').val(),
            WithdrawPublishKey: $('#txtWithdrawPublishKey').val(),
        }
        debugger;

        var isValid = true;
        if (!dataModel.WithdrawPublishKey)
            isValid = be.notify('Địa chỉ ví không được bỏ trống!!!', "", 'error');

        if (!dataModel.CMNDImage)
            isValid = be.notify('CMND không được bỏ trống!!!', "", 'error');

        if (!dataModel.BankCardImage)
            isValid = be.notify('Thẻ NGân Hàng không được bỏ trống!!!', "", 'error');

        if (!dataModel.BankBillImage)
            isValid = be.notify('Ảnh Tự Sướng Cùng CMND mặt trước không được bỏ trống!!!', "", 'error');

        return isValid;
    }

    function registerEvents() {
        $('body').on('click', '#btnSelectCMNDImg', function () {
            $('#fileInputCMNDImage').click();
        });

        $('body').on('click', '#btnSelectBankCardImg', function () {
            $('#fileInputBankCardImage').click();
        });

        $('body').on('click', '#btnSelectBankBillImg', function () {
            $('#fileInputBankBillImage').click();
        });

        $("body").on('change', '#fileInputCMNDImage', function () {
            changeFileInputImage(this, "#txtCMNDImage", "#contentCMNDImage");
        });
        $("body").on('change', '#fileInputBankCardImage', function () {
            changeFileInputImage(this, "#txtBankCardImage", "#contentBankCardImage");
        });
        $("body").on('change', '#fileInputBankBillImage', function () {
            changeFileInputImage(this, "#txtBankBillImage", "#contentBankBillImage");
        });

        $('body').on('click', '.btn-view', function (e) { loadDetails(e, this) });

        $('#btnSendKYC').on('click', function (e) { saveData(e) });

        
    };

    

    function changeFileInputImage(element, inputBack, contentBack) {
        var fileUpload = $(element).get(0);
        var files = fileUpload.files;
        var data = new FormData();
        for (var i = 0; i < files.length; i++) {
            data.append(files[i].name, files[i]);
        }
        $.ajax({
            type: "POST",
            url: "/Admin/Upload/UploadImage",
            contentType: false,
            processData: false,
            data: data,
            beforeSend: function () {
                be.startLoading();
            },
            success: function (path) {
                $(inputBack).val(path);
                $(contentBack).html('<img src="' + path + '"/>');
                be.stopLoading();
            },
            error: function (message) {
                be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                be.stopLoading();
            }
        });
    }

    function saveData(e) {
        e.preventDefault();
        if (validateData()) {
            $.ajax({
                type: "POST",
                url: "/Admin/KYC/SaveEntity",
                data: { kycVm: dataModel },
                dataType: "json",
                beforeSend: function () {
                    be.startLoading();
                },
                success: function (response) {
                    if (response.Success) {
                        be.notify('Gửi KYC thành công', '', 'success');

                        resetFormMaintainance();

                        $("#formKYC").hide();

                        loadData(true);
                    }
                    else {
                        be.notify(response.Message, '', 'error');
                    }

                    be.stopLoading();
                },
                error: function (message) {
                    be.notify(`jqXHR.responseText: ${message.responseText}`, `Status code: ${message.status}`, 'error');
                    be.stopLoading();
                }
            });
        }
    }

    function resetFormMaintainance() {
        $('#txtCMNDImage').val('');
        $('#txtBankCardImage').val('');
        $('#txtBankBillImage').val('');
        $('#txtWithdrawPublishKey').val('');
    }

    function loadData() {
        $.ajax({
            type: "GET",
            url: "/admin/kyc/GetAllByUserId",
            dataType: "json",
            beforeSend: function () {
                //be.startLoading();
            },
            success: function (response) {

                var template = $('#table-template').html();
                var render = "";

                $.each(response, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        WithdrawPublishKey: item.WithdrawPublishKey,
                        CMNDImage: item.CMNDImage == null ? '<img src="/admin-side/images/user.png" width=100' : '<img src="' + item.CMNDImage + '" width=100 />',
                        BankBillImage: item.BankBillImage == null ? '<img src="/admin-side/images/user.png" width=100' : '<img src="' + item.BankBillImage + '" width=100 />',
                        BankCardImage: item.BankCardImage == null ? '<img src="/admin-side/images/user.png" width=100' : '<img src="' + item.BankCardImage + '" width=100 />',
                        Type: be.getKYCType(item.Type),
                        Reason: item.Reason,
                    });
                });

                $('#tbl-content').html(render);

                //be.stopLoading();
            },
            error: function (message) {
                be.notify(`jqXHR.responseText: ${message.responseText}`, `Status code: ${message.status}`, 'error');
                //be.stopLoading();
            }
        });
    };

    function loadDetails(e, element) {
        e.preventDefault();
        $.ajax({
            type: "GET",
            url: "/Admin/kyc/GetById",
            data: { id: $(element).data('id') },
            dataType: "json",
            beforeSend: function () {
                be.startLoading();
            },
            success: function (response) {

                $('#imgCMNDImage').attr('src', response.CMNDImage);
                $('#imgBankBillImage').attr('src', response.BankBillImage);
                $('#imgBankCardImage').attr('src', response.BankCardImage);
                $('#txtReason').val(response.Reason);
                $('#inputWithdrawPublishKey').val(response.WithdrawPublishKey);
                $('#badgeType').html(be.getKYCType(response.Type));

                $('#modal-add-edit').modal('show');

                be.stopLoading();
            },
            error: function (message) {
                be.notify(`jqXHR.responseText: ${message.responseText}`, `Status code: ${message.status}`, 'error');
                be.stopLoading();
            }
        });
    };
}