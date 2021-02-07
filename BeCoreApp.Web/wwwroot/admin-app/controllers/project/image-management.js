var ImageManagement = function () {
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {

        $('body').on('click', '.btn-images', function (e) {
            loadImages($(this).data('id'));
        });

        $('body').on('click', '.btn-delete-image', function (e) {
            $(this).closest('div').remove();
        });

        $("#fileImage").on('change', function () {
            uploadImage(this)
        });

        $("#btnSaveImages").on('click', function () {
            saveImages()
        });
    }

    function uploadImage(element) {

        var fileUpload = $(element).get(0);
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
                $('#image-list').append('<div class="col-md-12"><img width="200"  data-path="' + path + '" src="' + path + '"><a href="#" class="btn btn-danger m-btn m-btn--icon m-btn--icon-only m-btn--custom m-btn--pill btn-delete-image"><i class="la la-trash"></i></a></div>');
            },
            error: function (message) {
                be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
            }
        });
    }

    function saveImages() {
        var imageList = [];
        $.each($('#image-list').find('img'), function (i, item) {
            imageList.push($(this).data('path'));
        });

        $.ajax({
            url: '/admin/Project/SaveImages',
            data: {
                projectId: $('#hidId').val(),
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
    }

    function loadImages(projectId) {
        $.ajax({
            url: '/admin/Project/GetImages',
            data: { projectId: projectId },
            type: 'get',
            dataType: 'json',
            success: function (response) {
                $('#hidId').val(projectId);

                $("#fileImage").val('');
                var render = '';
                $.each(response, function (i, item) {
                    render += '<div class="col-md-12"><img width="200" data-path="' + item.Path + '" src="' + item.Path + '"><a href="#" class="btn btn-danger m-btn m-btn--icon m-btn--icon-only m-btn--custom m-btn--pill btn-delete-image"><i class="la la-trash"></i></a></div>'
                });
                $('#image-list').html(render);

                $('#modal-image-manage').modal('show');
            },
            error: function (message) {
                be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
            }
        });
    }
}