var WalletController = function () {
    this.initialize = function () {
        registerEvents();
        registerControl();
    }
    function registerControl() {
        jQuery('#qrcodeCanvas').qrcode({
            text: $("#txtPublishKey").val()
        });
        jQuery('#qrcodeCanvasVT').qrcode({
            text: $("#txtPublishKeyVT").val()
        });

        jQuery('#qrcodeCanvas1').qrcode({
            text: $("#txtPublishKey").val()
        });

        $(".numberFormat").each(function () {

            var numberValue = parseFloat($(this).val().replace(/,/g, ''));
            if (!numberValue) {
                $(this).val(be.formatCurrency(0));
            }
            else {
                $(this).val(be.formatCurrency(numberValue));
            }
        });

    }
    var registerEvents = function () {

        $('body').on('click', '#btnCopyPublishKey', function (e) {
            copyPublishKey();
        });
        $('body').on('click', '#btnCopyPublishKeyVT', function (e) {
            copyPublishKeyVT();
        });

        $('.numberFormat').on("keypress", function (e) {
            var keyCode = e.which ? e.which : e.keyCode;
            var ret = ((keyCode >= 48 && keyCode <= 57) || keyCode == 46);
            if (ret)
                return true;
            else
                return false;
        });

        $(".numberFormat").focusout(function () {
            var numberValue = parseFloat($(this).val().replace(/,/g, ''));
            if (!numberValue) {
                $(this).val(be.formatCurrency(0));
            }
            else {
                $(this).val(be.formatCurrency(numberValue));
            }
        });

        $("#txtAmount").on('keyup', function (e) {
            var amountValue = parseFloat($(this).val().replace(/,/g, ''));
            var systemFeeValue = $("#txtSystemFee").val();
            var amountTransferValue = amountValue - (amountValue * (systemFeeValue / 100));
            $("#txtAmountTransfer").val(be.formatCurrency(amountTransferValue));
        });

        $('#btnWithdrawUSDT').on('click', function (e) {
            e.preventDefault();
            validateWithdrawUSDT();
        });

        $('#btnWithdrawVSCoin').on('click', function (e) {
            e.preventDefault();
            validateWithdrawVSCoin();
        });

        $('#btnExchange').on('click', function (e) {
            e.preventDefault();
            exchange();
        });

        $('#btnUpdateDepositEther').on('click', function (e) {
            e.preventDefault();
            UpdateEthereumDeposit();
        });
    }



    function UpdateEthereumDeposit() {
        debugger;
        var transactionHash = $('#txtTransactionHash').val();
        if (!transactionHash) {
            be.notify('Vui lòng nhập mã giao dịch', '', 'error');
        }
        else {
            $.ajax({
                type: "POST",
                url: "/Admin/Wallet/UpdateEthereumDeposit",
                data: { transactionHash: transactionHash },
                dataType: "json",
                beforeSend: function () {
                    be.startLoading();
                },
                success: function (response) {
                    debugger;
                    if (response.Success) {
                        be.success('Cập nhật ETH phí thành công.', response.Message, function () {
                            window.location.reload();
                        });
                    }
                    else {
                        be.error('Cập nhật ETH phí thất bại.', response.Message);
                    }

                    be.stopLoading();
                },
                error: function (message) {
                    be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                    be.stopLoading();
                }
            });
        }
    }

    function exchange() {

        $.ajax({
            type: "POST",
            url: "/Admin/Wallet/Exchange",
            dataType: "json",
            beforeSend: function () {
                be.startLoading();
            },
            success: function (response) {
                debugger;
                if (response.Success) {
                    be.success('Chuyển Đổi Thành Công', response.Message, function () {
                        window.location.reload();
                    });
                }
                else {
                    be.error('Chuyển Đổi Thất Bại', response.Message);
                }

                be.stopLoading();
            },
            error: function (message) {
                be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                be.stopLoading();
            }
        });
    }

    function validateWithdrawUSDT() {
        var data = {
            AddressReceiving: $('#txtAddressReceiving').val(),
            Balance: parseFloat($('#txtBalance').val().replace(/,/g, '')),
            Amount: parseFloat($('#txtAmount').val().replace(/,/g, '')),
            AmountTransfer: parseFloat($('#txtAmountTransfer').val().replace(/,/g, '')),
        };


        var isValid = true;

        if (data.Balance <= 0) {
            isValid = be.notify('Số dư khả dụng không đủ!', '', 'error');
        }

        if (data.Amount <= 0) {
            isValid = be.notify('Vui lòng nhập số USDT cần rút', '', 'error');
        }

        if (data.Balance < data.Amount) {
            isValid = be.notify('Số USDT cần rút phải bé hơn hoặc bằng số dư khả dụng!', '', 'error');
        }

        if (data.AmountTransfer <= 0) {
            isValid = be.notify('Số USDT nhận được không đủ', '', 'error');
        }

        if (isValid) {

            be.confirm('Bạn muốn rút USDT về ví: ' + data.AddressReceiving + '?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Wallet/WithdrawUSDT",
                    data: { modelJson: JSON.stringify(data) },
                    dataType: "json",
                    beforeSend: function () {
                        be.startLoading();
                    },
                    success: function (response) {
                        debugger;
                        if (response.Success) {
                            be.success('Rút Thành Công', response.Message, function () {
                                window.location.reload();
                            });
                        }
                        else {
                            be.error('Rút Thất Bại', response.Message);
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
    }
    function validateWithdrawVSCoin() {
        var data = {
            AddressReceiving: $('#txtVSCoinAddressReceiving').val(),
            Balance: parseFloat($('#txtVSCoinBalance').val().replace(/,/g, '')),
            AmountTransfer: parseFloat($('#txtVSCoinAmountTransfer').val().replace(/,/g, '')),
        };


        var isValid = true;

        if (data.Balance <= 0) {
            isValid = be.notify('Số dư VS Coin khả dụng không đủ!', '', 'error');
        }

        if (data.AmountTransfer <= 0) {
            isValid = be.notify('Vui lòng nhập số VS Coin rút', '', 'error');
        }

        if (data.Balance < data.AmountTransfer) {
            isValid = be.notify('Số VS Coin rút phải bé hơn hoặc bằng số dư khả dụng!', '', 'error');
        }

        debugger;

        if (isValid) {

            be.confirm('Bạn muốn rút ' + data.AmountTransfer + ' VS Coin về ví: ' + data.AddressReceiving + '?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Wallet/WithdrawVSCoin",
                    data: { modelJson: JSON.stringify(data) },
                    dataType: "json",
                    beforeSend: function () {
                        be.startLoading();
                    },
                    success: function (response) {
                        debugger;
                        if (response.Success) {
                            be.success('Rút VS Coin Thành Công', response.Message, function () {
                                window.location.reload();
                            });
                        }
                        else {

                            //be.error('Rút VS Coin Thất Bại', response.Message);
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
    }
    function copyPublishKey() {
        var copyText = $("#txtPublishKey");
        copyText.select();
        document.execCommand("copy");
    }
    function copyPublishKeyVT() {
        var copyText = $("#txtPublishKeyVT");
        copyText.select();
        document.execCommand("copy");
    }
}