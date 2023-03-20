function UploadImageToDraft(file) {
    $('.BtnSave').addClass('spinner spinner-sm spinner-left').attr('disabled', true);
    var data = new FormData();
    var result;

    data.append('file', file);

    return $.ajax({
        url: "/File/Upload",
        type: "POST",
        processData: false,
        contentType: false,
        async: false,
        data: data,
        success: function (response) {
            $('.BtnSave').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);

            if (response.includes("Draft"))
                result = response
        },
        error: function (er) {
            toastr.error(er);
        }
    });

    return result.responseText;
}

function DeleteImage(Path, Table = 'Optional') {
    $.ajax({
        url: "/File/Delete?path=" + Path + "&table=" + Table,
        type: "GET",
        processData: false,
        contentType: false,
        success: function (response) {
        },
        error: function (er) {
        }
    });
}