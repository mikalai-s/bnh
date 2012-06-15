(function () {

    var elem = $(".map-canvas-wrapper");
    var gpsLocationAttr = elem.attr("gpsLocation");
    var gpsBoundsAttr = elem.attr("gpsBounds");

    var gpsLocation = gpsLocationAttr ? $.parseJSON(gpsLocationAttr) : null;
    var gpsBounds = gpsBoundsAttr ? $.parseJSON(gpsBoundsAttr) : null;

    var overlays = [];

    if (gpsLocation || gpsBounds) {
        overlays.push({
            name: "community",
            center: gpsLocation,
            polygons: gpsBounds
        });
    }

    new Global.Map("#mapCanvas", {
        overlays: overlays,
        zoom: elem.attr("zoom") * 1,
        center: gpsLocation
    });
})();