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
            order: [[0, 'desc']],

            "language": {
                processing: '<i class="spinner spinner-left spinner-dark spinner-sm"></i>'
            },
            "initComplete": function (settings, json) {
                $('#kt_datatable1 tbody').fadeIn();
            },
            columnDefs: [
                {
                    targets: -1,
                    width: '130px',
                    orderable: false,

                    render: function (data, type, full, meta) {


                        var actions = '';
                        
                        actions +=
                            '<a class="btn btn-bg-secondary btn-sm mr-1" download target="_blank" href="' + data + '">' +
                                '<i class="fa fa-download" ></i> Download' +
                            '</a> '

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
var KTDatatablesBasicScrollable1 = function () {

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
            },
            dom: 'Bfrtip',
            buttons: [
                {
                    extend: 'excel',
                    messageTop: function () {
                        return '';
                    },
                    title: 'Careers',
                    exportOptions: {
                        columns: [0, 1, 2, 3, 4, 5]
                    }
                }
            ]
            ,
            columnDefs: [
                {
                    targets: -1,

                    orderable: false,

                    render: function (data, type, full, meta) {


                        var actions = '';

                        actions += '<a class="btn btn-bg-secondary btn-sm mr-1" download target="_blank" href="' + data + '">' +
                            '<i class="fa fa-download"></i> Download' +
                            '</a> '

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

    //$('.kt_datepicker_range').datepicker({
    //    todayHighlight: true,
    //});

    $("#btnSearch").on("click", function () {

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
            url: '/Admin/Candidates/List',
            type: 'POST',
            data: {
                fromDate: $('#fromDate').val(),
                toDate: $('#toDate').val(),
            },
            success: function (data) {
                "use strict";

                if (data != null) {

                    $("#Candidates").html(data);
                    KTDatatablesBasicScrollable1.init();
                }
            }
        });

    })//btnSearch;

});

//function DownloadFile(e) {
    
//    debugger;
//    $.ajax({
//        url: '/Client/Career/DownloadContent',
//        type: 'POST',
//        data: { 'Path': e.href },
//        success: (response) => {
//            if (response.success) {
//                toastr.success("File is Download");
//            }
//        },
//        error: function (error) {
//            toastr.error("Some thing went wrong !");
//        }
//    });
//}


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
    table1.row.add([
        row.Date,
        row.Name,
        row.Gender,
        row.Experience,
        row.Position,
        row.CV,
        row.CV,
    ]).draw(true);

}