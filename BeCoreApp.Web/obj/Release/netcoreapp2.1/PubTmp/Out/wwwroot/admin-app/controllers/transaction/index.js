var TransactionController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
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
    }

    function loadData(isPageChanged) {

        $.ajax({
            type: 'GET',
            data: {
                keyword: $('#txt-search-keyword').val(),
                page: be.configs.pageIndex,
                pageSize: be.configs.pageSize
            },
            url: '/admin/transaction/GetAllPaging',
            dataType: 'json',
            beforeSend: function () {
                be.startLoading();
            },
            success: function (response) {
                var template = $('#table-template').html();
                var render = "";
                $.each(response.Results, function (i, item) {
                    render += Mustache.render(template, {
                        UserName: item.AppUserName,
                        Type: item.TypeName,
                        TransactionHas: item.TransactionHas,
                        ContractAddress: item.ContractAddress,
                        DecimalPlaces: item.DecimalPlaces,
                        AddressReceiving: item.AddressTo,
                        Amount: item.Amount > 0 ? be.formatCurrency(item.Amount) : "",
                        AmountTransfer: be.formatCurrency(item.AmountTransfer),
                        AppUserName: item.AppUserName,
                        DateCreated: be.dateTimeFormatJson(item.DateCreated),
                    });
                });

                $('#lbl-total-records').text(response.RowCount);

                $('#tbl-content').html(render);

                if (response.RowCount)
                    be.wrapPaging(response.RowCount, function () {
                        loadData();
                    }, isPageChanged);

                be.stopLoading();
            },
            error: function (message) {
                be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                be.stopLoading();
            }
        });
    }
}