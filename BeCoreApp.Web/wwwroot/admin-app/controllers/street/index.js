var StreetController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
        registerControls()
    }

    class StreetViewModel {
        constructor() {
            this.Id = +($('#hidId').val());;
            this.Name = $('#txtName').val();
            this.SeoPageTitle = $('#txtSeoPageTitle').val();
            this.SeoKeywords = $('#txtSeoKeywords').val();
            this.SeoDescription = $('#txtSeoDescription').val();
            this.ProvinceId = +($('#ProvinceId').val());
            this.DistrictId = +($('#DistrictId').val());
            this.WardId = +($('#WardId').val());
            this.Status = $('#ckStatus').prop('checked') === true ? 1 : 0;
        }
        Validate() {
            var isValid = true;
            if (!this.Name) {
                be.notify('Tên không được bỏ trống!!!', "", 'error');
                isValid = false;
            }

            if (!this.ProvinceId) {
                be.notify('Tỉnh/Thành Phố không được bỏ trống!!!', "", 'error');
                isValid = false;
            }

            if (!this.DistrictId) {
                be.notify('Quận/Huyện không được bỏ trống!!!', "", 'error');
                isValid = false;
            }

            if (!this.WardId) {
                be.notify('Xã/Phường không được bỏ trống!!!', "", 'error');
                isValid = false;
            }

            return isValid;
        }
    }
    function registerControls() {
        $('#ProvinceId,#ProvinceSearch').select2({
            placeholder: "Chọn tỉnh/thành phố",
            allowClear: true,
        });
        $('#DistrictId,#DistrictSearch').select2({
            placeholder: "Chọn quận/huyện",
            allowClear: true,
        });

        $('#WardId,#WardSearch').select2({
            placeholder: "Chọn xã/phường",
            allowClear: true,
        });
    }
    function registerEvents() {

        $('#txt-search-keyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadData(true);
            }
        });

        $('body').on('change', "#ProvinceSearch", function (e) {
            getDistricts("#boxDllDistrictSearch", false, +($('#ProvinceSearch').val()),
                () => {
                    getWards("#boxDllWardSearch", false, +($('#DistrictSearch').val()), () => loadData(true));
                });
        });
        $('body').on('change', "#DistrictSearch", function (e) {
            getWards("#boxDllWardSearch", false, +($('#DistrictSearch').val()), () => loadData(true));
        });
        $('body').on('change', "#WardSearch", function (e) {
            loadData(true);
        });


        $('body').on('change', "#ProvinceId", function (e) {
            getDistricts("#boxDllDistrictId", true, +($('#ProvinceId').val()),
                () => {
                    getWards("#boxDllWardId", true, +($('#DistrictId').val()));
                });
        });
        $('body').on('change', "#DistrictId", function (e) {
            getWards("#boxDllWardId", true, +($('#DistrictId').val()));
        });

        $('body').on('change', "#ddl-show-page", function () {
            be.configs.pageSize = $(this).val();
            be.configs.pageIndex = 1;
            loadData(true);
        });

        $('body').on('click', "#btn-create", function () {
            resetFormMaintainance();
            $('#modal-add-edit').modal('show');
        });

        $('body').on('click', '.btn-edit', function (e) { loadDetails(e, this) });

        $('body').on('click', '#btnSave', function (e) { saveStreet(e) });

        $('body').on('click', '.btn-delete', function (e) { deleteStreet(e, this) });
    };
    function getDistricts(element, type, provinceId, cb) {
        $.ajax({
            type: "GET",
            url: "/Admin/Street/GetDistricts",
            data: { provinceId, type },
            dataType: "html",
            beforeSend: function () {
                be.startLoading();
            },
            success: function (response) {
                $(element).html(response);

                be.stopLoading();

                if (cb) cb();
            },
            error: function (message) {
                be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                be.stopLoading();
            }
        });
    };
    function getWards(element, type, districtId, cb) {
        $.ajax({
            type: "GET",
            url: "/Admin/Street/GetWards",
            data: { districtId, type },
            dataType: "html",
            beforeSend: function () {
                be.startLoading();
            },
            success: function (response) {
                $(element).html(response);
                be.stopLoading();

                if (cb) cb();
            },
            error: function (message) {
                be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                be.stopLoading();
            }
        });
    };
    function saveStreet(e) {
        e.preventDefault();

        var streetVm = new StreetViewModel();
        if (streetVm.Validate()) {
            $.ajax({
                type: "POST",
                url: "/Admin/Street/SaveEntity",
                data: { streetVm },
                dataType: "json",
                beforeSend: function () {
                    be.startLoading();
                },
                success: function () {

                    be.notify('Cập nhật đường thành công', "", 'success');

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
    function deleteStreet(e, element) {
        e.preventDefault();
        be.confirm('Are you sure to delete?', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/Street/Delete",
                data: { id: $(element).data('id') },
                dataType: "json",
                beforeSend: function () {
                    be.startLoading();
                },
                success: function () {
                    be.notify('Xóa đường thành công', "", 'success');
                    be.stopLoading();
                    loadData(true);
                },
                error: function (message) {
                    be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                    be.stopLoading();
                }
            });
        });
    };
    function resetFormMaintainance() {
        $('#hidId').val(0);
        $('#txtName').val('');
        $('#txtSeoPageTitle').val('');
        $('#txtSeoKeywords').val('');
        $('#txtSeoDescription').val('');
        $('#ckStatus').prop('checked', true);
        $('#ProvinceId').val(null).trigger('change');
    };
    function loadData(isPageChanged) {

        $.ajax({
            type: "GET",
            url: "/admin/Street/GetAllPaging",
            data: {
                provinceId: +($("#ProvinceSearch").val()),
                districtId: +($("#DistrictSearch").val()),
                wardId: +($("#WardSearch").val()),
                keyword: $('#txt-search-keyword').val(),
                page: be.configs.pageIndex,
                pageSize: be.configs.pageSize
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
                        Name: item.Name,
                        ProvinceName: item.ProvinceName,
                        DistrictName: item.DistrictName,
                        WardName: item.WardName,
                        Id: item.Id,
                        Status: be.getStatus(item.Status)
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
            url: "/Admin/Street/GetById",
            data: { id: $(element).data('id') },
            dataType: "json",
            beforeSend: function () {
                be.startLoading();
            },
            success: function (response) {

                $('#hidId').val(response.Id);
                $('#txtName').val(response.Name);
                $('#txtSeoPageTitle').val(response.SeoPageTitle);
                $('#txtSeoKeywords').val(response.SeoKeywords);
                $('#txtSeoDescription').val(response.SeoDescription);
                $('#ckStatus').prop('checked', response.Status === 1);
                be.stopLoading();

                $('#ProvinceId').val(response.ProvinceId).trigger('change');

                getDistricts("#boxDllDistrictId", true, response.ProvinceId,
                    () => {
                        $('#DistrictId').val(response.DistrictId).trigger('change');

                        getWards("#boxDllWardId", true, response.DistrictId,
                            () => $('#WardId').val(response.WardId).trigger('change'));
                    });

                $('#modal-add-edit').modal('show');
            },
            error: function (message) {
                be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                be.stopLoading();
            }
        });
    };
}