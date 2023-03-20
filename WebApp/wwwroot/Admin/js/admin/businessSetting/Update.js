
$(document).ready(function () {
	initIntlInputs();
	$('#Userform input').prop('disabled', true);
	$('#Userform textarea').prop('disabled', true);
	$('#edit-cancel').hide();
	$('#save-changes').hide();
	$('#edit-profile').fadeIn();

	$('#edit-profile').click(function () {

		$('#Userform input').prop('disabled', false);
		$('#Userform textarea').prop('disabled', false);

		$('#edit-profile').hide();
		$('#edit-cancel').fadeIn();
		$('#save-changes').fadeIn();
	});

	$('#edit-cancel').click(function () {
		$('#Userform input').prop('disabled', true);
		$('#Userform textarea').prop('disabled', true);
		$('#edit-cancel').hide();
		$('#save-changes').hide();
		$('#edit-profile').fadeIn();
	});

});

function TaxInclusive() {
	if ($('input[name="IsTaxInclusiveCheck"]').is(':checked')) {
		$('#IsTaxInclusive').val("true");
	} else {
		$('#IsTaxInclusive').val("false");
	}
}
function MaruCompare() {
	if ($('input[name="IsMaruCompareCheck"]').is(':checked')) {
		$('#IsMaruCompare').val("true");
	} else {
		$('#IsMaruCompare').val("false");
	}
}

