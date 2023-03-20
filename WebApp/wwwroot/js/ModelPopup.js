var x;
var element
jQuery(document).ready(function () {
    jQuery.ajaxSetup({ cache: false });
    jQuery(".data_modal").on("click", function (e) {
        jQuery('#myModalContent').load(this.href, function () {
            jQuery('#myModal').modal({
                keyboard: false
            }, 'show');
            bindForm(this);
        });
        return false;
    });
});

function OpenModelPopup(elem, url, isedit) {

    try {
        //jQuery.ajaxSetup({ cache: false });
        //jQuery.ajaxSetup({ cache: false });
        jQuery('#myModal').modal({}, 'show');
        jQuery('#myModalContent').html('<div class="spinner spinner-lg spinner-center spinner-dark spinner-modal"></div>');
        jQuery('#myModalContent').load(url, function () {
            bindForm(this, elem, isedit);
        });
        element = elem;
    } catch (err) {
        console.log("From catch " + err)
    }

}

    }

}

    }

}

function bindForm(dialog, elem, isedit) {

    jQuery('form', dialog).submit(function () {
        var id = $(this).attr('id');
        $(this).closest('.modal').find(`button[type=submit][form=${id}]`).addClass('spinner spinner-sm spinner-left').attr('disabled', true);
    });

    jQuery('.form', dialog).submit(function () {

        Common.Ajax(this.method, this.action, jQuery(this).serialize(), 'json', function (result) {
            callback(result, dialog, elem, isedit);
        }
            , true, false)

        return false;
    });
}

function callback(response, dialog, elem, isedit) {

    if (response.success) {
        toastr.success(response.message);

        if (isedit) {
            Table.row($(elem).closest('tr')).remove().draw();
        }

        addRow(response.data);
        jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);
        jQuery('#myModal').modal('hide');
    }
    else {
        jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);

        toastr.error(response.message);
    }
}
