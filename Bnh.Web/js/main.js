
var gmapsUrl = 'http://maps.google.com/maps/api/js?v=3&sensor=false';
//var gmapsUrl = 'http://maps.googleapis.com/maps/api/js?key=AIzaSyBXgUOTPfgbS4kHE7fm_xr2za_O1ApA_TM&sensor=false';

// special google maps api definition
define(
    'gmaps',
    ['async!' + gmapsUrl],
    function () {
        // return the gmaps namespace for brevity
        return window.google.maps;
    });
define(
    'gmaps-drawing',
    ['async!' + gmapsUrl + '&libraries=drawing'],
    function () { 
        // return the gmaps namespace for brevity
        return window.google.maps;
    });