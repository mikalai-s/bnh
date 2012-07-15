(function () {

    var elem = $(".map-canvas-wrapper");
    var gpsLocationAttr = elem.attr("gpsLocation");
    var gpsBoundsAttr = elem.attr("gpsBounds");

    var gpsLocation = gpsLocationAttr ? $.parseJSON(gpsLocationAttr) : null;
    var gpsBounds = gpsBoundsAttr ? $.parseJSON(gpsBoundsAttr) : null;

    var map = new Global.Map("#mapCanvas", {
        zoom: elem.attr("zoom") * 1,
        center: gpsLocation
    });

    if (gpsBounds) {
        map.addPolygon($.map(gpsBounds, map.deserializeCoordinates));
    }
})();