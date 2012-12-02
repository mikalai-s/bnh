define(
    ["jquery", "map"],
    function ($, Map) {
        "use strict";

        $(".map-canvas-wrapper").each(function (i, element) {
            var elem = $(element);

            var gpsLocationAttr = elem.attr("gpsLocation");
            var gpsBoundsAttr = elem.attr("gpsBounds");

            var gpsLocation = gpsLocationAttr ? $.parseJSON(gpsLocationAttr) : null;
            var gpsBounds = gpsBoundsAttr ? $.parseJSON(gpsBoundsAttr) : null;

            var map = new Map(elem.find("#mapCanvas"), {
                zoom: elem.attr("zoom") * 1,
                center: gpsLocation
            });

            if (gpsBounds) {
                map.addPolygon($.map(gpsBounds, map.deserializeCoordinates));
            }
        });
    }
);