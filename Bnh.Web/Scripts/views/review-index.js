define(
    ["jquery", "knockout", "tinymce"],
    function ($, ko) {
        "use strict";

        function Page(record) {
            this.title = record.Title;
            this.rating = record.Rating;
            this.targetUrlId = record.TargetUrlId;
            this.targetName = record.TargetName;
            this.pagerLinks = record.PagerLinks;
            this.reviews = $.map(record.Reviews, function (r) {
                return new Review(r);
            });
            this.addReviewLink = record.AddReviewLink;
        }

        function Review(record) {
            this.reviewId = record.ReviewId;
            this.userName = record.UserName;
            this.userAvatarSrc = record.UserAvatarSrc;
            this.ratings = $.map(record.Ratings, function (r) {
                return new RatingQuestion(r);
            });
            this.message = record.Message;
            this.created = record.Created;
            this.comments = ko.observableArray($.map(record.Comments, function (c) {
                return new Comment(c);
            }));
            this.postCommentActionUrl = record.PostCommentActionUrl;

            this.commentsVisible = ko.observable(false);
            this.commentsLinkText = ko.observable("");

            this.addCommentVisible = ko.observable(false);
            this.newComment = ko.observable();

            ko.computed(function () {
                var comments = this.comments();
                if (!comments.length) {
                    this.commentsLinkText("No comments left yet");
                } else {
                    this.commentsLinkText(this.commentsVisible() ? "Hide comments" : 'View ' + comments.length + ' comments');
                }
            }, this);
        }

        function Comment(record) {
            this.userName = record.UserName;
            this.message = record.Message;
            this.userAvatarSrc = record.UserAvatarSrc;
            this.created = record.Created;
        }

        function RatingQuestion(record) {
            this.question = record.Question;
            this.answerHtml = record.AnswerHtml;
        }

        Review.prototype.onUsefulClick = function () {
        };

        Review.prototype.onPostComment = function () {
            var self = this;
            $.ajax({
                url: this.postCommentActionUrl,
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify({ 
                    reviewId: this.reviewId,
                    message: this.newComment()
                }),
                success: function (postedComment) {
                    self.comments.push(new Comment(postedComment));
                },
                error: function (result) {
                    console.error("Unable to post a comment!", result);
                }
            });
        };



        // TODO: move it out of here
        ko.bindingHandlers.slideVisible = {
            init: function (element, valueAccessor) {
                var value = ko.utils.unwrapObservable(valueAccessor()); // Get the current value of the current property we're bound to
                $(element).toggle(value); // jQuery will hide/show the element depending on whether "value" or true or false
            },
            update: function (element, valueAccessor, allBindingsAccessor) {
                // First get the latest data that we're bound to
                var value = valueAccessor(), allBindings = allBindingsAccessor();

                // Next, whether or not the supplied model property is observable, get its current value
                var valueUnwrapped = ko.utils.unwrapObservable(value);

                // Grab some more data from another binding property
                var duration = allBindings.slideDuration || 200; // 400ms is default duration unless otherwise specified

                // Now manipulate the DOM element
                if (valueUnwrapped == true)
                    $(element).show(duration); // Make the element visible
                else
                    $(element).hide(duration);   // Make the element invisible
            }
        };

        ko.bindingHandlers.tinymce = {
            init: function (element, valueAccessor, allBindingsAccessor, context) {
                var options = allBindingsAccessor().tinymceOptions || {};
                var modelValue = valueAccessor();
                var value = ko.utils.unwrapObservable(valueAccessor());
                var el = $(element);


                options.setup = function (ed) {

                    ed.onChange.add(function (editor, l) { //handle edits made in the editor. Updates after an undo point is reached.
                        if (ko.isWriteableObservable(modelValue)) {
                            modelValue(l.content);
                        }
                    });

                    ed.onInit.add(function (ed, evt) { // Make sure observable is updated when leaving editor.
                        var dom = ed.dom;
                        var doc = ed.getDoc();
                        tinymce.dom.Event.add(doc, 'blur', function (e) {
                            if (ko.isWriteableObservable(modelValue)) {
                                modelValue(ed.getContent({ format: 'raw' }));
                            }
                        });
                    });

                };

                //handle destroying an editor (based on what jQuery plugin does)
                ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                    $(element).parent().find("span.mceEditor,div.mceEditor").each(function (i, node) {
                        var ed = tinyMCE.get(node.id.replace(/_parent$/, ""));
                        if (ed) {
                            ed.remove();
                        }
                    });
                });

                //$(element).tinymce(options);
                setTimeout(function () { tinymce.init(options); }, 0);
                el.val(value);

            },
            update: function (element, valueAccessor, allBindingsAccessor, context) {
                var el = $(element);
                var value = ko.utils.unwrapObservable(valueAccessor());
                var id = el.attr('id');

                //handle programmatic updates to the observable
                // also makes sure it doesn't update it if it's the same.
                // otherwise, it will reload the instance, causing the cursor to jump.
                if (id !== undefined && id !== '') {
                    var content = tinyMCE.getInstanceById(id).getContent({ format: 'raw' });
                    if (content !== value) {
                        el.val(value);
                    }
                }
            }
        };

        //$(".delete a").click(function () {
        //    var $a = $(this);
        //    if (confirm("Are you sure you want to delete the review?")) {
        //        $.ajax({
        //            url: $a.attr("href"),
        //            type: 'delete',
        //            dataType: 'json',
        //            data: { reviewId: $a.attr("reviewid") },
        //            success: function () {
        //                $a.closest(".review-container").remove();
        //            },
        //            error: function () {
        //                console.error("Unable to delete review!");
        //            }
        //        });
        //    }
        //    return false;
        //});

        //$(".commentsLink").click(function () {
        //    var $link = $(this), $commentsSection, text;

        //    $commentsSection = $link.closest(".review-container").find(".commentsSection");
        //    $commentsSection.toggle("fast");

        //    text = $link.attr("data-text");
        //    $link.attr("data-text", $link.text());
        //    $link.text(text);

        //    return false;
        //});

        //$(".addCommentLink").click(function () {
        //    var $addCommentSection = $(this).closest(".review-container").find(".addCommentSection");

        //    $addCommentSection.toggle("fast");

        //    return false;
        //});

        //$(".postCommentButton").click(function (e) {
        //    var $button = $(this), $review;
        //    $review = $button.closest(".review-container");
        //    $.ajax({
        //        url: $button.attr("href"),
        //        type: 'post',
        //        contentType: 'application/json',
        //        data: JSON.stringify({ 
        //            reviewId: $review.attr("id"), 
        //            comment: { message: tinyMCE.get($review.find("textarea").attr("data-index")).getContent() }
        //        }),
        //        success: function () {
        //            $review.find(".addCommentSection").toggle("fast");
        //        },
        //        error: function (result) {
        //            console.error("Unable to post a comment!", result);
        //        }
        //    });
        //    e.preventDefault();
        //    return false;
        //});
        

        

     


        //// initialize all textareas on the page
        //tinyMCE.init({
        //    mode: "textareas",
        //    encoding: "xml",
        //    theme: "simple",
        //    forced_root_block: false
        //});

        return Page;
    }
);
