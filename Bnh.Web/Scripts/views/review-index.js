define(
    ["jquery", "knockout", "order!tinymce", "order!jqtinymce"],
    function ($, ko) {
        "use strict";

        function Page(record) {
            var self = this;

            this.title = record.Title;
            this.rating = record.Rating;
            this.targetUrlId = record.TargetUrlId;
            this.targetName = record.TargetName;
            this.pagerLinks = record.PagerLinks;
            this.reviews = ko.observableArray($.map(record.Reviews, function (r) {
                return new Review(r, self);
            }));
            this.addReviewLink = record.AddReviewLink;
            this.admin = record.Admin;
            this.deleteReviewUrl = record.DeleteReviewUrl;
            this.deleteCommentUrl = record.DeleteCommentUrl;
        }

        function Review(record, page) {
            var self = this;

            this.page = page;
            this.reviewId = record.ReviewId;
            this.userName = record.UserName;
            this.userAvatarSrc = record.UserAvatarSrc;
            this.ratings = $.map(record.Ratings, function (r) {
                return new RatingQuestion(r, self);
            });
            this.message = record.Message;
            this.created = record.Created;
            this.comments = ko.observableArray($.map(record.Comments, function (c) {
                return new Comment(c, self);
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

        function Comment(record, review) {
            this.review = review;
            this.commentId = record.CommentId;
            this.userName = record.UserName;
            this.message = record.Message;
            this.userAvatarSrc = record.UserAvatarSrc;
            this.created = record.Created;
        }

        function RatingQuestion(record, review) {
            this.review = review;
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
                success: function (data, textStatus, jqXHR) {
                    self.comments.push(new Comment(data, self));
                    self.newComment("");
                    self.addCommentVisible(false);
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    console.error("Unable to post a comment!", jqXhr, textStatus, errorThrown);
                }
            });
        };

        Review.prototype.onCancelComment = function () {
            this.addCommentVisible(false);
        };

        Review.prototype.onDeleteReview = function () {
            var self = this;
            if (confirm("Are you sure you want to delete the review?")) {
                $.ajax({
                    url: this.page.deleteReviewUrl,
                    type: 'delete',
                    dataType: 'json',
                    data: { reviewId: this.reviewId },
                    success: function (data, textStatus, jqXHR) {
                        self.page.reviews.remove(self);
                    },
                    error: function (jqXhr, textStatus, errorThrown) {
                        console.error("Unable to delete review!", jqXhr, textStatus, errorThrown);
                    }
                });
            }
        };


        Comment.prototype.onDeleteComment = function () {
            var self = this;
            if (confirm("Are you sure you want to delete the comment?")) {
                $.ajax({
                    url: this.review.page.deleteCommentUrl,
                    type: 'delete',
                    dataType: 'json',
                    data: {
                        reviewId: this.review.reviewId,
                        commentId: this.commentId
                    },
                    success: function (data, textStatus, jqXHR) {
                        self.review.comments.remove(self);
                    },
                    error: function (jqXhr, textStatus, errorThrown) {
                        console.error("Unable to delete review!", jqXhr, textStatus, errorThrown);
                    }
                });
            }
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
           //     options = $.extend(options, { script_url: '/scripts/libs/tiny_mce/tiny_mce.js' });
                setTimeout(function () { $(element).tinymce(options); }, 0);
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

        return Page;
    }
);
