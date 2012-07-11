(function (ko, $) {
    "use strict"

    function ZoneViewModel() {
        var self = this;
        self.name = ko.observable();
        self.communities = ko.observableArray();
    }

    function CommuntiyViewModel(communityDto) {
        var self = this;
        self.id = ko.observable(communityDto.Id);
        self.name = ko.observable(communityDto.Name);
        self.urlId = ko.observable(communityDto.UrlId);
        self.hasLake = ko.observable(communityDto.HasLake);
        self.distanceToCityCenter = ko.observable(communityDto.DistanceToCenter);
        self.hasWaterFeature = ko.observable(communityDto.HasWaterFeature);
        self.hasClubOrFacility = ko.observable(communityDto.HasClubOfFacility);
        self.hasMountainView = ko.observable(communityDto.HasMountainView);
        self.hasParksAndPathways = ko.observable(communityDto.HasParksAndPathways);
        self.hasShoppingPlaza = ko.observable(communityDto.HasShoppingPlaza);
        self.deleteUrl = ko.observable(communityDto.DeleteUrl);
        self.deleteTitle = ko.computed(function () {
            return "Delete " + self.name();
        });
        self.detailsUrl = ko.observable(communityDto.DetailsUrl);
        self.infoPopup = ko.observable(communityDto.InfoPopup);
        if ((communityDto.GpsBounds != null) && (communityDto.GpsBounds != 'null')) {
            self.gpsBounds = $.map($.parseJSON(communityDto.GpsBounds), Global.Map.deserializeCoordinates);
            self.associatedMapObject = Global.Map.addPolygon(self.gpsBounds, self.infoPopup());
        }
        self.gpsLocation = Global.Map.deserializeCoordinates($.parseJSON(communityDto.GpsLocation));

        self.communityMouseover = function () {
            Global.Map.highlightPolygon(self.associatedMapObject);

        };
        self.communityMouseout = function () {
            Global.Map.dehighlightPolygon(self.associatedMapObject);
        };
    }

    ko.bindingHandlers.infoPopup = {
        init: function (element, valueAccessor) {
            $(element).balloon({ position: "right", contents: valueAccessor(),
                css: {
                    borderRadius: 0,
                    backgroundColor: 'white'
                }
            });
        }
    };

    ko.bindingHandlers.slideVisible = {
        update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            // First get the latest data that we're bound to
            var value = valueAccessor();

            // Next, whether or not the supplied model property is observable, get its current value
            var valueUnwrapped = ko.utils.unwrapObservable(value);

            element = $(element);

            // Now manipulate the DOM element
            if (valueUnwrapped == true)
                element.show(); // Make the element visible
            else
                element.hide(); // Make the element invisible
            if (viewModel.associatedMapObject != null) {
                viewModel.associatedMapObject.setVisible(valueUnwrapped);
            }
        }
    };

    function CommunityFilterViewModel(config) {
        var self = this;
        self.remoteness = ko.observable();
        self.hasLake = ko.observable(false);
        self.hasWaterFeature = ko.observable(false);
        self.hasClubOrFacility = ko.observable(false);
        self.hasMountainView = ko.observable(false);
        self.hasParksAndPathways = ko.observable(false);
        self.hasShoppingPlaza = ko.observable(false);

        this.isCommunityVisible = function (cp) {
            var visible =
             (!(self.remoteness()) || self.remoteness() >= cp.distanceToCityCenter()) &&
                 (!(self.hasLake()) || self.hasLake() === cp.hasLake()) &&
                     (!(self.hasWaterFeature()) || self.hasWaterFeature() === cp.hasWaterFeature()) &&
                         (!(self.hasClubOrFacility()) || self.hasClubOrFacility() === cp.hasClubOrFacility()) &&
                             (!(self.hasMountainView()) || self.hasMountainView() === cp.hasMountainView()) &&
                                 (!(self.hasParksAndPathways()) || self.hasParksAndPathways() === cp.hasParksAndPathways()) &&
                                     (!(self.hasShoppingPlaza()) || self.hasShoppingPlaza() === cp.hasShoppingPlaza()) &&
                                         true;
            return visible;
        };
    }

    function CommunityPageViewModel() {
        var self = this;
        self.zones = ko.observableArray();
        self.filter = ko.observable(new CommunityFilterViewModel());
        self.initialize = function () {
            $.getJSON("/Community/Zones", function (data) {
                var mappedZones = $.map(data, function (item) {
                    var zone = new ZoneViewModel();
                    zone.name(item.Name);
                    zone.communities($.map(item.Communities, function (item) {
                        var community = new CommuntiyViewModel(item);
                        return community;
                    }));
                    return zone;
                });
                self.zones(mappedZones);
            });
        };
        self.initialize();
    }



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
})(ko, jQuery);