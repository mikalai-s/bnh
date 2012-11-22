// Takes care of loading page modules and history
// Uses History API if it's available

define(
    ["jquery", "knockout", "debug"],
    function ($, ko) {

        var $page = $("#page"),
            $body = $page.find("#body");


        // checks whether given browser support History API
        function supportsHistoryApi() {
            return history.pushState;
        }

        // initialize current page
        function init() {
            // add title and description binding for the page
            $page.find("header .title").attr("data-bind", "html: title");
            $page.find("header .description").attr("data-bind", "html: description");

            // handle popstate event only if browser supports History API
            if (supportsHistoryApi()) {
                // Hook popstate event in 1 second to prevent Chrome's popstate on page load
                // Ugly but effective
                window.setTimeout(function () {
                    // hook browser's back button click
                    $(window).on('popstate', function () {
                        var module = history.state && history.state.module;
                        if (module) {
                            loadPage(module);
                        }
                        else {
                            history.go();
                        }
                    });
                }, 700);
            }

            // handle click event of all links that have module attribute
            hookSinglePageLinks($(document));

            // get current page module
            var module = $body.attr("data-module");
            if (module) {
                require([module, "text!" + module + ".htm"], function (pageViewModel, pageTemplate) {
                    if (typeof (pageViewModel) === "function" && initViewModel) {
                        $body.html(pageTemplate);

                        // TODO: vm as global just for debug
                        vm = new pageViewModel(initViewModel);

                        ko.applyBindings(vm, $page[0]);

                        hookSinglePageLinks($page);
                    }
                });
            }
        }

        // handle all single page links on the page
        function hookSinglePageLinks(container) {
            // hook single page links only if browser supports History API
            if (supportsHistoryApi()) {
                container.find("a[module]")
                    .on("click", function () {
                        var module = $(this).attr("data-module");

                        history.pushState({ module: module }, 'Entry', this.href);

                        loadPage(module);

                        return false;
                    });
            }
        }

        function initPage(module, html, viewModel) {
            $body.html(html);

            ko.applyBindings(viewModel, document.getElementsByTagName("html")[0]);

            // handle single page link in given template
            hookSinglePageLinks($page);

            // everything is loaded and binded - load module script
            require([module]);
        }

        // load given module into current page
        function loadPage(module) {
            $.ajax({
                url: window.location.pathname,
                type: "post",
                success: function (data, status, xhr) {
                    var title = xhr.getResponseHeader("page-title") || window.location.pathname,
                        requiresTemplate = xhr.getResponseHeader("requires-template");

                    if (requiresTemplate) {
                        require(["text!" + module + ".htm"], function (template) {
                            initPage(module, template, $.extend(true, data, { title: title }));
                        });
                    }
                    else {
                        initPage(module, data, { title: title });
                    }
                }
            });
        }

        init();
    }
);
