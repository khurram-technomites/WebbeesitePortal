var PageNo = 1;
var MoreRecordExist = true;
var IsUnseenRecoredExist = false;
var IsUnReadRecoredExist = false;
var UnSeenMessages = 0;

jQuery(document).ready(function () {
    $.ajax({
        type: 'Get',
        url: '/Notifications/LoadNotifications?pageNo=' + PageNo,
        success: function (data) {
            if (data.success) {
                BindNotification(data);
            }
        }
    });

    jQuery(function ($) {
        $("#ulNotification").on('scroll', function () {
            if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
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
                            url: '/Notifications/LoadNotifications?pageNo=' + PageNo,
                            success: function (response) {
                                LoadMoreNotification(response);
                            }
                        });
                    }
                }
            }
        })
    });

});

function BindNotification(response) {

    var DivNotify = '';

    if (response.data.length > 0) {
        if (response.data.length < 5) {
            MoreRecordExist = false;
        }

        UnSeenMessages = response.data.filter(function (obj) {
            return obj.IsSeen == false;
        }).length;

        if (UnSeenMessages > 0) {
            $('#NotificationCount').html(UnSeenMessages + ' new');
            $('#NotificationCount').fadeIn();

            $('.notification-alert').removeClass('kt-hidden');
            playSound();
            IsUnseenRecoredExist = true;
        }

        IsUnReadRecoredExist = response.data.filter(function (obj) {
            return obj.IsRead == false;
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
        url: '/Notifications/Notifications/',
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

        $(DivNotify).prependTo("#ulNotification");
        $('.notify-empty-template').remove();
        $('[data-toggle="popover"]').popover();
    }
}

function getHtmlizeNotification(v) {
    var HTMLNotification = '';
    if (!v.IsRead) {
        //UnRead Messages
        HTMLNotification += '<div class="kt-notification__item ' + (v.IsSeen ? '' : 'un-seen') + '">';
        HTMLNotification += '<div class="kt-notification__item-icon">';
        HTMLNotification += '<i class="fa fa-bell"></i>';
        HTMLNotification += '</div>';
        HTMLNotification += '<div class="kt-notification__item-details">';
        HTMLNotification += '<a href="' + v.Link + '"  class="kt-notification__item-title"style="font-size: 12px;color: black;font-weight: 600;">';
        HTMLNotification += v.Title;
        HTMLNotification += '</a>';
        HTMLNotification += '<div class="kt-notification__item-time">';
        HTMLNotification += '<i class="flaticon-avatar"></i> ' + v.SenderName + ' <span class="float-right" style="color: #13135f;font-weight: 600;"><i class="flaticon-calendar-with-a-clock-time-tools"></i> ' + v.Date + '</span>';
        HTMLNotification += '</div>';
        HTMLNotification += '</div>';
        HTMLNotification += '<i class="fa fa-circle notification-read-circle" onclick="IsRead(' + v.NotificationReceiverId + ',this);" data-container="body" data-toggle="popover" data-trigger="hover" data-placement="top" data-content="mark as read"></i>';
        HTMLNotification += '</div>';
    } else {
        HTMLNotification += '<div class="kt-notification__item kt-notification__item--read">';
        HTMLNotification += '<div class="kt-notification__item-icon">';
        HTMLNotification += '<i class="fa fa-bell"></i>';
        HTMLNotification += '</div>';
        HTMLNotification += '<div class="kt-notification__item-details">';
        HTMLNotification += '<a href="' + v.Link + '"  class="kt-notification__item-title"style="font-size: 12px;color: black;font-weight: 600;">';
        HTMLNotification += v.Title;
        HTMLNotification += '</a>';
        HTMLNotification += '<div class="kt-notification__item-time">';
        HTMLNotification += '<i class="flaticon-avatar"></i> ' + v.SenderName + ' <span class="float-right" style="color: #13135f;font-weight: 600;"> <i class="flaticon-calendar-with-a-clock-time-tools"></i> ' + v.Date + '</span>';
        HTMLNotification += '</div>';
        HTMLNotification += '</div>';
        HTMLNotification += '<i class="fa fa-circle-notch notification-read-circle" data-container="body" data-toggle="popover" data-trigger="hover" data-placement="top" data-content="Read"></i>';
        HTMLNotification += '</div>';
    }
    return HTMLNotification;
}

function IsRead(notificationId, element) {
    $.ajax({
        type: 'POST',
        url: '/Notifications/NotificationsIsRead/?notificationId=' + notificationId,
        success: function (response) {
            if (response.success) {
                $(element).addClass('fa-circle-notch').remove('fa-circle').attr('data-content', 'Read').attr('onclick', '');
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
            url: '/Notifications/NotificationsReadAll/?receiverId=' + receiverId,
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
            url: '/Notifications/NotificationsIsSeen/?receiverId=' + receiverId,
            success: function (response) {
                if (response.success) {

                    $('.kt-notification__item.un-seen').fadeTo(2000, 1, function () {
                        $(this).css("backgroundColor", "#FFFFFF");
                    });

                    setTimeout(function () {
                        $('#NotificationCount').html('0 new');
                        $('#NotificationCount').fadeOut();
                        $('.notification-alert').addClass('kt-hidden');
                    }, 3000);
                }
                else {
                }
            }
        });
    }
}

function playSound() {
    var filename = "../Assets/Audio/Notification";
    var mp3Source = '<source src="' + filename + '.mp3" type="audio/mpeg">';
    var oggSource = '<source src="' + filename + '.ogg" type="audio/ogg">';
    var embedSource = '<embed hidden="true" autostart="true" loop="false" src="' + filename + '.mp3">';
    document.getElementById("sound").innerHTML = '<audio autoplay="autoplay">' + mp3Source + oggSource + embedSource + '</audio>';
}