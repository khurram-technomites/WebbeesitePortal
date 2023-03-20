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
			"order": [[0, "desc"]],
			"language": {
				processing: '<i class="spinner spinner-left spinner-dark spinner-sm"></i>'
			},
			"initComplete": function (settings, json) {
				$('#kt_datatable1 tbody').fadeIn();
			},
			columnDefs: [
				{
					targets: 0,
					/*className: "dt-center",*/
					width: '130px',
				},
				{
					targets: 1,
					//width: '75px',
					render: function (data, type, full, meta) {

						if (!data) {
							return '<span>-</span>';
						}
						
						var coupon = data.split('|');
						console.log("Coupon", data);
						if (coupon.length > 2) {
							return '<div class="d-flex align-items-center">' +
								'<a href="javascript:void(0);" class="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg">' + coupon[0] + '</a>' +
								'</div>' + 
							'<div class="d-flex align-items-center">' +
								'<span class="text-muted font-weight-bold" style="display: flex;align-items: center;justify-content: center;"><i class="fa fa-hashtag fa-sm px-1"></i>  ' + coupon[1]  + ' <i class="fa fa-circle px-2" style="font-size: 4px;"></i> ' + coupon[3] + (coupon[2] == "FixedAmount" ? ' <i class="fa fa-credit-card fa-sm px-2"></i>' : ' <i class="fa fa-credit-card fa-sm px-2"></i>') + '</span>' +
								'</div>' +
								'</div>';
						} else {
							return '<span>-</span>';
						}
					},
				},
				{
					targets: 2,
					className: "dt-center",
					orderable: false,
					width: '130px',
					render: function (data, type, full, meta) {

						return '<button type="button" class="btn btn-bg-secondary btn-sm mr-1" onclick="OpenModelPopup(this,\'/Restaurant/CouponCategory/Index/' + data + '\',true)">' +
							'<i class="fa fa-stream"></i> Categories' +
							'</button> ';
					},
				},
				{
					targets: 3,
					className: "dt-center",
					orderable: false,
					width: '130px',
					render: function (data, type, full, meta) {

						if (data) {
							var actions = '<button type="button" class="btn btn-outline-success btn-sm mr-1" onclick="OpenModelPopup(this,\'/Restaurant/Coupon/GetCustomers/' + data + '\',true)">' +
								'<i class="fa fa-user"></i> Customers' +
								'</button> ';
							return actions;
						}
						return '';
					},
				},
				{
					targets: 4,
					width: '75px',
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
					targets: -1,
					title: 'Actions',
					className: "dt-center",
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

						actions += '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Restaurant/Coupon/Edit/' + data[1] + '\',true)">' +
							'<i class="fa fa-pen"></i>' +
							'</button> ' +
							'<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Restaurant/Coupon/Details/' + data[1] + '\')" title="View">' +
							'<i class="fa fa-folder-open"></i>' +
							'</button> ' +
							'<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="Delete(this,' + data[1] + ')"><i class="fa fa-trash"></i></button>';

						if (typeof status[isActive] === 'undefined') {
							actions += '<button type="button" class="btn btn-outline-success btn-sm mr-1" onclick="Activate(this, ' + data[1] + ')">' +
								'<i class="fa fa-check-circle" aria-hidden="true"></i> Activate' +
								'</button>';
						} else {
							actions += '<button type="button" class="btn btn-sm mr-1' + status[isActive].class + '" onclick="Activate(this, ' + data[1] + ')">' +
								'<i class="fa ' + status[isActive].icon + '" aria-hidden="true"></i> ' + status[isActive].title +
								'</button>';
						}

						return actions;
					},
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

			$.ajax({
				url: '/Restaurant/Coupon/Delete/' + record,
				type: 'POST',
				data: {
					"__RequestVerificationToken":
						$("input[name=__RequestVerificationToken]").val()
				},
				success: function (result) {
					if (result.success != undefined) {
						if (result.success) {

							toastr.success('Coupon deleted successfully ...');

							table1.row($(element).closest('tr')).remove().draw();
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
							swal.fire("Access Denied", "Your are not authorize to perform this action, For further details please contact Restaurantistrator !", "warning").then(function () {
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
				url: '/Restaurant/Coupon/ToggleActiveStatus/' + record,
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
		row.name + '|' + row.couponCode + '|' + row.type + '|' + row.value,
		row.id,
		row.id,
		row.isActive,
		row.isActive + ',' + row.id,
	]).draw(true);

}
