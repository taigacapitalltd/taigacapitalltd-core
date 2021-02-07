var ProjectController = function () {
    var imageManagement = new ImageManagement();
    var libraryManagement = new LibraryManagement();
    this.initialize = function () {
        loadData();
        registerEvents();
        registerControls();
        imageManagement.initialize();
        libraryManagement.initialize();
    }

    class ProjectViewModel {
        constructor() {
            this.Id = +($('#hidId').val());
            this.Name = $('#txtName').val();
            this.Image = $('#txtImage').val();
            this.TotalArea = $('#txtTotalArea').val();
            this.AreageBuild = $('#txtAreageBuild').val();
            this.ProgressBuild = $('#txtProgressBuild').val();
            this.ProjectScale = $('#txtProjectScale').val();
            this.HandOverTheHouse = $('#txtHandOverTheHouse').val();
            this.Video = $('#txtVideo').val();
            this.Address = $('#txtAddress').val();
            this.ProvinceId = +($('#ProvinceId').val());
            this.DistrictId = +($('#DistrictId').val());
            this.WardId = +($('#WardId').val());
            this.ProjectCategoryId = +($('#ProjectCategoryId').val());
            this.EnterpriseId = +($('#EnterpriseId').val());
            this.Status = $('#ckStatus').prop('checked') === true ? 1 : 0;
            this.HomeFlag = $('#ckShowHome').prop('checked');
            this.SeoPageTitle = $('#txtSeoPageTitle').val();
            this.SeoKeywords = $('#txtSeoKeywords').val();
            this.SeoDescription = $('#txtSeoDescription').val();
            this.Content = CKEDITOR.instances.txtContent.getData();
            this.Location = CKEDITOR.instances.txtLocation.getData();
            this.OverallDiagram = CKEDITOR.instances.txtOverallDiagram.getData();
            this.Infrastructure = CKEDITOR.instances.txtInfrastructure.getData();
            this.GroundDesign = CKEDITOR.instances.txtGroundDesign.getData();
            this.FinancialSupport = CKEDITOR.instances.txtFinancialSupport.getData();
        }

        Validate() {
            var isValid = true;
            if (!this.Name) {
                be.notify('Tên không được bỏ trống!!!', "", 'error');
                isValid = false;
            }

            if (!this.Image) {
                be.notify('Ảnh bìa không được bỏ trống!!!', "", 'error');
                isValid = false;
            }

            if (!this.Content) {
                be.notify('Tổng quan không được bỏ trống!!!', "", 'error');
                isValid = false;
            }

            if (!this.ProjectCategoryId) {
                be.notify('Loại dự án không được bỏ trống!!!', "", 'error');
                isValid = false;
            }

            if (!this.EnterpriseId) {
                be.notify('Nhà thầu không được bỏ trống!!!', "", 'error');
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
        CKEDITOR.replace('txtContent', {});
        CKEDITOR.replace('txtLocation', {});
        CKEDITOR.replace('txtOverallDiagram', {});
        CKEDITOR.replace('txtInfrastructure', {});
        CKEDITOR.replace('txtGroundDesign', {});
        CKEDITOR.replace('txtFinancialSupport', {});

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

        $('#EnterpriseId,#EnterpriseSearch').select2({
            placeholder: "Chọn nhà thầu",
            allowClear: true,
        });

        $('#ProjectCategoryId,#ProjectCategorySearch').select2({
            placeholder: "Chọn loại dự án",
            allowClear: true,
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

        $('body').on('change', "#ddl-show-page", function () {
            be.configs.pageSize = $(this).val();
            be.configs.pageIndex = 1;
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
                    be.notify('Upload hình thành công!', "", 'success');
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

        $('body').on('click', '#btnSave', function (e) { saveProject(e); });

        $('body').on('click', '.btn-delete', function (e) { deleteProject(e, this); });
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

    function saveProject(e) {
        e.preventDefault();

        var vm = new ProjectViewModel();
        if (vm.Validate()) {
            $.ajax({
                type: "POST",
                url: "/Admin/Project/SaveEntity",
                data: { vm },
                dataType: "json",
                beforeSend: function () {
                    be.startLoading();
                },
                success: function () {
                    be.notify('Cập nhật dự án thành công', "", 'success');

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

    function deleteProject(e, element) {
        e.preventDefault();
        be.confirm('Are you sure to delete?', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/Project/Delete",
                data: { id: $(element).data('id') },
                dataType: "json",
                beforeSend: function () {
                    be.startLoading();
                },
                success: function () {
                    be.notify('Xóa dự án thành công', "", 'success');
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
            url: "/Admin/Project/GetById",
            data: { id: id },
            dataType: "json",
            beforeSend: function () {
                be.startLoading();
            },
            success: function (response) {
                $('#hidId').val(response.Id);
                $('#txtName').val(response.Name);
                $('#txtImage').val(response.Image);
                $('#txtAddress').val(response.Address);
                $('#txtTotalArea').val(response.TotalArea);
                $('#txtAreageBuild').val(response.AreageBuild);
                $('#txtProgressBuild').val(response.ProgressBuild);
                $('#txtProjectScale').val(response.ProjectScale);
                $('#txtHandOverTheHouse').val(response.HandOverTheHouse);
                $('#txtVideo').val(response.Video);

                $('#ProjectCategoryId').val(response.ProjectCategoryId).trigger('change');
                $("#EnterpriseId").val(response.EnterpriseId).trigger('change');

                CKEDITOR.instances.txtContent.setData(response.Content);
                CKEDITOR.instances.txtLocation.setData(response.Location);
                CKEDITOR.instances.txtOverallDiagram.setData(response.OverallDiagram);
                CKEDITOR.instances.txtInfrastructure.setData(response.Infrastructure);
                CKEDITOR.instances.txtGroundDesign.setData(response.GroundDesign);
                CKEDITOR.instances.txtFinancialSupport.setData(response.FinancialSupport);

                $('#txtSeoPageTitle').val(response.SeoPageTitle);
                $('#txtSeoKeywords').val(response.SeoKeywords);
                $('#txtSeoDescription').val(response.SeoDescription);

                $('#ckStatus').prop('checked', response.Status == 1);
                $('#ckShowHome').prop('checked', response.HomeFlag);

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


    function resetFormMaintainance() {
        $('#hidId').val(0);
        $('#txtName').val('');
        $('#txtImage').val('');
        $('#txtAddress').val('');
        $('#txtTotalArea').val('');
        $('#txtAreageBuild').val('');
        $('#txtProgressBuild').val('');
        $('#txtProjectScale').val('');
        $('#txtHandOverTheHouse').val('');
        $('#txtVideo').val('');
        $('#ProjectCategoryId').val(null).trigger('change');
        $('#ckStatus').prop('checked', true);
        $('#ckShowHome').prop('checked', false);
        $('#txtSeoPageTitle').val('');
        $('#txtSeoKeywords').val('');
        $('#txtSeoDescription').val('');
        $('#ProvinceId').val(null).trigger('change');
        $("#EnterpriseId").val(null).trigger('change');
        CKEDITOR.instances.txtContent.setData('');
        CKEDITOR.instances.txtLocation.setData('');
        CKEDITOR.instances.txtOverallDiagram.setData('');
        CKEDITOR.instances.txtInfrastructure.setData('');
        CKEDITOR.instances.txtGroundDesign.setData('');
        CKEDITOR.instances.txtFinancialSupport.setData('');
    }
    function loadData(isPageChanged) {
        $.ajax({
            type: 'GET',
            data: {
                enterpriseId: +($("#EnterpriseSearch").val()),
                projectCategoryId: +($("#ProjectCategorySearch").val()),
                provinceId: +($("#ProvinceSearch").val()),
                districtId: +($("#DistrictSearch").val()),
                wardId: +($("#WardSearch").val()),
                keyword: $('#txt-search-keyword').val(),
                page: be.configs.pageIndex,
                pageSize: be.configs.pageSize
            },
            url: '/admin/project/GetAllPaging',
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
                        EnterpriseName: item.EnterpriseName,
                        ProvinceName: item.ProvinceName,
                        DistrictName: item.DistrictName,
                        WardName: item.WardName,
                        Image: item.Image == null ? '<img src="/admin-side/images/user.png" width=200' : '<img src="' + item.Image + '" width=200 />',
                        //CreatedDate: be.dateTimeFormatJson(item.DateCreated),
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