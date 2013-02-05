define(
    ["jquery", "twitter"],
    function ($) {
        "use strict";

        var $tabs = $('a[data-toggle="tab"]');

        function switchTab($a) {
            var tabId = $a.closest(".brick-wrapper").attr("id");
            var tabIndex = $a.closest("li").index();
            var $associatedBricks = $a.closest(".wall-wrapper").find(".brick-wrapper.tabid_" + tabId);

            $associatedBricks.hide();
            $associatedBricks.parent().find(".tabindex_" + tabIndex).show();
        }

        $tabs.on('shown', function (e) {
            switchTab($(this));
        });
    }
);