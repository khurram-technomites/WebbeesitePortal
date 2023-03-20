"use strict";
var table2;
var KTDatatablesBasicScrollable = function () {

    var initTable = function () {
        var table = $('#kt_datatable1');

        // begin first table
        table2 = table.DataTable({
            //scrollY: '50vh',
            scrollX: true,
            scrollCollapse: true,

            "language": {
                processing: '<i class="spinner spinner-left spinner-dark spinner-sm"></i>'
            },
            "initComplete": function (settings, json) {
                $('#kt_datatable1 tbody').fadeIn();
            },
            columnDefs: [
                {
                    targets: -1,
                    title: 'actions',
                    orderable: false,
                    classname: "dt-center",
                    width: '90px',
                    className: "dt-center",
                    render: function (data, type, full, meta) {
                        //data = data.split(',');
                        //var isactive = data[0].touppercase();
                        //var status = {
                        //	"true": {
                        //		'title': 'deactivate',
                        //		'icon': 'fa-times-circle',
                        //		'class': ' btn-outline-danger'
                        //	},
                        //	"false": {
                        //		'title': 'activate',
                        //		'icon': 'fa-check-circle',
                        //		'class': ' btn-outline-success'
                        //	},
                        //};

                        var actions = '';

                        actions +=
                            '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Client/CustomerAppointment/Detail?id=' + data + '\')" title="view">' +
                            '<i class="fa fa-folder-open"></i>'


                        //if (typeof status[isactive] === 'undefined') {
                        //	actions += '<button type="button" class="btn btn-outline-success btn-sm mr-1" onclick="activate(this, ' + data[1] + ')">' +
                        //		'<i class="fa fa-check-circle" aria-hidden="true"></i> activate' +
                        //		'</button>';
                        //} else {
                        //	actions += '<button type="button" class="btn btn-sm mr-1' + status[isactive].class + '" onclick="activate(this, ' + data[1] + ')">' +
                        //		'<i class="fa ' + status[isactive].icon + '" aria-hidden="true"></i> ' + status[isactive].title +
                        //		'</button>';
                        //}




                        return actions;
                    },
                },
                {
                    targets: 4,
                    width: '75px',
                    className: "dt-center",

                },

                {
                    targets: 3,
                    width: '75px',
                    className: "dt-center",
                    //render: function (data, type, full, meta) {

                    //    var data = full[3].toUpperCase();
                    //    var status = {
                    //        "CASHIER": {
                    //            'title': 'CASHIER',
                    //            'class': ' label-light-success'
                    //        },
                    //        "KITCHEN": {
                    //            'title': 'KITCHEN',
                    //            'class': ' label-light-warning'
                    //        },
                    //        "PACKAGING": {
                    //            'title': 'PACKAGING',
                    //            'class': ' label-light-danger'
                    //        },
                    //    };

                    //    if (typeof status[data] === 'undefined')
                    //        return '<span class="label label-lg font-weight-bold label-light-danger label-inline">-</span>';
                    //    else
                    //        return '<span class="label label-lg font-weight-bold' + status[data].class + ' label-inline">' + status[data].title + '</span>';
                    //}

                },

                {
                    targets: 2,
                    width: '75px',
                    className: "dt-center",

                },

                {

                    targets: 1,
                    width: '100px',
                    //render: function (data, type, full, meta) {

                    //    var printer = data.split('|');
                    //    var printerIcon = '/Images/Icons/printerIcon.png';

                    //    if (printer[1].toString().toLowerCase() == 'true') {
                    //        defaultLabel = '<small class="label-inline text-success">Default Printer</small>';
                    //    }

                    //    return '<div class="d-flex align-items-center">' +
                    //        '<div class="symbol symbol-35 flex-shrink-0 mr-4">' +
                    //        '<div class="symbol-label" style="background-image: url(\'' + printerIcon + '\')"></div>' +
                    //        '</div>' +
                    //        '<div>' +
                    //        '<a href="#" class="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg">' + printer[0] + '</a><br>' +
                    //        // '<a href="#" class="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg">' + vendor[3] + '</a>' +
                    //        '<small class="text-muted font-weight-bold d-block">' + printer[1] + '</small>' +
                    //        '</div>' +
                    //        '</div>';

                    //},
                    //render: function (data, type, full, meta) {

                    //	data = data.toString();
                    //	data = data.toUpperCase();
                    //	var status = {
                    //		"TRUE": {
                    //			'title': 'Active',
                    //			'class': ' label-light-success'
                    //		},
                    //		"FALSE": {
                    //			'title': 'InActive',
                    //			'class': ' label-light-danger'
                    //		},
                    //	};
                    //	if (typeof status[data] === 'undefined') {

                    //		return '<span class="label label-lg font-weight-bold label-light-danger label-inline">Inactive</span>';
                    //	}
                    //	return '<span class="label label-lg font-weight-bold' + status[data].class + ' label-inline">' + status[data].title + '</span>';
                    //},
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
            initTable();
        },
    };
}();

jQuery(document).ready(function () {
    KTDatatablesBasicScrollable.init();
    $("#fromDate").datepicker({
        todayHighlight: true,
    });

    $("#toDate").datepicker({
        todayHighlight: true,
    });

    $("#fromDate").change(function () {

        if (new Date($("#fromDate").val()) > new Date($("#toDate").val())) {
            $('#toDate').datepicker('setDate', new Date($("#fromDate").val()));
            $("#toDate").datepicker("option", "minDate", new Date($("#fromDate").val()));
        }
    });

    $("#toDate").change(function () {

        if (new Date($("#fromDate").val()) > new Date($("#toDate").val())) {
            $('#fromDate').datepicker('setDate', new Date($("#toDate").val()));
            $("#fromDate").datepicker("option", "maxDate", new Date($("#toDate").val()));
        }
    });


    var fromDate = $('#fromDate').val();
    var toDate = $('#toDate').val();

    $('#from').val(fromDate);
    $('#to').val(toDate);

    //$('.kt_datepicker_range').datepicker({
    //    todayHighlight: true,
    //});



    $("#btnSearchfilter").on("click", function () {

        var fromDate = $('#fromDate').val();
        var toDate = $('#toDate').val();

        if (fromDate == "" && toDate == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Please! Select Date',
            })
        }
        else if (fromDate == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Please! Select From Date',
            })
        }
        else if (toDate == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Please! Select To Date',
            })
        }

        $.ajax({
            url: '/Vendor/CoachBooking/List',
            type: 'POST',
            data: {
                fromDate: $('#fromDate').val(),
                toDate: $('#toDate').val(),
            },
            success: function (data) {
                "use strict";

                if (data != null) {

                    $("#Orders").html(data);
                    KTDatatablesBasicScrollable.init();
                    $('#from').val(fromDate);
                    $('#to').val(toDate);

                    var td = data.includes("</td>");
                    if (td) {
                        $('#btnSubmit').show();
                    }
                }
                else {
                    $('#btnSubmit').hide();
                }
            }
        });

    })//btnSearch;


});

//function StatusChange(element, record) {
//    swal.fire({
//        title: 'Are you sure?',
//        text: "You won't be able to revert this!",
//        type: 'warning',
//        showCancelButton: true,
//        confirmButtonText: 'Yes, do it!'
//    }).then(function (result) {
//        if (result.value) {
//            $(element).find('i').hide();
//            $(element).addClass('spinner spinner-left spinner-sm').attr('disabled', true);

//            $.ajax({
//                url: '/Vendor/CoachBooking/Status/' + record,
//                type: 'POST',
//                data: JSON.stringify({ status: $(element).text().trim() }),
//                success: function (response) {
//                    if (response.success) {
//                        toastr.success(response.message);
//                        table1.row($(element).closest('tr')).remove().draw();
//                        addRow(response.data);
//                    } else {
//                        toastr.error(response.message);
//                        $(element).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
//                        $(element).find('i').show();
//                    }
//                }
//            });
//        } else {
//            //swal("Cancelled", "Your imaginary file is safe :)", "error");
//        }
//    });
//}
function ChangeBranch(f) {

    var dest = $(f).parents(":eq(2)");
    var branchId = $(dest).find("#RestaurantBranch").val();
    var status = $(dest).find("#Status").val();
    var daterangePicker = $(dest).find("#DateRangePicker").val();



    $.ajax({
        url: '/Supplier/SupplierOrder/SupplierList',
        type: 'POST',
        data: {
            branchId: branchId,
            status: status,
            orderRequiredDate: daterangePicker,
        },
        success: function (data) {
            "use strict";

            if (data != null) {

                $("#Orders").html(data);
                KTDatatablesBasicScrollable.init();
                //$('#from').val(fromDate);
                //$('#to').val(toDate);

                //var td = data.includes("</td>");
                //if (td) {
                //    $('#btnSubmit').show();
                //}
            }
            //else {
            //    $('#btnSubmit').hide();
            //}
        }
    });

};

function ChangeStatus(e) {
    $('#submit-btn').addClass('spinner spinner-sm spinner-left').attr('disabled', true);
    $.ajax({
        url: '/Supplier/SupplierOrder/ChangeStatus',
        type: 'Post',
        data: {
            OrderId: $("#Id").val(),
            Status: $("#txtstatus").val()
        },
        success: function (response) {
            if (response.success == true) {

                table2.row($(element).closest('tr')).remove().draw();
                addRow(response.data);
                toastr.success(response.message);
                $('#myModal').modal('hide');
                $('select[name=Status]').find('option').each(function (k, v) {
                    $(v).prop('disabled', false);
                });

            } else {
                $('#submit-btn').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);
            }
        }
    });
    return false;
};

function callback(dialog, elem, isedit, response) {

    if (response.success) {
        toastr.success(response.message);

        if (isedit) {

            table2.row($(elem).closest('tr')).remove().draw();
        }
        if (response.data.Status == "Pending") {
            addRow(response.data);
        }
        else if (response.data.Status == "Processing") {
            addRow(response.data);
        }
        else if (response.data.Status == "Confirmed") {
            addRow(response.data);
        }
        else {
            table2.row($(elem).closest('tr')).remove();
        }
        jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);
        jQuery('#myModal').modal('hide');
    }
    else {
        jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);

        toastr.error(response.message);
    }
}

function addRow(row) {
    console.log("Row", row)

    table1.row.add([
        row.AppointmentDate,
        row.AppointmentTime,
        row.CustomerName,
        row.CustomerEmail,
        row.id,
    ]).draw(true);
}