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
            //dom: 'Bfrtip',

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
                width: "230px",
                className: "dt-center",
                render: function (data, type, full, meta) {

                    return '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="Delete(this,' + data + ')"><i class="fa fa-trash"></i></button>';

                },
            },


            ],

            // buttons: [
            //        {
            //            extend: 'print',
            //            messageTop: function () {
            //                //if ( printCounter === 1 ) {
            //                return html;
            //                //}
            //                //else {
            //                //    return 'You have printed this document '+printCounter+' times';
            //                //}
            //            },
            //            title: '',
            //            exportOptions: {
            //                columns: [0, 1]
            //            }
            //        },
            //{
            //    extend: 'excel',
            //    messageTop: function () {
            //        return 'Subscribers';
            //    },
            //    title: '',
            //    exportOptions: {
            //        columns: [0, 1]
            //    }
            //}
            // ],


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


//function Activate(element, record) {
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
//                url: '/Admin/Brands/Activate/' + record,
//                type: 'Get',
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

function callback(dialog, elem, isedit, response) {

    if (response.success) {
        toastr.success(response.message);

        jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);

        //if (isedit) {
        //    table1.row($(elem).closest('tr')).remove().draw();
        //}

        //addRow(response.data);
        jQuery('#myModal').modal('hide');
    }
    else {
        jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);

        toastr.error(response.message);
    }
}

function addRow(row) {
    table1.row.add([
        '<td data-order=' + row.ID + '> ' + row.Date + '</td>',
        row.Email,

        //row.IsActive + ',' + row.ID,
    ]).draw(true);

}

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
                url: '/SparePart/Subscriber/DeleteSubscriber/' + record,
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
                            toastr.success('Subscriber deleted successfully ...');

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
