var Global = Global || { };

function Map(mapCanvas, options) {

        var self = this;
        self.deserializeCoordinates = function(coord) {
                if (!coord || !coord.lat || !coord.lng)
                    return;

                return new google.maps.LatLng(coord.lat, coord.lng);
            };
        var myLatLng = self.deserializeCoordinates(options.center);
        var myOptions = {
            zoom: options.zoom,
            center: myLatLng,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        var $mapCanvas = $(mapCanvas);
        if($mapCanvas.length === 0) {
            return;
        }

        self.mapInstance = new google.maps.Map($mapCanvas[0], myOptions);
        self.highlightPolygon = function(polygon) {
            if (!polygon)
                return;
            polygon.setOptions({
                strokeWeight: 2,
                fillOpacity: 0.75
            });
        };
        self.dehighlightPolygon = function(polygon) {
            if (!polygon)
                return;
            polygon.setOptions({
                strokeWeight: 1,
                fillOpacity: 0.35
            });
        };
        self.addPolygon = function(coords, infoPopUpContent) {
            var polygon = new google.maps.Polygon({
                paths: coords,
                strokeColor: "#FF3333",
                strokeOpacity: 0.8,
                strokeWeight: 2,
                fillColor: "#FFaaaa",
                fillOpacity: 0.35,
                map: self.mapInstance
            });

            var infoPopUp = new google.maps.InfoWindow({
                content : infoPopUpContent
            });

            google.maps.event.addListener(polygon, 'click', function(event) {
                infoPopUp.setPosition(event.latLng);
                infoPopUp.open(self.mapInstance);
            });
            google.maps.event.addListener(polygon, 'mouseover', function(event) {   
                self.highlightPolygon(polygon);   
            });

            google.maps.event.addListener(polygon, 'mouseout', function(event) {
                self.dehighlightPolygon(polygon);
            });

            return polygon;
        };
        
    };
