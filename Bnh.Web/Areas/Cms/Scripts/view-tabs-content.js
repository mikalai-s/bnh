define(
    ["jquery", "twitter"],
    function ($) {
        "use strict";

        var $tabs = $('a[data-toggle="tab"]');

        function switchTab($a) {
            var tabName = $a.closest(".tabs-content").attr("id");
            var tabIndex = $a.closest("li").index();
            var $associatedBricks = $a.closest(".wall-wrapper").find(".brick-wrapper.tabname_" + tabName);

            $associatedBricks.hide();
            $associatedBricks.parent().find(".tabindex_" + tabIndex).show();
        }

        $tabs.on('shown', function (e) {
            switchTab($(this));
        });
    }
);