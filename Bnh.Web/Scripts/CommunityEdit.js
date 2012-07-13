(function () {
    "use strict"

    var gpsLocationField = $("#GpsLocation");
    var gpsBoundsField = $("#GpsBounds");

    var overlays = {
        marker: gpsLocationField.val() ? $.parseJSON(gpsLocationField.val()) : null,
        polygon: gpsBoundsField.val() ? $.parseJSON(gpsBoundsField.val()) : null,
        zoom: 14
    };

    var mapEditor = new Global.MapEditor("#mapCanvas", overlays);

    $("form").validate({
        submitHandler: function(form) {
            gpsLocationField.val($.toJSON(mapEditor.getLocation()));
            gpsBoundsField.val($.toJSON(mapEditor.getBounds()));

            // do other stuff for a valid form
            form.submit();
        }
    });

})();