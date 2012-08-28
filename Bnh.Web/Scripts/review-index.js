define(
    ["jquery", "tinymce"],
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

        $(".commentsLink").click(function () {
            var $link = $(this), $commentsSection, text;

            $commentsSection = $link.closest(".review").find(".commentsSection");
            $commentsSection.toggle("fast");

            text = $link.attr("data-text");
            $link.attr("data-text", $link.text());
            $link.text(text);

            return false;
        });

        $(".addCommentLink").click(function () {
            var $addCommentSection = $(this).closest(".review").find(".addCommentSection");

            $addCommentSection.toggle("fast");

            return false;
        });

        $(".postCommentButton").click(function (e) {
            var $button = $(this), $review;
            $review = $button.closest(".review");
            $.ajax({
                url: $button.attr("href"),
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify({ 
                    reviewId: $review.attr("id"), 
                    comment: { message: tinyMCE.get($review.find("textarea").attr("data-index")).getContent() }
                }),
                success: function () {
                    $review.find(".addCommentSection").toggle("fast");
                },
                error: function (result) {
                    console.error("Unable to post a comment!", result);
                }
            });
            e.preventDefault();
            return false;
        });
        

        

     


        // initialize all textareas on the page
        tinyMCE.init({
            mode: "textareas",
            encoding: "xml",
            theme: "simple",
            forced_root_block: false
        });

    }
);
