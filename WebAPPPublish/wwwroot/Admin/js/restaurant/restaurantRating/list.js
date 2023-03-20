"use strict";
var table1;
var KTDatatablesBasicScrollable = function () {

    var initTable1 = function () {
        var table = $('#kt_datatable1');

        // begin first table
        table1 = table.DataTable({
            /*scrollY: '50vh',*/
            /*scrollX: true,*/
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
                    className: "dt-center",
                    width: '130px',
                },
                {
                    targets: 2,
                    //width: '75px',
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
									<a href="javascript:;" class="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg">${title[1]}</a>
                                    ${title[2] != '' ? `<span class="text-muted font-weight-bold d-block" style="color:#008bb9 !important">${title[2]}</span>` : ``}
								</div>
							</div>`;
                    },
                },
                {
                    targets: 4,
                    width: '20px',
                    render: function (data, type, full, meta) {
                        var split = data.split(',');
                        var html = '';
                        var Stockstatus = split[0].replace(" ", "");
                        if (split[0].replace(" ", "") == 'True') {
                            html += `<span class="d-flex flex-center  switch switch-danger">
					<label>
						<input  type="checkbox" id="IsDefault" name="IsDefault" value="${split[0]}"  onchange="GoLive(${split[1].replace(" ", "")})" checked="checked"/>
						<span></span>
					</label>
				</span>`;
                        } else {
                            html += `<span class="d-flex flex-center  switch switch-danger">
					<label>
						<input  type="checkbox" id="IsDefault" name="IsDefault" value="${split[0]}"  onchange="GoLive(${split[1].replace(" ", "")})" />
						<span></span>
					</label>
				</span>`;
                        }

                        return html;
                    }
                },
				{
                    targets: -1,
                    width: '200px',
                    title: 'Actions',
                    className: "dt-center",
                    orderable: false,
                    render: function (data, type, full, meta) {

                        data = data.split(',');
                        var ApprovalStatus = data[0];
                        var actions = '';
                        actions += '<a type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Restaurant/RestaurantRating/Details?RatingId=' + data[2] + '\')" title="View">' +
                            '<i class="fa fa-folder-open"></i>' +
                            '</a> ';


                        //actions += '<button type="button" class="btn btn-outline-success btn-sm mr-1" onclick="Approval(this,' + data[2] + ', ' + true + ',true)">' +
                        //	'<i class="fa fa-check-circle"></i> Approve' +
                        //	'</button> ' +
                        //	'<button type="button" class="btn btn-outline-danger btn-sm mr-1" onclick="Approval(this,' + data[2] + ', ' + false + ',true)">' +
                        //	'<i class="fa fa-times-circle"></i> Reject' +
                        //	'</button> ';


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
            /*scrollY: '50vh',*/
            /*scrollX: true,*/
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
                    className: "dt-center",
                    width: '130px',
                },
                {
                    targets: 2,
                    //width: '75px',
                    render: function (data, type, full, meta) {
                        if (!data) {
                            return '<span>-</span>';
                        }
                        var title = data.split('|');
                        full = full[4].split(',');
                        return `<div class="d-flex align-items-center">
								<div class="symbol symbol-50 flex-shrink-0 mr-4">
									<div class="symbol-label" style="background-image: url(${title[0]})"></div>
								</div>
								<div>
									<a onclick="OpenModelPopup(this,\'/Restaurant/RestaurantRating/Details?RatingId=${full[2]} \')" class="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg">${title[1]}</a>
                                    ${title[2] != '' ? `<span class="text-muted font-weight-bold d-block" style="color:#008bb9 !important">${title[2]}</span>` : ``}
								</div>
							</div>`;
                    },
                },
                {
                    targets: -1,
                    width: '200px',
                    title: 'Actions',
                    className: "dt-center",
                    orderable: false,
                    render: function (data, type, full, meta) {

                        data = data.split(',');
                        var actions = '';

                        actions += `<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Restaurant/RestaurantRating/Details?Id=${data[2]}')" title="View">' +
							'<i class="fa fa-folder-open"></i>' +
							'</button>`;


                        return actions;
                    },
                },
                //{
                //	targets: 4,
                //	width: '205px',

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
function Approval(element, data, status) {
    swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, do it!'
    }).then(function (result) {
        if (result.isConfirmed) {
            $(element).find('i').hide();
            $(element).addClass('spinner spinner-left spinner-sm').attr('disabled', true);
            $.ajax({
                url: '/Restaurant/RestaurantRating/ToggleActiveStatus?RatingId=' + data,
                type: 'POST',
                data: { 'id': data, 'status': status },
                success: function (response) {
                    if (response.success) {
                        toastr.options = {
                            "positionClass": "toast-bottom-right",
                        };
                        toastr.success('Rating approved successfully ...');
                        $(element).addClass('spinner spinner-left spinner-sm').attr('disabled', true);
                        table1.row($(element).closest('tr')).remove().draw();

                    } else {
                        toastr.error('Error! Ops Something wen wrong!');
                        $(element).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
                        $(element).find('i').show();
                    }
                }
            });
        }
    });
}

jQuery(document).ready(function () {
    KTDatatablesBasicScrollable.init();
    $('.btn-rating').each(function (k, v) {
        var rating = parseFloat($(v).attr('data'));
        $(this).find('i:lt(' + (rating) + ')').addClass("la-star").removeClass("la-star-o");
    });

    $(".seemore").click(function () {
        Swal.fire($(this).text());
    });
});

function callback(dialog, elem, isedit, response) {


    if (response.success) {
        toastr.success(response.message);

        if (isedit) {
            table1.row($(elem).closest('tr')).remove().draw();
        }
        if (response.data.IsApproved == false) {
            addRow(response.data);
        }
        else {
            table1.row($(elem).closest('tr')).remove();
        }

        //addRow(response.data);
        //jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);
        //jQuery('#myModal').modal('hide');
    }
    else {
        jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);

        toastr.error(response.message);
    }
}
function ChangeStatus(f) {
    var dest = $(f).parents(":eq(2)");
    var status = $(dest).find("#Status").val();
    $.ajax({
        url: '/Restaurant/RestaurantRating/ChangeStatus',
        type: 'POST',
        data: {
            status: status,
        },
        success: function (data) {
            "use strict";
            if (data != null) {

                if (status == "Processing") {

                    $("#Ratings").html(data);
                    KTDatatablesBasicScrollable.init();
                }
                if (status == "Approved") {
                    $("#Ratings").html(data);
                    KTDatatablesBasicScrollable1.init();
                }

            }

        }
    });

};
function GoLive(f) {
    $.ajax({
        url: '/Restaurant/RestaurantRating/GoLive?RatingId=' + f,
        type: 'POST',
        success: function (data) {
            "use strict";
            if (data != null) {

                if (response.success) {
                    toastr.success(response.message);
                } else {
                    toastr.error(response.message);
                    $(element).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
                    $(element).find('i').show();
                }

            }

        }
    });

};
function addRow(row) {
    table1.row.add([
        row.date,
        row.restaurant,
        row.user.logo + '|' + row.user.firstName + '|' + row.user.phoneNumber,
        row.rating,
        row.showonweb + ',' + isActive + ',' + row.iD,
    ]).draw(true);

}