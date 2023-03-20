"use strict";
var table1;
var KTDatatablesBasicScrollable = function () {

    var initTable1 = function () {
        var table = $('#kt_datatable2');

        // begin first table
        table1 = table.DataTable({
            //scrollY: '50vh',
            scrollX: true,
            scrollCollapse: true,
            width: "230px",
            orderable: true,
            "language": {
                processing: '<i class="spinner spinner-left spinner-dark spinner-sm"></i>'
            },
            "initComplete": function (settings, json) {
                $('#kt_datatable2 tbody').fadeIn();
            },
            columnDefs: [
                {
                targets: -1,
                title: 'Actions',
                orderable: false,
                width: "250px",
                className: "dt-center",

                    render: function (data, type, full, meta) {
                        var isActive = full[3].toString();
                        isActive = isActive.toUpperCase();
                        var actions = '';
                            var status = {
                                "TRUE": {
                                    'title': 'Deactivate',
                                    'icon': 'fa-times-circle',
                                    'class': ' btn-outline-danger'
                                },
                                "FALSE": {
                                    'title': 'Activate',
                                    'icon': 'fa-check-circle',
                                    'class': ' btn-outline-success'
                                },
                        };
                        actions += '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Admin/Customer/Edit?Id=' + data + '\',true)">' +
                                '<i class="fa fa-pen"></i>' +
                                '</button> ' +
                                '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Admin/Customer/Details?Id=' + data + '\')" title="View">' +
                                '<i class="fa fa-folder-open"></i>' +
                            '</button> ' +
                            '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="Delete(this,' + data + ')"><i class="fa fa-trash"></i></button>';
                        if (typeof status[isActive] === 'undefined') {
                            actions += `<button type="button" class="btn btn-outline-success btn-sm mr-1" onclick='Activate(this,${data})'>`;
                            actions += '<i class="fa fa-check-circle" aria-hidden="true"></i> Activate' +
                                '</button>';
                        } else {
                            actions += `<button type="button" class="btn btn-sm mr-1' + ${status[isActive].class} + "' onclick='Activate(this,${data})'>`;
                            actions += '<i class="fa ' + status[isActive].icon + '" aria-hidden="true"></i> ' + status[isActive].title +
                                '</button>';
                        }
                            return actions;
                },
            },
            {
                targets: 3,
                width: '75px',
                render: function (data, type, full, meta) {
                    var data = data.toString();
                    data = data.toUpperCase();
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
                    if (typeof status[data] === 'undefined') {

                        return '<span class="label label-lg font-weight-bold label-light-danger label-inline">Inactive</span>';
                    }
                    return '<span class="label label-lg font-weight-bold' + status[data].class + ' label-inline">' + status[data].title + '</span>';
                },
            },
            {
                targets: 0,
                width: "130px"
            },
            ],
        });
    };

    return {
        //main function to initiate the module
        init: function () {
            initTable1();
        },
    };
}();

jQuery(document).ready(function () {
    KTDatatablesBasicScrollable.init();
});


function Delete(element, record) {
    swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!'
    }).then(function (result) {
        if (result.value) {

            $.ajax({
                url: '/Admin/Customer/Delete?Id=' + record,
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
                            toastr.success('User deleted successfully ...');

                            table1.row($(element).closest('tr')).remove().draw();
                            location.reload();
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

function Activate(element, record) {
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
                url: '/Admin/Customer/ToggleActiveStatus?Id=' + record,
                type: 'Get',
                success: function (response) {
                    if (response.success) {
                        toastr.success(response.message);
                        table1.row($(element).closest('tr')).remove().draw();
                        addRow(response.data);
                    //    location.reload();
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



function callback(dialog, elem, isedit, response) {
    if (response.success) {
        toastr.success(response.message);

        if (isedit) {
            table1.row($(elem).closest('tr')).remove().draw();
        }

        addRow(response.data);
        jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);
    }
    else {
        jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);

        toastr.error(response.message);
    }
}

function addRow(row) {
    
    table1.row.add([
        row.creationDate,
        row.customerName,
        row.email,
        row.isActive,
        row.id,
    ]).draw(false);

}