$(document).ready(function () {
    debugger;

    initIntlInputs();
    if (City != null) {
        $('#CountryId').trigger('change');
    }

});
$('#CountryId').change(function () {

    var count = 0;
    var $dropdown = $("#CityId");
    if ($(this).val() == 0) {
        $dropdown.empty();
        $dropdown.append($("<option />").val('').text("Please Select Country First!"));
    }
    else {
        $('.spinner-left').removeAttr("hidden", "hidden")

        $.ajax({
            type: 'Get',
            url: '/Vendor/Client/GetCityBycountryId?countryId=' + $(this).val(),
            success: function (response) {
                $dropdown.empty();
                $dropdown.append($("<option />").val('').text("Select city"));
                var cities = response.data;
                $.each(cities, function (k, v) {
                    $dropdown.append($("<option />").val(v.id).text(v.name));
                    count++;
                });
                $('.spinner-left').attr("hidden", "hidden")
                if (City != null)
                    $dropdown.val(City)
            }
        });
    }
});
$("#ClientCompleteYourProfile").submit(function () {
    debugger
    $('#DraftButton').addClass('spinner spinner-sm spinner-left');
    $('#DraftButton').attr('disabled', true);

    var data = {
        Id: $('input[name="GarageId"]').val(),
        NameAsPerTradeLicense: $('input[name="NameAsPerTradeLicense"]').val(),
        ClientIndustryId: $('select[name="ClientIndustryId"]').val(),
        ClientTypeId: $('select[name="ClientTypeId"]').val(),
        CountryId: $('select[name="CountryId"]').val(),
        CityId: $('select[name="CityId"]').val(),
        ContactPersonName: $('input[name="ContactPersonName"]').val(),
        ContactPersonEmail: $('input[name="ContactPersonEmail"]').val(),
        ContactPersonNumber: $('input[id="ContactNumber1Code"]').val() + '-' + $('input[name="ContactPersonNumber"]').val(),
        ContactPersonNumber01: $('input[id="ContactNumber2Code"]').val() + '-' + $('input[name="ContactPersonNumber01"]').val(),
        Whatsapp: $('input[id="Whatsappcode"]').val() + '-' + $('input[name="Whatsapp"]').val(),
        Address: $('input[name="Address"]').val(),
        UserId: $('input[name="UserId"]').val(),
        User:
        {
            UserId: $('input[name="UserId"]').val(),
            Password: $('input[name="Password"]').val(),
            PhoneNumber: $("#PhoneNumberCode").val() + '-' + $('input[name="PhoneNumber"]').val()
        },
        GarageBusinessSetting:
        {
            Id: $('input[name="GarageBusinessSettingId"]').val(),
            GarageId: $('input[name="GarageId"]').val(),
            Facebook: $('input[name="GarageBusinessSetting.Facebook"]').val(),
            Instagram: $('input[name="GarageBusinessSetting.Instagram"]').val(),
            LinkedIn: $('input[name="GarageBusinessSetting.LinkedIn"]').val(),
            Youtube: $('input[name="GarageBusinessSetting.Youtube"]').val(),
            Twitter: $('input[name="GarageBusinessSetting.Twitter"]').val(),
            Snapchat: $('input[name="GarageBusinessSetting.Snapchat"]').val(),
            Pinterest: $('input[name="GarageBusinessSetting.Pinterest"]').val(),
            Behnace: $('input[name="GarageBusinessSetting.Behnace"]').val(),
            Tiktok: $('input[name="GarageBusinessSetting.Tiktok"]').val(),
            VatTaxID: $('input[name="VatTaxID"]').val(),
            BusinessRegistration: $('input[name="BusinessRegistration"]').val()
        },
        GarageContentManagement:
        {
            GarageId: $('input[name="GarageId"]').val(),
            Id: $('input[name="GarageContentManagementId"]').val(),
            CEOName: $('input[name="CEOName"]').val(),
        }
    }

    $.ajax({
        url: "/Vendor/Client/Edit",
        type: "POST",
        data: data,
        success: function (response) {
            if (response.success) {
                $('#DraftButton').removeClass('spinner spinner-sm spinner-left');
                $('#DraftButton').attr('disabled', false);
                toastr.success(response.message);
                $('input[name="GarageBusinessSettingId"]').val(response.result.Result.GarageBusinessSetting.Id)
                $('input[name="GarageContentManagementId"]').val(response.result.Result.GarageContentManagement.Id)
                //For Edit Layout
                if (Layout == "ClientEdit") {
                    LoadPage('step3', '/Vendor/Client/Content')
                }//For Main Layout
                else {
                    LoadPage('step2', '/Vendor/Client/Package')
                }
            }
            else {
                $('#DraftButton').removeClass('spinner spinner-sm spinner-left');
                $('#DraftButton').attr('disabled', false);
                toastr.error(response.message);
            }

        },
        error: function (er) {
            $('#DraftButton').removeClass('spinner spinner-sm spinner-left');
            $('#DraftButton').attr('disabled', false);
            toastr.error("Some thing went wrong !");
        }
    });
    return false;
});