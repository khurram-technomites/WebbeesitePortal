"use strict";

jQuery(document).ready(function () {
    var _URL = window.URL || window.webkitURL;
    var logoPath;
    $(".logo").change(function (e) {
        var file, img;

        if ((file = this.files[0])) {
            if (this.files[0].size >= 500000) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Image size should be equal to 300 KB and dimension should be 1920 x 1000!',
                    //footer: '<a href>Image size should be less than or equal to  100KB and dimension should be 1713x540</a>'
                }).then(function (result) {
                    $(".cancelimage").trigger('click');
                });
                $(".logo").val("");
            }
            img = new Image();
            img.onload = function () {

                if (this.width < 1920 || this.width > 1920) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Image size should be equal to 300 KB and dimension should be 1920 x 1000!',
                        //  footer: '<a href>Image dimension should be 1713x540 and size should less than 100 KB</a>'
                    }).then(function (result) {
                        $(".cancelimage").trigger('click');
                    });
                    $(".logo").val("");
                }
                else if (this.height < 1000 || this.height > 1000) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Image size should be equal to 300 KB and dimension should be 1920 x 1000!',
                        // footer: '<a href>Image dimension should be 1713x540 and size should less than 100 KB</a>'
                    }).then(function (result) {
                        $(".cancelimage").trigger('click');
                    });
                    $(".logo").val("");
                }

                else {
                    img.onerror = function () {
                        alert("not a valid file: " + file.type);
                    };
                }
            };

            img.src = _URL.createObjectURL(file);

            logoPath = UploadImageToDraft(file).responseText;
            $(".ImageBanner").val(logoPath);


        }

    });
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
                url: '/Restaurant/RestaurantBannerSetting/Delete/' + record,
                type: 'POST',
                data: {
                    "__RequestVerificationToken":
                        $("input[name=__RequestVerificationToken]").val()
                },
                success: function (result) {
                    if (result.success != undefined) {
                        if (result.success) {
                            toastr.options = {
                                "positionClass": "toast-bottom-right",
                            };
                            toastr.success('Banner Deleted Successfully');

                            $(element).closest('.banner').remove();
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

function DeleteMobileBanner(element, record) {

    swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!'
    }).then(function (result) {
        if (result.value) {

            $.ajax({
                url: '/Restaurant/RestaurantBannerSetting/DeletePromotionBanner/' + record,
                type: 'POST',
                data: {
                    "__RequestVerificationToken":
                        $("input[name=__RequestVerificationToken]").val()
                },
                success: function (result) {
                    if (result.success != undefined) {
                        if (result.success) {
                            toastr.options = {
                                "positionClass": "toast-bottom-right",
                            };
                            toastr.success('Banner Deleted Successfully');

                            $(element).closest('.banner').remove();
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