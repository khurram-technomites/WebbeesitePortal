$(document).ready(function () {
    if (window.location.href.includes("#")) {
        
        var url = window.location.href;
        var Contact = url.substring(url.indexOf('#') + 1);
        $("#otpContact").html(Contact);
        history.pushState({}, null, window.location.href.replace("#" + Contact, ""));
        TimerOTP(5);
    }
    else {
        TimerOTP(5);
    }

    $('.digit-group').find('input').each(function () {
        $(this).attr('maxlength', 1);
        $(this).on('keyup', function (e) {
            var parent = $($(this).parent());

            if (e.keyCode === 8 || e.keyCode === 37) {
                var prev = parent.find('input#' + $(this).data('previous'));

                if (prev.length) {
                    $(prev).select();
                }
            } else if ((e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode >= 65 && e.keyCode <= 90) || (e.keyCode >= 96 && e.keyCode <= 105) || e.keyCode === 39) {
                var next = parent.find('input#' + $(this).data('next'));

                if (next.length) {
                    $(next).select();
                } else {
                    if (parent.data('autosubmit')) {
                        parent.submit();
                    }
                }
            }
        });
    });

    //OTP Verification
    $('#OTPVerificationForm').submit(function () {
        
        var form = $(this);
        $('#btnOtpVerfication').addClass('spinner spinner-sm spinner-left').attr('disabled', true);

        
        var OTP = "";
        $('.digit-group').find('input').each(function () {
            OTP = OTP + $(this).val();
        });
        var UserId = $("#UserId").val();
        var otp = Number(OTP);
        /*var contact = $('#otpContact').html();*/
       /* contact = contact.replace("+", "");*/

        if (otp == null || otp == NaN || otp == "") {
            showErrorMsg(form, 'danger', "Invalid OTP");
            $('#btnOtpVerfication').html('Submit').attr('disabled', false);
        }
        //else if (contact == null || contact == "") {
        //    showErrorMsg(form, 'danger', "Invalid Contact number");
        //    $('#btnOtpVerfication').html('Submit').attr('disabled', false);
        //}

        else {
            console.log(otp);
            $.ajax({
                url: "/Account/VerifyOTP?UserId=" + UserId + "&OTP=" + OTP,
                type: 'Post',
                //data: {
                //    UserId: UserId,
                //    OTP: OTP
                //},
                success: function (response) {
                    if (response.success) {
                        showErrorMsg(form, 'success', response.message);
                        setTimeout(function () {
                            window.location.href = response.url;
                        }, 3000);
                    } else {
                        showErrorMsg(form, 'danger', response.message);
                        
                        $('#btnOtpVerfication').html('Submit').attr('disabled', false);
                        if (response.description) {
                            $(form).prepend$(response.description);
                        }
                    }
                }
            });
        }
        return false;
    });
    //OTP Verification

    //OTP Resend


    $('#otpResend').click(function () {

        var form = $(this).closest('form');

        var Email = $('#otpEmail').html();
        $('#otpResend').slideUp();
        $('#btnOtpVerfication').html('<span class="fa fa-circle-notch fa-spin"></span> Submit').attr('disabled', true);
        $.ajax({
            url: "/Account/ResendOTP?Email=" + Email,
            type: 'Post',
            //data: {
            //    __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
            //    Contact: contact,
            //},
            success: function (response) {

                if (response.success) {
                    showErrorMsg(form, 'success', response.message);
                    $('#btnOtpVerfication').html('Submit').attr('disabled', false).show();
                    TimerOTP(120); // Countdown Timer
                } else {
                    showErrorMsg(form, 'danger', response.message);
                    $('#otpResend').slideDown();
                }
            },
            error: function (e) {
                showErrorMsg(form, 'danger', "Ooops, something went wrong.Try to refresh this page or feel free to contact us if the problem persists.");
                $('#btnOtpVerfication').html('Submit').attr('disabled', false).show();
                $('#otpResend').slideDown();
            },
            failure: function (e) {
                showErrorMsg(form, 'danger', "Ooops, something went wrong.Try to refresh this page or feel free to contact us if the problem persists.");
                $('#btnOtpVerfication').html('Submit').attr('disabled', false).show();
                $('#otpResend').slideDown();
            }
        });
        return false;
    });

    //OTP Resend
});

var showErrorMsg = function (form, type, msg) {
    var alert = $('<div class="alert kt-alert kt-alert--outline alert alert-' + type + ' " role="alert">\
			<button type="button" class="close" data-dismiss="alert" aria-label="Close"><i class="fa fa-times"></i></button>\
			<span></span>\
		</div>');

    form.find('.alert').remove();
    alert.prependTo(form);
    //KTUtil.animateClass(alert[0], 'fadeIn animated');
    $(alert).slideDown();
    setTimeout(function () {
        $(alert).slideUp();
    }, 6000);
    alert.find('span').html(msg);
}

function TimerOTP(time) {

    let timerOn = true;

    function timer(remaining) {
        var m = Math.floor(remaining / 60);
        var s = remaining % 60;

        m = m < 10 ? '0' + m : m;
        s = s < 10 ? '0' + s : s;
        document.getElementById('timer').innerHTML = m + ':' + s;
        remaining -= 1;

        if (remaining >= 0 && timerOn) {
            setTimeout(function () {
                timer(remaining);
            }, 1000);
            return;
        }

        if (!timerOn) {
            // Do validate stuff here
            return;
        }

        // Do timeout stuff here
        //alert('Timeout for otp');
        $('#btnOtpVerfication').attr('disabled', true).slideUp();
        $('#otpResend').slideDown();
    }

    timer(time); //seconds
}
