var EnterpriseController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
        registerControls();
    }
    class EnterpriseFieldViewModel {
        constructor(enterpriseId, fieldId) {
            this.EnterpriseId = enterpriseId;
            this.FieldId = fieldId;
            this.Status = 1;
        }
    }
    class EnterpriseViewModel {
        constructor(enterpriseFieldVMs) {
            this.Id = +($('#hidId').val());
            this.Name = $('#txtName').val();
            this.Image = $('#txtImage').val();
            this.Content = CKEDITOR.instances.txtContent.getData();
            this.Email = $('#txtEmail').val();
            this.Website = $('#txtWebsite').val();
            this.Address = $('#txtAddress').val();
            this.Phone = $('#txtPhone').val();
            this.Hotline = $('#txtHotline').val();
            this.ProvinceId = +($('#ProvinceId').val());
            this.DistrictId = +($('#DistrictId').val());
            this.WardId = +($('#WardId').val());
            this.Status = $('#ckStatus').prop('checked') === true ? 1 : 0;
            this.HomeFlag = $('#ckShowHome').prop('checked');
            this.SeoPageTitle = $('#txtSeoPageTitle').val();
            this.SeoKeywords = $('#txtSeoKeywords').val();
            this.SeoDescription = $('#txtSeoDescription').val();
            this.EnterpriseFieldVMs = enterpriseFieldVMs;
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

            if (!this.EnterpriseFieldVMs || this.EnterpriseFieldVMs.length == 0) {
                be.notify('Lĩnh Vực không được bỏ trống!!!', "", 'error');
                isValid = false;
            }

            return isValid;
        }
    }
    function GetEnterpriseFieldVMs() {
        var enterpriseFieldVMs = [];
        $.each($('#FieldId').val(), function (index, value) {
            var enterpriseFieldVM = new EnterpriseFieldViewModel(index, value);
            enterpriseFieldVMs.push(enterpriseFieldVM);
        });

        return enterpriseFieldVMs;
    }
    function registerControls() {
        CKEDITOR.replace('txtContent', {});

        //Fix: cannot click on element ck in modal
        $.fn.modal.Constructor.prototype.enforceFocus = function () {
            $(document)
                .off('focusin.bs.modal') // guard against infinite focus loop
                .on('focusin.bs.modal', $.proxy(function (e) {
                    if (
                        this.$element[0] !== e.target && !this.$element.has(e.target).length
                        // CKEditor compatibility fix start.
                        && !$(e.target).closest('.cke_dialog, .cke').length
                        // CKEditor compatibility fix end.
                    ) {
                        this.$element.trigger('focus');
                    }
                }, this));
        };

        $('#FieldSearch').select2({
            placeholder: "Chọn lĩnh vực hoạt động",
            allowClear: true,
        });
        $('#FieldId').select2({
            placeholder: "Chọn lĩnh vực hoạt động",
        });

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

        $('body').on('change', "#FieldSearch", function (e) {
            loadData(true);
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

        $('body').on('click', '#btnSelectImg', function () {
            $('#fileInputImage').click();
        });

        $("body").on('change', '#fileInputImage', function () {
            var fileUpload = $(this).get(0);
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
                    $('#txtImage').val(path);
                    be.notify('Upload image succesful!', "", 'success');
                    be.stopLoading();
                },
                error: function (message) {
                    be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                    be.stopLoading();
                }
            });
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            loadDetails(that);
        });

        $('body').on('click', '#btnSave', function (e) { saveEnterprise(e); });

        $('body').on('click', '.btn-delete', function (e) { deleteEnterprise(e, this); });
    }

    function getDistricts(element, type, provinceId, cb) {
        $.ajax({
            type: "GET",
            url: "/Admin/Enterprise/GetDistricts",
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
            url: "/Admin/Enterprise/GetWards",
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

    function saveEnterprise(e) {
        e.preventDefault();

        var enterpriseVm = new EnterpriseViewModel(GetEnterpriseFieldVMs());
        if (enterpriseVm.Validate()) {
            $.ajax({
                type: "POST",
                url: "/Admin/Enterprise/SaveEntity",
                data: { enterpriseVm },
                dataType: "json",
                beforeSend: function () {
                    be.startLoading();
                },
                success: function () {
                    be.notify('Cập nhật doanh nghiệp thành công', "", 'success');

                    $('#modal-add-edit').modal('hide');

                    resetFormMaintainance();

                    be.stopLoading();

                    loadData(true);
                },
                error: function (message) {
                    be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                    be.stopLoading();
                }
            });
        }
    }

    function deleteEnterprise(e, element) {
        e.preventDefault();
        be.confirm('Are you sure to delete?', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/Enterprise/Delete",
                data: { id: $(element).data('id') },
                dataType: "json",
                beforeSend: function () {
                    be.startLoading();
                },
                success: function () {
                    be.notify('Xóa doanh nghiệp thành công', "", 'success');
                    be.stopLoading();
                    loadData();
                },
                error: function (message) {
                    be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                    be.stopLoading();
                }
            });
        });
    }

    function loadDetails(id) {
        $.ajax({
            type: "GET",
            url: "/Admin/Enterprise/GetById",
            data: { id: id },
            dataType: "json",
            beforeSend: function () {
                be.startLoading();
            },
            success: function (response) {
                $('#hidId').val(response.Id);
                $('#txtName').val(response.Name);
                $('#txtImage').val(response.Image);
                $('#txtEmail').val(response.Email);
                $('#txtWebsite').val(response.Website);
                $('#txtAddress').val(response.Address);
                $('#txtPhone').val(response.Phone);
                $('#txtHotline').val(response.Hotline);
                $('#txtSeoPageTitle').val(response.SeoPageTitle);
                $('#txtSeoKeywords').val(response.SeoKeywords);
                $('#txtSeoDescription').val(response.SeoDescription);
                CKEDITOR.instances.txtContent.setData(response.Content);
                $('#ckStatus').prop('checked', response.Status == 1);
                $('#ckShowHome').prop('checked', response.HomeFlag);

                initDropDownField(response.Id);

                be.stopLoading();

                $('#ProvinceId').val(response.ProvinceId).trigger('change');

                getDistricts("#boxDllDistrictId", true, response.ProvinceId,
                    () => {
                        $('#DistrictId').val(response.DistrictId).trigger('change');

                        getWards("#boxDllWardId", true, response.DistrictId,
                            () => {
                                $('#WardId').val(response.WardId).trigger('change')
                            });
                    });

                $('#modal-add-edit').modal('show');
            },
            error: function (message) {
                be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                be.stopLoading();
            }
        });
    }

    function initDropDownField(enterpriseId) {
        $.ajax({
            url: "/Admin/Enterprise/GetAllFieldsByEnterpriseId",
            type: 'GET',
            dataType: 'json',
            data: { enterpriseId: enterpriseId },
            async: false,
            success: function (response) {
                $("#FieldId").val(response).change();
            }
        });
    }

    function resetFormMaintainance() {
        $('#hidId').val(0);
        $('#txtName').val('');
        $('#txtImage').val('');
        $('#txtEmail').val('');
        $('#txtWebsite').val('');
        $('#txtAddress').val('');
        $('#txtPhone').val('');
        $('#txtHotline').val('');
        $('#ckStatus').prop('checked', true);
        $('#ckShowHome').prop('checked', false);
        $('#txtSeoPageTitle').val('');
        $('#txtSeoKeywords').val('');
        $('#txtSeoDescription').val('');
        $('#ProvinceId').val(null).trigger('change');
        $("#FieldId").val(null).change();
    }
    function loadData(isPageChanged) {
        $.ajax({
            type: 'GET',
            data: {
                fieldId: +($("#FieldSearch").val()),
                provinceId: +($("#ProvinceSearch").val()),
                districtId: +($("#DistrictSearch").val()),
                wardId: +($("#WardSearch").val()),
                keyword: $('#txt-search-keyword').val(),
                page: be.configs.pageIndex,
                pageSize: be.configs.pageSize
            },
            url: '/admin/enterprise/GetAllPaging',
            dataType: 'json',
            beforeSend: function () {
                be.startLoading();
            },
            success: function (response) {

                var template = $('#table-template').html();
                var render = "";

                $.each(response.Results, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Name: item.Name,
                        //FieldName: item.EnterpriseFieldVMs.map(x => '<span class="m-badge m-badge--warning m-badge--wide m--margin-top-5 m--margin-bottom-5">' + x.Field.Name + '</span>').join("<br/>"),
                        //FieldName:"",
                        ProvinceName: item.ProvinceName,
                        DistrictName: item.DistrictName,
                        WardName: item.WardName,
                        Image: item.Image == null ? '<img src="/admin-side/images/user.png" width=50' : '<img src="' + item.Image + '" width=50 />',
                        Email: item.Email,
                        Phone: item.Phone,
                        Hotline: item.Hotline,
                        CreatedDate: be.dateTimeFormatJson(item.DateCreated),
                        Status: be.getStatus(item.Status)
                    });
                });
                $('#lbl-total-records').text(response.RowCount);

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
    }
}