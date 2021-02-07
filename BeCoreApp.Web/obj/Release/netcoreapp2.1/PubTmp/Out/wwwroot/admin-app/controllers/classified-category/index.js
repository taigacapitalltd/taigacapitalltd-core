var ClassifiedCategoryController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
        registerControl()
    }

    class ClassifiedCategoryViewModel {
        constructor() {
            this.Id = +($('#hidIdM').val());;
            this.Name = $('#txtName').val();
            this.TypeId = +($('#TypeId').val());
            this.SeoPageTitle = $('#txtSeoPageTitle').val();
            this.SeoKeywords = $('#txtSeoKeywords').val();
            this.SeoDescription = $('#txtSeoDescription').val();
            this.Status = $('#ckStatus').prop('checked') === true ? 1 : 0;
        }
        Validate() {
            var isValid = true;
            if (!this.Name) {
                be.notify('Tên không được bỏ trống!!!', "", 'error');
                isValid = false;
            }

            if (!this.TypeId) {
                be.notify('Hình thức không được bỏ trống!!!', "", 'error');
                isValid = false;
            }

            return isValid;
        }
    }
    function registerControl() {
        $('#TypeId,#TypeSearch').select2({
            placeholder: "Chọn hình thức",
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

        $("#TypeSearch").on('change', function (e) {
            e.preventDefault();
            loadData(true);
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

        $('#btnSave').on('click', function (e) { saveClassifiedCategory(e) });

        $('body').on('click', '.btn-delete', function (e) { deleteClassifiedCategory(e, this) });
    };

    function saveClassifiedCategory(e) {
        e.preventDefault();

        var classifiedCategoryVm = new ClassifiedCategoryViewModel();
        if (classifiedCategoryVm.Validate()) {
            $.ajax({
                type: "POST",
                url: "/Admin/ClassifiedCategory/SaveEntity",
                data: { classifiedCategoryVm },
                dataType: "json",
                beforeSend: function () {
                    be.startLoading();
                },
                success: function () {

                    be.notify('Cập nhật loại rao vặt thành công', "", 'success');

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

    function deleteClassifiedCategory(e, element) {
        e.preventDefault();
        be.confirm('Are you sure to delete?', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/ClassifiedCategory/Delete",
                data: { id: $(element).data('id') },
                dataType: "json",
                beforeSend: function () {
                    be.startLoading();
                },
                success: function () {
                    be.notify('Xóa loại rao vặt thành công', "", 'success');
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
        $('#hidIdM').val(0);
        $('#txtName').val('');
        $('#txtSeoPageTitle').val('');
        $('#txtSeoKeywords').val('');
        $('#txtSeoDescription').val('');
        $('#ckStatus').prop('checked', true);
        $('#TypeId').val(null).trigger('change');
    };

    function loadData(isPageChanged) {

        $.ajax({
            type: "GET",
            url: "/admin/ClassifiedCategory/GetAllPaging",
            data: {
                typeId: +($("#TypeSearch").val()),
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
                        TypeName: item.TypeName,
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
            url: "/Admin/ClassifiedCategory/GetById",
            data: { id: $(element).data('id') },
            dataType: "json",
            beforeSend: function () {
                be.startLoading();
            },
            success: function (response) {
                $('#hidIdM').val(response.Id);
                $('#txtSeoPageTitle').val(response.SeoPageTitle);
                $('#txtSeoKeywords').val(response.SeoKeywords);
                $('#txtSeoDescription').val(response.SeoDescription);
                $('#txtName').val(response.Name);
                $('#TypeId').val(response.TypeId).trigger('change');
                $('#ckStatus').prop('checked', response.Status === 1);

                $('#modal-add-edit').modal('show');

                be.stopLoading();
            },
            error: function (message) {
                be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                be.stopLoading();
            }
        });
    };
}