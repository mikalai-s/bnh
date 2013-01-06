define(
    ["jquery", "map-editor", "json"],
    function ($, MapEditor) {
        "use strict"

        var gpsLocationField = $("#GpsLocation");
        var gpsBoundsField = $("#GpsBounds");

        var overlays = {
            marker: gpsLocationField.val() ? JSON.parse(gpsLocationField.val()) : null,
            polygon: gpsBoundsField.val() ? JSON.parse(gpsBoundsField.val()) : null,
            zoom: 14
        };

        var mapEditor = new MapEditor("#mapCanvas", overlays);

        // handle onsubmit event to gather gps coordiates from map
        $("input:submit, button:submit").click(function () {
            gpsLocationField.val(JSON.stringify(mapEditor.getLocation()));
            gpsBoundsField.val(JSON.stringify(mapEditor.getBounds()));
        });
    }
);