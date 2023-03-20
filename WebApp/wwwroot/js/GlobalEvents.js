$(document).ready(function () {

    $(function () {
        $(document).ajaxError(function (e, xhr) {
            if (xhr.status == 401) {
                var response = $.parseJSON(xhr.responseText);
                window.location = response.LogOnUrl;
            }
            else if (xhr.status == 403) {

                try {
                    
                    var response = $.parseJSON(xhr.responseText);
                    if (!swal.isVisible()) {
                        $('#myModal').modal('hide');

                        swal.fire(response.Error, response.Message, "error").then(function () {
                            $('#myModal').modal('hide');
                        });
                    }
                } catch (ex) {                    
                    if (!swal.isVisible()) {
                        $('#myModal').modal('hide');
                        swal.fire("Access Denied", "Your are not authorize to perform this action, For further details please contact administrator !", "error").then(function () {
                            $('#myModal').modal('hide');
                        });
                    }
                }
            }
        });
    });

});