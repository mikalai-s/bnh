define(
    ["jquery", "rs"],
    function ($) {
        "use strict";

        var matched, browser;

        // Use of jQuery.browser is frowned upon.
        // More details: http://api.jquery.com/jQuery.browser
        // jQuery.uaMatch maintained for back-compat
        jQuery.uaMatch = function (ua) {
            ua = ua.toLowerCase();

            var match = /(chrome)[ \/]([\w.]+)/.exec(ua) ||
                /(webkit)[ \/]([\w.]+)/.exec(ua) ||
                /(opera)(?:.*version|)[ \/]([\w.]+)/.exec(ua) ||
                /(msie) ([\w.]+)/.exec(ua) ||
                ua.indexOf("compatible") < 0 && /(mozilla)(?:.*? rv:([\w.]+)|)/.exec(ua) ||
                [];

            return {
                browser: match[1] || "",
                version: match[2] || "0"
            };
        };

        matched = jQuery.uaMatch(navigator.userAgent);
        browser = {};

        if (matched.browser) {
            browser[matched.browser] = true;
            browser.version = matched.version;
        }

        // Chrome is Webkit, but Webkit is also Safari.
        if (browser.chrome) {
            browser.webkit = true;
        } else if (browser.webkit) {
            browser.safari = true;
        }

        jQuery.browser = browser;

        require(
            ["libs/royalslider/modules/jquery.rs.fullscreen", "libs/royalslider/modules/jquery.rs.thumbnails"],
            function () {

                $(".royalSlider").royalSlider({
                    // options go here
                    // as an example, enable keyboard arrows nav
                    keyboardNavEnabled: true,
                    autoHeight: true,
                    imageScaleMode: 'fit',
                    loop: true,
                    controlNavigation: 'thumbnails',
                    fullscreen: {
                        enabled: true,
                        native: true
                    },
                    thumbs: {
                        appendSpan: true,
                        firstMargin: true,
                        paddingBottom: 4
                    }
                }).css({
                    visibility: "visible"
                })

            });
    }
);