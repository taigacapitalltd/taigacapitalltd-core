var KYCManageController = function () {
    this.initialize = function () {
        be.startLoading();
        loadData();
        be.stopLoading();

        registerEvents();
    }

    var dataModel = null;
    function validateData(validateReason) {
        dataModel = {
            Id: $('#hdId').val(),
            AppUserId: $('#hdAppUserId').val(),
            Reason: $('#txtReason').val()
        }

        var isValid = true;

        if (validateReason == true) {
            if (!dataModel.Reason)
                isValid = be.notify('Ghi chú không được bỏ trống!!!', "", 'error');
        }

        return isValid;
    }

    function registerEvents() {
        $('#txt-search-keyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadData(true);
            }
        });

        $('body').on('change', "#ddl-show-page", function () {
            be.configs.pageSize = $(this).val();
            be.configs.pageIndex = 1;
            loadData(true);
        });

        $('body').on('click', '.btn-view', function (e) { loadDetails(e, this) });

        $('body').on('click', '#btnApproval', function (e) { approvalData(e) });

        $('body').on('click', '#btnReject', function (e) { rejectData(e) });

        $('body').on('click', '#btnLock', function (e) { lockData(e) });

        $('body').on('click', '#btnUnLock', function (e) { unLockData(e) });

        $('body').on('click', '.btn-delete', function (e) { deleteData(e, this) });
    };


    function approvalData(e) {
        e.preventDefault();
        if (validateData(false)) {
            $.ajax({
                type: "POST",
                url: "/Admin/kycmanage/approval",
                data: { kycVm: dataModel },
                dataType: "json",
                beforeSend: function () {
                    be.startLoading();
                },
                success: function (response) {
                    if (response.Success) {

                        be.notify('Duyệt KYC thành công', '', 'success');
                        $('#modal-add-edit').modal('hide');
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

    function rejectData(e) {
        e.preventDefault();
        if (validateData(true)) {
            $.ajax({
                type: "POST",
                url: "/Admin/kycmanage/reject",
                data: { kycVm: dataModel },
                dataType: "json",
                beforeSend: function () {
                    be.startLoading();
                },
                success: function (response) {
                    if (response.Success) {
                        be.notify('Hủy KYC thành công', '', 'success');
                        $('#modal-add-edit').modal('hide');
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

    function lockData(e) {
        e.preventDefault();
        if (validateData(true)) {
            $.ajax({
                type: "POST",
                url: "/Admin/kycmanage/Lock",
                data: { kycVm: dataModel },
                dataType: "json",
                beforeSend: function () {
                    be.startLoading();
                },
                success: function (response) {
                    if (response.Success) {
                        be.notify('Khóa KYC thành công', '', 'success');
                        $('#modal-add-edit').modal('hide');
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

    function unLockData(e) {
        e.preventDefault();
        if (validateData(false)) {
            $.ajax({
                type: "POST",
                url: "/Admin/kycmanage/UnLock",
                data: { kycVm: dataModel },
                dataType: "json",
                beforeSend: function () {
                    be.startLoading();
                },
                success: function (response) {
                    if (response.Success) {
                        be.notify('Mở khóa KYC thành công', '', 'success');
                        $('#modal-add-edit').modal('hide');
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

    function deleteData(e, element) {
        e.preventDefault();
        be.confirm('Bạn muốn xóa KYC này ?', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/kycmanage/Delete",
                data: { id: $(element).data('id') },
                dataType: "json",
                beforeSend: function () {
                    be.startLoading();
                },
                success: function (response) {
                    if (response.Success) {
                        be.notify('Xóa KYC thành công', "", 'success');
                        loadData(true);
                    }
                    else {
                        be.notify(response.Message, '', 'error');
                    }

                    be.stopLoading();
                },
                error: function (message) {
                    be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                    be.stopLoading();
                }
            });
        });
    }

    function loadData(isPageChanged) {
        $.ajax({
            type: "GET",
            data: {
                keyword: $('#txt-search-keyword').val(),
                page: be.configs.pageIndex,
                pageSize: be.configs.pageSize
            },
            url: "/admin/kycmanage/GetAllPaging",
            dataType: "json",
            beforeSend: function () {
                //be.startLoading();
            },
            success: function (response) {
                debugger;
                var template = $('#table-template').html();
                var render = "";

                $.each(response.Results, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        FullName: item.FullName,
                        UserName: item.UserName,
                        ID: item.ID,
                        //WithdrawPublishKey: item.WithdrawPublishKey,
                        //CMNDImage: item.CMNDImage == null ? '<img src="/admin-side/images/user.png" width=100' : '<img src="' + item.CMNDImage + '" width=100 />',
                        //BankBillImage: item.BankBillImage == null ? '<img src="/admin-side/images/user.png" width=100' : '<img src="' + item.BankBillImage + '" width=100 />',
                        //BankCardImage: item.BankCardImage == null ? '<img src="/admin-side/images/user.png" width=100' : '<img src="' + item.BankCardImage + '" width=100 />',
                        Type: be.getKYCType(item.Type),
                        Reason: item.Reason,
                    });
                });

                $('#lbl-total-records').text(response.RowCount);

                $('#tbl-content').html(render);

                if (response.RowCount)
                    be.wrapPaging(response.RowCount, function () { loadData() }, isPageChanged);

                //be.stopLoading();
            },
            error: function (message) {
                be.notify(`jqXHR.responseText: ${message.responseText}`, `Status code: ${message.status}`, 'error');
                //be.stopLoading();
            }
        });
    };

    function resetData() {
        debugger;
        $('#hdId').val('');
        $('#hdAppUserId').val('');
        $('#imgCMNDImage').attr('src', '');
        $('#imgBankBillImage').attr('src', '');
        $('#imgBankCardImage').attr('src', '');
        $('#txtReason').val('');
        $('#inputWithdrawPublishKey').val('');
        $('#inputFullName').val('');
        $('#inputUserName').val('');
        $('#inputID').val('');
        $('#inputEmail').val('');
        $('#inputPhoneNumber').val('');
        $('#inputCMND').val('');
        $('#badgeType').html('');

    }

    function loadDetails(e, element) {
        debugger;
        resetData();

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
                debugger;
                $('#hdId').val(response.Id);
                $('#hdAppUserId').val(response.AppUserId);
                $('#imgCMNDImage').attr('src', response.CMNDImage);
                $('#imgBankBillImage').attr('src', response.BankBillImage);
                $('#imgBankCardImage').attr('src', response.BankCardImage);
                $('#txtReason').val(response.Reason);
                $('#inputWithdrawPublishKey').val(response.WithdrawPublishKey);
                $('#inputFullName').val(response.FullName);
                $('#inputUserName').val(response.UserName);
                $('#inputID').val(response.ID);
                $('#inputEmail').val(response.Email);
                $('#inputPhoneNumber').val(response.PhoneNumber);
                $('#inputCMND').val(response.CMND);
                $('#badgeType').html(be.getKYCType(response.Type));

                if (response.Type != 1) {
                    $('#btnReject').hide();
                    $('#btnApproval').hide();
                    $('#btnLock').hide();


                    if (response.Type == 4) {
                        $('#btnUnLock').show();
                    }
                    else {
                        $('#btnUnLock').hide();
                    }
                }
                else {
                    $('#btnLock').show();
                    $('#btnUnLock').hide();
                    $('#btnReject').show();
                    $('#btnApproval').show();
                }

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