﻿define(
    ["jquery", "map-editor", "jqjson"],
    function ($, MapEditor) {
        "use strict"

        var gpsLocationField = $("#GpsLocation");
        var gpsBoundsField = $("#GpsBounds");

        var overlays = {
            marker: gpsLocationField.val() ? $.parseJSON(gpsLocationField.val()) : null,
            polygon: gpsBoundsField.val() ? $.parseJSON(gpsBoundsField.val()) : null,
            zoom: 14
        };

        var mapEditor = new MapEditor("#mapCanvas", overlays);

        // handle onsubmit event to gather gps coordiates from map
        $("input:submit, button:submit").click(function () {
            gpsLocationField.val($.toJSON(mapEditor.getLocation()));
            gpsBoundsField.val($.toJSON(mapEditor.getBounds()));
        });
    }
);