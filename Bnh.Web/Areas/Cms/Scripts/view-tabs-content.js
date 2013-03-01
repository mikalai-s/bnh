define(
    ["jquery", "twitter"],
    function ($) {
        "use strict";

        function processBricks($tab, showHide) {
            var brickClasses = $.map($tab.data("bricks"), function (id) {
                return ".id_" + id;
            });
            $tab.closest(".wall-wrapper").find(brickClasses.join(","))[showHide]();
        }

        $('a[data-toggle="tab"]').on('shown', function (e) {
            processBricks($(e.relatedTarget), "hide");
            processBricks($(e.target), "show");
        });
    }
);