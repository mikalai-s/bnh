/*global define, tinymce*/

define(
    ["jquery", "tinymce", "encoding"],
    function ($, t, encoding) {
        "use strict";

        tinymce.init({
            mode: "textareas",
            theme: "simple",

            forced_root_block: false,

            setup: function (ed) {
                ed.onSaveContent.add(function (i, o) {
                    o.content = encoding.encode64(o.content);
                });
            }
        });

        $(".scale")
            .on("mousemove", function (e) {
                var scale = $(this);
                scale.find(".l").width(e.offsetX / scale.width() * 100 - 1 + "%");
            })
            .on("click", function (e) {
                var scale = $(this);
                scale.closest(".controls").find("input.value").val(e.offsetX / scale.width());
            })
            .on("mouseout", function (e) {
                var scale = $(this),
                    value = scale.closest(".controls").find("input.value").val();
                scale.find(".l").width(value * 100 + "%");
            });
    }
);
