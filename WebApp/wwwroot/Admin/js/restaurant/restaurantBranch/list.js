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
                width: '230px',
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

                    actions += '<a class="btn btn-bg-secondary btn-icon btn-sm mr-1" href="/Restaurant/RestaurantBranch/Edit?id=' + data[1] + '">' +
                        '<i class="fa fa-pen"></i>' +
                        '</a> ' +

                        '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="Delete(this,' + data[1] + ')"><i class="fa fa-trash"></i></button>' +

                        '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Restaurant/RestaurantBranch/GenerateQR/' + data[1] + '\')" title="View">' +
                        '<i class="fa fa-qrcode"></i>' +
                        '</button> ';

                    if (typeof status[isActive] === 'undefined') {
                        if (full[3] && full[3].toUpperCase() === "TRUE") {
                            actions += '<button type="button" class="btn btn-outline-success btn-sm mr-1" disabled="disabled">' +
                                '<i class="fa fa-check-circle" aria-hidden="true"></i> Activate' +
                                '</button>';
                        } else {
                            actions += '<button type="button" class="btn btn-outline-success btn-sm mr-1" onclick="Activate(this, ' + data[1] + ')">' +
                                '<i class="fa fa-check-circle" aria-hidden="true"></i> Activate' +
                                '</button>';
                        }

                    } else {
                        if (full[3] && full[3].toUpperCase() === "TRUE") {
                            actions += '<button type="button" class="btn btn-outline-success btn-sm mr-1" disabled="disabled">' +
                                '<i class="fa fa-check-circle" aria-hidden="true"></i> Activate' +
                                '</button>';
                        } else {
                            actions += '<button type="button" class="btn btn-sm mr-1' + status[isActive].class + '" onclick="Activate(this, ' + data[1] + ')">' +
                                '<i class="fa ' + status[isActive].icon + '" aria-hidden="true"></i> ' + status[isActive].title +
                                '</button>';
                        }
                    }

                    return actions;
                },
            },

            //{
            //	//targets: 1,
            //	////width: '75px',
            //	//render: function (data, type, full, meta) {
            //	//	if (!data) {
            //	//		return '<span>-</span>';
            //	//	}
            //	//	var RestaurantBranch = data.split('|');
            //	//	return '<div class="d-flex align-RestaurantBranchs-center">' +
            //	//		'<div class="symbol symbol-50 flex-shrink-0 mr-4">' +
            //	//		'<div class="symbol-label" style="background-image: url(\'' + RestaurantBranch[0] + '\')"></div>' +
            //	//		'</div>' +
            //	//		'<div>' +
            //	//		'<a href="#" class="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg">' + RestaurantBranch[1] + '</a>' +
            //	//		//'<span class="text-muted font-weight-bold d-block">' + RestaurantBranch[1] + '</span>' +
            //	//		'</div>' +
            //	//		'</div>';
            //	//},
            //},
            {
                targets: 2,
                width: '70px',
                class: 'dt-center',
                render: function (data, type, full, meta) {

                    data = data.toString();
                    data = data.toUpperCase();
                    var status = {
                        "TRUE": {
                            'title': 'Active',
                            'class': ' label-light-success'
                        },
                        "FALSE": {
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
                width: '70px',
                class: 'dt-center',
                render: function (data, type, full, meta) {
                    var split = data.split(',');
                    var html = '';
                    if (split[0].replace(" ", "") == 'False') {
                        html += `<span class="d-flex flex-center  switch switch-danger">
					<label>
						<input  type="checkbox" id="IsDefault" name="IsDefault" value="${split[0]}"  onchange="GoLive(this, ${split[1].replace(" ", "")})" checked="checked"/>
						<span></span>
					</label>
				</span>`;
                    } else {
                        html += `<span class="d-flex flex-center  switch switch-danger">
					<label>
						<input  type="checkbox" id="IsDefault" name="IsDefault" value="${split[0]}"  onchange="GoLive(this, ${split[1].replace(" ", "")})" />
						<span></span>
					</label>
				</span>`;
                    }

                    return html;
                },
            },
            {
                targets: 4,
                width: '70px',
                class: 'dt-center',
                render: function (data, type, full, meta) {
                    var split = data.split(',');
                    var html = '';
                    if (split[0].replace(" ", "") == 'True') {
                        html += `<span class="d-flex flex-center  switch switch-danger">
					<label>
						<input  type="checkbox" id="IsDefault" name="IsDefault" value="${split[0]}"  onchange="SetMain(this, ${split[1].replace(" ", "")})" checked="checked"/>
						<span></span>
					</label>
				</span>`;
                    } else {
                        html += `<span class="d-flex flex-center  switch switch-danger">
					<label>
						<input  type="checkbox" id="IsDefault" name="IsDefault" value="${split[0]}"  onchange="SetMain(this, ${split[1].replace(" ", "")})" />
						<span></span>
					</label>
				</span>`;
                    }

                    return html;
                },
            },
            {
                targets: 1,
                width: '300px',
                class: 'dt-left'
            },
            {
                targets: 0,
                width: '130px',
                class: 'dt-left'
            },
            {
                targets: 3,
                width: '230px',
                class: 'dt-center'
            },
                //{
                //	targets: 4,
                //	width: '130px',
                //	orderable: false,
                //	render: function (data, type, full, meta) {
                //		if (data) {
                //			data = data.toUpperCase();
                //			if (data === "TRUE") {
                //				return '<span class="label label-lg font-weight-bold label-light-dark label-inline">Parent Removed</span>';
                //			}
                //		}
                //		return '';
                //	},
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
                url: '/Restaurant/RestaurantBranch/Delete/' + record,
                type: 'POST',
                data: {
                    "__RequestVerificationToken":
                        $("input[name=__RequestVerificationToken]").val()
                },
                success: function (result) {
                    if (result.success != undefined) {
                        if (result.success) {
                            if (result.hadChilds == true) {
                                location.reload();
                            } else {
                                toastr.options = {
                                    "positionClass": "toast-bottom-right",
                                };
                                toastr.success('RestaurantBranch Deleted Successfully');

                                table1.row($(element).closest('tr')).remove().draw();
                            }
                        }
                        else {
                            toastr.error(result.message);
                        }
                    } else {
                        swal.fire("Your are not authorize to perform this action", "For further details please contact Restaurantistrator !", "warning").then(function () {
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
                            swal.fire("Access Denied", "Your are not authorize to perform this action, For further details please contact Administrator !", "warning").then(function () {
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
                url: '/Restaurant/RestaurantBranch/ToggleActiveStatus/' + record,
                type: 'Get',
                success: function (response) {
                    if (response.success) {
                        toastr.success(response.message);
                        location.reload();
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
                            swal.fire("Access Denied", "Your are not authorize to perform this action, For further details please contact Restaurantistrator !", "warning").then(function () {
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

function GoLive(obj, f) {
    if ($(obj).is(':checked')) {
        $.ajax({
            url: '/Restaurant/RestaurantBranch/ToggleCloseStatus?id=' + f,
            type: 'POST',
            success: function (data) {
                "use strict";
                if (data.success) {
                    toastr.success(data.message)
                }
            }
        });
    } else {
        Swal.fire({
            title: 'Are you sure?',
            type: 'warning',
            html: '<input type="time" id="swal-input1" class="swal2-input">',
            showCancelButton: true,
            confirmButtonText: 'Reject!',
            showCancelButton: true
        }).then((result) => {
            if (result.value) {
                $(element).find('i').hide();
                $(element).addClass('spinner spinner-left spinner-sm').attr('disabled', true);
                
                $.ajax({
                    url: '/Restaurant/RestaurantBranch/ToggleCloseStatus?id=' + f + '&ClosingTimeSpan=' + $("#swal-input1").val(),
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
};

function SetMain(obj, f) {
    $.ajax({
        url: '/Restaurant/RestaurantBranch/ToggleMainBranchStatus?id=' + f,
        type: 'POST',
        success: function (data) {
            "use strict";
            if (data.success) {
                location.reload();
            }
        }
    });
};
