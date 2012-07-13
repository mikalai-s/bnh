var Global = Global || { };

(function () {
    "use strict"


    Global.MapEditor = Global.MapEditor || function(mapCanvas, overlays) {
        var currentValues = {
            marker: null,
            polygon: null
        };

        var center = overlays.marker ? 
            new deserializeCoord(overlays.marker) : 
            new google.maps.LatLng(51.02844, -114.071045);  // Calgary by default

        // create map object
        var options = {
            zoom: overlays.zoom ? overlays.zoom : 11,
            center: center, 
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        var map = new google.maps.Map($(mapCanvas)[0], options);

        // add existing overlays
        if(overlays.marker) {
            currentValues.marker = new google.maps.Marker({
                position: center,
                icon: new google.maps.MarkerImage("http://google.com/mapfiles/ms/micons/orange.png"),
                draggable: true,
                map: map
            });
        }
        if(overlays.polygon) {
            var coords = [];
            for(var i = 0; i < overlays.polygon.length; i ++) {
                coords.push(deserializeCoord(overlays.polygon[i]))
            }
            currentValues.polygon = new google.maps.Polygon({
                paths: coords,
                strokeColor: "#FF3333",
                strokeOpacity: 0.8,
                strokeWeight: 2,
                fillColor: "#FFaaaa",
                fillOpacity: 0.35,
                editable: true,
                map: map
            });
            addDeleteButton(currentValues.polygon, "http://i.imgur.com/RUrKV.png");
        }

        // create drawin manager
        var drawingManager = new google.maps.drawing.DrawingManager({
            
            drawingControl: true,
            drawingControlOptions: {
                position: google.maps.ControlPosition.TOP_CENTER,
                drawingModes: [google.maps.drawing.OverlayType.MARKER, google.maps.drawing.OverlayType.POLYGON]
            },
            markerOptions: {
                icon: new google.maps.MarkerImage("http://google.com/mapfiles/ms/micons/orange.png"),
                draggable: true
            },
            polygonOptions: {
                strokeColor: "#FF3333",
                strokeOpacity: 0.8,
                strokeWeight: 2,
                fillColor: "#FFaaaa",
                fillOpacity: 0.35,
                editable: true
            }
        });
        drawingManager.setMap(map);

        google.maps.event.addListener(drawingManager, 'overlaycomplete', function (event) {
            drawingManager.setDrawingMode(null);
            currentValues[event.type] = event.overlay;

            if (event.type === "polygon") {
                addDeleteButton(event.overlay, "http://i.imgur.com/RUrKV.png");
            }
        });

        return {
            getLocation: function() {
                return serializeCoord(currentValues.marker ? currentValues.marker.getPosition() : null);
            },
            getBounds: function() {
                var path = currentValues.polygon ? currentValues.polygon.getPath().getArray() : null;
                if(!path) {
                    return null;
                }

                var points = [];
                for(var i = 0; i < path.length; i ++) {
                    points[i] = serializeCoord(path[i]);
                }
                return points;
            }
        }
    };


    
    function serializeCoord(latLng) {
        if(!latLng)
            return;

        return {
            lat: latLng.lat(),
            lng: latLng.lng()
        };
    }

    function deserializeCoord(coord) {
        if(!coord || !coord.lat || !coord.lng)
            return;

        return new google.maps.LatLng(coord.lat, coord.lng);
    }



    function addDeleteButton(poly, imageUrl) {
        var path = poly.getPath();
        path["btnDeleteClickHandler"] = {};
        path["btnDeleteImageUrl"] = imageUrl;

        google.maps.event.addListener(poly.getPath(), 'set_at', pointUpdated);
        google.maps.event.addListener(poly.getPath(), 'insert_at', pointUpdated);
    }

    function pointUpdated(index) {
        var path = this;
        var btnDelete = getDeleteButton(path.btnDeleteImageUrl);

        if (btnDelete.length === 0) {
            var undoimg = $("img[src$='http://maps.gstatic.com/mapfiles/undo_poly.png']");

            undoimg.parent().css('height', '21px !important');
            undoimg.parent().parent().append('<div style="overflow-x: hidden; overflow-y: hidden; position: absolute; width: 30px; height: 27px;top:21px;"><img src="' + path.btnDeleteImageUrl + '" class="deletePoly" style="height:auto; width:auto; position: absolute; left:0;"/></div>');

            // now get that button back again!
            btnDelete = getDeleteButton(path.btnDeleteImageUrl);
            btnDelete.hover(function () { $(this).css('left', '-30px'); return false; },
                    function () { $(this).css('left', '0px'); return false; });
            btnDelete.mousedown(function () { $(this).css('left', '-60px'); return false; });
        }

        // if we've already attached a handler, remove it
        if (path.btnDeleteClickHandler)
            btnDelete.unbind('click', path.btnDeleteClickHandler);

        // now add a handler for removing the passed in index
        path.btnDeleteClickHandler = function () {
            path.removeAt(index);
            return false;
        };
        btnDelete.click(path.btnDeleteClickHandler);
    }

    function getDeleteButton(imageUrl) {
        return $("img[src$='" + imageUrl + "']");
    }

})();