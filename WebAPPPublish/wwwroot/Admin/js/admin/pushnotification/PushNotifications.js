var PageNo = 1;
var MoreRecordExist = true;
var IsUnseenRecoredExist = false;
var IsUnReadRecoredExist = false;
var UnSeenMessages = 0;
var UndeliveredMessages = 0;

jQuery(document).ready(function () {
    $.ajax({
        type: 'Get',
        url: '/Admin/Notification/LoadNotifications?pageNo=' + PageNo,
        success: function (data) {
            if (data.success) {
                BindNotification(data);

                $("#ulNotification").bind('scroll', function () {

                    if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {

                        console.log("bottoms up")
                        if (MoreRecordExist) {
                            if ($('#liNotificationSpinner').length == 0) {
                                $("#ulNotification").append('<li id="liNotificationSpinner">\
										<div class="text-center">\
											<i class="fa fa-spinner fa-spin"></i>\
										</div>\
									</li>');
                                PageNo++;
                                $.ajax({
                                    type: 'Get',
                                    url: '/Admin/Notification/LoadNotifications?pageNo=' + PageNo,
                                    success: function (response) {
                                        LoadMoreNotification(response);
                                    }
                                });
                            }
                        }
                    }
                })
            }
        }
    });
});
/*
$("#ViewAllNotification").click(function () {
    
    $.ajax({
        type: 'Get',
        url: '/Admin/Notification/ViewAllNotification?pageNo=' + PageNo,
        success: function (data) {
            if (data.success) {
               
            }
        }
    });

});*/

function BindNotification(response) {

    var DivNotify = '';

    if (response.data.length > 0) {
        if (response.data.length < 5) {
            MoreRecordExist = false;
        }

        UnSeenMessages = response.data.filter(function (obj) {
            return obj.isSeen == false;
        }).length;

        if (UnSeenMessages > 0) {
            $('#NotificationCount').html(UnSeenMessages + ' new');
            $('#NotificationCount').fadeIn();

            $('.notification-alert').show();
            IsUnseenRecoredExist = true;
        }

        UndeliveredMessages = response.data.filter(function (obj) {
            return obj.isDelivered == false;
        }).length;


        console.log("UndeliveredMessages", UndeliveredMessages)

        if (UndeliveredMessages > 0) {
            playSound();
        }

        IsUnReadRecoredExist = response.data.filter(function (obj) {
            return obj.isRead == false;
        }).length > 0 ? true : false;

        $.each(response.data, function (k, v) {
            DivNotify += getHtmlizeNotification(v);
        });

        $('[data-toggle="popover"]').popover();

    }
    else {
        DivNotify += '<i class="notify-empty-template" style="display: block;text-align: center;">No new Notification</i>'
    }
    $('#ulNotification').empty();
    $('#ulNotification').append(DivNotify);
    $('[data-toggle="popover"]').popover();
}

function LoadMoreNotification(response) {
    if (response.data.length < 5) {
        MoreRecordExist = false;
    }
    UnSeenMessages = response.data.filter(function (obj) {
        return obj.IsSeen == false;
    }).length;

    if (UnSeenMessages > 0) {
        UnSeenMessages += Number($('#NotificationCount').html().replace(' new', ''));

        $('#NotificationCount').html(UnSeenMessages + ' new');
        $('#NotificationCount').fadeIn();

        $('.notification-alert').removeClass('kt-hidden');

        playSound();
        IsUnseenRecoredExist = true;
    }

    IsUnReadRecoredExist = response.data.filter(function (obj) {
        return obj.IsRead == false;
    }).length > 0 ? true : false;

    var DivNotify = '';
    $.each(response.data, function (k, v) {
        DivNotify += getHtmlizeNotification(v);
    });
    $('[data-toggle="popover"]').popover();

    $('#liNotificationSpinner').hide();
    $('#liNotificationSpinner').remove();
    $('#ulNotification').append(DivNotify);
    $('[data-toggle="popover"]').popover();
}

function FetchNewNotification() {
    $.ajax({
        type: 'Get',
        url: '/Admin/Notification/GetNotifications/',
        success: function (response) {
            if (response.success) {
                BindNewNotifications(response);
            }
            else {
                $('#btnLogout').click();
            }
        }
    });
}

function BindNewNotifications(response) {

    if (response.data.length > 0) {
        UnSeenMessages = response.data.filter(function (obj) {
            return obj.IsSeen == false;
        }).length;

        if (UnSeenMessages > 0) {
            UnSeenMessages += Number($('#NotificationCount').html().replace(' new', ''));

            $('#NotificationCount').html(UnSeenMessages + ' new');
            $('#NotificationCount').fadeIn();

            $('.notification-alert').removeClass('kt-hidden').show();

            playSound();
            IsUnseenRecoredExist = true;
        }

        IsUnReadRecoredExist = response.data.filter(function (obj) {
            return obj.IsRead == false;
        }).length > 0 ? true : false;

        var DivNotify = '';
        $.each(response.data, function (k, v) {
            DivNotify += getHtmlizeNotification(v);
        });

        $('[data-toggle="popover"]').popover();

        $(DivNotify).prependTo("#ulNotification");
        $('.notify-empty-template').remove();
        $('[data-toggle="popover"]').popover();
    }
}

function getHtmlizeNotification(v) {

    console.log(v)
    var Title;
    if (v.notification.title == undefined) {
        Title = 'New Notification'
    }
    else {
        Title = v.notification.title
    }

    var HTMLNotification = '';
    if (!v.isRead) {
        //UnRead Messages
        HTMLNotification += '<div class="d-flex align-items-center mb-4" ' + (v.isSeen ? '' : 'un-seen') + '" style="border-radius: 8px">';
        HTMLNotification += '<div class="symbol symbol-40 symbol-light-danger mr-5">';
        HTMLNotification += '<a href="' + (v.notification.url ? v.notification.url : 'javascript:;') + '" style="display: block; text-decoration: none; color: inherit">';
        HTMLNotification += '<span class="symbol-label">';
        HTMLNotification += '<span class="svg-icon svg-icon-lg svg-icon-warning">';
        HTMLNotification += '<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">';
        HTMLNotification += '<g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">'
        HTMLNotification += '<rect x="0" y="0" width="24" height="24" />'
        HTMLNotification += '<path d="M16,15.6315789 L16,12 C16,10.3431458 14.6568542,9 13,9 L6.16183229,9 L6.16183229,5.52631579 C6.16183229,4.13107011 7.29290239,3 8.68814808,3 L20.4776218,3 C21.8728674,3 23.0039375,4.13107011 23.0039375,5.52631579 L23.0039375,13.1052632 L23.0206157,17.786793 C23.0215995,18.0629336 22.7985408,18.2875874 22.5224001,18.2885711 C22.3891754,18.2890457 22.2612702,18.2363324 22.1670655,18.1421277 L19.6565168,15.6315789 L16,15.6315789 Z" fill="#000000" />'
        HTMLNotification += '<path d="M1.98505595,18 L1.98505595,13 C1.98505595,11.8954305 2.88048645,11 3.98505595,11 L11.9850559,11 C13.0896254,11 13.9850559,11.8954305 13.9850559,13 L13.9850559,18 C13.9850559,19.1045695 13.0896254,20 11.9850559,20 L4.10078614,20 L2.85693427,21.1905292 C2.65744295,21.3814685 2.34093638,21.3745358 2.14999706,21.1750444 C2.06092565,21.0819836 2.01120804,20.958136 2.01120804,20.8293182 L2.01120804,18.32426 C1.99400175,18.2187196 1.98505595,18.1104045 1.98505595,18 Z M6.5,14 C6.22385763,14 6,14.2238576 6,14.5 C6,14.7761424 6.22385763,15 6.5,15 L11.5,15 C11.7761424,15 12,14.7761424 12,14.5 C12,14.2238576 11.7761424,14 11.5,14 L6.5,14 Z M9.5,16 C9.22385763,16 9,16.2238576 9,16.5 C9,16.7761424 9.22385763,17 9.5,17 L11.5,17 C11.7761424,17 12,16.7761424 12,16.5 C12,16.2238576 11.7761424,16 11.5,16 L9.5,16 Z" fill="#000000" opacity="0.3" />'
        HTMLNotification += '</g>'
        HTMLNotification += '</svg>'
        HTMLNotification += '</span>'
        HTMLNotification += '</span>'
        HTMLNotification += '</div>'
        HTMLNotification += '<div class="kt-notification__item-details">';
        HTMLNotification += '<span class="kt-notification__item-title" style="font-size: 12px;color: black;font-weight: 600;">';
        HTMLNotification += Title;
        HTMLNotification += '</span>';
        HTMLNotification += '<div class="kt-notification__item-time">';
        HTMLNotification += v.notification.description
        HTMLNotification += '</div>';
        HTMLNotification += '</a>';
        HTMLNotification += '</div>';
        HTMLNotification += '<i class="fa fa-circle notification-read-circle pl-31 cursor-pointer" onclick="IsRead(' + v.id + ',this);" data-container="body" data-toggle="popover" data-trigger="hover" data-placement="top" float="right" data-content="mark as read"></i>';
        HTMLNotification += '</div>';
    } else {
        HTMLNotification += '<div class="d-flex align-items-center mb-4" >';
        HTMLNotification += '<div class="symbol symbol-40 symbol-light-danger mr-5">';
        HTMLNotification += '<a href="' + (v.notification.url ? "/" + v.notification.url : 'javascript:;') + '" style="display: block; text-decoration: none; color: inherit">';
        HTMLNotification += '<span class="symbol-label">';
        HTMLNotification += '<span class="svg-icon svg-icon-lg svg-icon-warning">';
        HTMLNotification += '<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">';
        HTMLNotification += '<g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">'
        HTMLNotification += '<rect x="0" y="0" width="24" height="24" />'
        HTMLNotification += '<path d="M16,15.6315789 L16,12 C16,10.3431458 14.6568542,9 13,9 L6.16183229,9 L6.16183229,5.52631579 C6.16183229,4.13107011 7.29290239,3 8.68814808,3 L20.4776218,3 C21.8728674,3 23.0039375,4.13107011 23.0039375,5.52631579 L23.0039375,13.1052632 L23.0206157,17.786793 C23.0215995,18.0629336 22.7985408,18.2875874 22.5224001,18.2885711 C22.3891754,18.2890457 22.2612702,18.2363324 22.1670655,18.1421277 L19.6565168,15.6315789 L16,15.6315789 Z" fill="#000000" />'
        HTMLNotification += '<path d="M1.98505595,18 L1.98505595,13 C1.98505595,11.8954305 2.88048645,11 3.98505595,11 L11.9850559,11 C13.0896254,11 13.9850559,11.8954305 13.9850559,13 L13.9850559,18 C13.9850559,19.1045695 13.0896254,20 11.9850559,20 L4.10078614,20 L2.85693427,21.1905292 C2.65744295,21.3814685 2.34093638,21.3745358 2.14999706,21.1750444 C2.06092565,21.0819836 2.01120804,20.958136 2.01120804,20.8293182 L2.01120804,18.32426 C1.99400175,18.2187196 1.98505595,18.1104045 1.98505595,18 Z M6.5,14 C6.22385763,14 6,14.2238576 6,14.5 C6,14.7761424 6.22385763,15 6.5,15 L11.5,15 C11.7761424,15 12,14.7761424 12,14.5 C12,14.2238576 11.7761424,14 11.5,14 L6.5,14 Z M9.5,16 C9.22385763,16 9,16.2238576 9,16.5 C9,16.7761424 9.22385763,17 9.5,17 L11.5,17 C11.7761424,17 12,16.7761424 12,16.5 C12,16.2238576 11.7761424,16 11.5,16 L9.5,16 Z" fill="#000000" opacity="0.3" />'
        HTMLNotification += '</g>'
        HTMLNotification += '</svg>'
        HTMLNotification += '</span>'
        HTMLNotification += '</span>'
        HTMLNotification += '</div>'
        HTMLNotification += '<div class="kt-notification__item-details">';
        HTMLNotification += '<span class="kt-notification__item-title" style="font-size: 12px;color: black;font-weight: 200;">';
        HTMLNotification += Title;
        HTMLNotification += '</span>';
        HTMLNotification += '<div class="kt-notification__item-time">';
        HTMLNotification += v.notification.description
        HTMLNotification += '</div>';
        HTMLNotification += '</a>';
        HTMLNotification += '</div>';
        HTMLNotification += '<i class="fa fa-circle-notch notification-read-circle pl-31 cursor-pointer" data-container="body" data-toggle="popover" data-trigger="hover" data-placement="top" data-content="Read"></i>';
        HTMLNotification += '</div>';
    }
    return HTMLNotification;
}

function IsRead(notificationId, element) {
    
    $.ajax({
        type: 'POST',
        url: '/Admin/Notification/MarkNotificationsAsRead/?notificationId=' + notificationId,
        success: function (response) {
            if (response.success) {
                console.log(element)
                $(element).addClass('fa-circle-notch').remove('fa-circle').attr('data-content', 'Read').attr('onclick', '');
                $(element).parent().css('background-color', '#fff');
            }
            else {

            }
        }
    });
}

function AllRead(receiverId) {
    if (IsUnReadRecoredExist) {
        $.ajax({
            type: 'POST',
            url: '/Admin/Notification/NotificationsReadAll/?receiverId=' + receiverId,
            success: function (response) {
                if (response.success) {
                    $('.fa-circle').fadeTo(500, 1, function () {
                        $(this).addClass('fa-circle-notch').remove('fa-circle').attr('data-content', 'Read').attr('onclick', '');;
                    });

                    $('.kt-notification__item.un-seen').fadeTo(500, 1, function () {
                        $(this).css("backgroundColor", "#FFFFFF");
                    });

                    setTimeout(function () {
                        $('#NotificationCount').html('0 new');
                        $('#NotificationCount').fadeOut();
                        $('.notification-alert').addClass('kt-hidden');
                    }, 500);
                    IsUnReadRecoredExist = false;
                    IsUnseenRecoredExist = false;
                }
                else {
                }
            }
        });
    }
}

function IsSeen(receiverId) {
    if (IsUnseenRecoredExist) {

        $.ajax({
            type: 'POST',
            url: '/Admin/Notification/MarkNotificationsAsSeen/?receiverId=' + receiverId,
            success: function (response) {
                if (response.success) {

                    $('.kt-notification__item.un-seen').fadeTo(2000, 1, function () {
                        $(this).css("backgroundColor", "#FFFFFF");
                    });

                    setTimeout(function () {
                        $('#NotificationCount').html('0 new');
                        $('#NotificationCount').fadeOut();
                        $('.notification-alert').hide();
                    }, 3000);
                }
                else {
                }
            }
        });
    }
}

function playSound() {
    var filename = "https://cdn.fougito.com/Audios/Notification";
    var mp3Source = '<source src="' + filename + '.mp3" type="audio/mpeg">';
    var oggSource = '<source src="' + filename + '.ogg" type="audio/ogg">';
    var embedSource = '<embed hidden="true" autostart="true" loop="false" src="' + filename + '.mp3">';
    document.getElementById("sound").innerHTML = '<audio autoplay="autoplay">' + mp3Source + oggSource + embedSource + '</audio>';
}