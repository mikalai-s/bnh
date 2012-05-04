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

    function onCommunityClicked(name) {
        var href = $(".community:contains(" + name + ")").attr("href");
        if (href) {
            window.location.href = href;
        }
    }

    ko.bindingHandlers.slideVisible = {
        update: function (element, valueAccessor, allBindingsAccessor) {
            // First get the latest data that we're bound to
            var value = valueAccessor(), allBindings = allBindingsAccessor();

            // Next, whether or not the supplied model property is observable, get its current value
            var valueUnwrapped = ko.utils.unwrapObservable(value);

            element = $(element);

            // Now manipulate the DOM element
            if (valueUnwrapped == true)
                element.show(); // Make the element visible
            else
                element.hide();   // Make the element invisible

            map.setVisible(element.find(".community").text(), valueUnwrapped);
        }
    };

    var filterViewModel = new CommunityFilterViewModel();
    // filterViewModel

    ko.applyBindings(filterViewModel);
})();