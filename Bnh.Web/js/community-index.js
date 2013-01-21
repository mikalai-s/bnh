define( 
    ["knockout", "jquery", "map", "twitteroverrides"],
    function (ko, $, Map) {
        "use strict";

        var map;

        function CommunityViewModel(community, pageViewModel) {
            var associatedMapObject;

            if ((community.GpsBounds != null) && (community.GpsBounds != 'null')) {
                this.gpsBounds = $.map(JSON.parse(community.GpsBounds), map.deserializeCoordinates);
                associatedMapObject = map.addPolygon(this.gpsBounds, community.PopupHtml);
            }

            this.gpsLocation = map.deserializeCoordinates(JSON.parse(community.GpsLocation));

            this.visible = ko.computed(function() {
                var visible = pageViewModel.filter.isVisible(community.UrlId);

                if (associatedMapObject) {
                    associatedMapObject.setVisible(visible);
                }

                return visible;

            }, this);
        }

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

                visible = visible && (!value || eval(property.Comparer)(value, cp.Properties[property.Name]));
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
            this.filter = new CommunityFilterViewModel();
            this.slide = ko.observable(0);
            this.communities = {};

            for (var urlId in page_communityData) {
                if (page_communityData.hasOwnProperty(urlId)) {
                    this.communities[urlId] = new CommunityViewModel(page_communityData[urlId], this);
                }
            }
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
        $('.community a').popover({
            animation: true,
            placement: "right",
            html: true,
            trigger: 'hover',
            template: '<div class="popover"><div class="arrow"></div><div class="popover-inner"><h3 class="popover-title" style="display: none"></h3><div class="popover-content"><p></p></div></div></div>',
            delay: {
                show: 300,
                hide: 100
            }
        });

    }
);