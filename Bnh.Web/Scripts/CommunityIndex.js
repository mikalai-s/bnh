(function () {
    "use strict"

    var overlays = [];

    var communities = $(".community");
    var filterLegend = $("#filterLegend");
    var filterParameters = $("#filterParameters");
    var filters = $(".filter");


    communities.each(function (i, elem) {
        elem = $(elem);
        var gpsLocationAttr = elem.attr("gpsLocation");
        var gpsBoundsAttr = elem.attr("gpsBounds");

        var gpsLocation = gpsLocationAttr ? $.parseJSON(gpsLocationAttr) : null;
        var gpsBounds = gpsBoundsAttr ? $.parseJSON(gpsBoundsAttr) : null;

        if (gpsLocation || gpsBounds) {
            overlays.push({
                name: elem.text(),
                center: gpsLocation,
                polygons: gpsBounds
            });
        }
    });

    var map = new Global.Map("#mapCanvas", {
        overlays: overlays,
        zoom: 11,
        center: {
            lat: 51.02844,
            lng: -114.071045
        },
        polygonClick: onCommunityClicked
    });

    communities.on("mousemove", function (e) {
        map.highlight($(e.srcElement).text());
    });

    communities.on("mouseout", function (e) {
        map.dehighlight($(e.srcElement).text());
    });

    filterLegend.click(function () {
        if (filterParameters.is(":visible")) {
            filterParameters.hide("fast");
            filterLegend.text("[show]");
        } else {
            filterParameters.show("fast");
            filterLegend.text("[hide]");
        }
    });

    //    filters.change(function () {

    //    });

    function onCommunityClicked(name) {
        var href = $(".community:contains(" + name + ")").attr("href");
        if (href) {
            window.location.href = href;
        }
    }

    ko.applyBindings(new communityFilterViewModel());
})();