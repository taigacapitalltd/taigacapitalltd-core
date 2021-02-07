var CustomerHomeController = function () {
    this.initialize = function () {
        //loadData();
        registerEvents();
    }

    function registerEvents() {

        $('body').on('click', '#btnCopyReferlink', function (e) {
            copyReferLink();
        });
    }

    function copyReferLink() {
        var copyText = $("#txtReferlink");
        copyText.select();
        document.execCommand("copy");
    }

    function loadData() {
        $.ajax({
            type: "GET",
            url: "/admin/CustomerHome/GetCustomerChild",
            dataType: "json",
            beforeSend: function () {
                be.startLoading();
            },
            success: function (response) {

                var template = $('#table-template').html();
                var render = "";

                $.each(response, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        FullName: item.FullName,
                        ReferLink: 'http://cryptolifefund.com/Admin/Account/Register/' + item.Id,
                        RecruitingCount: item.RecruitingCount,
                        LevelStrategy: item.LevelStrategy,
                        ActiveDate: be.dateFormatJson(item.ActiveDate),
                    });
                });


                $('#tbl-content').html(render);

                be.stopLoading();
            },
            error: function (message) {
                be.notify(`jqXHR.responseText: ${message.responseText}`, `Status code: ${message.status}`, 'error');
                be.stopLoading();
            }
        });
    };

    function formatCurrency(num) {
        num = num.toString().replace(/\$|\,/g, '');
        if (isNaN(num))
            num = "0";
        sign = (num == (num = Math.abs(num)));
        num = Math.floor(num * 100 + 0.50000000001);
        cents = num % 100;
        num = Math.floor(num / 100).toString();
        if (cents < 10)
            cents = "0" + cents;
        for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
            num = num.substring(0, num.length - (4 * i + 3)) + ',' +
                num.substring(num.length - (4 * i + 3));
        return (((sign) ? '' : '-') + num + '.' + cents);
    }

}