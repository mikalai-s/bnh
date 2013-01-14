define(
    ["knockout", "jquery", "map"],
    function (ko, $, Map) {
        "use strict";

        //var map,
        //    filterLegend = $("#filterLegend"),
        //    filterParameters = $("#filterParameters");

        //function ZoneViewModel(name, communities) {
        //    var self = this;
        //    self.name = ko.observable(name || "");
        //    self.communities = ko.observableArray(communities || []);
        //}

        //function CommuntiyViewModel(community, uiHelpers) {
        //    var self = this;
        //    self.id = ko.observable(community.Id);
        //    self.name = ko.observable(community.Name);
        //    self.urlId = ko.observable(community.UrlId);
        //    self.hasLake = ko.observable(community.HasLake);
        //    self.distanceToCityCenter = ko.observable(community.DistanceToCenter);
        //    self.hasWaterFeature = ko.observable(community.HasWaterFeature);
        //    self.hasClubOrFacility = ko.observable(community.HasClubOfFacility);
        //    self.hasMountainView = ko.observable(community.HasMountainView);
        //    self.hasParksAndPathways = ko.observable(community.HasParksAndPathways);
        //    self.hasShoppingPlaza = ko.observable(community.HasShoppingPlaza);
        //    self.deleteUrl = ko.observable(uiHelpers.deleteUrl);
        //    self.deleteTitle = ko.computed(function () {
        //        return "Delete " + self.name();
        //    });
        //    self.detailsUrl = ko.observable(uiHelpers.detailsUrl);
        //    self.infoPopup = ko.observable(uiHelpers.infoPopup);
        //    if ((community.GpsBounds != null) && (community.GpsBounds != 'null')) {
        //        self.gpsBounds = $.map(JSON.parse(community.GpsBounds), map.deserializeCoordinates);
        //        self.associatedMapObject = map.addPolygon(self.gpsBounds, self.infoPopup());
        //    }
        //    self.gpsLocation = map.deserializeCoordinates(JSON.parse(community.GpsLocation));

        //    self.communityMouseover = function () {
        //        map.highlightPolygon(self.associatedMapObject);

        //    };
        //    self.communityMouseout = function () {
        //        map.dehighlightPolygon(self.associatedMapObject);
        //    };
        //}

        //ko.bindingHandlers.infoPopup = {
        //    init: function (element, valueAccessor) {
        //        $(element).balloon({
        //            position: "right",
        //            contents: valueAccessor(),
        //            delay: 200,
        //            minLifetime: 0,
        //            css: {
        //                borderRadius: 0,
        //                backgroundColor: 'white'
        //            }
        //        });
        //    }
        //};

        ko.bindingHandlers.slider = {
            update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
                // First get the latest data that we're bound to
                var value = valueAccessor(),
                    $slider = $(element).children().first(), // requires inner container div
                    $slides = $slider.children(),
                    valueUnwrapped = ko.utils.unwrapObservable(value),
                    offset;

                if (isNaN(valueUnwrapped)) {
                    throw new Error("Slide index in NaN");
                }

                if (valueUnwrapped + 1 > $slides.length) {
                } else {
                    offset = valueUnwrapped * -($slides.first().width());
                    $slider.animate({ "left": offset + "px" }, 600);
                }

                //if (viewModel.associatedMapObject != null) {
                //    viewModel.associatedMapObject.setVisible(valueUnwrapped);
                //}
            }
        };

        //function CommunityFilterViewModel(config) {
        //    var self = this;
        //    self.remoteness = ko.observable();
        //    self.hasLake = ko.observable(false);
        //    self.hasWaterFeature = ko.observable(false);
        //    self.hasClubOrFacility = ko.observable(false);
        //    self.hasMountainView = ko.observable(false);
        //    self.hasParksAndPathways = ko.observable(false);
        //    self.hasShoppingPlaza = ko.observable(false);

        //    this.isCommunityVisible = function (cp) {
        //        var visible =
        //         (!(self.remoteness()) || self.remoteness() >= cp.distanceToCityCenter()) &&
        //             (!(self.hasLake()) || self.hasLake() === cp.hasLake()) &&
        //                 (!(self.hasWaterFeature()) || self.hasWaterFeature() === cp.hasWaterFeature()) &&
        //                     (!(self.hasClubOrFacility()) || self.hasClubOrFacility() === cp.hasClubOrFacility()) &&
        //                         (!(self.hasMountainView()) || self.hasMountainView() === cp.hasMountainView()) &&
        //                             (!(self.hasParksAndPathways()) || self.hasParksAndPathways() === cp.hasParksAndPathways()) &&
        //                                 (!(self.hasShoppingPlaza()) || self.hasShoppingPlaza() === cp.hasShoppingPlaza()) &&
        //                                     true;
        //        return visible;
        //    };
        //}

        function CommunityPageViewModel() {
            var self = this;
            //self.zones = ko.observableArray([]);
            //self.filter = ko.observable(new CommunityFilterViewModel());
            self.slide = ko.observable(0);
            //self.initialize = function () {
            //    $.getJSON("/Community/Zones", function (data) {
            //        var zones = $.map(data, function (communities, zone) {
            //            var communities = $.map(communities, function (item) {
            //                return new CommuntiyViewModel(item.community, item.uiHelpers);
            //            });
            //            return new ZoneViewModel(zone, communities);
            //        });
            //        self.zones(zones);
            //    });
            //};
            //self.initialize();

            self.onToggleSlide = function () {
                this.slide(this.slide() === 0 ? 1 : 0);
            };
        }

        //filterLegend.click(function () {
        //    if (filterParameters.is(":visible")) {
        //        filterParameters.hide("fast");
        //        filterLegend.text("[show]");
        //    } else {
        //        filterParameters.show("fast");
        //        filterLegend.text("[hide]");
        //    }
        //});

        var map = new Map($("#mapCanvas"), {
            zoom: 11,
            center: {
                lat: 51.02844,
                lng: -114.071045
            }
        });

        ko.applyBindings((window.vm = new CommunityPageViewModel()));
    }
);