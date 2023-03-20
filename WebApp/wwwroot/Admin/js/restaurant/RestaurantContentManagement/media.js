var _URL = window.URL || window.webkitURL;
var logoPath = "";
var OldThemeColor;
$(document).ready(function () {

	$('.image-upload:not(.banner) > i.fa-camera').click(function () {
		$(this).closest('.image-upload').find(`input[type="file"]`).trigger('click');
	});

	$('.theme-link.media').click(function () {
		$(this).closest('.row').find(`input[type="file"]`).trigger('click');
	});

	BindImageUploader("#Restaurantlogo", "#image", "#CropperModal", "#PreviewImage");

	BindImageUploader("#RestaurantFavicon", "#image", "#CropperModal", "#PreviewFavicon");

	BindImageUploader("#RestaurantSecondarylogo", "#image", "#CropperModal", "#PreviewSecondaryImage");

	BindImageUploader("#RestaurantThumbnailImage", "#image", "#CropperModal", "#PreviewThumbnailImage");

	BindImageUploader("#MenuBanner", "#image", "#CropperModal", "#MenuBannerPreview");

	BindImageUploader("#FooterBanner", "#image", "#CropperModal", "#FooterBannerPreview");

	BindImageUploader("#AboutUsBanner", "#image", "#CropperModal", "#AboutUsPreview");

	$('.theme-link.data').click(function () {

		$(this).hide();
		$(this).closest('.card').find(`.btn-actions`).show();

		if ($(this).closest('.card').hasClass('about-us-section')) {
			AboutUsEditor.isReadOnly = false;
		} else if ($(this).closest('.card').hasClass('terms-and-conditions-section')) {
			TermsAndConditionsEditor.isReadOnly = false;
		} else if ($(this).closest('.card').hasClass('privacy-policy-section')) {
			PrivacyPolicyEditor.isReadOnly = false;
		} else if ($(this).closest('.card').hasClass('delivery-policy-section')) {
			DeliveryPolicyEditor.isReadOnly = false;
		} else if ($(this).closest('.card').hasClass('return-policy-section')) {
			ReturnPolicyEditor.isReadOnly = false;
		}

		if ($(this).closest('.theme-color-section').hasClass('theme-color-section')) {
			$(this).closest('.theme-color-section').find(`.theme-color`).addClass('edit');
			OldThemeColor = $('#color').val();
		}

	});

	$('.btn-cancel').click(function () {

		$(this).closest('.card').find(`.btn-actions`).hide();
		$(this).closest('.card').find(`.theme-link.data`).show();

		if ($(this).closest('.card').hasClass('about-us-section')) {
			AboutUsEditor.isReadOnly = true;
		} else if ($(this).closest('.card').hasClass('terms-and-conditions-section')) {
			TermsAndConditionsEditor.isReadOnly = true;
		} else if ($(this).closest('.card').hasClass('privacy-policy-section')) {
			PrivacyPolicyEditor.isReadOnly = true;
		} else if ($(this).closest('.card').hasClass('delivery-policy-section')) {
			DeliveryPolicyEditor.isReadOnly = true;
		} else if ($(this).closest('.card').hasClass('return-policy-section')) {
			ReturnPolicyEditor.isReadOnly = true;
		}

		if ($(this).closest('.theme-color-section').hasClass('theme-color-section')) {
			$(this).closest('.theme-color-section').find(`.theme-color`).removeClass('edit');

			$('.theme-color').css('background', OldThemeColor)

			$('.theme-color > span').css('color', OldThemeColor)
			$('#color').css('background', OldThemeColor)
		}
	});

	$('.theme-color > span').click(function () {
		$('#color').trigger('click');
	});

	$('#color').change(function () {
		$('.theme-color').css('background', $(this).val())

		$('.theme-color > span').css('color', $(this).val())
		$(this).css('background', $(this).val())
	});


	$('.cropper-shape').click(function () {

		$('#CropperModal').removeClass($('.cropper-shape.active').attr('data'));

		$('.cropper-shape').removeClass('active');
		$(this).addClass('active');

		$('#CropperModal').addClass($(this).attr('data'));
	})


	$('.background-color').click(function () {
		$('.background-color').removeClass('active');
		$(this).addClass('active');


		$('.cropper-view-box').css('background', $('.background-color.active').attr('data'))
	})


	$('.cropper-aspectratio').click(function () {
		$('.cropper-aspectratio').removeClass('active');
		$(this).addClass('active');


		$('#image').attr('aspectratio', $(this).attr('data'));

		cropper.setAspectRatio($(this).attr('data'))		
	})

	
});

function SaveCroppedImage(element) {

	
	var data = new FormData();
	data.append("Id", $('#RestaurantID').val());
	data.append("filePath", $(element).closest('.image-upload').find('.image-preview').attr('src'));

	$.ajax({
		url: $(element).closest('.image-upload').attr('action'),
		type: "POST",
		processData: false,
		contentType: false,
		data: data,
		success: function (response) {
			if (response.success) {
				toastr.success(response.message);

				$(element).closest('.image-upload').find('.image-preview').attr('src', response.filePath);

				if (OldImage) {
					$.ajax({
						url: "/File/Delete?path=" + Path,
						type: "GET",
						processData: false,
						contentType: false,
						success: function (response) {
							OldImage = ''
						},
						error: function (er) {
							OldImage = ''
						}
					});
				}
			} else {
				toastr.error(response.message);
			}
		},
		error: function (er) {
			toastr.error(er);
		}
	});
}

function SaveContent(element, content) {
	
	$(element).addClass('spinner spinner-sm spinner-left').attr('disabled', true);

	var data = new FormData();
	data.append("Id", $('#Id').val());
	//if (content === "AboutUs") {
		
	//}
	//} else if (content === "TermsAndConditions") {
	//	data.append("TermsAndConditions", TermsAndConditionsEditor.getData());
	//} else if (content === "PrivacyPolicy") {
	//	data.append("PrivacyPolicy", PrivacyPolicyEditor.getData());
	//} else if (content === "DeliveryPolicy") {
	//	data.append("DeliveryPolicy", DeliveryPolicyEditor.getData());
	//} else if (content === "ReturnPolicy") {
	//	data.append("ReturnPolicy", ReturnPolicyEditor.getData());
	//}

	data.append("AboutUs", AboutUsEditor.getData());
	data.append("TermsAndConditions", TermsAndConditionsEditor.getData());
	data.append("PrivacyPolicy", PrivacyPolicyEditor.getData());
	data.append("DeliveryPolicy", DeliveryPolicyEditor.getData());
	data.append("ReturnPolicy", ReturnPolicyEditor.getData());

	data.append("__RequestVerificationToken", $('input[name=__RequestVerificationToken]').val());

	$.ajax({
		url: "/Restaurant/RestaurantContentManagement/Update/",
		type: "POST",
		processData: false,
		contentType: false,
		data: data,
		success: function (response) {
			if (response.success) {
				toastr.success(response.message);

				$(element).closest('.card').find(`.theme-link.data`).show();
				$(element).closest('.card').find(`.btn-actions`).hide();



				if (content === "AboutUs") {
					AboutUsEditor.isReadOnly = true;
				} else if (content === "TermsAndConditions") {
					TermsAndConditionsEditor.isReadOnly = true;
				} else if (content === "PrivacyPolicy") {
					PrivacyPolicyEditor.isReadOnly = true;
				} else if (content === "DeliveryPolicy") {
					DeliveryPolicyEditor.isReadOnly = true;
				} else if (content === "ReturnPolicy") {
					ReturnPolicyEditor.isReadOnly = true;
				}

			} else {
				toastr.error(response.message);
			}
			$(element).removeClass('spinner spinner-sm spinner-left').attr('disabled', false);
		},
		error: function (er) {
			toastr.error(er);
			$(element).removeClass('spinner spinner-sm spinner-left').attr('disabled', false);
		}
	});
}

function SaveThemeColor(element) {

	$(element).addClass('spinner spinner-sm spinner-left').attr('disabled', true);

	var data = new FormData();
	data.append("Id", $('#RestaurantID').val());
	data.append("ThemeColor", $('#color').val());

	$.ajax({
		url: "/Restaurant/RestaurantContentManagement/UpdateColorTheme/",
		type: "POST",
		processData: false,
		contentType: false,
		data: data,
		success: function (response) {
			if (response.success) {
				toastr.success(response.message);

				$(element).closest('.card').find(`.theme-link.data`).show();
				$(element).closest('.card').find(`.btn-actions`).hide();

				$('.theme-color-section').find(`.theme-color`).removeClass('edit');
			} else {
				toastr.error(response.message);
			}

			$(element).removeClass('spinner spinner-sm spinner-left').attr('disabled', false);
		},
		error: function (er) {
			$(element).removeClass('spinner spinner-sm spinner-left').attr('disabled', false);
			toastr.error(er);
		}
	});
}

$().cropper('getCroppedCanvas', {
	fillColor: '#fff'
})