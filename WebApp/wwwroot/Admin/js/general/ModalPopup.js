var x;
var element
jQuery(document).ready(function () {
	jQuery.ajaxSetup({ cache: false });
	jQuery(".data_modal").on("click", function (e) {
		jQuery('#myModalContent').load(this.href, function () {
			jQuery('#myModal').modal({
				keyboard: false
			}, 'show');
			bindForm(this);
		});
		return false;
	});
});

function OpenModelPopup(elem, url, isedit) {
	jQuery.ajaxSetup({ cache: false });
	jQuery.noconflict;
	jQuery('#myModal').modal({}, 'show');
	jQuery('#myModalContent').html('<div class="spinner spinner-lg spinner-center spinner-dark spinner-modal"></div>');
	jQuery('#myModalContent').load(url, function () {
		bindForm(this, elem, isedit);
	});
	element = elem;
}
function OpenModelPopup2(elem, url, isedit) {
	jQuery.ajaxSetup({ cache: false });
	jQuery.noconflict;
	jQuery('#myModal2').modal({}, 'show');
	jQuery('#myModalContent').html('<div class="spinner spinner-lg spinner-center spinner-dark spinner-modal"></div>');
	jQuery('#myModalContent').load(url, function () {
		bindForm(this, elem, isedit);
	});
	element = elem;
}
function bindForm(dialog, elem, isedit) {

	jQuery('form', dialog).submit(function () {
		var id = $(this).attr('id');
		$(this).closest('.modal').find(`button[type=submit][form=${id}]`).addClass('spinner spinner-sm spinner-left').attr('disabled', true);
	});

	jQuery('.form', dialog).submit(function () {
		jQuery.ajax({
			url: this.action,
			type: this.method,
			data: jQuery(this).serialize(),
			success: function (result) {
				if (callback) {
					callback(dialog, elem, isedit, result);
				}
				else {
					if (result.success) {
						var id = "replacetarget";
						//x = result.row;
						//t.row.add(result.row).draw(false);

						jQuery('#' + id).html('<div class="lds-ripple"><div class="lds-pos"></div><div class="lds-pos"></div></div>');
						jQuery('#myModal').modal('hide');
						if (result.index) {
							id += result.index;
						}
						jQuery('#' + id).load(result.url, function () {

							jQuery('#dataTables-example').DataTable({
								responsive: true,
								pageLength: 10,
								sPaginationType: "full_numbers",
								oLanguage: {
									oPaginate: {
										sFirst: "<<",
										sPrevious: "<",
										sNext: ">",
										sLast: ">>"
									}
								}
							});
							if (result.callback != null) {
								GetList();
							}
						});
						//jQuery('#datatable-responsive').DataTable().ajax.reload();
						//  Load data from the server and place the returned HTML into the matched element
					} else {
						jQuery('#divMessage').text(result.message);
						jQuery('#divMessage').fadeIn(result.message);
					}
				}
			}
		});
		return false;
	});
}
