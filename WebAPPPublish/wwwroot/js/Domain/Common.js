var Common;

Common = {

    Ajax: function (httpMethod, url, data, type, successCallBack, async, cache) {
        if (typeof async == "undefined") {
            async = true;
        }
        if (typeof cache == "undefined") {
            cache = false;
        }

        var ajaxObj = $.ajax({
            type: httpMethod.toUpperCase(),
            url: url,
            data: data,
            contentType: "application/x-www-form-urlencoded",
            dataType: type,
            traditional: true,
            async: async,
            cache: cache,
            success: successCallBack,
            error: function (err, type, httpStatus) {
                Common.AjaxFailureCallback(err, type, httpStatus);
            }
        });

        return ajaxObj;
    },

    DisplaySuccess: function (message) {
        Common.ShowSuccessSavedMessage(message);
    },

    DisplayError: function (error) {
        Common.ShowFailSavedMessage(message);
    },

    AjaxFailureCallback: function (err, type, httpStatus) {
        var failureMessage = 'Error occurred in ajax call' + err.status + " - " + err.responseText + " - " + httpStatus;
        console.log(failureMessage);
    },

    ShowSuccessSavedMessage: function (messageText) {

        $.blockUI({ message: messageText });
        setTimeout($.unblockUI, 1500);
    },

    ShowFailSavedMessage: function (messageText) {

        $.blockUI({ message: messageText });
        setTimeout($.unblockUI, 1500);
    }
}

function getObjectFormFields(formSelector) {
    /// <summary>Função que retorna objeto com base nas propriedades name dos elementos do formulário.</summary>
    /// <param name="formSelector" type="String">Seletor do formulário</param>

    var form = $(formSelector);

    var result = {};
    var arrayAuxiliar = [];
    form.find(":input:text").each(function (index, element) {
        var name = $(element).attr('name');

        var value = $(element).val();
        result[name] = value;
    });

    form.find(":input:password").each(function (index, element) {
        var name = $(element).attr('name');

        var value = $(element).val();
        result[name] = value;
    });

    form.find(":input[type=hidden]").each(function (index, element) {
        var name = $(element).attr('name');
        var value = $(element).val();
        result[name] = value;
    });


    form.find("input:checked").each(function (index, element) {
        var name;
        var value;
        if ($(this).attr("type") == "radio") {
            name = $(element).attr('name');
            value = $(element).val();
            result[name] = value;
        }
        else if ($(this).attr("type") == "checkbox") {
            name = $(element).attr('name');
            value = $(element).val();
            if (result[name]) {
                if (Array.isArray(result[name])) {
                    result[name].push(value);
                } else {
                    var aux = result[name];
                    result[name] = [];
                    result[name].push(aux);
                    result[name].push(value);
                }

            } else {
                result[name] = [];
                result[name].push(value);
            }
        }

    });

    form.find("select option:selected").each(function (index, element) {
        var name = $(element).parent().attr('name');
        var value = $(element).val();
        result[name] = value;

    });

    arrayAuxiliar = [];
    form.find("checkbox:checked").each(function (index, element) {
        var name = $(element).attr('name');
        var value = $(element).val();
        result[name] = arrayAuxiliar.push(value);
    });

    form.find("textarea").each(function (index, element) {
        var name = $(element).attr('name');
        var value = $(element).val();
        result[name] = value;
    });

    if (Array.isArray(result.IsActive))
        result.IsActive = true;

    result['__RequestVerificationToken'] = $('input[name=__RequestVerificationToken]').val();

    return result;
}

function SetActiveStatus(data) {
    var status = {
        "TRUE": {
            'title': 'Active',
            'class': ' label-light-success'
        },
        "FALSE": {
            'title': 'InActive',
            'class': ' label-light-danger'
        },
    };

    return '<span class="label label-lg font-weight-bold' + status[data].class + ' label-inline">' + status[data].title + '</span>';
}

function Activate(element, URI) {
    swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, do it!'
    }).then(function (result) {
        if (result.value) {
            $(element).find('i').hide();
            $(element).addClass('spinner spinner-left spinner-sm').attr('disabled', true);

            $.ajax({
                url: URI,
                type: 'Get',
                success: function (response) {
                    if (response.success) {
                        toastr.success(response.message);
                        Table.row($(element).closest('tr')).remove().draw();
                        addRow(response.data);
                    } else {
                        toastr.error(response.message);
                        $(element).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
                        $(element).find('i').show();
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == 403) {
                        try {
                            var response = $.parseJSON(xhr.responseText);
                            swal.fire(response.Error, response.Message, "warning").then(function () {
                                $('#myModal').modal('hide');
                            });
                        } catch (ex) {
                            swal.fire("Access Denied", "Your are not authorize to perform this action, For further details please contact administrator !", "warning").then(function () {
                                $('#myModal').modal('hide');
                            });
                        }

                        $(element).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
                        $(element).find('i').show();

                    }
                }
            });
        } else {
            //swal("Cancelled", "Your imaginary file is safe :)", "error");
        }
    });
}

function Delete(element, URI) {

    swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!'
    }).then(function (result) {
        if (result.value) {

            $.ajax({
                url: URI,
                type: 'POST',
                data: {
                    "__RequestVerificationToken":
                        $("input[name=__RequestVerificationToken]").val()
                },
                success: function (result) {
                    if (result.success != undefined) {
                        if (result.success) {
                            toastr.options = {
                                "positionClass": "toast-bottom-right",
                            };
                            toastr.success('Area Deleted Successfully');

                            Table.row($(element).closest('tr')).remove().draw();
                        }
                        else {
                            toastr.error(result.message);
                        }
                    } else {
                        swal.fire("Your are not authorize to perform this action", "For further details please contact administrator !", "warning").then(function () {
                        });
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == 403) {
                        try {
                            var response = $.parseJSON(xhr.responseText);
                            swal.fire(response.Error, response.Message, "warning").then(function () {
                                $('#myModal').modal('hide');
                            });
                        } catch (ex) {
                            swal.fire("Access Denied", "Your are not authorize to perform this action, For further details please contact administrator !", "warning").then(function () {
                                $('#myModal').modal('hide');
                            });
                        }


                        $(element).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
                        $(element).find('i').show();

                    }
                }
            });
        }
    });
}

