require(['goog!maps,2,sensor:false', 'debug'], function () {
    debug.log('in maps');

    var map = new google.maps.Map2(document.getElementById("map"));
    map.setCenter(new google.maps.LatLng(37.4419, -122.1419), 13);
})