var UserBonusController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }

    class UserBonusViewModel {
        constructor() {
            this.Id = +($('#hidId').val());
            this.RewardPoint = parseFloat($('#txtRewardPoint').val().replace(/,/g, ''));
            this.RecruitingBonus = parseFloat($('#txtRecruitingBonus').val().replace(/,/g, ''));
            this.UplineFund = parseFloat($('#txtUplineFund').val().replace(/,/g, ''));
            this.MoneyReceive = parseFloat($('#txtMoneyReceive').val().replace(/,/g, ''));
        }

        Validate() {
            //var isValid = true;

            //if (!this.RewardPoint) {
            //    isValid = be.notify('Reward Point không được bỏ trống!!!', "", 'error');
            //}

            //if (!this.RecruitingBonus) {
            //    isValid = be.notify('Recruiting Bonus không được bỏ trống!!!', "", 'error');
            //}

            //if (!this.UplineFund) {
            //    isValid = be.notify('Upline Fund không được bỏ trống!!!', "", 'error');
            //}

            //if (!this.MoneyReceive) {
            //    isValid = be.notify('Money Receive không được bỏ trống!!!', "", 'error');
            //}

            return true;
        }
    }

    function registerEvents() {
        //$('.numeric').on("keypress", function (e) {
        //    var keyCode = e.which ? e.which : e.keyCode;
        //    var ret = ((keyCode >= 48 && keyCode <= 57) || keyCode == 46);
        //    if (ret)
        //        return true;
        //    else
        //        return false;
        //});

        $(".numeric").focusout(function () {

            var numberValue = parseFloat($(this).val().replace(/,/g, ''));
            if (!numberValue) {
                $(this).val(formatCurrency(0));
            }
            else {
                $(this).val(formatCurrency(numberValue));
            }
        });

        $('body').on('click', '.btn-edit', function (e) {
            loadDetails(e, this);
        });

        $('#btnSave').on('click', function (e) {
            saveUserBonus(e);
        });
    };

    function saveUserBonus(e) {
        e.preventDefault();

        var userBonusVm = new UserBonusViewModel();
        if (userBonusVm.Validate()) {
            $.ajax({
                type: "POST",
                url: "/Admin/AppUserBonus/SaveEntity",
                data: { modelJson: JSON.stringify(userBonusVm) },
                dataType: "json",
                beforeSend: function () {
                    be.startLoading();
                },
                success: function () {

                    be.notify('Cập nhật cấu hình thưởng thành công', "", 'success');

                    $('#modal-add-edit').modal('hide');

                    resetFormMaintainance();

                    be.stopLoading();

                    loadData(true);
                },
                error: function (message) {
                    be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                    be.stopLoading();
                },
            });
        }
    };

    function resetFormMaintainance() {
        $('#hidId').val(0);
        $('#txtRewardPoint').val(0.00);
        $('#txtRecruitingBonus').val(0.00);
        $('#txtUplineFund').val(0.00);
        $('#txtMoneyReceive').val(0.00);
        $('#txtLevelStrategy').val('');
    };

    function loadData(isPageChanged) {
        $.ajax({
            type: "GET",
            url: "/admin/AppUserBonus/GetAll",
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
                        RewardPoint: formatCurrency(item.RewardPoint),
                        RecruitingBonus: formatCurrency(item.RecruitingBonus),
                        UplineFund: formatCurrency(item.UplineFund),
                        MoneyReceive: formatCurrency(item.MoneyReceive),
                        LevelStrategyName: item.LevelStrategyName,
                        DateCreated: be.dateTimeFormatJson(item.DateCreated),
                        DateModified: be.dateTimeFormatJson(item.DateModified),
                        Status: be.getStatus(item.Status),
                    });
                });

                $("#lbl-total-records").text(response.RowCount);

                $('#tbl-content').html(render);

                if (response.RowCount) {
                    be.wrapPaging(response.RowCount, function () {
                        loadData();
                    }, isPageChanged);
                }

                be.stopLoading();
            },
            error: function (message) {
                be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                be.stopLoading();
            }
        });
    };

    function loadDetails(e, element) {
        e.preventDefault();
        $.ajax({
            type: "GET",
            url: "/Admin/AppUserBonus/GetById",
            data: { id: $(element).data('id') },
            dataType: "json",
            beforeSend: function () {
                be.startLoading();
            },
            success: function (response) {
                $('#hidId').val(response.Id);
                $('#txtRewardPoint').val(formatCurrency(response.RewardPoint));
                $('#txtRecruitingBonus').val(formatCurrency(response.RecruitingBonus));
                $('#txtUplineFund').val(formatCurrency(response.UplineFund));
                $('#txtMoneyReceive').val(formatCurrency(response.MoneyReceive));
                $('#txtLevelStrategy').val(response.LevelStrategyName);
                $('#modal-add-edit').modal('show');
                be.stopLoading();
            },
            error: function (message) {
                be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
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