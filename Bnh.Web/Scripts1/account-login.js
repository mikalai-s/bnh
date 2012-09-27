define(
    ["jquery"],
    function ($) {
        $("#submitLink")
            .on("click", function () {
                $('#returnUrl').val(window.location);
                $('#logonForm').submit();
                return false;
            });
    }
);