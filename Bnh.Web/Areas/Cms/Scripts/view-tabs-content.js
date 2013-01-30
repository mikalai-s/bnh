define(
    ["jquery", "twitter"],
    function ($) {
        "use strict";

        var tabs = $('.tabs-content a');

        tabs.click(function (e) {
            e.preventDefault();
            $(this).tab('show');
        });
    }
);