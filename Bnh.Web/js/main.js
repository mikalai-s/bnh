/// <reference path="libs/twitter/bootstrapx-clickover.js" />
require.config({
    paths: {
        jquery: 'libs/jquery/jquery-1.8.3.min' // '//ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min'
        , knockout: 'libs/knockout/knockout-2.2.0'
        , async: 'libs/require/async'
        , jqui: 'libs/jquery/jquery-ui-1.8.20'
        , json: 'libs/json2/json2'
        , tinymce: 'libs/tiny_mce/tiny_mce'
        , text: 'libs/require/text.min'
        , debug: 'libs/debug/ba-debug.min'
        , twitter: 'libs/twitter/bootstrap'
        , twitteroverrides: 'libs/twitter/overrides'
    },
    shim: {
        twitter: {
            deps: ["jquery"]
        },
        twitteroverrides: {
            deps: ["twitter"]
        }
    }
    //urlArgs: "bust=" + (new Date()).getTime()
});

var gmapsUrl = 'http://maps.google.com/maps/api/js?v=3&sensor=false';
//var gmapsUrl = 'http://maps.googleapis.com/maps/api/js?key=AIzaSyBXgUOTPfgbS4kHE7fm_xr2za_O1ApA_TM&sensor=false';

// special google maps api definition
define(
    'gmaps',
    ['async!' + gmapsUrl],
    function () {
        // return the gmaps namespace for brevity
        return window.google.maps;
    });
define(
    'gmaps-drawing',
    ['async!' + gmapsUrl + '&libraries=drawing'],
    function () {
        // return the gmaps namespace for brevity
        return window.google.maps;
    });


// common setup for all pages
require(["jquery", "twitter", "twitteroverrides"], function ($) {

    // setup search popup
    $('#search-button').popover({
        placement: "left",
        html: true,
        content: $("#popover-search-form")[0].outerHTML,
        trigger: 'click',
        template: '<div class="popover"><div class="arrow"></div><div class="popover-inner"><h3 class="popover-title" style="display:none"></h3><div class="popover-content"><p></p></div></div></div>'
    });
});

