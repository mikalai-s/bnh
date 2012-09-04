define(
    ["order!tinymce", "order!jqtinymce"],
    function () {
        "use strict";

        $("textarea").tinymce.({
            encoding: "xml",
            theme: "simple",

            forced_root_block: false
        });
    }
);
