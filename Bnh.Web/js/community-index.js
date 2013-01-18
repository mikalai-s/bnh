define(
    ["knockout", "jquery", "map"],
    function (ko, $, Map) {
        "use strict";

        var map;

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

        function CommunityFilterViewModel(config) {

            this.properties = {};

            for(var i = 0; i < page_filterProperties.length; i += 1) {
                var property = page_filterProperties[i];
                this.properties[property.Name] = ko.observable(property.DefaultValue);
            }

            this.dirty = ko.dirtyFlag(this.properties);
        }

        CommunityFilterViewModel.prototype.isVisible = function (communityUrlId) {
            var cp = page_communityData[communityUrlId];
            var self = this.properties;
            var visible = true;
            for (var i = 0; i < page_filterProperties.length; i += 1) {
                var property = page_filterProperties[i];
                var value = this.properties[property.Name]();

                visible = visible && (!value || eval(property.Comparer)(value, cp[property.Name]));
            }

            return visible;
        };

        //-------------------------------------------
        // Implementation of dirty flag inspired by
        // http://www.knockmeout.net/2011/05/creating-smart-dirty-flag-in-knockoutjs.html
        ko.dirtyFlag = function createDirtyFlag(root, isSuspended) {
            var originalJSON, currentJSON, isDirty,
                resumeResetRequested = false,
                isSuspendedObservable = ko.observable(!!isSuspended);

            // subscribe to all observables in root (recursively),
            // which causes this dependent observable to re-evaluate
            // every time any observable in "root" changes
            currentJSON = ko.computed(function () {
                if (isSuspendedObservable()) {
                    return null; // while dirtyFlag is suspended, only subscribe to isSuspendedObservable changes
                }
                var rootJson = ko.toJSON(root); // subscribe to every observable in the model
                if (resumeResetRequested) {
                    resumeResetRequested = false;
                    originalJSON(rootJson); // when this computation function finishes, dirtyTest function will run (subcribed to currentJSON); it will set isDirty to false (because both original and current JSON now point to the same value)
                }
                return rootJson;
            });

            // take snapshot of "root" at creation time
            // (can be updated with "reset" function, see below)
            originalJSON = ko.observable(currentJSON());

            // initialize isDirty observable
            isDirty = ko.observable(false);
            isDirty.originalJSON = originalJSON;
            isDirty.currentJSON = currentJSON;
            isDirty.reset = function () {
                isDirty.resume(true);
            };
            isDirty.suspend = function () {
                isSuspendedObservable(true);
            };
            isDirty.resume = function (resetOnResume) {
                if (isSuspendedObservable()) {
                    resumeResetRequested = resetOnResume;
                    isSuspendedObservable(false);
                } else if (resetOnResume) {
                    originalJSON(currentJSON());
                    isDirty(false);
                }
            };

            // recompute dirty flag each time currentJSON changes
            currentJSON.subscribe(function dirtyTest() {
                if (!isSuspendedObservable()) { // only update isDirty if not suspended
                    isDirty(currentJSON() !== originalJSON());
                }
            });

            return isDirty;
        };

        function CommunityPageViewModel() {
            var self = this;
            //self.zones = ko.observableArray([]);
            self.filter = new CommunityFilterViewModel();
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

        }

        CommunityPageViewModel.prototype.onToggleSlide = function () {
            this.slide(this.slide() === 0 ? 1 : 0);
        };

        map = new Map($("#mapCanvas"), {
            zoom: 11,
            center: {
                lat: 51.02844,
                lng: -114.071045
            }
        });

        ko.applyBindings((window.vm = new CommunityPageViewModel()));


        // setup search popup
        $('.community').popover({
            animation: true,
            placement: "right",
            html: true,
            content: function () { return $(this)[0].innerHTML; },
            trigger: 'hover',
            template: '<div class="popover"><div class="arrow"></div><div class="popover-inner"><h3 class="popover-title" style="display:none"></h3><div class="popover-content"><p></p></div></div></div>',
            delay: {
                show: 300,
                hide: 200
            }
        });

    }
);