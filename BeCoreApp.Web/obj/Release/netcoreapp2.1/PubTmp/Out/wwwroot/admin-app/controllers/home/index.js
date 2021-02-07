var HomeController = function () {
    this.initialize = function () {
        loadDataPaging();
        registerEvents();
    }

    function activeMember(numberActive) {
        $.ajax({
            type: "POST",
            url: "/Admin/Home/ActiveMember",
            data: { numberActive: numberActive },
            dataType: "json",
            beforeSend: function () {
                be.startLoading();
            },
            success: function (response) {
                be.notify('Kích Hoạt Member Thành Công!!!', "", 'success');
                be.stopLoading();

                loadDataPaging(true);

                location.reload();
            },
            error: function (message) {
                be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                be.stopLoading();
            },
        });
    }

    function resetMember() {
        $.ajax({
            type: "POST",
            url: "/Admin/Home/ResetMember",
            dataType: "json",
            beforeSend: function () {
                be.startLoading();
            },
            success: function (response) {
                be.notify('Reset Member Thành Công!!!', "", 'success');
                be.stopLoading();

                loadDataPaging(true);

                location.reload();
            },
            error: function (message) {
                be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                be.stopLoading();
            },
        });
    }

    function registerEvents() {
        $("#ddl-show-page").on('change', function () {
            debugger;
            be.configs.pageUserSize = $(this).val();
            be.configs.pageIndex = 1;
            loadDataPaging(true);
        });

        $('#btnActive').on('click', function (e) {

            e.preventDefault();

            var numberActive = +($("#ListNumberActive").val());
            if (numberActive > 0) {
                activeMember(numberActive);
            }
            else {
                be.notify('Vui lòng chọn số member active', '', 'error');
            }
        });

        $('#btnReset').on('click', function (e) {
            e.preventDefault();
            resetMember();
        });
    };

    function loadDataPaging(isPageChanged) {
        debugger;
        $.ajax({
            type: "GET",
            url: "/admin/home/GetAllPaging",
            data: {
                page: be.configs.pageIndex,
                pageSize: be.configs.pageUserSize
            },
            dataType: "json",
            beforeSend: function () {
                be.startLoading();
            },
            success: function (response) {

                var template = $('#table-template').html();
                var render = "";

                $.each(response.Results, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        FullName: item.FullName,
                        Balance: be.formatCurrency(item.Balance),
                        UplineFund: be.formatCurrency(item.UplineFund),
                        RewardPoint: be.formatCurrency(item.RewardPoint),
                        RecruitingCount: item.RecruitingCount,
                        InvestmentMoney: be.formatCurrency(item.InvestmentMoney),
                        LevelStrategy: item.LevelStrategy,
                        ActiveDate: be.dateTimeFormatJson(item.ActiveDate),
                        DateCreated: be.dateTimeFormatJson(item.DateCreated),
                        Status: be.getStatus(item.Status)
                    });
                });

                $("#lbl-total-records").text(response.RowCount);

                $('#tbl-content').html(render);
                debugger;
                if (response.RowCount) {
                    be.wrapUserPaging(response.RowCount, function () {
                        loadDataPaging();
                    }, isPageChanged);
                }

                be.stopLoading();
            },
            error: function (message) {
                be.notify(`jqXHR.responseText: ${message.responseText}`, `Status code: ${message.status}`, 'error');
                be.stopLoading();
            }
        });
    };


    function loadData(reload) {
        $.ajax({
            url: '/Admin/Home/GetAll',
            dataType: 'json',
            beforeSend: function () {
                be.startLoading();
            },
            success: function (response) {
                //response.sort(function (a, b) {
                //    return a.data.sortOrder - b.data.sortOrder;
                //});

                if (reload) {
                    $('div#jstree').jstree(true).settings.core.data = response;
                    $('div#jstree').jstree(true).refresh();
                }
                else {
                    fillData(response);
                }

                be.stopLoading();

            },
            error: function (message) {
                be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                be.stopLoading();
            }
        });
    }

    var fillData = function (response) {
        $("div#jstree").jstree({
            plugins: ["table", "state", "types"],
            core: {
                themes: { responsive: false },
                check_callback: true,
                data: response
            },
            table: {
                columns: [
                    {
                        header: "Tên",
                        format: function (v) {
                            if (v)
                                return '<span class="m--margin-right-100">' + v + '</span >'
                        }
                    },
                    {
                        header: "Thu Nhập", value: "balance",
                        format: function (v) {
                            return '<span class="m--margin-right-50">$' + v + '.00</span >'
                        }
                    },
                    {
                        header: "Tiền Nạp", value: "investmentMoney",
                        format: function (v) {
                            if (v)
                                return '<span class="m--margin-right-100">$' + v + '.00</span >'
                        }
                    },
                    {
                        header: "Cấp Độ", value: "levelStrategy",
                        format: function (v) {
                            return '<strong class="m--font-success m--margin-right-100">' + v + '</strong>'
                        },
                    },
                    {
                        header: "Tuyến Trên", value: "uplineFund",
                        format: function (v) {
                            if (v)
                                return '<span class="m--margin-right-50">$' + v + '.00</span >'
                        }
                    },
                    //{
                    //    header: "Reference", value: "referenceId",
                    //    format: function (v) {
                    //        if (v)
                    //            return '<span class="m--margin-right-10">' + v + '</span >'
                    //    }
                    //},
                    {
                        header: "Điểm Thưởng", value: "rewardPoint",
                        format: function (v) {
                            return '<span class="m--margin-right-70">' + v + '.00</span >'
                        }
                    },
                    {
                        header: "Tuyển Dụng", value: "recruitingCount",
                        format: function (v) {
                            return '<span class="m--margin-right-100">' + v + '</span >'
                        }
                    },
                    {
                        header: "Trạng Thái", value: "status",
                        format: function (v) {
                            if (v)
                                return '<strong class="m--font-success m--margin-right-30">Kích hoạt</strong>'

                            return '<strong class="m--font-danger m--margin-right-30">Vô hiệu</strong>'
                        },
                        width: 40
                    }
                ],
                resizable: true,
                draggable: true,
                width: "100%",
                height: "100%",
            },
            types: {
                default: { "icon": "fa fa-folder m--font-success" },
                file: { "icon": "fa fa-file  m--font-success" }
            },
            state: { "key": "demo2" }
        })
    }
}