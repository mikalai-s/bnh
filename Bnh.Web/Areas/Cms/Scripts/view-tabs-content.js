define(
    ["jquery", "twitter"],
    function ($) {
        "use strict";

        $('a[data-toggle="tab"]').on('shown', function (e) {
            var tabId = $(this).closest(".brick-wrapper").attr("id");
            var tabIndex = $(this).closest("li").index();
            var $associatedBricks = $(this).closest(".wall-wrapper").find(".brick-wrapper.tabid_" + tabId);

            $associatedBricks.hide();
            $associatedBricks.parent().find(".tabindex_" + tabIndex).show();
        });


    }
);