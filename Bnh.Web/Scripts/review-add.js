define(
    ["tinymce"],
    function () {
        "use strict";

        tinyMCE.init({
            mode: "textareas",
            encoding: "xml",
            theme: "simple",

            forced_root_block: false,

            // Skin options
            skin: "o2k7",
            skin_variant: "silver"
        });
    }
);
