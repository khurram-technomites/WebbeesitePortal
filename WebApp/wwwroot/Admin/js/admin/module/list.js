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
            columnDefs: [
                {

                    targets: -1,
                    title: 'actions',
                    orderable: false,
                    classname: "dt-center",
                    width: '90px',
                    className: "dt-center",
                    render: function (data, type, full, meta) {
                     
                        var actions = '';
                        var issytem = full[1].split('|')

                        actions += '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Admin/Module/Edit?id=' + data + '\',true)">' +
                            '<i class="fa fa-pen"></i>' +
                            '</button> ' +
                            '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Admin/Module/Details?id=' + data + '\')" title="view">' +
                            '<i class="fa fa-folder-open"></i>' +
                            '</button> ' 
                        if (issytem[1] == "False") {
                            actions += '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="Delete(this,' + data + ')"><i class="fa fa-trash"></i></button>';
                            }
                            

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
                    targets: 1,
                    width: "130px",
                    title: 'Name',
                    className: "dt-center",
                    render: function (data, type, full, meta) {
                        var data2 = data.split('|')
                        var action = '';
                        action += '<div class="car-name" title="' + data2[0] + '">' 
                        if (data2[1] == "True" || data2[1] == "true") {
                            action +=   '<span  class="label label-lg font-weight-bold label-warning label-inline">Sytem</span></br>' 
                        }
                        if (data2[2] == "True" || data2[2] == "true") {
                            action += '<span  class="label label-lg font-weight-bold label-light-dark label-inline">Default</span></br>'
                        }
                        action += '<a href="javascript:;" class="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg">' + data2[0] + '</a>' +
                            '</div>'
                        return action; 
                    }
                },
                {
                    targets: 0,
                    width: "130px"
                },
                {
                    targets: 2,
                    width: "130px"
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
                            }
                        };
                        //if (typeof status[data] === 'undefined') {

                        //    return '<span class="label label-lg font-weight-bold label-light-danger label-inline">Inactive</span>';
                        //}
                        var actions = ""
                        if (typeof status[data] === 'undefined') {
                            actions += '<button type="button" class="btn label label-lg font-weight-bold label-light-danger label-inline mr-1" onclick="Activate(this, ' + full[3] + ')">' +
                        		'<i class="fa fa-check-circle" aria-hidden="true"></i> activate' +
                        		'</button>';
                        } else {
                            actions += '<button type="button" class="btn label label-lg font-weight-bold label-light-success label-inline mr-1' + status[data].class + '" onclick="Activate(this, ' + full[3] + ')">' +
                                '<i class="fa ' + status[data].icon + '" aria-hidden="true"></i> ' + status[data].title +
                        		'</button>';
                        }
                        return actions;
/*                        return '<span class="label label-lg font-weight-bold' + status[data].class + ' label-inline">' + status[data].title + '</span>';*/
                        //return `<label class="label label-inline label-primary">${data}</label>`;
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
                url: '/Admin/Module/Delete/' + record,
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
                            toastr.success('Record deleted successfully ...');

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
                url: '/Admin/Module/ToggleActiveStatus/' + record,
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
        jQuery('#myModal').modal('hide');
    }
    else {
        jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);

        toastr.error(response.message);
    }
}

function addRow(row) {
    table1.row.add([
        row.createdOn,
        row.name + '|' + row.isSystem + '|' + row.isDefault,
        row.price,
        row.status,
        row.id

    ]).draw(true);

}
