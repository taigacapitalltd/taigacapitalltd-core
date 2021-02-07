var CustomerController = function () {
    this.initialize = function () {
        be.startLoading();
        loadData();
        be.stopLoading();
        registerEvents();
    }

    function registerEvents() {

        $('#txt-search-keyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadData(true);
            }
        });

        $("#ddl-show-page").on('change', function () {
            be.configs.pageSize = $(this).val();
            be.configs.pageIndex = 1;
            loadData(true);
        });

        $("#btn-create").on('click', function () {
            resetFormMaintainance();
            $('#modal-add-edit').modal('show');
        });

        $('body').on('click', '.btn-edit', function (e) { loadDetails(e, this) });

        $('body').on('click', '.btn-delete', function (e) { deleteCustomer(e, this) });

        $('body').on('click', '.btn-revoke', function (e) { revokeKYC(e, this) });
    };

    function deleteCustomer(e, element) {
        e.preventDefault();
        be.confirm('Bạn muốn xóa thành viên này?', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/User/DeleteCustomer",
                data: { id: $(element).data('id') },
                beforeSend: function () {
                    be.startLoading();
                },
                success: function (response) {
                    debugger;
                    if (response.Success) {
                        be.notify(response.Message, '', 'success');

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
        });
    }

    function revokeKYC(e, element) {
        e.preventDefault();
        be.confirm('Bạn muốn thu hồi kyc này?', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/User/RevokeKYC",
                data: { id: $(element).data('id') },
                beforeSend: function () {
                    be.startLoading();
                },
                success: function (response) {
                    debugger;
                    if (response.Success) {
                        be.notify(response.Message, '', 'success');

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
        });
    }

    function loadData(isPageChanged) {
        $.ajax({
            type: "GET",
            url: "/admin/user/GetAllCustomerPaging",
            data: {
                keyword: $('#txt-search-keyword').val(),
                page: be.configs.pageIndex,
                pageSize: be.configs.pageSize
            },
            dataType: "json",
            beforeSend: function () {
                //be.startLoading();
            },
            success: function (response) {

                var template = $('#table-template').html();
                var render = "";

                $.each(response.Results, function (i, item) {
                    render += Mustache.render(template, {
                        FullName: item.FullName,
                        UserName: item.UserName,
                        WalletStart: item.WalletStart,
                        WalletUsdt: item.WalletUsdt,
                        WalletValuesShare: item.WalletValuesShare,
                        WalletEthereum: item.WalletEthereum,
                        Id: item.Id,
                        SponsorNo: item.ID,
                        Email: item.Email,
                        PhoneNumber: item.PhoneNumber,
                        EmailConfirmed: be.getEmailConfirmed(item.EmailConfirmed),
                        Activated: be.getActivated(item.IsActivated),
                        UpdatedKYC: be.getUpdatedKYC(item.IsUpdatedKYC),
                    });
                });

                $("#lbl-total-records").text(response.RowCount);

                $('#tbl-content').html(render);

                if (response.RowCount) {
                    be.wrapPaging(response.RowCount, function () {
                        loadData();
                    }, isPageChanged);
                }

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
            url: "/Admin/User/GetById",
            data: { id: $(element).data('id') },
            dataType: "json",
            beforeSend: function () {
                be.startLoading();
            },
            success: function (response) {
                $('#hidId').val(response.Id);
                $('#txtFullName').val(response.FullName);
                $('#txtUserName').val(response.UserName);
                $('#txtEmail').val(response.Email);
                $('#txtPhoneNumber').val(response.PhoneNumber);
                $('#ckStatus').prop('checked', response.Status === 1);
                $('#txtPassword').val(response.Password);
                $('#txtConfirmPassword').val(response.Password);
                $('#RoleId').val(response.Roles[0]).trigger('change');
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