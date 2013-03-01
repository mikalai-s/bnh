/*global define, tinymce*/

define(
    ["knockout"],
    function (ko) {
        "use strict";

        function Tabs() {
            this.tabs = ko.observableArray();
            this.availableBricks = ko.observableArray();
        }

        Tabs.prototype.addTab = function () {
            this.tabs.push(new Tab());
        };

        Tabs.prototype.removeTab = function (index) {
            this.tabs.splice(index, 1);
        };


        function Tab() {
            this.title = ko.observable();
            this.bricks = ko.observableArray();
        }

        Tab.prototype.addBrick = function () {
            this.bricks.push({ name: "", value: "" });
        };

        Tab.prototype.removeBrick = function (index) {
            this.bricks.splice(index, 1);
        };

        Tab.prototype.getInputKeyNameAttribute = function (index) {
            return "content.tabs[" + index + "].key";
        };

        Tab.prototype.getInputValueNameAttribute = function (index) {
            return "content.tabs[" + index + "].value";
        }

        var tabs = new Tabs();
        tabs.availableBricks.push({});
        for (var i = 0; i < page_availableBricks.length; i ++) {
            tabs.availableBricks.push(page_availableBricks[i]);
        }

        for (var tabName in page_tabs) {
            var tab = new Tab();
            tab.title(tabName);
            tab.bricks(page_tabs[tabName]);
            tabs.tabs.push(tab);
        }

        ko.applyBindings(window.tabs = tabs);

    }
);
