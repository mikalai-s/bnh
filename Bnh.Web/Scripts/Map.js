(function () {
    "use strict"

    var myLatLng = new google.maps.LatLng(51.02844, -114.071045);
    var myOptions = {
        zoom: 11,
        center: myLatLng,
        mapTypeId: google.maps.MapTypeId.TERRAIN
    };

    var bermudaTriangle;

    var map = new google.maps.Map(document.getElementById("mapCanvas"), myOptions);

    var triangleCoords = [
        new google.maps.LatLng(50.890176, -114.088511),
        new google.maps.LatLng(50.88912, -114.074049),
        new google.maps.LatLng(50.879129, -114.085894)
    ];

    // Construct the polygon
    bermudaTriangle = new google.maps.Polygon({
        paths: triangleCoords,
        strokeColor: "#FF3333",
        strokeOpacity: 0.8,
        strokeWeight: 1,
        fillColor: "#FFaaaa",
        fillOpacity: 0.35,
        editable: true,
        map: map
    });

    var infowindow = new google.maps.InfoWindow({
        content: "Silverado"
    });

//    google.maps.event.addListener(bermudaTriangle, 'click', function (event) {
//        infowindow.setPosition(event.latLng);
//        infowindow.open(map);
//    });

    google.maps.event.addListener(bermudaTriangle, 'mouseover', function (event) {
        highlightCommunity();
    });

    google.maps.event.addListener(bermudaTriangle, 'mouseout', function (event) {
        dehighlightCommunity();
    });



    $(".zone-community a").on("mousemove", function() {
        if(this.innerText === "Silverado") {
            highlightCommunity();
        }        
    });

    $(".zone-community a").on("mouseout", function() {
        dehighlightCommunity();
    });

    function highlightCommunity() {
     infowindow.setPosition(triangleCoords[0]);
        infowindow.open(map);
        bermudaTriangle.setOptions({
                strokeWeight: 2,
                fillOpacity: 0.75,
            });
    }

    function dehighlightCommunity() {
    infowindow.close();
        
        bermudaTriangle.setOptions({
            strokeWeight: 1,
            fillOpacity: 0.35
        });   
    }

})();