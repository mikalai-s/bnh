/*global define, tinymce*/

define(
    ["jquery", "tinymce"],
    function () {
        "use strict";

        tinymce.init({
            mode: "textareas",
            encoding: "xml",
            theme: "simple",

            forced_root_block: false
        });
    }
);
