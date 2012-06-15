var Global = Global || { };

(function () {
    "use strict"

    Global.Map = Global.Map || function(mapCanvas, options) {
        var polygon;

        var myLatLng = deserializeCoord(options.center);
        var myOptions = {
            zoom: options.zoom,
            center: myLatLng,
            mapTypeId: google.maps.MapTypeId.TERRAIN
        };
        var $mapCanvas = $(mapCanvas);
        if($mapCanvas.length === 0) {
            return;
        }

        var map = new google.maps.Map($mapCanvas[0], myOptions);

        var namedOverlays = [];
        if(options.overlays) {
            for(var i = 0; i < options.overlays.length; i ++) {
                var coords = [];
                for(var j = 0; j < options.overlays[i].polygons.length; j ++) {
                    coords.push(deserializeCoord(options.overlays[i].polygons[j]))
                }

                namedOverlays[options.overlays[i].name] = addPolygon(options.overlays[i].name, map, coords, options.polygonClick);
            }
        }

        return {
            highlight: function(name) {
                highlightPolygon(namedOverlays[name]);
            },
            dehighlight: function(name) {
                dehighlightPolygon(namedOverlays[name]);
            },
            setVisible: function(name, visible) {
                setPolygonVisible(namedOverlays[name], visible);
            }
        };

        
        
//        var infowindow = new google.maps.InfoWindow({
//            content: "Silverado"
    };

//    google.maps.event.addListener(bermudaTriangle, 'click', function (event) {
//        infowindow.setPosition(event.latLng);
//        infowindow.open(map);
//    });
/* 
    };

    */


    function deserializeCoord(coord) {
        if(!coord || !coord.lat || !coord.lng)
            return;

        return new google.maps.LatLng(coord.lat, coord.lng);
    }


    function addPolygon(name, map, coords, clickHandler) {
        var polygon = new google.maps.Polygon({
            paths: coords,
            strokeColor: "#FF3333",
            strokeOpacity: 0.8,
            strokeWeight: 2,
            fillColor: "#FFaaaa",
            fillOpacity: 0.35,
            map: map
        });

        google.maps.event.addListener(polygon, 'mouseover', function (event) {
            highlightPolygon(polygon);
        });

        google.maps.event.addListener(polygon, 'mouseout', function (event) {
            dehighlightPolygon(polygon);
        });

        if(clickHandler) {
            google.maps.event.addListener(polygon, 'click', function (event) {
                clickHandler(name);
            });
        }

        return polygon;
    }

    function highlightPolygon(polygon) {
        if(!polygon)
            return;
    // infowindow.setPosition(triangleCoords[0]);
       // infowindow.open(map);
        polygon.setOptions({
                strokeWeight: 2,
                fillOpacity: 0.75,
            });
    }

    function dehighlightPolygon(polygon) {
        if(!polygon)
            return;

       // infowindow.close();
        
        polygon.setOptions({
            strokeWeight: 1,
            fillOpacity: 0.35
        });   
    }

    function setPolygonVisible(polygon, visible) {
        if(!polygon)
            return;

        polygon.setVisible(visible);
    }

})();