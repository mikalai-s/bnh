define(
    ["jquery", "galleria"],
    function ($) {
        "use strict";

        requirejs(["libs/galleria/themes/classic/galleria.classic.min"], function () {
            Galleria.run('.galleria');
        });
    }
);