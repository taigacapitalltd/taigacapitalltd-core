var ReferralController = function () {
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {

        $('body').on('click', '.btn-detail', function (e) {
            loadDetails(e, $(this).attr('data-blog-id'));
        });
        $('body').on('click', '.btn-link', function (e) {
            loadGetReferralLinkUpline(e, $(this).attr('data-blog-id'));
        });

        document.getElementById("btnCopyReferlink").addEventListener("click", function () {
            copyToClipboard(document.getElementById("txtLink"));
        });
    }

    function copyToClipboard(elem) {
        // create hidden text element, if it doesn't already exist
        var targetId = "_hiddenCopyText_";
        var isInput = elem.tagName === "INPUT" || elem.tagName === "TEXTAREA";
        var origSelectionStart, origSelectionEnd;
        if (isInput) {
            // can just use the original source element for the selection and copy
            target = elem;
            origSelectionStart = elem.selectionStart;
            origSelectionEnd = elem.selectionEnd;
        } else {
            // must use a temporary form element for the selection and copy
            target = document.getElementById(targetId);
            if (!target) {
                var target = document.createElement("textarea");
                target.style.position = "absolute";
                target.style.left = "-9999px";
                target.style.top = "0";
                target.id = targetId;
                document.body.appendChild(target);
            }
            target.textContent = elem.textContent;
        }
        // select the content
        var currentFocus = document.activeElement;
        target.focus();
        target.setSelectionRange(0, target.value.length);

        // copy the selection
        var succeed;
        try {
            succeed = document.execCommand("copy");
        } catch (e) {
            succeed = false;
        }
        // restore original focus
        if (currentFocus && typeof currentFocus.focus === "function") {
            currentFocus.focus();
        }

        if (isInput) {
            // restore prior selection
            elem.setSelectionRange(origSelectionStart, origSelectionEnd);
        } else {
            // clear temporary content
            target.textContent = "";
        }
        return succeed;
    }


    function loadGetReferralLinkUpline(e, blogId) {
        debugger;
        e.preventDefault();

        $.ajax({
            type: "GET",
            url: "/Admin/Referral/GetReferralLinkUpline",
            data: { blogId: blogId },
            dataType: "json",
            beforeSend: function () {
                be.startLoading();
            },
            success: function (response) {
                $('.txtName').html(response.BlogName);
                $('#txtLink').val(response.ReferralLink);
                $('#txtDescription').html(response.Description);

                be.stopLoading();

                $('#modal-link').modal('show');
            },
            error: function (message) {
                be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                be.stopLoading();
            }
        });
    }

    function loadDetails(e, blogId) {
        debugger;
        e.preventDefault();

        $.ajax({
            type: "GET",
            url: "/Admin/Blog/GetById",
            data: { id: blogId },
            dataType: "json",
            beforeSend: function () {
                be.startLoading();
            },
            success: function (response) {
                $('.txtName').html(response.Name);
                $('.box-content').html(response.MildContent);
                $('.box-video').html(response.Video);
                be.stopLoading();

                $('#modal-detail').modal('show');
            },
            error: function (message) {
                be.notify(`${message.responseText}`, `Status code: ${message.status}`, 'error');
                be.stopLoading();
            }
        });
    }


}