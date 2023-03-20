/// <reference path="uppy.js" />
var OldImage = '';
var BannerPath = ''
var Uploader, OriginalImage, Modal, ImagePreview, RequireAdditional, Thumbnail;

var IsPageLoaded = false;

var aspectratio = NaN;
var minCroppedWidth = 0;
var minCroppedHeight = 0;

function BindImageUploader(uploader, image, modal, imagePreview, option, requireAdditional = false,thumbnail = false) {
    console.log(uploader)
    OldImage = '';

    if (option) {
        aspectratio = option.aspectRatio;
        minCroppedWidth = option.minCroppedWidth;
        minCroppedHeight = option.minCroppedHeight;
    }

    Uploader = uploader;
    OriginalImage = image;
    Modal = modal;
    ImagePreview = imagePreview;
    RequireAdditional = requireAdditional;
    Thumbnail = thumbnail;

    $(uploader).change(function (e) {
        console.log("changes")
        var files = e.target.files;

        var reader;
        var file;
        var url;

        if (files && files.length > 0) {
            file = files[0];

            if (URL) {
                done(URL.createObjectURL(file), uploader, image, imagePreview, Modal, requireAdditional, thumbnail);
            } else if (FileReader) {
                reader = new FileReader();
                reader.onload = function (e) {
                    done(reader.result, uploader, image, imagePreview, Modal, requireAdditional, thumbnail);
                };
                reader.readAsDataURL(file);
            }
        }
    })

    if (!IsPageLoaded) {
        IsPageLoaded = true;
        $(Modal).on('shown.bs.modal', function () {
            cropper = new Cropper(document.getElementById('image'), {
                aspectRatio: Number($('#image').attr('aspectratio')),
                viewMode: 0,
                /*viewMode: 1,*/
                crop: function (event) {
                    var width = event.detail.width;
                    var height = event.detail.height;

                    if (width < Number($('#image').attr('minCroppedWidth')) || height < Number($('#image').attr('minCroppedHeight'))) {
                        cropper.setData({
                            width: Number($('#image').attr('minCroppedWidth')),
                            height: Number($('#image').attr('minCroppedHeight')),
                        });
                    }
                    /*console.log(JSON.stringify(cropper.getData(true)));*/
                    /*data.textContent = JSON.stringify(cropper.getData(true));*/
                },
            });
        }).on('hidden.bs.modal', function () {
            cropper.destroy();
            cropper = null;
        });

        $('#crop').click(function () {

            $('#crop').addClass('spinner spinner-left').prop('disabled', true);
            var canvas;

            if (cropper) {

                let fileExtension = 'jpeg';
                //if ($('#CropperModal').hasClass('restaurant-logo')) {
                //    fileExtension = 'png'
                //}

                if ($(Modal).hasClass('round')) {
                    // Crop
                    croppedCanvas = cropper.getCroppedCanvas({
                        fillColor: $('.background-color.active').attr('data')
                    });

                    // Round
                    roundedCanvas = getRoundedCanvas(croppedCanvas);

                    $(ImagePreview).attr('src', roundedCanvas.toDataURL());
                    canvas = roundedCanvas;

                } else {
                    canvas = cropper.getCroppedCanvas({
                        fillColor: $('.background-color.active').attr('data')
                    });
                    $(ImagePreview).attr('src', canvas.toDataURL());
                }

                canvas.toBlob(function (blob) {

                    new Compressor(blob, {
                        quality: 1,

                        success(result) {
                            if (Thumbnail) {
                                Thumbnailimage = UploadImageToDraft(new File([result], "image." + fileExtension)).responseText.replace(" ", "%20");

                                console.log('After Compressor : ' + Thumbnailimage);

                                $(ImagePreview).attr('src', Thumbnailimage);

                                $(ImagePreview).closest('.image-upload').removeClass('empty').addClass('uploaded');

                                $(ImagePreview).closest('.image-upload').find('i.fa-trash').attr('onclick', `DeleteFile(this,'${Thumbnailimage}')`);

                                $('#crop').removeClass('spinner spinner-left').prop('disabled', false);

                                $(Modal).modal('hide');

                                if (typeof SaveCroppedImage != 'undefined') {
                                    SaveCroppedImage(Uploader);
                                }
                            }
                            else if (RequireAdditional) {
                                BannerPath = UploadImageToDraft(new File([result], "image." + fileExtension)).responseText.replace(" ", "%20");

                                console.log('After Compressor : ' + BannerPath);

                                $(ImagePreview).attr('src', BannerPath);

                                $(ImagePreview).closest('.image-upload').removeClass('empty').addClass('uploaded');

                                $(ImagePreview).closest('.image-upload').find('i.fa-trash').attr('onclick', `DeleteFile(this,'${BannerPath}')`);

                                $('#crop').removeClass('spinner spinner-left').prop('disabled', false);

                                $(Modal).modal('hide');

                                if (typeof SaveCroppedImage != 'undefined') {
                                    SaveCroppedImage(Uploader);
                                }
                            }
                            else {
                                logoPath = UploadImageToDraft(new File([result], "image." + fileExtension)).responseText.replace(" ", "%20");

                                console.log('After Compressor : ' + logoPath);

                                $(ImagePreview).attr('src', logoPath);

                                $(ImagePreview).closest('.image-upload').removeClass('empty').addClass('uploaded');

                                $(ImagePreview).closest('.image-upload').find('i.fa-trash').attr('onclick', `DeleteFile(this,'${logoPath}')`);

                                $('#crop').removeClass('spinner spinner-left').prop('disabled', false);

                                $(Modal).modal('hide');

                                if (typeof SaveCroppedImage != 'undefined') {
                                    SaveCroppedImage(Uploader);
                                }
                            }
                        },
                        error(err) {
                            console.log(err.message);
                        },
                    });
                }, 'image', 0.95);
            } else {
                $('#crop').removeClass('spinner spinner-left').prop('disabled', false);
            }
        });
    }
}

var done = function (url, uploader, image, imagePreview, modal, requireAdditional, thumbnail) {
    $(uploader).val('');
    $(image).attr('src', url);

    $('#image').attr('aspectratio', $(uploader).attr('aspectratio'))
    $('#image').attr('minCroppedWidth', $(uploader).attr('minCroppedWidth'))
    $('#image').attr('minCroppedHeight', $(uploader).attr('minCroppedHeight'))


    Uploader = uploader;
    OriginalImage = image;
    Modal = modal;
    ImagePreview = imagePreview;
    RequireAdditional = requireAdditional;
    Thumbnail = thumbnail;


    if (uploader === "#Restaurantlogo" || uploader === "#Favicon" || uploader === "#GarageLogo" || uploader === "#SparePartLogo" || uploader === "#Supplierlogo" || uploader === "#RestaurantSecondarylogo" || uploader === "#RestaurantThumbnailImage" || uploader === "#RestaurantFavicon" ||
        uploader === "#ThumbnailImage" || uploader === "#logo" || uploader === "#GarageSecondarylogo" || uploader === "#GarageThumbnailImage" || uploader === "#AboutUsBanner" || uploader === "#CEOImage" || uploader === "#FooterBanner" || uploader === "#ServiceBanner" || uploader === "#ServiceThumbnail"|| uploader === "#InnerPagesBanner" ) {
        $(Modal).addClass('restaurant-logo');
        console.log("Class added")
        $(Modal).addClass($('.cropper-shape.active').attr('data'));
    } else {
        $(Modal).removeClass('restaurant-logo');

        $(Modal).removeClass($('.cropper-shape.active').attr('data'));
    }

    $(Modal).modal('show');
};

function DeleteFile(elem, Path, Table = 'Optional') {
    logoPath = "";

    if (!Path.startsWith('https://cdn.fougito.com/Draft')) {
        OldImage = Path;

        $(elem).closest('.image-upload').find('img').attr('src', '');
        $(elem).closest('.image-upload').removeClass('uploaded').addClass('empty');
    }
    else {

        let existingClasses = $(elem).attr('class');
        $(elem).attr('class', 'fa fa-circle-notch fa-spin').prop('disabled', true);

        $.ajax({
            url: "/File/Delete?path=" + Path + "&table=" + Table,
            type: "GET",
            processData: false,
            contentType: false,
            success: function (response) {
                console.log(response);
                $(elem).attr('class', existingClasses).prop('disabled', false);
                $(elem).closest('.image-upload').removeClass('uploaded').addClass('empty');
            },
            error: function (er) {

                $(elem).attr('class', existingClasses).prop('disabled', false);


                if (OldImage.length > 0) {
                    $(elem).closest('.image-upload').find('img').attr('src', OldImage);
                } else {
                    $(elem).closest('.image-upload').find('img').attr('src', '');
                    $(elem).closest('.image-upload').removeClass('uploaded').addClass('empty');
                }
            }
        });
    }
}

function Compress() {
    document.getElementById('file').addEventListener('change', (e) => {
        const file = e.target.files[0];

        if (!file) {
            return;
        }

        new Compressor(file, {
            quality: 0.6,

            // The compression process is asynchronous,
            // which means you have to access the `result` in the `success` hook function.
            success(result) {
                const formData = new FormData();

                // The third parameter is required for server
                formData.append('file', result, result.name);

                // Send the compressed image file to server with XMLHttpRequest.
                axios.post('/path/to/upload', formData).then(() => {
                    console.log('Upload success');
                });
            },
            error(err) {
                console.log(err.message);
            },
        });
    });
}

function getRoundedCanvas(sourceCanvas) {
    var canvas = document.createElement('canvas');
    var context = canvas.getContext('2d');
    var width = sourceCanvas.width;
    var height = sourceCanvas.height;

    canvas.width = width;
    canvas.height = height;
    context.fillStyle = "blue";
    context.imageSmoothingEnabled = true;
    context.drawImage(sourceCanvas, 0, 0, width, height);
    context.globalCompositeOperation = 'destination-in';
    context.beginPath();
    context.arc(width / 2, height / 2, Math.min(width, height) / 2, 0, 2 * Math.PI, true);
    context.fill();
    return canvas;
}