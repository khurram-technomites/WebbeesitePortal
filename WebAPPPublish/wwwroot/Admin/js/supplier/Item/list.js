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
            },
            columnDefs: [{
                targets: -1,
                title: 'Actions',
                orderable: false,
                width: "230px",
                className: "dt-center",

                render: function (data, type, full, meta) {
                    data = data.split(',');
                    var isActive = data[0].toUpperCase();
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

                    var actions = '';
                    actions += '<a type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" href="/Supplier/Item/Edit/' + data[1] + '">' +
                        '<i class="fa fa-pen"></i>' +
                        '</a> ' +
                        '<a type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" href="/Supplier/Item/Details/' + data[1] + '" title="View">' +
                        '<i class="fa fa-folder-open"></i>' +
                        '</a> ' +
                        '<a type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="Delete(this,' + data[1] + ')"><i class="fa fa-trash"></i></a>';


                    return actions;
                },
            },
            {
                targets: 1,
                width: '320px',
                render: function (data, type, full, meta) {
                    if (!data) {
                        return '<span>-</span>';
                    }
                    var Item = data.split(',');
                    return '<div class="d-flex align-items-center">' +
                        '<div class="symbol symbol-50 flex-shrink-0 mr-4">' +
                        '<div class="symbol-label" style="background-image: url(\'' + Item[1] + '\')"></div>' +
                        '</div>' +
                        '<div>' +
                        '<a href="#" class="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg">' + Item[0] + '</a>' +
                        //'<span class="text-muted font-weight-bold d-block">' + Item[1] + '</span>' +
                        '</div>' +
                        '</div>';
                },
            },
            {
                targets: 2,
                width: '20px',
                render: function (data, type, full, meta) {
                    var split = data.split(',');
                    var html = '';
                    var Stockstatus = split[0].replace(" ", "");
                    if (split[0].replace(" ", "") == 'InStock') {
                        html += `<span class="d-flex flex-center  switch switch-danger">
					<label>
						<input  type="checkbox" id="IsDefault" name="IsDefault" value="${split[0]}"  onchange="GoLive('${Stockstatus}', ${split[1].replace(" ", "")})" checked="checked"/>
						<span></span>
					</label>
				</span>`;
                    } else {
                        html += `<span class="d-flex flex-center  switch switch-danger">
					<label>
						<input  type="checkbox" id="IsDefault" name="IsDefault" value="${split[0]}"  onchange="GoLive('${Stockstatus}', ${split[1].replace(" ", "")})" />
						<span></span>
					</label>
				</span>`;
                    }

                    return html;
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
                url: '/Admin/Item/Delete/' + record,
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
                            toastr.success('Item deleted successfully ...');

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
                url: '/Admin/CarMake/ToggleActiveStatus/' + record,
                type: 'Get',
                success: function (response) {
                    if (response.success) {
                        toastr.success(response.message);
                        table1.row($(element).closest('tr')).remove().draw();
                        addRow(response.data);
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
        row.date,
        row.name,
        row.isActive,
        row.isActive + ',' + row.id,
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
            /*$(element).find('i').hide();
            $(element).addClass('spinner spinner-left spinner-sm').attr('disabled', true);*/
            $.ajax({
                url: '/Supplier/Item/Delete?Id=' + record,
                success: function (result) {
                    if (result.success != undefined) {
                        if (result.success) {
                            toastr.options = {
                                "positionClass": "toast-bottom-right",
                            };
                            toastr.success('Item deleted successfully ...');
                            $(element).closest("tr").remove();
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

function GoLive(element, record) {
            $.ajax({
                url: '/Supplier/Item/ToggleStatus?Id=' + record + '&StockStatus=' + element ,
                type: 'Post',
                success: function (response) {
                    if (response.success) {
                        toastr.success(response.message);
                        table1.row($(element).closest('tr')).remove().draw();
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
    //    } else {
    //        //swal("Cancelled", "Your imaginary file is safe :)", "error");
    //    }
    //});
}
//function GoLive(f) {
//    $.ajax({
//        url: '/Supplier/Item/Edit' + f,
//        type: 'Post',
//        success: function (data) {
//            "use strict";
//            if (data.success) {
//                toastr.success(data.message)
//            }

//        }
//    });

//};
