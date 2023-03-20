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
                        var data2 = data.split('|');
                        var UserId = str1 + data2[0] + str2;
                        var actions = '';
                        
                        if (data2[1] == "Processing") {
                            actions += '<labeL class="btn label label-lg font-weight-bold label-light-primary label-inline mr-1">Processing</label>'
                        }
                        else if (data2[1] != "Draft") {
                            actions += '<a class="btn btn-bg-secondary btn-icon btn-sm mr-1" href="/Vendor/Client/ClientEdit/' + data2[0] + '" title="Edit">' +
                                '<i class="fa fa-pen"></i>' +
                                '</a> ';
                            actions += `<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick='Delete(this,${UserId})'><i class="fa fa-trash"></i></button>`;
                        }
                        else {
                            actions += '<a class="btn btn-bg-secondary btn-icon btn-sm mr-1" href="/Vendor/Client/Client/' + data2[0] + '" title="Edit">' +
                                '<i class="fa fa-pen"></i>' +
                                '</a> ';
                            actions += `<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick='Delete(this,${UserId})'><i class="fa fa-trash"></i></button>`;
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
									<div class="symbol-label" style="background-image: url(${title[0].replace(" ", "")})"></div>
								</div>
								<div>
									<a href="javascript:;" class="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg">${title[1]}</a>
                                    ${title[2] != '' ? `<span class="text-muted font-weight-bold d-block" style="color:#008bb9 !important;white-space: nowrap; overflow: hidden; text-overflow: ellipsis;"> ${title[2]}</span>` : ``}
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
                    title: 'Package',
                    /*width: '200px',*/
                    className: "dt-center",
                    render: function (data, type, full, meta) {
                        var data2 = full[5].split('|');
                        if (data2[1] == "Active") {
                            return '<a class="btn btn-bg-warning  mr-1" href="/Vendor/Client/PackageEdit/' + data + '"" title="Package">' +
                                'Package' +
                                '</a> ';
                        }
                        else {
                            return '<a class="btn btn-bg-secondary  mr-1" href="javascript:;" title="Package">' +
                                'Package' +
                                '</a> ';
                        }
                       
                        
                    }
                },
                {
                    targets: 3,
                    width: '130px',
                    render: function (data, type, full, meta) {
                       
                        return ' <button onclick="OpenModelPopup(this,\'/Vendor/Client/ClientPurchases/' + data + '\',true)" style="background-color:#00A2A4; color:white"  class="btn  ml-auto">Purchases</button>';

                    }
                },
                {
                    targets: 4,
                    width: '130px',
                    render: function (data, type, full, meta) {
                        return '<a class="btn btn-bg-secondary  btn-sm mr-1" target="_blank" href="' + data + '" title="Website">' +
                            data +
                            '</a> ';
                        
                    }
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
                url: '/Vendor/Client/Delete/' + record,
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
                            toastr.success('Garage deleted successfully ...');

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
                url: '/Vendor/Client/ToggleActiveStatus?Id=' + id + '&flag=' + flag,
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
                url: '/Admin/Client/ToggleActiveStatus/' + record,
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
       /* location.reload();*/
    }
    else {
        jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);

        toastr.error(response.message);
    }
}

function addRow(row) {
    table1.row.add([
        row.Result.Garage.Logo + '|' + row.Result.Garage.NameAsPerTradeLicense + '|' + row.Result.Garage.ContactPersonEmail,
        row.Result.Garage.CreationDate,
        row.Result.Garage.Id,
        row.Result.Garage.Slug,
        row.Result.Garage.Website,
        row.Result.Garage.Id
    ]).draw(true);
}


function Reject(element, flag, id) {
    Swal.fire({
        title: 'Are you sure?',
        type: 'warning',
        input: 'text',
        showCancelButton: true,
        confirmButtonText: 'Reject!',
        showCancelButton: true
    }).then((result) => {
        if (result.value) {
            $(element).find('i').hide();
            $(element).addClass('spinner spinner-left spinner-sm').attr('disabled', true);

            $.ajax({
                url: '/Admin/Client/ToggleActiveStatus?Id=' + id + '&flag=' + flag + '&RejectionReason=' + result.value,
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