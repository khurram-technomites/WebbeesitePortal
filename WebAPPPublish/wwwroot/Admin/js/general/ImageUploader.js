/// <reference path="uppy.js" />
var OldImage = '';
var Uploader, OriginalImage, Modal, ImagePreview;

var IsPageLoaded = false;

var aspectratio = 1;
var minCroppedWidth = 0;
var minCroppedHeight = 0;

function BindImageUploader(uploader, image, modal, imagePreview, option) {

	OldImage = '';

	if (option) {
		AspectRatio = option.aspectRatio;
		minCroppedWidth = option.minCroppedWidth;
		minCroppedHeight = option.minCroppedHeight;
	}

	Uploader = uploader;
	OriginalImage = image;
	Modal = modal;
	ImagePreview = imagePreview;

	$(Uploader).change(function (e) {

		var files = e.target.files;

		var reader;
		var file;
		var url;

		if (files && files.length > 0) {
			file = files[0];

			if (URL) {
				done(URL.createObjectURL(file), Uploader, OriginalImage, Modal);
			} else if (FileReader) {
				reader = new FileReader();
				reader.onload = function (e) {
					done(reader.result, Uploader, OriginalImage, Modal);
				};
				reader.readAsDataURL(file);
			}
		}
	})

	if (!IsPageLoaded) {
		IsPageLoaded = true;
		$(Modal).on('shown.bs.modal', function () {
			cropper = new Cropper(document.getElementById('image'), {
				aspectRatio: aspectratio,
				viewMode: 1,
				
				crop: function (event) {
					var width = event.detail.width;
					var height = event.detail.height;

					if (width < minCroppedWidth || height < minCroppedHeight) {
						cropper.setData({
							width: minCroppedWidth,
							height: minCroppedHeight,
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
				canvas = cropper.getCroppedCanvas();
				$(ImagePreview).attr('src', canvas.toDataURL());

				canvas.toBlob(function (blob) {


					var file = new File([blob], "image.jpeg");
					logoPath = UploadImageToDraft(file).responseText;

					$(ImagePreview).attr('src', logoPath);

					$(ImagePreview).closest('.image-upload').removeClass('empty').addClass('uploaded');

					$(ImagePreview).closest('.image-upload').find('i.fa-pen').attr('onclick', `DeleteFile(this,'${logoPath}')`);

					$('#crop').removeClass('spinner spinner-left').prop('disabled', false);

					$(Modal).modal('hide');

					if (typeof SaveCroppedImage != 'undefined') {
						SaveCroppedImage(Uploader);
					}

				},'image/jpeg', 0.95);
			} else {
				$('#crop').removeClass('spinner spinner-left').prop('disabled', false);
			}
		});
	}
}

var done = function (url, uploader, image, modal) {
	$(Uploader).val('');
	$(OriginalImage).attr('src', url);

	$(Modal).modal('show');
};

function DeleteFile(elem, Path) {
	logoPath = "";

	if (!Path.startsWith('https://cdn.fougito.com/Draft')) {
		OldImage = Path;

		//$(elem).closest('.image-upload').find('img').attr('src', '');
		//$(elem).closest('.image-upload').removeClass('uploaded').addClass('empty');

		$('i.fa-camera').trigger('click');
	}
	else {

		let existingClasses = $(elem).attr('class');
		$(elem).attr('class', 'fa fa-circle-notch fa-spin').prop('disabled', true);

		$.ajax({
			url: "/File/Delete?path=" + Path,
			type: "GET",
			processData: false,
			contentType: false,
			success: function (response) {
				//console.log(response);
				//$(elem).attr('class', existingClasses).prop('disabled', false);
				//$(elem).closest('.image-upload').removeClass('uploaded').addClass('empty');

				$('i.fa-camera').trigger('click');
			},
			error: function (er) {

				$(elem).attr('class', existingClasses).prop('disabled', false);
				$('i.fa-camera').trigger('click');

				//if (OldImage.length > 0) {
				//	$(elem).closest('.image-upload').find('img').attr('src', OldImage);
				//} else {
				//	$(elem).closest('.image-upload').find('img').attr('src', '');
				//	$(elem).closest('.image-upload').removeClass('uploaded').addClass('empty');
				//}
			}
		});
	}
}

//function Compress() {
//	document.getElementById('file').addEventListener('change', (e) => {
//		const file = e.target.files[0];

//		if (!file) {
//			return;
//		}

//		new Compressor(file, {
//			quality: 0.6,

//			// The compression process is asynchronous,
//			// which means you have to access the `result` in the `success` hook function.
//			success(result) {
//				const formData = new FormData();

//				// The third parameter is required for server
//				formData.append('file', result, result.name);

//				// Send the compressed image file to server with XMLHttpRequest.
//				axios.post('/path/to/upload', formData).then(() => {
//					console.log('Upload success');
//				});
//			},
//			error(err) {
//				console.log(err.message);
//			},
//		});

//	});
//}