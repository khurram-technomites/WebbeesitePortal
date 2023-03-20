
$(document).ready(function () {

	/*$('#Userform input').prop('disabled', true);*/
	$('#Userform textarea').prop('disabled', true);
	/*$('#edit-cancel').fadeIn();*/
	$('#save-changes').hide();
	$('#edit-terms-condition').fadeIn();
	$('#edit-privacy-policy').fadeIn();
	$('#edit-DeliveryPolicy').fadeIn();
	$('#edit-ReturnPolicy').fadeIn();
	$('#terms-condition-save-changes').fadeIn();
	$('#privacy-policy-save-changes').fadeIn();
	$('#DeliveryPolicy-save-changes').fadeIn();
	$('#ReturnPolicy-save-changes').fadeIn();
	$('#AboutUs-save-changes').fadeIn();
	$('#edit-AboutUs').fadeIn();
	$('#edit-terms-condition').click(function () {

		$('#termsconditionform input').prop('disabled', false);
		$('#termsconditionform textarea').prop('disabled', false);

		$('#edit-terms-condition').hide();
		$('#edit-terms-condition-cancel').fadeIn();
		$('#terms-condition-save-changes').fadeIn();
	});
	$('#edit-privacy-policy').click(function () {

		$('#privacyPolicyform input').prop('disabled', false);
		$('#privacyPolicyform textarea').prop('disabled', false);

		$('#edit-privacy-policy').hide();
		$('#edit-privacy-policy-cancel').fadeIn();
		$('#privacy-policy-save-changes').fadeIn();
	});
	$('#edit-DeliveryPolicy').click(function () {

		$('#deliverPolicyform input').prop('disabled', false);
		$('#deliverPolicyform textarea').prop('disabled', false);

		$('#edit-DeliveryPolicy').hide();
		$('#edit-DeliveryPolicy-cancel').fadeIn();
		$('#DeliveryPolicy-save-changes').fadeIn();
	});
	$('#edit-ReturnPolicy').click(function () {

		$('#returnPolicyform input').prop('disabled', false);
		$('#returnPolicyform textarea').prop('disabled', false);

		$('#edit-ReturnPolicy').hide();
		$('#edit-ReturnPolicy-cancel').fadeIn();
		$('#ReturnPolicy-save-changes').fadeIn();
	});
	$('#edit-AboutUs').click(function () {

		$('#aboutusform input').prop('disabled', false);
		$('#aboutusform textarea').prop('disabled', false);

		$('#edit-AboutUs').hide();
		$('#edit-AboutUs-cancel').fadeIn();
		$('#AboutUs-save-changes').fadeIn();
	});
	$('#terms-condition-save-changes').click(function () {

		$('#termsconditionform input').prop('disabled', true);
		$('#termsconditionform textarea').prop('disabled', true);

		$('#edit-terms-condition').fadeIn();
		$('#edit-terms-condition-cancel').hide();
		$('#terms-condition-save-changes').fadeIn();
	});
	$('#privacy-policy-save-changes').click(function () {

		$('#privacyPolicyform input').prop('disabled', true);
		$('#privacyPolicyform textarea').prop('disabled', true);

		$('#edit-privacy-policy').fadeIn();
		$('#edit-privacy-policy-cancel').hide();
		$('#privacy-policy-save-changes').fadeIn();
	});
	$('#DeliveryPolicy-save-changes').click(function () {

		$('#deliverPolicyform input').prop('disabled', true);
		$('#deliverPolicyform textarea').prop('disabled', true);

		$('#edit-DeliveryPolicy').fadeIn();
		$('#edit-DeliveryPolicy-cancel').hide();
		$('#DeliveryPolicy-save-changes').fadeIn();
	});
	$('#ReturnPolicy-save-changes').click(function () {

		$('#returnPolicyform input').prop('disabled', true);
		$('#returnPolicyform textarea').prop('disabled', true);

		$('#edit-ReturnPolicy').fadeIn();
		$('#edit-ReturnPolicy-cancel').hide();
		$('#ReturnPolicy-save-changes').fadeIn();
	});
	$('#AboutUs-save-changes').click(function () {

		$('#aboutusform input').prop('disabled', true);
		$('#aboutusform textarea').prop('disabled', true);

		$('#edit-AboutUs').fadeIn();
		$('#edit-AboutUs-cancel').hide();
		$('#AboutUs-save-changes').fadeIn();
	});
	$('#edit-terms-condition-cancel').click(function () {
		$('#termsconditionform input').prop('disabled', true);
		$('#termsconditionform textarea').prop('disabled', true);
		$('#edit-terms-condition-cancel').hide();
		$('#terms-condition-save-changes').hide();
		$('#edit-terms-condition').fadeIn();

	});
	$('#edit-privacy-policy-cancel').click(function () {
		$('#privacyPolicyform input').prop('disabled', true);
		$('#privacyPolicyform textarea').prop('disabled', true);
		$('#edit-privacy-policy-cancel').hide();
		$('#privacy-policy-save-changes').hide();
		$('#edit-privacy-policy').fadeIn();

	});
	$('#edit-DeliveryPolicy-cancel').click(function () {
		$('#deliverPolicyform input').prop('disabled', true);
		$('#deliverPolicyform textarea').prop('disabled', true);
		$('#edit-DeliveryPolicy-cancel').hide();
		$('#DeliveryPolicy-save-changes').hide();
		$('#edit-DeliveryPolicy').fadeIn();

	});
	$('#edit-ReturnPolicy-cancel').click(function () {
		$('#returnPolicyform input').prop('disabled', true);
		$('#returnPolicyform textarea').prop('disabled', true);
		$('#edit-ReturnPolicy-cancel').hide();
		$('#ReturnPolicy-save-changes').hide();
		$('#edit-ReturnPolicy').fadeIn();

	});
	$('#edit-AboutUs-cancel').click(function () {
		$('#aboutusform input').prop('disabled', true);
		$('#aboutusform textarea').prop('disabled', true);
		$('#edit-AboutUs-cancel').hide();
		$('#AboutUs-save-changes').hide();
		$('#edit-AboutUs').fadeIn();

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

