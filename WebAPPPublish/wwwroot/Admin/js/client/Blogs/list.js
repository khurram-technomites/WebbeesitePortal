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
                    targets: 0,
                    className: "dt-center",
                    width: '150px',
                },
                {
                    targets: -1,
                    title: 'Actions',
                    orderable: false,
                    width: "230px",
                    className: "dt-center",

                    render: function (data, type, full, meta) {
                        var isActive = full[2].toString();
                        isActive = isActive.toUpperCase();
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
                        actions += '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Client/Blog/Edit?Id=' + data + '\')" title="View">' +
                            '<i class="fa fa-pen"></i>' +
                            '</button> ' +
                            '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Client/Blog/Details/' + data + '\')" title="View">' +
                            '<i class="fa fa-folder-open"></i>' +
                            '</button> ' +
                            '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="Delete(this,' + data + ')"><i class="fa fa-trash"></i></button>';
                        if (typeof status[isActive] === 'undefined') {
                            actions += '<button type="button" class="btn btn-outline-success btn-sm mr-1" onclick="Activate(this, ' + data + ')">' +
                                '<i class="fa fa-check-circle" aria-hidden="true"></i> Activate' +
                                '</button>';
                        } else {
                            actions += '<button type="button" class="btn btn-sm mr-1' + status[isActive].class + '" onclick="Activate(this, ' + data + ')">' +
                                '<i class="fa ' + status[isActive].icon + '" aria-hidden="true"></i> ' + status[isActive].title +
                                '</button>';
                        }
                        return actions;
                    },
                },
                {
                    targets: 1,
                    //width: '75px',
                    render: function (data, type, full, meta) {
                        if (!data) {
                            return '<span>-</span>';
                        }
                        var title = data.split('|');

                        return `<div class="d-flex align-items-center">
								<div class="symbol symbol-50 flex-shrink-0 mr-4">
									<div class="symbol-label" style="background-image: url(${title[0].replaceAll(" ", "%20")})"></div>
								</div>
								<div>
									<a href="javascript:;" class="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg">${title[1]}</a>
								</div>
							</div>`;
                    },
                },
                {
                    targets: 2,
                    width: '60px',
                    className: "dt-center",
                    render: function (data, type, full, meta) {

                        data = data.toUpperCase().toString();
                        debugger;
                        var status = {
                            "TRUE": {
                                'title': 'Active',
                                'class': ' label-light-success'
                            },
                            "FALSE": {
                                'title': 'Inactive',
                                'class': ' label-light-danger'
                            },
                        };
                        if (typeof status[data] === 'undefined') {

                            return '<span class="label label-lg font-weight-bold label-light-danger label-inline">Inactive</span>';
                        }
                        return '<span class="label label-lg font-weight-bold' + status[data].class + ' label-inline">' + status[data].title + '</span>';
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
                url: '/Client/Blog/Delete/' + record,
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
                            toastr.success('Blog deleted successfully ...');

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
        //text: "You won't be able to revert this!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, do it!'
    }).then(function (result) {
        if (result.value) {
            $(element).find('i').hide();
            $(element).addClass('spinner spinner-left spinner-sm').attr('disabled', true);

            $.ajax({
                url: '/Client/Blog/ToggleActiveStatus/' + record,
                type: 'Get',
                success: function (response) {
                    if (response.success) {

                        toastr.success(response.message);
                        table1.row($(element).closest('tr')).remove().draw();
                        addRow(response.data);

                    }

                    else {
                        toastr.error(response.message);
                        $(element).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
                        $(element).find('i').show();
                    }
                    /* location.reload();*/
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
    debugger;
    table1.row.add($('<tr>' +
        '<td data-order=' + row.id + '>' + row.date + '</td >' +
        '<td>' + row.image + "|" + row.title + '</td>' +
        '<td>' + row.status + '</td>' +
        '<td nowrap="nowrap">' + row.id + '</td>' +
        '</tr>'
    )).draw();

    //table1.row.add([
    //    row.id,
    //    row.creationDate,
    //    row.title + "|" + row.image,
    //    row.status,
    //]).draw(true);
}