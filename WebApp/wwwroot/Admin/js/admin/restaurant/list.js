"use strict";
var table1;
var KTDatatablesBasicScrollable = function () {

    var initTable1 = function () {
        var table = $('#kt_datatable1');

        // begin first table
        table1 = table.DataTable({
            orderable: true,
            "language": {
                processing: '<i class="spinner spinner-left spinner-dark spinner-sm"></i>'
            },
            "initComplete": function (settings, json) {
                $('#kt_datatable1 tbody').fadeIn();
            },
            columnDefs: [
                {
                    targets: 0,
                    width: '130px',
                },
                {
                    targets: -1,
                    title: 'Actions',
                    orderable: false,
                    width: "230px",
                    className: "dt-center",

                    render: function (data, type, full, meta) {
                        
                        let str1 = new String('"');
                        let str2 = new String('"');
                        var UserId = str1 + data + str2;
                        var status = full[2].toString().toUpperCase();
                        var actions = '';


                        actions += '<a href="/Admin/Restaurant/Edit/' + data + '" class="btn btn-bg-secondary btn-icon btn-sm mr-1">' +
                            '<i class="fa fa-pen"></i>' +
                            '</a> ';
                        actions += `<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick='Delete(this,${UserId})'><i class="fa fa-trash"></i></button>`;

                        if (status === 'PROCESSING') {
                            actions += `<button type="button" class="btn btn-sm mr-1 btn-outline-success" onclick='Activate(this,${UserId})'>`;
                            actions += '<i class="fa fa-check-circle" aria-hidden="true"></i> Activate</button>';
                            actions += `<button type="button" class="btn btn-sm mr-1 btn-outline-danger" onclick='Activate(this,${UserId})'>`;
                            actions += '<i class="fa fa-times-circle" aria-hidden="true"></i> Deactivate</button>';
                        }
                        else if (status === 'INACTIVE') {
                            actions += `<button type="button" class="btn btn-sm mr-1 btn-outline-success" onclick='Activate(this,${UserId})'>`;
                            actions += '<i class="fa fa-check-circle" aria-hidden="true"></i> Activate</button>';
                        }
                        else if (status === 'ACTIVE') {
                            actions += `<button type="button" class="btn btn-sm mr-1 btn-outline-danger" onclick='Activate(this,${UserId})'>`;
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
								<div class="symbol symbol-50 flex-shrink-0 mr-4">
									<div class="symbol-label" style="background-image: url(${title[0]})"></div>
								</div>
								<div>
									<a href="/Admin/Restaurant/Edit/${full[3]}" class="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg">${title[1]}</a>
                                    ${title[2] != '' ? `<span class="text-muted font-weight-bold d-block" style="color:#008bb9 !important;white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">${title[2]}</span>` : ``}
								</div>
							</div>`;
                    },
                },

                {
                    targets: 1,
                    width: '240px',
                    render: function (data, type, full, meta) {
                        
                        console.log(data);
                        if (!data) {
                            return '<span>-</span>';
                        }
                        var restaurant = data.split('|');
                        return '<div class="d-flex align-items-center">' +
                            '<div class="symbol symbol-50 flex-shrink-0 mr-4">' +
                            '<div class="symbol-label" style="background-image: url(\'' + restaurant[0] + '\')"></div>' +
                            '</div>' +
                            '<div>' +
                            '<a href="#" class="text-dark-75 text-hover-primary mb-1 ">' + restaurant[1] + '</a><br>' +
                            // '<a href="#" class="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg">' + vendor[3] + '</a>' +
                            '<a href="#" class="label label-inline font-weight-bolder mb-2 text-left opacity-70" >Team : ' + restaurant[2] + '</a>' +
                            '</div>' +
                            '</div>';

                    },
                },
                {
                    targets: 2,
                    title: 'Status',
                    width: "75px",
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
                            }
                        };
                        if (typeof status[data] === 'undefined') {

                            return '<span class="label label-lg font-weight-bold label-light-danger label-inline">Inactive</span>';
                        }
                        return '<span class="label label-lg font-weight-bold' + status[data].class + ' label-inline">' + status[data].title + '</span>';
                        //return `<label class="label label-inline label-primary">${data}</label>`;
                    }
                }
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
            /*$(element).find('i').hide();
            $(element).addClass('spinner spinner-left spinner-sm').attr('disabled', true);*/
            $.ajax({
                url: '/Admin/Restaurant/Delete/' + record,
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
                            toastr.success('Row deleted successfully ...');
                            $(element).closest("tr").remove();
                            table1.row($(element).closest('tr')).remove().draw();
                            
                            $("#restaurant_Id").append("<option value='" + $('.restaurantlName').val() + "'>" + $('.restaurantlName').text() + "</option>");
                            //$("#restaurant_Id").append("option[value = '" + $('#restaurant_Id').val() + "']");
                            //deleterow();
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
function deleterow() {
    $(".RestaurantSupplerlist").on("click", ".deletebutton", function () {
        $(this).closest("tr").remove();
        $("#restaurant_Id option[value='" + $('#restaurant_Id').val() + "']").append();
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
                url: '/Admin/Restaurant/ToggleActiveStatus?Id=' + record,
                type: 'Get',
                success: function (response) {
                    if (response.success) {
                        toastr.success(response.message);
                        table1.row($(element).closest('tr')).remove().draw();
                        addRow(response.data);
                        //location.reload();
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
    //
    if (response.success) {
        toastr.success(response.message);
        location.replace("/Admin/Restaurant/Edit/" + response.data.id);

        if (isedit) {
            table1.row($(elem).closest('tr')).remove().draw();
        }

        //addRow(response.data);
        jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);
    }
    else {
        jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);

        toastr.error(response.message);
    }
}
function addRow(row) {
    console.log(row)
    table1.row.add(
        $('<tr>' +
            '<td data-order=' + row.id + '>' + row.creationDate + '</td >' +
            '<td>' + row.name + '</td>' +
            '<td>' + row.status + '</td>' +
            '<td nowrap="nowrap">' + row.id + '</td>' +
            '</tr>'
        )).draw(true);
}