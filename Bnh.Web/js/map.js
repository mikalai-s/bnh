define(
    ["gmaps"],
    function () {
        "use strict"

        function ResetMapButton(controlDiv, map) {
            controlDiv.style.padding = '5px';

            // Set CSS for the control border.
            var controlUI = document.createElement('div');
            controlUI.style.backgroundColor = 'white';
            controlUI.style.borderStyle = 'solid';
            controlUI.style.borderWidth = '1px';
            controlUI.style.cursor = 'pointer';
            controlUI.style.textAlign = 'center';
            controlUI.title = 'Click to reset map state';
            controlDiv.appendChild(controlUI);

            // Set CSS for the control interior.
            var controlText = document.createElement('div');
            controlText.style.fontFamily = 'Arial,sans-serif';
            controlText.style.fontSize = '13px';
            controlText.style.padding = '1px 6px';
            controlText.innerHTML = 'Reset Map';
            controlUI.appendChild(controlText);

            // Setup the click event listeners: simply set the map to Chicago.
            google.maps.event.addDomListener(controlUI, 'click', function () {
                map.mapInstance.setCenter(map.mapOptions.center);
                map.mapInstance.setZoom(map.mapOptions.zoom);
                map.closeInfoPopUp();
            });
        }

        function Map($mapCanvas, options) {

            var self = this,
                mapCanvas = $mapCanvas.get(0);
            self.deserializeCoordinates = function (coord) {
                if (!coord || !coord.lat || !coord.lng)
                    return;

                return new google.maps.LatLng(coord.lat, coord.lng);
            };
            var myLatLng = self.deserializeCoordinates(options.center);
            self.mapOptions = {
                zoom: options.zoom || 11,
                center: myLatLng,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            if (!mapCanvas) {
                return;
            }
            self.mapInstance = new google.maps.Map(mapCanvas, self.mapOptions);
            self.highlightPolygon = function (polygon) {
                if (!polygon)
                    return;
                polygon.setOptions({
                    strokeWeight: 2,
                    fillOpacity: 0.75
                });
            };
            self.dehighlightPolygon = function (polygon) {
                if (!polygon)
                    return;
                polygon.setOptions({
                    strokeWeight: 1,
                    fillOpacity: 0.35
                });
            };
            var infoPopUp = new google.maps.InfoWindow();
            self.showInfoPopUp = function (coordinates, content) {
                infoPopUp.setContent(content);
                infoPopUp.setPosition(coordinates);
                infoPopUp.open(self.mapInstance);
            };
            self.closeInfoPopUp = function () {
                infoPopUp.close();
            };
            self.addPolygon = function (coords, infoPopUpContent) {
                var polygon = new google.maps.Polygon({
                    paths: coords,
                    strokeColor: "#FF3333",
                    strokeOpacity: 0.8,
                    strokeWeight: 1,
                    fillColor: "#FFaaaa",
                    fillOpacity: 0.35,
                    map: self.mapInstance
                });

                if (infoPopUpContent) {
                    google.maps.event.addListener(polygon, 'click', function (event) {
                        self.showInfoPopUp(event.latLng, infoPopUpContent);
                    });
                    google.maps.event.addListener(polygon, 'mouseover', function (event) {
                        self.highlightPolygon(polygon);
                    });

                    google.maps.event.addListener(polygon, 'mouseout', function (event) {
                        self.dehighlightPolygon(polygon);
                    });
                }

                return polygon;
            };
            self.initializeCustomControls = function () {
                var restoreControlDiv = document.createElement('div');
                var restoreControl = new ResetMapButton(restoreControlDiv, self);
                restoreControlDiv.index = 1;
                self.mapInstance.controls[google.maps.ControlPosition.TOP_CENTER].push(restoreControlDiv);
            };
            self.initializeCustomControls();

            // check whether there is need to process initialiiy hidden map
            if ($mapCanvas.parent().hasClass("initially-hidden-map")) {
                google.maps.event.addListenerOnce(self.mapInstance, 'idle', function () {
                    // when map is loaded - attach it to its container (because in markup they are separated)
                    var $container = $("#" + $mapCanvas.data("mapcontainer"));
                    if ($container.length) {
                        $mapCanvas.appendTo($container);
                    }
                });
            }
        };

        return Map;

    }
);