/*global window, define, require, tinymce */

define(
    ["jquery", "knockout", "jqjson"],
    function ($, ko) {
        "use strict";

        function Link(record) {
            this.text = record.Text;
            this.href = record.Href;
            this.css = record.Css;
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

        function Review(record, page) {
            var self = this;

            this.page = page;
            // deferred object for lazy initialization of tinymce
            this.tinyMceInitTrigger = $.Deferred();
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
                if (comments.length) {
                    this.commentsLinkText(this.commentsVisible()
                        ? "Hide comments"
                        : 'View ' + comments.length + ' comments');
                }
            }, this);
        }

        function Page(record) {
            var self = this;

            this.title = ko.observable(record.Title);
            this.description = ko.observable(record.Description);
            this.rating = record.Rating;
            this.targetUrlId = record.TargetUrlId;
            this.targetName = record.TargetName;
            this.pagerLinks = record.PagerLinks && $.map(record.PagerLinks, function (r) {
                return new Link(r);
            });
            this.reviews = ko.observableArray($.map(record.Reviews, function (r) {
                return new Review(r, self);
            }));
            this.addReviewLink = record.AddReviewLink;
            this.admin = record.Admin;
            this.deleteReviewUrl = record.DeleteReviewUrl;
            this.deleteCommentUrl = record.DeleteCommentUrl;
        }


        Review.prototype.onUsefulClick = function () {
        };

        Review.prototype.onAddComment = function () {
            var self = this;

            this.addCommentVisible(!this.addCommentVisible());
            if (this.addCommentVisible()) {
                // load tineymce and trigger init callbacks
                require(["tinymce"], function () {
                    self.tinyMceInitTrigger.resolve();
                });
            }
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
                success: function (data) {
                    self.comments.push(new Comment(data, self));
                    self.newComment("");
                    self.addCommentVisible(false);
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    window.console.error("Unable to post a comment!",
                        jqXhr, textStatus, errorThrown);
                }
            });
        };

        Review.prototype.onCancelComment = function () {
            this.addCommentVisible(false);
        };

        Review.prototype.onDeleteReview = function () {
            var self = this;
            if (window.confirm("Are you sure you want to delete the review?")) {
                $.ajax({
                    url: this.page.deleteReviewUrl,
                    headers: {
                        'X-HTTP-Method-Override': 'DELETE'
                    },
                    type: 'post',
                    contentType: 'application/json',
                    data: jQuery.toJSON({ reviewId: this.reviewId }),
                    success: function () {
                        self.page.reviews.remove(self);
                    },
                    error: function (jqXhr, textStatus, errorThrown) {
                        window.console.error("Unable to delete review!",
                            jqXhr, textStatus, errorThrown);
                    }
                });
            }
        };


        Comment.prototype.onDeleteComment = function () {
            var self = this;
            if (window.confirm("Are you sure you want to delete the comment?")) {
                $.ajax({
                    url: this.review.page.deleteCommentUrl,
                    headers: {
                        'X-HTTP-Method-Override': 'DELETE'
                    },
                    type: 'post',
                    contentType: 'application/json',
                    data: jQuery.toJSON({
                        reviewId: this.review.reviewId,
                        commentId: this.commentId
                    }),
                    success: function () {
                        self.review.comments.remove(self);
                    },
                    error: function (jqXhr, textStatus, errorThrown) {
                        window.console.error("Unable to delete review!",
                            jqXhr, textStatus, errorThrown);
                    }
                });
            }
        };



        // TODO: move it out of here
        ko.bindingHandlers.slideVisible = {
            init: function (element, valueAccessor) {
                // Get the current value of the current property we're bound to
                var value = ko.utils.unwrapObservable(valueAccessor());
                // jQuery will hide/show the element depending on whether "value" or true or false
                $(element).toggle(value);
            },
            update: function (element, valueAccessor, allBindingsAccessor) {
                // First get the latest data that we're bound to
                var value = valueAccessor(), allBindings = allBindingsAccessor(),
                    valueUnwrapped,
                    duration;

                // Next, whether or not the supplied model property is observable, get its current value
                valueUnwrapped = ko.utils.unwrapObservable(value);

                // Grab some more data from another binding property
                duration = allBindings.slideDuration || 200; // 400ms is default duration unless otherwise specified

                // Now manipulate the DOM element
                if (valueUnwrapped === true) {
                    $(element).show(duration); // Make the element visible
                } else {
                    $(element).hide(duration);   // Make the element invisible
                }
            }
        };

        ko.bindingHandlers.tinymce = {
            init: function (element, valueAccessor, allBindingsAccessor) {
                var options = allBindingsAccessor().tinymceOptions || {},
                    modelValue = valueAccessor(),
                    value = ko.utils.unwrapObservable(valueAccessor()),
                    el = $(element);

                options.mode = "exact";
                options.setup = function (ed) {

                    ed.onChange.add(function (ed) { //handle edits made in the editor. Updates after an undo point is reached.
                        if (ko.isWriteableObservable(modelValue)) {
                            modelValue(ed.getContent());
                        }
                    });

                    ed.onInit.add(function (ed) { // Make sure observable is updated when leaving editor.
                        var doc = ed.getDoc();
                        tinymce.dom.Event.add(doc, 'blur', function () {
                            if (ko.isWriteableObservable(modelValue)) {
                                modelValue(ed.getContent({ format: 'raw' }));
                            }
                        });
                    });

                };

                //handle destroying an editor (based on what jQuery plugin does)
                ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                    /*jslint unparam: true */
                    $(element).parent().find("span.mceEditor,div.mceEditor").each(function (i, node) {
                        var ed = tinymce.get(node.id.replace(/_parent$/, ""));
                        if (ed) {
                            ed.remove();
                        }
                    });
                    /*jslint unparam: false */
                });

                el.val(value);

                if (options.initTrigger) {
                    // deferred initialization of tinymce
                    options.initTrigger.always(function () {
                        window.setTimeout(function () {
                            options.elements = el.attr("id");
                            tinymce.init(options);
                        }, 0);
                    });
                } else {
                    // otherwise - immidiate initialization
                    window.setTimeout(function () {
                        options.elements = el.attr("id");
                        tinymce.init(options);
                    }, 0);
                }
            },
            update: function (element, valueAccessor) {
                var el = $(element),
                    value = ko.utils.unwrapObservable(valueAccessor()),
                    id,
                    tm,
                    content;
                id = el.attr('id');

                //handle programmatic updates to the observable
                // also makes sure it doesn't update it if it's the same.
                // otherwise, it will reload the instance, causing the cursor to jump.
                if (id !== undefined && id !== '' && typeof (tinymce) !== "undefined") {
                    tm = tinymce.getInstanceById(id);
                    if (tm) {
                        content = tm.getContent({ format: 'raw' });
                        if (content !== value) {
                            tm.setContent(value, { format: 'raw' });
                        }
                    }
                }
            }
        };

        return Page;
    }
);
