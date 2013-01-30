/*global define, tinymce*/

define(
    ["jquery"],
    function () {
        "use strict";

        var remove = function () {

            var $this = $(this);

            $this.parent().remove();

        };


        $(".button-remove").click(remove);

        $("#button-add").click(function () {

            var $newImagesDiv = $("#new-tabs");

            $newImagesDiv
                .append('<div><input name="tabs" style="width: 700px;" type="text" /><input class="button-remove btn" type="button" value="Remove" /></div>')
                .find(".button-remove")
                .click(remove);

        });


    }
);
