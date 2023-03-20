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

	BindImageUploader("#CEOImage", "#image", "#CropperModal", "#CEOImagePreview");

	BindImageUploader("#SparePartSecondarylogo", "#image", "#CropperModal", "#PreviewSecondaryImage");

	BindImageUploader("#SparePartThumbnailImage", "#image", "#CropperModal", "#PreviewThumbnailImage");

	BindImageUploader("#FooterBanner", "#image", "#CropperModal", "#FooterBannerPreview");

	BindImageUploader("#AboutUsBanner", "#image", "#CropperModal", "#AboutUsPreview");

	$('.theme-link.data').click(function () {

		$(this).hide();
		$(this).closest('.card').find(`.btn-actions`).show();

		if ($(this).closest('.card').hasClass('about-us-section')) {
			AboutUsEditor.isReadOnly = false;
		} else if ($(this).closest('.card').hasClass('mission-section')) {
			Mission.isReadOnly = false;
		} else if ($(this).closest('.card').hasClass('vision-section')) {
			Vision.isReadOnly = false;
		} else if ($(this).closest('.card').hasClass('values-section')) {
			Values.isReadOnly = false;
		} else if ($(this).closest('.card').hasClass('CEOMessage-section')) {
			CEOMessage.isReadOnly = false;
		} else if ($(this).closest('.card').hasClass('CEOName-section')) {
			$('#CEOName').removeAttr('disabled')
		} else if ($(this).closest('.card').hasClass('PrivacyPolicy-section')) {
			PrivacyPolicyEditor.isReadOnly = false;
		} else if ($(this).closest('.card').hasClass('TAC-section')) {
			TermsAndConditionsEditor.isReadOnly = false;
		} else if ($(this).closest('.card').hasClass('PromoSection01-section')) {
			$('#PromoSection01').removeAttr('disabled')
			$('#PromoSection01Count').removeAttr('disabled')
		} else if ($(this).closest('.card').hasClass('PromoSection02-section')) {
			$('#PromoSection02').removeAttr('disabled')
			$('#PromoSection02Count').removeAttr('disabled')
		} else if ($(this).closest('.card').hasClass('PromoSection03-section')) {
			$('#PromoSection03').removeAttr('disabled')
			$('#PromoSection03Count').removeAttr('disabled')
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
		} else if ($(this).closest('.card').hasClass('mission-section')) {
			Mission.isReadOnly = true;
		} else if ($(this).closest('.card').hasClass('vision-section')) {
			Vision.isReadOnly = true;
		} else if ($(this).closest('.card').hasClass('values-section')) {
			Values.isReadOnly = true;
		} else if ($(this).closest('.card').hasClass('CEOMessage-section')) {
			CEOMessage.isReadOnly = true;
		} else if ($(this).closest('.card').hasClass('CEOName-section')) {
			$('#CEOName').attr('disabled', 'disabled')
		}
		else if ($(this).closest('.card').hasClass('PrivacyPolicy-section')) {
			PrivacyPolicyEditor.isReadOnly = true;
		} else if ($(this).closest('.card').hasClass('TAC-section')) {
			TermsAndConditionsEditor.isReadOnly = true;
		} else if ($(this).closest('.card').hasClass('PromoSection01-section')) {
			$('#PromoSection01').attr('disabled', 'disabled')
			$('#PromoSection01Count').attr('disabled', 'disabled')
		} else if ($(this).closest('.card').hasClass('PromoSection02-section')) {
			$('#PromoSection02').attr('disabled', 'disabled')
			$('#PromoSection02Count').attr('disabled', 'disabled')
		} else if ($(this).closest('.card').hasClass('PromoSection03-section')) {
			$('#PromoSection03').attr('disabled', 'disabled')
			$('#PromoSection03Count').attr('disabled', 'disabled')
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
	debugger;
	$(element).addClass('spinner spinner-sm spinner-left').attr('disabled', true);

	var data = new FormData();
	data.append("Id", $('#Id').val());
	if (content === "AboutUs") {
		data.append("AboutUsDescription", AboutUsEditor.getData());
	} else if (content === "CEOMessage") {
		data.append("CEOMessage", CEOMessage.getData());
	} else if (content === "CEOName") {
		data.append("CEOName", $("#CEOName").val());
	} else if (content === "mission") {
		data.append("Mission", Mission.getData());
	} else if (content === "vision") {
		data.append("Vision", Vision.getData());
	} else if (content === "values") {
		data.append("Values", Values.getData());
	} else if (content === "PrivacyPolicy") {
		data.append("PrivacyPolicy", PrivacyPolicyEditor.getData());
	} else if (content === "TermsAndConditions") {
		data.append("TermsAndConditions", TermsAndConditionsEditor.getData());
	} else if (content === "PromoSection01") {
		data.append("PromoSection01", $("#PromoSection01").val());
	} else if (content === "PromoSection02") {
		data.append("PromoSection02", $("#PromoSection02").val());
	} else if (content === "PromoSection03") {
		data.append("PromoSection03", $("#PromoSection03").val());
	}

	data.append("PromoSection01Count", $("#PromoSection01Count").val());
	data.append("PromoSection02Count", $("#PromoSection02Count").val());
	data.append("PromoSection03Count", $("#PromoSection03Count").val());

	data.append("__RequestVerificationToken", $('input[name=__RequestVerificationToken]').val());

	$.ajax({
		url: "/SparePart/ContentManagement/Edit/",
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
				} else if (content === "CEOMessage") {
					CEOMessage.isReadOnly = true;;
				} else if (content === "CEOName") {
					$("#CEOName").attr("disabled", "disabled");
				} else if (content === "mission") {
					Mission.isReadOnly = true;
				} else if (content === "vision") {
					Vision.isReadOnly = true;
				} else if (content === "values") {
					Values.isReadOnly = true;
				} else if (content === "PrivacyPolicy") {
					PrivacyPolicyEditor.isReadOnly = true;
				} else if (content === "TermsAndConditions") {
					TermsAndConditionsEditor.isReadOnly = true;
				} else if (content === "PromoSection01") {
					$("#PromoSection01").attr("disabled", "disabled");
					$("#PromoSection01Count").attr("disabled", "disabled");
				} else if (content === "PromoSection02") {
					$("#PromoSection02").attr("disabled", "disabled");
					$("#PromoSection02Count").attr("disabled", "disabled");
				} else if (content === "PromoSection03") {
					$("#PromoSection03").attr("disabled", "disabled");
					$("#PromoSection03Count").attr("disabled", "disabled");
				}

				if ($('#Id').val() == 0)
					$('#Id').val(response.data)

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
	data.append("ThemeColor", $('#color').val());

	$.ajax({
		url: "/SparePart/ContentManagement/UpdateThemeColor/",
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
