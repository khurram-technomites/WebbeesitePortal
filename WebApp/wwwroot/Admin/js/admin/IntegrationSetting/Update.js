
$(document).ready(function () {

	$('#Userform input').prop('disabled', true);
	$('#Userform textarea').prop('disabled', true);
	/*$('#edit-cancel').fadeIn();*/
	$('#save-changes').hide();
	$('#edit-profile').fadeIn();
	$('#edit-email').fadeIn();
	$('#edit-sms').fadeIn();
	$('#edit-fcm').fadeIn();
	$('#edit-map').fadeIn();
	$('#edit-payment').fadeIn();
	$('#edit-profile').click(function () {

		$('#Userform input').prop('disabled', false);
		$('#Userform textarea').prop('disabled', false);

		$('#edit-profile').hide();
		$('#edit-cancel').fadeIn();
		$('#save-changes').fadeIn();
	});
	$('#edit-email').click(function () {

		$('#Emailform input').prop('disabled', false);
		$('#Emailform textarea').prop('disabled', false);

		$('#edit-email').hide();
		$('#edit-cancel').fadeIn();
		$('#email-save-changes').fadeIn();
	});
	$('#email-save-changes').click(function () {
		$('#Emailform input').prop('disabled', true);
		$('#Emailform textarea').prop('disabled', true);
		$('#edit-email').fadeIn();
		$('#edit-cancel').hide();
		$('#email-save-changes').hide();
	});
	$('#edit-sms').click(function () {

		$('#SMSform input').prop('disabled', false);
		$('#SMSform textarea').prop('disabled', false);

		$('#edit-sms').hide();
		$('#edit-sms-cancel').fadeIn();
		$('#sms-save-changes').fadeIn();
	});
	$('#sms-save-changes').click(function () {

		$('#SMSform input').prop('disabled', true);
		$('#SMSform textarea').prop('disabled', true);

		$('#edit-sms').fadeIn();
		$('#edit-sms-cancel').hide();
		$('#sms-save-changes').hide();
	});
	$('#edit-fcm').click(function () {

		$('#FCMform input').prop('disabled', false);
		$('#FCMform textarea').prop('disabled', false);

		$('#edit-fcm').hide();
		$('#edit-fcm-cancel').fadeIn();
		$('#fcm-save-changes').fadeIn();
	});
	$('#fcm-save-changes').click(function () {

		$('#FCMform input').prop('disabled', true);
		$('#FCMform textarea').prop('disabled', true);

		$('#edit-fcm').fadeIn();
		$('#edit-fcm-cancel').hide();
		$('#fcm-save-changes').hide();
	});
	$('#edit-map').click(function () {

		$('#Mapform input').prop('disabled', false);
		$('#Mapform textarea').prop('disabled', false);

		$('#edit-map').hide();
		$('#edit-map-cancel').fadeIn();
		$('#map-save-changes').fadeIn();
	});
	$('#map-save-changes').click(function () {

		$('#Mapform input').prop('disabled', true);
		$('#Mapform textarea').prop('disabled', true);

		$('#edit-map').fadeIn();
		$('#edit-map-cancel').hide();
		$('#map-save-changes').hide();
	});
	$('#edit-payment').click(function () {

		$('#Paymentform input').prop('disabled', false);
		$('#Paymentform textarea').prop('disabled', false);

		$('#edit-payment').hide();
		$('#edit-payment-cancel').fadeIn();
		$('#payment-save-changes').fadeIn();
	});
	$('#payment-save-changes').click(function () {

		$('#Paymentform input').prop('disabled', true);
		$('#Paymentform textarea').prop('disabled', true);

		$('#edit-payment').fadeIn();
		$('#edit-payment-cancel').hide();
		$('#payment-save-changes').hide();
	});
	$('#edit-cancel').click(function () {
		$('#Userform input').prop('disabled', true);
		$('#Userform textarea').prop('disabled', true);
		$('#edit-cancel').hide();
		$('#edit-email').fadeIn();
		$('#email-save-changes').hide(); 
	});
	$('#edit-sms-cancel').click(function () {
		$('#SMSform input').prop('disabled', true);
		$('#SMSform textarea').prop('disabled', true);
		$('#edit-sms-cancel').hide();
		$('#sms-save-changes').hide();
		$('#edit-sms').fadeIn();

	});
	$('#edit-fcm-cancel').click(function () {
		$('#FCMform input').prop('disabled', true);
		$('#FCMform textarea').prop('disabled', true);
		$('#edit-fcm-cancel').hide();
		$('#fcm-save-changes').hide();
		$('#edit-fcm').fadeIn();

	});
	$('#edit-map-cancel').click(function () {
		$('#Mapform input').prop('disabled', true);
		$('#Mapform textarea').prop('disabled', true);
		$('#edit-map-cancel').hide();
		$('#map-save-changes').hide();
		$('#edit-map').fadeIn();

	});
	$('#edit-payment-cancel').click(function () {
		$('#Paymentform input').prop('disabled', true);
		$('#Paymentform textarea').prop('disabled', true);
		$('#edit-payment-cancel').hide();
		$('#payment-save-changes').hide();
		$('#edit-payment').fadeIn();

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

