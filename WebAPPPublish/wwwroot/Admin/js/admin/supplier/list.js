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
            "language": {
                processing: '<i class="spinner spinner-left spinner-dark spinner-sm"></i>'
            },
            "initComplete": function (settings, json) {
                $('#kt_datatable1 tbody').fadeIn();
                if (settings.aoData.length <= 0) {
                    $('.excel-btn').prop('disabled', true);
                }
                else {
                    $('.excel-btn').prop('disabled', false);
                }
            },
            // Pagination settings


            columnDefs: [
                {
                    targets: 0,
                    className: "dt-center",
                    width: '130px',
                }, {
                    targets: -1,
                    title: 'Actions',
                    orderable: false,
                    width: '230px',
                    className: "dt-center",
                    render: function (data, type, full, meta) {

                        data = data.split(',');
                        var isActive = data[0].toUpperCase();
                        var status = {
                            "ACTIVE": {
                                'title': 'Deactivate',
                                'icon': 'fa-times-circle',
                                'class': ' btn-outline-danger'
                            },
                            "DEACTIVE": {
                                'title': 'Activate',
                                'icon': 'fa-check-circle',
                                'class': ' btn-outline-success'
                            },
                        };

                        var actions = '';

                        actions += '<a  class="btn btn-bg-secondary btn-icon btn-sm mr-1" href="/Admin/Supplier/Edit/' + data[1] + '">' +
                            '<i class="fa fa-pen"></i>' +
                            '</a> ' +
                            '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1"  onclick="OpenModelPopup(this,\'/Admin/Supplier/SupplierDetails/' + data[1] + '\')" title="View">' +
                            '<i class="fa fa-folder-open"></i>' +
                            '</button> ' +
                            '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1"" onclick="Delete(this,' + data[1] + ')"><i class="fa fa-trash"></i></button>';


                        if (typeof status[isActive] === 'undefined') {
                            actions += '<button type="button" class="btn btn-outline-success btn-sm mr-1" onclick="Activate(this, ' + data[1] + ')">' +
                                '<i class="fa fa-check-circle" aria-hidden="true"></i> Activate' +
                                '</button>';
                        } else {
                            actions += '<button type="button" class="btn btn-sm mr-1' + status[isActive].class + '" onclick="Activate(this, ' + data[1] + ')">' +
                                '<i class="fa ' + status[isActive].icon + '" aria-hidden="true"></i> ' + status[isActive].title +
                                '</button>';
                        }

                        return actions;
                    },
                },
                {
                    targets: 1,
                    width: '270px',
                    render: function (data, type, full, meta) {

                        if (!data) {
                            return '<span>-</span>';
                        }
                        var vendor = data.split('|');
                        return '<div class="d-flex align-items-center">' +
                            '<div class="symbol symbol-50 flex-shrink-0 mr-4">' +
                            '<div class="symbol-label" style="background-image: url(\'' + vendor[0] + '\')"></div>' +
                            '</div>' +
                            '<div>' +
                            '<a href="#" class="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg">' + vendor[1] + '</a><br>' +
                            '<a href="#" class="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg">' + vendor[3] + '</a>' +
                            '<span class="text-muted font-weight-bold d-block">' + vendor[2] + '</span>' +
                            '</div>' +
                            '</div>';

                    },
                },
                {
                    targets: 4,
                    width: '75px',
                    className: "dt-center",
                    render: function (data, type, full, meta) {
                        data = data.toUpperCase();
                        
                        var status = {
                            "ACTIVE": {
                                'title': 'Active',
                                'class': ' label-light-success'
                            },
                            "INACTIVE": {
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

                    targets: 3,
                    title: 'Transaction History',
                    orderable: false,
                    width: '180px',
                    className: "dt-center",

                    render: function (data, type, full, meta) {

                        /*data = data.split(',');*/
                        var actions = "";

                        actions += '<button class="btn btn-bg-secondary" onclick="OpenModelPopup(this,\'/Admin/Supplier/SupplierTransaction?SupplierId=' + data + '\');">' +
                            '<i class="fa fa-server"></i>' +
                            'Transaction History</button> ';

                        return actions;
                    },
                },
                //{
                //    targets: 4,
                //    title: 'Email Verification',
                //    orderable: false,
                //    width: '75px',
                //    className: "dt-center",
                //    render: function (data, type, full, meta) {

                //        data = data.split(',');
                //        var isActive = data[0].toUpperCase();
                //        var status = {
                //            "TRUE": {
                //                'title': ' Email Sent',
                //                'icon': 'fa-check-circle',
                //                'class': ' btn-outline-success'
                //            },
                //            "FALSE": {
                //                'title': 'Not Sent',
                //                'icon': 'fa-times-circle',
                //                'class': ' btn-outline-danger'
                //            },
                //        };

                //        var actions = '';
                //        if (!isActive) {
                //            isActive = "FALSE";
                //        }

                //        var checkEmailSent = (status[isActive].title == " Email Sent") ? "1" : "this";

                //        if (typeof status[isActive] === 'undefined') {
                //            actions += '<button type="button" class="btn btn-outline-danger btn-sm mr-1" onclick="EmailSent(this, ' + data[1] + ')">' +
                //                '<i class="fa fa-times-circle" aria-hidden="true"></i> Not Sent' +
                //                '</button>';
                //        } else {
                //            actions += '<button type="button" class="btn btn-sm mr-1' + status[isActive].class + '" onclick="EmailSent(' + checkEmailSent + ', ' + data[1] + ')">' +
                //                '<i class="fa ' + status[isActive].icon + '" aria-hidden="true"></i> ' + status[isActive].title +
                //                '</button>';
                //        }

                //        return actions;
                //    },
                //},
                {

                    targets: 2,
                    title: 'Documents',
                    orderable: false,
                    width: '150px',
                    className: "dt-center",

                    render: function (data, type, full, meta) {

                        /*data = data.split(',');*/
                        var actions = "";

                        actions += '<button type="button" class="btn btn-bg-secondary  btn-sm mr-1" onclick="OpenModelPopup(this,\'/Admin/Supplier/DocumentModel?SupplierId=' + data + '\')" title="View">' +
                            '<i class="fa fa-server"></i>' +
                            'Documents</button> ';

                        return actions;
                    },
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
                url: '/Admin/Supplier/Delete/' + record,
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
                            toastr.success('Supplier Deleted Successfully');

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
                url: '/Admin/Supplier/Activate/' + record,
                type: 'Get',
                success: function (response) {
                    if (response.success) {
                        toastr.success(response.message);
                        table1.row($(element).closest('tr')).remove().draw();
                        
                        /*addRow(response.data);*/
                        location.reload();
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
function EmailSent(element, record) {

    if (element == "1") {
        swal.fire({
            title: 'Email Verified',
            type: 'success',
        });
    }
    else {
        swal.fire({
            title: 'Email Verification',
            type: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Send Mail!'
        }).then(function (result) {
            if (result.value) {
                $(element).find('i').hide();
                $(element).addClass('spinner spinner-left spinner-sm').attr('disabled', true);

                $.ajax({
                    url: '/Admin/Vendor/EmailSent/' + record,
                    type: 'Get',
                    success: function (response) {
                        if (response.success) {
                            toastr.success(response.message);
                            setTimeout(function () { location.reload(); }, 1000);
                            //table1.row($(element).closest('tr')).remove().draw();
                            //addRow(response.data);
                        } else {
                            toastr.error(response.message);
                            $(element).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
                            $(element).find('i').show();
                        }
                    },
                    error: function (response) {
                        $(element).removeClass('spinner spinner-sm spinner-left').attr('disabled', false);
                    }
                });
            } else {
                //swal("Cancelled", "Your imaginary file is safe :)", "error");
            }
        });
    }

}




function callback(dialog, elem, isedit, response) {

    if (response.success) {
        toastr.success(response.message);

        if (isedit) {
            table1.row($(elem).closest('tr')).remove().draw();
        }

        addRow(response.data);
        jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);
        jQuery('#myModal').modal('hide');
    }
    else {
        jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);

        toastr.error(response.message);
    }
}

function addRow(row) {
    
    console.log("Rpow", row);
    table1.row.add($('<tr>' +
        '<td data-order=' + row.ID + '>' + row.Date + '</td>' +
        '<td>' + row.Logo + '|' + row.Name + '|' + row.VendorCode + '|' + row.Email + '</td>' +
        '<td>' + row.ID + '</td>' +
        '<td>' + row.ID + '</td>' +
        '<td>' + row.IsEmailSent + ',' + row.ID + '</td>' +
        '<td>' + row.IsActive + '</td>' +
        '<td nowrap="nowrap">' + row.IsActive + ',' + row.ID + '</td>' +
        '</tr>'
    )).draw();

}