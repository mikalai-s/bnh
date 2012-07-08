(function () {
    "use strict"

    var filterLegend = $("#filterLegend");
    var filterParameters = $("#filterParameters");

    Global.Map = new Map("#mapCanvas", {
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