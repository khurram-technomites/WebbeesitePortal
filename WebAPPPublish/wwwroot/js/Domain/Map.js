var latitude;
var longitude;
var IsMapLoaded = false;
var MapAddress;
var IsGetCurrentLocation = false;
var DefaultMapMarker = "/images/icons/marker.png";

var lang = 'en';

function SetCurrentLocation(lat, lon, adr) {
    var currentLocation = {
        latitude: lat,
        longitude: lon,
        address: adr,
    }
    localStorage.setItem("currentLocation", JSON.stringify(currentLocation));
}

function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition);
        IsGetCurrentLocation = true;
    } else {
        alert("Geolocation is not supported by this browser.");

        latitude = 25.2048
        longitude = 55.2708
        InsertPosition(latitude, longitude);
        InsertLatLonInInput(latitude, longitude);
    }
}

function showPosition(position) {

    latitude = position.coords.latitude;
    longitude = position.coords.longitude;

    InsertLatLonInInput(latitude, longitude);
    InsertPosition(latitude, longitude);

}

function InsertPosition(latitude, longitude) {
    $.ajax({
        type: 'Get',
        url: `https://maps.googleapis.com/maps/api/geocode/json?latlng=${latitude},${longitude}&key=AIzaSyD96pSIJVVnH9RDyy-n2G3uu3FG8Ze9xyc`,
        success: function (response) {
            if (response.results.length > 0) {

                MapAddress = response.results[0].formatted_address;

                InsertMapAddressInInput(MapAddress);

                if (IsGetCurrentLocation) {
                    SetCurrentLocation(latitude, longitude, MapAddress);
                    IsGetCurrentLocation = false;
                }

            } else {
                alert("Geolocation is not supported by this browser.");
            }
        }
    });
}

function InsertLatLonInInput(latitude, longitude) {
    $('#Latitude').val(latitude);
    $('#Longitude').val(longitude);
}

function InsertMapAddressInInput(MapAddress) {
    $('#Address').val(MapAddress);
    $('#current-location').val(MapAddress);
}


function myMap() {

    if (!latitude) { latitude = 25.2048 }
    if (!longitude) { longitude = 55.2708 }
    ;
    var map = new google.maps.Map(document.getElementById('googleMap'), {
        zoom: 15,
        center: new google.maps.LatLng(latitude, longitude),
        mapTypeId: google.maps.MapTypeId.ROADMAP
    });

    var myMarker = new google.maps.Marker({
        position: new google.maps.LatLng(latitude, longitude),
        draggable: true,
        icon: DefaultMapMarker,
    });

    google.maps.event.addListener(myMarker, 'dragend', function (evt) {
        document.getElementById('drag-map').innerHTML = '<span>' + ChangeString('Drag marker on the map to select your desired location.', 'اسحب علامة على الخريطة لتحديد الموقع المطلوب.') + '</span>';

        latitude = evt.latLng.lat().toFixed(3);
        longitude = evt.latLng.lng().toFixed(3);

        InsertLatLonInInput(latitude, longitude);
        InsertPosition(latitude, longitude);

    });
    google.maps.event.addListener(myMarker, 'dragstart', function (evt) {
        document.getElementById('drag-map').innerHTML = '<span>' + ChangeString('Currently dragging marker ...', 'جارٍ سحب العلامة حاليًا ...') + '</span>';
    });
    map.setCenter(myMarker.position);
    myMarker.setMap(map);
}

function initAutocomplete() {

    var defaultBounds = new google.maps.LatLngBounds();
    var options = {
        bounds: defaultBounds,
        components: 'country: AE',
        //fields: ["address_components", "geometry", "icon", "name"],
        //strictBounds: true,
    };

    var inputs = document.getElementsByClassName('Address');

    var autocompletes = [];
    
    for (var i = 0; i < inputs.length; i++) {
        var autocomplete = new google.maps.places.Autocomplete(inputs[i], options);
        //autocomplete.setComponentRestrictions({
        //    country: ["us", "pr", "vi", "gu", "mp"],
        //});
        
        autocomplete.inputId = inputs[i].id;
        autocomplete.addListener('place_changed', fillIn);
        autocompletes.push(autocomplete);
    }

    function fillIn() {

        var place = this.getPlace();


        latitude = place.geometry.location.lat();
        longitude = place.geometry.location.lng();

        InsertLatLonInInput(latitude, longitude);

        myMap();

        MapAddress = place.formatted_address;

        InsertMapAddressInInput(MapAddress);

    }    

    myMap();
}

function openMap() {


    if (!IsMapLoaded) {

        if (!latitude) {

            getLocation();
        }
        setTimeout(function () {
            myMap();
        }, 1000)
        IsMapLoaded = true;
    }
    else {
        myMap();
        InsertMapAddressInInput(MapAddress)
    }
}

function ChangeString(en_text, ar_text) {
    return lang == 'en' ? en_text : ar_text;
};