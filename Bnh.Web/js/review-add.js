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
    }
);
