var LibraryManagement = function () {
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {

        $('body').on('click', '.btn-libraries', function (e) {
            loadLibraries($(this).data('id'));
        });

        $('body').on('click', '.btn-delete-library', function (e) {
            $(this).closest('div').remove();
        });

        $("#fileLibrary").on('change', function () {
            uploadLibrary(this)
        });

        $("#btnSaveLibraries").on('click', function () {
            saveLibraries()
        });
    }

    function uploadLibrary(element) {

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
                $('#library-list').append('<div class="col-md-12"><img width="200"  data-path="' + path + '" src="' + path + '"><a href="#" class="btn btn-danger m-btn m-btn--icon m-btn--icon-only m-btn--custom m-btn--pill btn-delete-library"><i class="la la-trash"></i></a></div>');
            },
            error: function (message) {
                be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
            }
        });
    }

    function saveLibraries() {
        var imageList = [];
        $.each($('#library-list').find('img'), function (i, item) {
            imageList.push($(this).data('path'));
        });

        debugger;
        $.ajax({
            url: '/admin/Project/SaveLibraries',
            data: {
                projectId: $('#hidProjectId').val(),
                images: imageList
            },
            type: 'post',
            dataType: 'json',
            success: function (response) {
                $('#modal-library-manage').modal('hide');
                $('#library-list').html('');
            },
            error: function (message) {
                be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
            }
        });
    }

    function loadLibraries(projectId) {
        $.ajax({
            url: '/admin/Project/GetLibraries',
            data: { projectId: projectId },
            type: 'get',
            dataType: 'json',
            success: function (response) {
                $('#hidProjectId').val(projectId);

                $("#fileLibrary").val('');
                var render = '';
                $.each(response, function (i, item) {
                    render += '<div class="col-md-12"><img width="200" data-path="' + item.Path + '" src="' + item.Path + '"><a href="#" class="btn btn-danger m-btn m-btn--icon m-btn--icon-only m-btn--custom m-btn--pill btn-delete-library"><i class="la la-trash"></i></a></div>'
                });
                $('#library-list').html(render);

                $('#modal-library-manage').modal('show');
            },
            error: function (message) {
                be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
            }
        });
    }
}