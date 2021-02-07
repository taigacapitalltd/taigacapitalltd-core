var productController = function () {
    var quantityManagement = new QuantityManagement();
    var imageManagement = new ImageManagement();
    var wholePriceManagement = new WholePriceManagement();

    this.initialize = function () {
        initTreeDdlBlogCategory('#ddlSearchBlogCategoryId');
        loadData();
        registerEvents();
        registerControls();
        quantityManagement.initialize();
        imageManagement.initialize();
        wholePriceManagement.initialize();
    }

    var dataProduct = null;
    function validateData() {
        dataProduct = {
            Id: $('#hidId').val(),
            Name: $('#txtName').val(),
            BlogCategoryId: $('#ddlBlogCategoryId').combotree('getValue'),
            Image: $('#txtImage').val(),
            Price: $('#txtPrice').val(),
            OriginalPrice: $('#txtOriginalPrice').val(),
            PromotionPrice: $('#txtPromotionPrice').val(),
            Description: $('#txtDescription').val(),
            MildContent: CKEDITOR.instances.txtContent.getData(),
            HomeFlag: $('#ckShowHome').prop('checked'),
            HotFlag: $('#ckHot').prop('checked'),
            Tags: $('#ddlTags').val(),
            ProductTypeId: $("#ddlProductTypes").val(),
            Status: $('#ckStatus').prop('checked') == true ? 1 : 0,
            SeoPageTitle: $('#txtSeoPageTitle').val(),
            SeoKeywords: $('#txtSeoKeywords').val(),
            SeoDescription: $('#txtSeoDescription').val()
        }

        var isValid = true;
        if (!dataProduct.Name)
            isValid = be.notify('Tên không được bỏ trống!!!', "", 'error');

        debugger;
        dataProduct.ProductTypeId = dataProduct.ProductTypeId.filter(function (x) { return x != "" });
        debugger;
        if (dataProduct.ProductTypeId.length == 0)
            isValid = be.notify('Hình thức không được bỏ trống!!!', "", 'error');

        if (!dataProduct.Image)
            isValid = be.notify('Ảnh bìa không được bỏ trống!!!', "", 'error');

        if (!dataProduct.Description)
            isValid = be.notify('Tóm tắt không được bỏ trống!!!', "", 'error');

        dataProduct.Tags = dataProduct.Tags.filter(function (x) { return x != "" });
        if (dataProduct.Tags.length == 0)
            isValid = be.notify('Chủ đề không được bỏ trống!!!', "", 'error');

        if (!dataProduct.MildContent)
            isValid = be.notify('Tổng quan không được bỏ trống!!!', "", 'error');

        if (!dataProduct.BlogCategoryId)
            isValid = be.notify('Loại sản phẩm không được bỏ trống!!!', "", 'error');

        return isValid;
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

        $('#ddlTags').select2({
            placeholder: "Chuyên Mục ...",
            allowClear: true,
            tags: true,
        });
        $('#ddlProductTypes').select2({
            placeholder: "Hình Thức ...",
            allowClear: true,
            maximumSelectionLength: 1,
            tags: true,
        });
    }

    function registerEvents() {
        $('#txt-search-keyword').on('keypress', function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadData(true);
            }
        });

        $('#ddl-show-page').on('change', function () {
            be.configs.pageSize = $(this).val();
            be.configs.pageIndex = 1;
            loadData(true);
        });

        $("#btnCreate").on('click', function (e) {
            e.preventDefault();
            resetFormMaintainance();
            initTreeDdlBlogCategory('#ddlBlogCategoryId');
            $('#modal-add-edit').modal('show');
        });

        $('#btnSelectImg').on('click', function () {
            $('#fileInputImage').click();
        });

        $("#fileInputImage").on('change', function () {
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
                    be.stopLoading();
                },
                error: function (message) {
                    be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                    be.stopLoading();
                },
            });
        });

        $('body').on('click', '.btn-edit', function (e) { loadDetails($(this).data('id')) });

        $('body').on('click', '#btnSave', function (e) { saveProduct(e) });

        $('body').on('click', '.btn-delete', function (e) { deleteProduct(e, this) });


        $('#btn-import').on('click', function () {
            $('#modal-import-excel').modal('show');
        });
        $('#btnImportExcel').on('click', function () {
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;

            // Create FormData object  
            var fileData = new FormData();
            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append("files", files[i]);
            }
            // Adding one more key to FormData object  
            fileData.append('categoryId', $('#ddlCategoryIdImportExcel').combotree('getValue'));
            $.ajax({
                url: '/Admin/Product/ImportExcel',
                type: 'POST',
                data: fileData,
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                success: function (data) {
                    $('#modal-import-excel').modal('hide');
                    loadData();
                },
                error: function (message) {
                    be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                },
            });
            return false;
        });
        $('#btn-export').on('click', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/Product/ExportExcel",
                beforeSend: function () {
                    be.startLoading();
                },
                success: function (response) {
                    be.stopLoading();
                    window.location.href = response;
                },
                error: function (message) {
                    be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                    be.stopLoading();
                },
            });
        });
    }

    function saveProduct(e) {
        e.preventDefault();

        if (validateData()) {
            $.ajax({
                type: "POST",
                url: "/Admin/Product/SaveEntity",
                data: { productVm: dataProduct },
                dataType: "json",
                beforeSend: function () {
                    be.startLoading();
                },
                success: function (response) {
                    be.notify('Cập nhật sản phẩm thành công', "", 'success');
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
    }

    function deleteProduct(e, element) {
        be.confirm('Are you sure to delete?', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/Product/Delete",
                data: { id: $(element).data('id') },
                dataType: "json",
                beforeSend: function () {
                    be.startLoading();
                },
                success: function (response) {
                    be.notify('Xóa sản phẩm thành công', "", 'success');
                    be.stopLoading();
                    loadData();
                },
                error: function (message) {
                    be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                    be.stopLoading();
                },
            });
        });
    }

    function loadDetails(id) {
        $.ajax({
            type: "GET",
            url: "/Admin/Product/GetById",
            data: { id: id },
            dataType: "json",
            beforeSend: function () {
                be.startLoading();
            },
            success: function (response) {
                $('#hidId').val(response.Id);
                $('#txtName').val(response.Name);
                $('#txtImage').val(response.Image);

                var tagArray = response.ProductTags;
                $('#ddlTags').val(tagArray.map(function (x) { return x.TagId })).trigger('change');
                $("#ddlProductTypes").val(response.ProductTypeId).trigger('change');
                $('#txtDescription').val(response.Description);
                $('#txtPrice').val(response.Price);
                $('#txtOriginalPrice').val(response.OriginalPrice);
                $('#txtPromotionPrice').val(response.PromotionPrice);
                $('#txtSeoKeywords').val(response.SeoKeywords);
                $('#txtSeoDescription').val(response.SeoDescription);
                $('#txtSeoPageTitle').val(response.SeoPageTitle);

                CKEDITOR.instances.txtContent.setData(response.MildContent);

                $('#ckStatus').prop('checked', response.Status == 1);
                $('#ckHot').prop('checked', response.HotFlag);
                $('#ckShowHome').prop('checked', response.HomeFlag);

                initTreeDdlBlogCategory('#ddlBlogCategoryId', response.BlogCategoryId);

                be.stopLoading();

                $('#modal-add-edit').modal('show');
            },
            error: function (message) {
                be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                be.stopLoading();
            },
        });
    }

    function initTreeDdlBlogCategory(selector, selectedId) {
        $.ajax({
            url: "/Admin/Product/GetAllBlogCategory",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {

                response.sort(function (a, b) {
                    return a.data.sortOrder - b.data.sortOrder;
                });

                if (selector == "#ddlBlogCategoryId") {
                    $(selector).combotree({
                        data: response
                    });
                }
                else {
                    $(selector).combotree({
                        data: response,
                        onSelect: function ($node) {
                            nodeBlogCategoryId = $node.id;
                            loadData(true);
                        }
                    });
                }

                if (selectedId != undefined)
                    $(selector).combotree('setValue', selectedId);
            },
            error: function (message) {
                be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
            }
        });
    }

    function resetFormMaintainance() {
        $('#hidId').val(0);
        $('#txtName').val('');
        $('#txtImage').val('');
        $('#ddlTags').val(null).trigger('change');
        $('#ddlProductTypes').val(null).trigger('change');
        $('#txtDescription').val('');
        $('#txtPrice').val(0);
        $('#txtOriginalPrice').val(0);
        $('#txtPromotionPrice').val(0);
        $('#txtSeoKeywords').val('');
        $('#txtSeoDescription').val('');
        $('#txtSeoPageTitle').val('');
        $('#ckStatus').prop('checked', true);
        $('#ckHot').prop('checked', false);
        $('#ckShowHome').prop('checked', false);
        CKEDITOR.instances.txtContent.setData('');
    }

    var nodeBlogCategoryId = 0;
    function loadData(isPageChanged) {
        $.ajax({
            type: 'GET',
            data: {
                blogCategoryId: nodeBlogCategoryId,
                keyword: $('#txt-search-keyword').val(),
                page: be.configs.pageIndex,
                pageSize: be.configs.pageSize
            },
            url: '/admin/product/GetAllPaging',
            dataType: 'json',
            beforeSend: function () {
                be.startLoading();
            },
            success: function (response) {
                var template = $('#table-template').html();
                var render = "";
                $.each(response.Results, function (i, item) {
                    let listTags = "";
                    $.each(item.ProductTags, function () {
                        listTags += '<span class="m-badge m-badge--warning m-badge--wide m--margin-bottom-5 m--margin-right-5">' + this.TagId + '</span>'
                    })
                    debugger;
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Name: item.Name,
                        Image: item.Image == null ? '<img src="/admin-side/images/user.png" width=100' : '<img src="' + item.Image + '" width=100 />',
                        Tags: listTags,
                        ProductType: item.ProductType.Name,
                        BlogCategoryName: item.BlogCategory.Name,
                        Price: be.formatNumber(item.Price, 0),
                        OriginalPrice: be.formatNumber(item.OriginalPrice, 0),
                        PromotionPrice: be.formatNumber(item.PromotionPrice === null ? 0 : item.PromotionPrice, 0),
                        CreatedDate: be.dateTimeFormatJson(item.DateCreated),
                        Status: be.getStatus(item.Status)
                    });
                });

                $('#lbl-total-records').text(response.RowCount);

                $('#tbl-content').html(render);

                if (response.RowCount)
                    be.wrapPaging(response.RowCount, function () { loadData() }, isPageChanged);

                be.stopLoading();

            },
            error: function (message) {
                be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                be.stopLoading();
            },
        });
    }
}