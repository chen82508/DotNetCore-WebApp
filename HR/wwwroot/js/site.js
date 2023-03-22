// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function disableF5(e) { if ((e.which || e.keyCode) == 116) e.preventDefault(); };

function toggleLoading(isShow) {
    if (isShow) {
        $('body').append('<div id="loading-backdrop" class="modal-backdrop fade show"></div>');
        $('#loading').addClass('show').css('display', 'block');
    } else {
        $('#loading').removeClass('show').css('display', 'none');
        $('body').find('.modal-backdrop').removeClass('show');
        setTimeout(() => { $('body').find('#loading-backdrop').remove(); }, 170);
    }
}

function getMethod(url, before, success, async) {
    $.ajax({
        url: url,
        method: 'GET',
        async: async,
        beforeSend: before,
        success: success,
        error: (err) => { console.log(err); }
    });
}

function postMethod(url, before, success, async, data) {
    $.ajax({
        url: url,
        method: 'POST',
        async: async,
        data: data,
        beforeSend: before,
        success: success,
        error: (err) => { console.log(err); }
    });
}

function patchMethod(url, data, success) {
    $.ajax({
        url: url,
        method: 'PATCH',
        async: false,
        data: data,
        success: success,
        error: (err) => { console.log(err); alert('更新失敗'); }
    });
}

function deleteMethod(url, data, success) {
    $.ajax({
        url: url,
        method: 'DELETE',
        async: true,
        data: data,
        success: success,
        error: (err) => { console.log(err); }
    });
}

const formatDate = (date) => {
    let formatted_date = `${date.getFullYear()}/${zeroPad(date.getMonth() + 1, 2)}/${zeroPad(date.getDate(), 2)} ${zeroPad(date.getHours(), 2)}:${zeroPad(date.getMinutes(), 2)}`;
    return formatted_date;
}

const zeroPad = (num, places) => String(num).padStart(places, '0');



var Upload = function (file) {
    this.file = file;
};

Upload.prototype.getType = function () {
    return this.file.type;
};
Upload.prototype.getSize = function () {
    return this.file.size;
};
Upload.prototype.getName = function () {
    return this.file.name;
};
Upload.prototype.doUpload = function (uploadUrl) {
    var that = this;
    var formData = new FormData();

    // add assoc key values, this will be posts values
    formData.append("file", this.file, this.getName());
    formData.append("upload_file", true);

    $.ajax({
        type: 'POST',
        url: uploadUrl,
        xhr: function () {
            var myXhr = $.ajaxSettings.xhr();
            if (myXhr.upload) {
                myXhr.upload.addEventListener('progress', that.progressHandling, false);
            }
            return myXhr;
        },
        success: function (data) {
            console.log(data);
        },
        error: function (error) {
            alert('上傳失敗');
        },
        async: false,
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        timeout: 60000
    });
};

Upload.prototype.progressHandling = function (event) {
    var percent = 0;
    var position = event.loaded || event.position;
    var total = event.total;
    var progress_bar_id = "#progress-wrp";
    if (event.lengthComputable) {
        percent = Math.ceil(position / total * 100);
    }
    // update progressbars classes so it fits your code
    $(progress_bar_id + " .progress-bar progress-bar-striped progress-bar-animated bg-success").css("width", + percent + "%");
    $(progress_bar_id + " .status").text(percent + "%");
};
