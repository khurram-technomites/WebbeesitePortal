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

    BindImageUploader("#FooterBanner", "#image", "#CropperModal", "#FooterBannerPreview");

    BindImageUploader("#AboutUsBanner", "#image", "#CropperModal", "#AboutUsPreview");

    $('.theme-link.data').click(function () {

        $(this).hide();
        $(this).closest('.card').find(`.btn-actions`).show();

        if ($(this).closest('.card').hasClass('about-us-section')) {
            Description.isReadOnly = false;
        } else if ($(this).closest('.card').hasClass('mission-section')) {
            Mission.isReadOnly = false;
        } else if ($(this).closest('.card').hasClass('vision-section')) {
            Vision.isReadOnly = false;
        } else if ($(this).closest('.card').hasClass('values-section')) {
            Values.isReadOnly = false;
        } else if ($(this).closest('.card').hasClass('section-04')) {
            Section04Values.isReadOnly = false;
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
            Description.isReadOnly = true;
        } else if ($(this).closest('.card').hasClass('mission-section')) {
            Mission.isReadOnly = true;
        } else if ($(this).closest('.card').hasClass('vision-section')) {
            Vision.isReadOnly = true;
        } else if ($(this).closest('.card').hasClass('values-section')) {
            Values.isReadOnly = true;
        } else if ($(this).closest('.card').hasClass('section-04')) {
            Section04Values.isReadOnly = true;
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
    data.append("ImagePath", AboutUsBanner);
    data.append("Description", Description.getData());
    data.append("Section01", Mission.getData());
    data.append("Section02", Vision.getData());
    data.append("Section03", Values.getData());
    data.append("Section04", Section04Values.getData());
    //data.append("Description", $("#Description").val());
    //data.append("Section01", $('#Section01').val());
    //data.append("Section02", $('#Section02').val());
    //data.append("Section03", $('#Section03').val());
    //data.append("Section04", $('#Section04').val());
    data.append("__RequestVerificationToken", $('input[name=__RequestVerificationToken]').val());

    $.ajax({
        url: "/SparePart/AppointmentManagement/Edit/",
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
                    Description.isReadOnly = true;
                } else if (content === "mission") {
                    Mission.isReadOnly = true;;
                } else if (content === "vision") {
                    Vision.isReadOnly = true;
                } else if (content === "values") {
                    Values.isReadOnly = true;
                } else if (content === "section4") {
                    Section04Values.isReadOnly = true;
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
        url: "/SparePart/AppointmentManagement/UpdateThemeColor/",
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