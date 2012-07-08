(function () {
    "use strict"

    var overlays = [];

    var filterLegend = $("#filterLegend");
    var filterParameters = $("#filterParameters");

    Global.Map = new Map("#mapCanvas", {
        overlays: overlays,
        zoom: 11,
        center: {
            lat: 51.02844,
            lng: -114.071045
        }
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

    ko.applyBindings(new CommunityPageViewModel());
})();