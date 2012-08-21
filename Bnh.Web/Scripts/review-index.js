define(
    ["jquery", "jqballoon"],
    function ($) {
        "use strict";

        $(".delete a").click(function () {
            var $a = $(this);
            if (confirm("Are you sure you want to delete the review?")) {
                $.ajax({
                    url: $a.attr("href"),
                    type: 'delete',
                    dataType: 'json',
                    data: { reviewId: $a.attr("reviewid") },
                    success: function () {
                        $a.closest(".review").remove();
                    },
                    error: function () {
                        console.error("Unable to delete review!");
                    }
                });
            }
            return false;
        });
    }
);
