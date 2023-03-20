"use strict";

// Class Definition
var KTLogin = function () {
	var _login;

	var showErrorMsg = function (form, type, msg) {
		var alert = $('<div class="kt-alert kt-alert--outline alert alert-' + type + ' " role="alert">\
			<button type="button" style="width: 5% !important;" class="close" data-dismiss="alert" aria-label="Close"><i class="fa fa-times"></i></button>\
			<span></span>\
		</div>');

		form.find('.alert').remove();
		alert.prependTo(form);
		//alert.animateClass('fadeIn animated');
		KTUtil.animateClass(alert[0], 'fadeIn animated');
		alert.find('span').html(msg);
	}

	var _showForm = function (form) {
		var cls = 'login-' + form + '-on';
		var form = 'kt_login_' + form + '_form';

		_login.removeClass('login-forgot-on');
		_login.removeClass('login-signin-on');
		_login.removeClass('login-signup-on');

		_login.addClass(cls);

		KTUtil.animateClass(KTUtil.getById(form), 'animate__animated animate__backInUp');
	}

	var _handleSignInForm = function () {
		var validation;
		const form = document.getElementById('kt_login_signin_form');
		// Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
		validation = FormValidation.formValidation(
			KTUtil.getById('kt_login_signin_form'),
			{
				fields: {
					NewPassword: {
						validators: {
							notEmpty: {
								message: 'Password is required'
							},
							regexp: {
								regexp: "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$",
								message: `Min. 8 characters, at least one uppercase letter, one lowercase <br/> letter, one number and one special character`,
							}
						}
					},
					ConfirmPassword: {
						validators: {
							notEmpty: {
								message: 'Confirm Password is required'
							},
							regexp: {
								regexp: "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$",
							},
							identical: {
								compare: function () {
									return form.querySelector('[name="NewPassword"]').value;
								},
								message: 'The password and its confirm are not the same'
							}
						}
					}
				},
				plugins: {
					trigger: new FormValidation.plugins.Trigger(),
					submitButton: new FormValidation.plugins.SubmitButton(),
					//defaultSubmit: new FormValidation.plugins.DefaultSubmit(), // Uncomment this line to enable normal button submit after form validation
					bootstrap: new FormValidation.plugins.Bootstrap()
				}
			}
		);

		// Revalidate the confirmation password when changing the password
		form.querySelector('[name="NewPassword"]').addEventListener('input', function () {
			validation.revalidateField('ConfirmPassword');
		});

		$('#kt_login_signin_submit').on('click', function (e) {
			e.preventDefault();

			var form = $(this).closest('form');
			validation.validate().then(function (status) {
				if (status == 'Valid') {
					$('#kt_login_signin_submit').addClass('spinner spinner-sm spinner-left').attr('disabled', true);
					$.ajax({
						url: '/Vendor/Account/ResetPassword',
						type: 'Post',
						data: $(form).serialize(),
						success: function (response) {
							// similate 2s delay
							if (response.success == true) {
								showErrorMsg(form, 'success', response.message);
								window.location.href = response.url;
							} else {
								showErrorMsg(form, 'danger', response.message);
								$('#kt_login_signin_submit').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);
							}
						}
					});
				} else {
					swal.fire({
						text: "Sorry, looks like there are some errors detected, please try again.",
						icon: "error",
						buttonsStyling: false,
						confirmButtonText: "Ok, got it!",
						customClass: {
							confirmButton: "btn font-weight-bold btn-light-primary"
						}
					}).then(function () {
						KTUtil.scrollTop();
					});
				}
			});
		});
	}

	// Public Functions
	return {
		// public functions
		init: function () {
			_login = $('#kt_login');

			_handleSignInForm();
		}
	};
}();

// Class Initialization
jQuery(document).ready(function () {
	KTLogin.init();
});
