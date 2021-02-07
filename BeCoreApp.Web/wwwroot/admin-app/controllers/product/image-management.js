var ImageManagement = function () {

    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        $('body').on('click', '.btn-images', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $('#hidId').val(that);
            loadImages();
            $('#modal-image-manage').modal('show');
        });

        $('body').on('click', '.btn-delete-image', function (e) {
            e.preventDefault();
            $(this).closest('div.col-md-3').remove();
        });

        $("#fileImage").on('change', function () {

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
                success: function (path) {
                    $('#image-list').append('<div class="col-md-3"><div class="oldBoxImage"><img class="oldImage" width="100%" height="100%" data-path="'
                        + path +
                        '" src="'
                        + path +
                        '"><a href="#" class="btn-delete-image oldUrlRemove">REMOVE</a></div></div>');
                },
                error: function (message) {
                    be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                }
            });
        });

        $("#btnSaveImages").on('click', function () {
            var imageList = [];

            $.each($('#image-list').find('img'), function (i, item) {
                imageList.push($(this).data('path'));
            });

            $.ajax({
                url: '/admin/Product/SaveImages',
                data: {
                    productId: $('#hidId').val(),
                    images: imageList
                },
                type: 'post',
                dataType: 'json',
                success: function (response) {
                    $('#modal-image-manage').modal('hide');
                    $('#image-list').html('');
                },
                error: function (message) {
                    be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                }
            });
        });
    }

    function loadImages() {
        $.ajax({
            url: '/admin/Product/GetImages',
            data: { productId: $('#hidId').val() },
            type: 'get',
            dataType: 'json',
            success: function (response) {
                var render = '';
                $.each(response, function (i, item) {
                    render += '<div class="col-md-3"><div class="oldBoxImage"><img class="oldImage" width="100%" height="100%" data-path="'
                        + item.Path +
                        '" src="'
                        + item.Path +
                        '"><a href="#" class="btn-delete-image oldUrlRemove">REMOVE</a></div></div>'
                });

                $('#image-list').html(render);
            },
            error: function (message) {
                be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
            }
        });
    }
}