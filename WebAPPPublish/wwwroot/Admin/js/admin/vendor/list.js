"use strict";
var table1;
var KTDatatablesBasicScrollable = function () {

    var initTable1 = function () {
        var table = $('#kt_datatable1');

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
                $('#kt_datatable1 tbody').fadeIn();
            },
            columnDefs: [
                {
                    targets: -1,
                    title: 'Actions',
                    orderable: false,
                    width: "300px",
                    className: "dt-center",

                    render: function (data, type, full, meta) {
                        let str1 = new String('"');
                        let str2 = new String('"');
                        var UserId = str1 + data + str2;
                        var status = full[3].toString().toUpperCase();
                        var actions = '';
                        actions += '<a class="btn btn-bg-secondary btn-icon btn-sm mr-1" href="/Admin/Vendor/Edit/' + data + '" title="Edit">' +
                            '<i class="fa fa-pen"></i>' +
                            '</a> ';
                        actions += `<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick='Delete(this,${UserId})'><i class="fa fa-trash"></i></button>`;

                        if (status === 'PROCESSING') {
                            actions += `<button type="button" class="btn btn-sm mr-1 btn-outline-success" onclick='activeChange(this,true,${UserId})'>`;
                            actions += '<i class="fa fa-check-circle" aria-hidden="true"></i> Approve</button>';
                            actions += `<button type="button" class="btn btn-sm mr-1 btn-outline-danger" onclick='Reject(this,false,${UserId})'>`;
                            actions += '<i class="fa fa-times-circle" aria-hidden="true"></i> Reject</button>';
                        }
                        else if (status === 'INACTIVE') {
                            actions += `<button type="button" class="btn btn-sm mr-1 btn-outline-success" onclick='activeChange(this,true,${UserId})'>`;
                            actions += '<i class="fa fa-check-circle" aria-hidden="true"></i> Activate</button>';
                        }
                        else if (status === 'ACTIVE') {
                            actions += `<button type="button" class="btn btn-sm mr-1 btn-outline-danger" onclick='activeChange(this,false,${UserId})'>`;
                            actions += '<i class="fa fa-times-circle" aria-hidden="true"></i> Deactivate</button>';
                        }

                        return actions;
                    },
                },
                {
                    targets: 1,
                    /*width: '75px',*/
                    render: function (data, type, full, meta) {
                        if (!data) {
                            return '<span>-</span>';
                        }
                        var title = data.split('|');
                        return `<div class="d-flex align-items-center">
								<div>
									<a href="javascript:;" class="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg">${title[0]}</a>
                                    ${title[1] != '' ? `<span class="text-muted font-weight-bold d-block" style="color:#008bb9 !important;white-space: nowrap; overflow: hidden; text-overflow: ellipsis;"> ${title[1]}</span>` : ``}
								</div>
							</div>`;
                    },
                },
                {
                    targets: 0,
                    width: '130px',
                },
                {
                    targets: 2,
                    title: 'Contact',
                    /*width: '200px',*/
                    className: "dt-center",
                    render: function (data, type, full, meta) {
                        if (data.includes("|")) {
                            var dataArr = data.split("|");

                            return dataArr[0] + ' <span style="font-size:12px;"><a href="javascript:void(0);">' + dataArr[1] + '</a></span>';
                        }
                        return data;
                    }
                },
                {
                    targets: 3,
                    title: 'Status',
                    width: '80px',
                    className: "dt-center",
                    render: function (data, type, full, meta) {
                        var data = data.toString().toUpperCase();
                        var status = {
                            "DRAFT": {
                                'title': 'Draft',
                                'class': ' label-light-primary'
                            },
                            "PROCESSING": {
                                'title': 'Processing',
                                'class': ' label-light-info'
                            },
                            "ACTIVE": {
                                'title': 'Active',
                                'class': ' label-light-success'
                            },
                            "INACTIVE": {
                                'title': 'InActive',
                                'class': ' label-light-danger'
                            },
                             "PENDING": {
                                 'title': 'Pending',
                                 'class': ' label-light-info'
                            },
                              "REJECTED": {
                                'title': 'Rejected',
                                  'class': ' label-light-danger'
                            }
                        };
                        if (typeof status[data] === 'undefined') {

                            return '<span class="label label-lg font-weight-bold label-light-danger label-inline">undefined</span>';
                        }
                        return '<span class="label label-lg font-weight-bold' + status[data].class + ' label-inline">' + status[data].title + '</span>';
                        //return `<label class="label label-inline label-primary">${data}</label>`;
                    }
                }
                //{
                //	targets: 0,
                //	width: "130px"
                //},
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
                url: '/Admin/Vendor/Delete/' + record,
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
                            toastr.success('Vendor deleted successfully ...');

                            table1.row($(element).closest('tr')).remove().draw();
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

function activeChange(element, flag, id) {
    swal.fire({
        title: 'Are you sure?',
        text: "You can revert this later",
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, do it!'
    }).then(function (result) {
        if (result.value) {
            $(element).find('i').hide();
            $(element).addClass('spinner spinner-left spinner-sm').attr('disabled', true);

            $.ajax({
                url: '/Admin/Vendor/ToggleActiveStatus?Id=' + id + '&flag=' + flag,
                type: 'Get',
                success: function (response) {
                    if (response.success) {
                        toastr.success(response.message);
                        table1.row($(element).closest('tr')).remove().draw();
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




function callback(dialog, elem, isedit, response) {
    if (response.success) {
        toastr.success(response.message);

        if (isedit) {
            table1.row($(elem).closest('tr')).remove().draw();
        }

        addRow(response.data);
        jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);
        location.reload();
    }
    else {
        jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);

        toastr.error(response.message);
    }
}

function addRow(row) {
    table1.row.add([
        row.creationDate,
        row.nameAsPerTradeLicense + '|' + row.contactPersonEmail,
        row.contactPersonNumber,
        row.status,
        row.id
    ]).draw(true);
}


function Reject(element, flag, id) {
    Swal.fire({
        title: 'Are you sure, you want to reject?',
        type: 'warning',
        input: 'text',
        confirmButtonText: 'Reject',
        confirmButtonColor: '#ffb000',
        showCancelButton: true,
        cancelButtonColor: '#F64E60',
        placeholderColor: '#ffb000',
        preConfirm: (value) => {

            if (!value) {
                Swal.showValidationMessage(
                    'You need to write something!'
                )
            }
        },
    }).then((result) => {
        if (result.value) {
            $(element).find('i').hide();
            $(element).addClass('spinner spinner-left spinner-sm').attr('disabled', true);
            debugger
            $.ajax({
                url: '/Admin/Vendor/ToggleActiveStatus?Id=' + id + '&flag=' + flag + '&RejectionReason=' + result.value,
                type: 'Get',
                success: function (response) {
                    if (response.success) {
                        toastr.success(response.message);
                        table1.row($(element).closest('tr')).remove().draw();
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
        }
    });
}