var Global = Global || { };

(function () {
    "use strict"

    Global.Map = Global.Map || function(mapCanvas, options) {
        var polygon;

        var myLatLng = new google.maps.LatLng(options.center.Ta, options.center.Ua);
        var myOptions = {
            zoom: options.zoom,
            center: myLatLng,
            mapTypeId: google.maps.MapTypeId.TERRAIN
        };
        var map = new google.maps.Map($(mapCanvas)[0], myOptions);

        var namedOverlays = [];
        if(options.overlays) {
            for(var i = 0; i < options.overlays.length; i ++) {
                var coords = [];
                for(var j = 0; j < options.overlays[i].polygons.length; j ++) {
                    coords.push(new google.maps.LatLng(options.overlays[i].polygons[j].Ta, options.overlays[i].polygons[j].Ua))
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
    
})();