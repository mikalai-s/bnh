define(["jquery", "twitter", "twitteroverrides"], function ($) {
    "use strict";

    // setup search popup
    $('#search-button').popover({
        placement: "left",
        html: true,
        content: $("#popover-search-form")[0].outerHTML,
        trigger: 'click',
        template: '<div class="popover"><div class="arrow"></div><div class="popover-inner"><h3 class="popover-title" style="display:none"></h3><div class="popover-content"><p></p></div></div></div>'
    });
});