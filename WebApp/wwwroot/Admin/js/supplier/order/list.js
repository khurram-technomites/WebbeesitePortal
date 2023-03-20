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
            "order": [[1, "desc"]],

            "language": {
                processing: '<i class="spinner spinner-left spinner-dark spinner-sm"></i>'
            },
            "initComplete": function (settings, json) {
                $('#kt_datatable1 tbody').fadeIn();
                FormatPrices();
            },
            columnDefs: [{

                targets: -1,
                title: 'Actions',
                orderable: false,


                render: function (data, type, full, meta) {
                    var actions = '';
                    actions += '<a class="btn btn-secondary btn-sm mr-1" href="/Supplier/SupplierOrder/Details?id=' + full[6] + '">' +
                        '<i class="fa fa-folder-open"></i> Details' +
                        '</a> ';
                    return actions
                },
            },
                {
                    targets: 2,
                    width: '280px',
                    class: "dt-center",
                    render: function (data, type, full, meta) {
                        if (!data) {
                            return '<span>-</span>';
                        }
                        var customer = data.split('|');
                        return '<div class="d-flex  flex-column align-items-center">' +
                            '<a href="#" class="text-dark-75 text-hover-primary mb-1 ">' + customer[0] + '</a>' +
                            '<a href="#" class="label label-inline font-weight-bolder mb-2 text-left opacity-70" >' + customer[1] + '</a>' +
                            '</div>' +
                            '</div>';

                    },
                },
                {
                    targets: 0,
                    width: '130px',
                    class: "dt-left"
                },
                {
                    targets: 1,
                    width: '75px',
                    class:"dt-left"
                },
                {
                    targets: 3,
                    width: '150px',
                    class: "dt-center"
                },
                {
                    targets: 4,
                    width: '100px',
                    class: "dt-center"
                },
                {
                    targets: 5,
                    width: '100px',
                    class: "dt-center",
                    render: function (data, type, full, meta) {
                        var status = {
                            "Pending": {
                                'title': 'Pending',
                                'class': ' label-light-dark'
                            },
                            "Confirmed": {
                                'title': 'Confirmed',
                                'class': ' label-light-success'
                            },
                            "Inprocess": {
                                'title': 'Processing',
                                'class': ' label-light-primary'
                            },
                            "Completed": {
                                'title': 'Completed',
                                'class': ' label-light-danger'
                            },

                            "Canceled": {
                                'title': 'Canceled',
                                'class': ' label-light-success'
                            },
                        };

                        if (typeof status[data] === 'undefined') {
                            return '<a  href="javascript:" class="label label-lg label-light-dark label-inline" onclick="OpenModelPopup(this,\'/Supplier/SupplierOrder/ChangeStatus/' + full[6] + '\',true)">' + data + '</a>';
                        }
                        return '<a href="javascript:" class="label label-lg ' + status[data].class + ' label-inline" onclick="OpenModelPopup(this,\'/Supplier/SupplierOrder/ChangeStatus/' + full[6] + '\',true)">' + data + ' </a>';


                    },
                },
               
            //{
            //    targets: 5,
            //    width: '100px',
            //    class: "dt-center",
            //    render: function (data, type, full, meta) {
            //        
            //        console.log("Ful", full);
            //        var status = {
            //            "Pending": {
            //                'title': 'Pending',
            //                'class': ' label-light-dark'
            //            },
            //            "Confirmed": {
            //                'title': 'Confirmed',
            //                'class': ' label-light-success'
            //            },
            //            "Inprocess": {
            //                'title': 'Processing',
            //                'class': ' label-light-primary'
            //            },
            //            "Completed": {
            //                'title': 'Completed',
            //                'class': ' label-light-danger'
            //            },

            //            "Canceled": {
            //                'title': 'Canceled',
            //                'class': ' label-light-success'
            //            },
            //        };
                    
            //        if (typeof status[data] === 'undefined') {
            //            return '<a  href="javascript:" class="label label-lg label-light-dark label-inline" onclick="OpenModelPopup(this,\'/Supplier/SupplierOrder/ChangeStatus/' + full[7] + '\',true)">' + data + '</a>';
            //        }
            //        return '<a href="javascript:" class="label label-lg ' + status[data].class + ' label-inline" onclick="OpenModelPopup(this,\'/Supplier/SupplierOrder/ChangeStatus/' + full[7] + '\',true)">' + data + ' </a>';


            //    },
            //},
                //{
                //    targets: 7,
                //    width: '75px',
                //    render: function (data, type, full, meta) {

                //        var status = {
                //            "Pending": {
                //                'title': 'Pending',
                //                'class': ' label-light-dark'
                //            },
                //            "Processing": {
                //                'title': 'Processing',
                //                'class': ' label-light-success'
                //            },
                //            "Fulfilled": {
                //                'title': 'Fulfilled',
                //                'class': ' label-light-primary'
                //            },
                //            "Not Fulfilled": {
                //                'title': 'Not Fulfilled',
                //                'class': ' label-light-danger'
                //            },

                //        };
                //        if (typeof status[data] === 'undefined') {

                //            //return '<a href="javascript:" class="label label-lg label-light-dark label-inline" onclick="OpenModelPopup(this,\'/Vendor/Order/ShipmentChange/' + full[7] + '\',true)">' + data + '</a>';
                //            return '<a hidden href="javascript:" class="label label-lg label-light-dark label-inline" >' + data + '</a>';
                //        }
                //        //return '<a href="javascript:" class="label label-lg ' + status[data].class + ' label-inline" onclick="OpenModelPopup(this,\'/Vendor/Order/ShipmentChange/' + full[7] + '\',true)">' + data + ' </a>';
                //        return '<a hidden href="javascript:" class="label label-lg ' + status[data].class + ' label-inline" >' + data + ' </a>';
                //    },
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
    var status = $(dest).find("#Status").val();
    var daterangePicker = $(dest).find("#DateRangePicker").val();
    $.ajax({
        url: '/Supplier/SupplierOrder/SupplierList',
        type: 'POST',
        data: {
            status: status,
            orderRequiredDate: daterangePicker,
        },
        success: function (data) {
            "use strict";
            console.log("Data", data);
            
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
                
                table1.row($(element).closest('tr')).remove().draw();
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

            table1.row($(elem).closest('tr')).remove().draw();
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
            table1.row($(elem).closest('tr')).remove();
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
    
    table1.row.add([
        row.date,
        row.orderNo,
        row.customerName + "|" + row.customerContact,
        row.total,
        row.orderRequiredDate,
        row.status,
        row.id,
    ]).draw(true);
}