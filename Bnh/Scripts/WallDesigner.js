/// <reference path="jquery-1.5.1-vsdoc.js" />
/// <reference path="jquery-ui-1.8.11.js" />
/// <reference path="jquery.json-2.3.js" />
/// <reference path="jquery.validate.js" />


(function () {

    $(function () {

        $("#addRowButton").click(onAddRowButtonClicked);

        initBricks();

        $("form").validate({
            submitHandler: function (form) {
                var data = {
                    wall: {
                        Id: form["Id"].value,
                        OwnerId: form["OwnerId"].value,
                        Title: form["Title"].value
                    },
                    bricks: []
                };

                var brickElements = $("#wallScene").children();
                for (var i = 0; i < brickElements.length; i++) {
                    data.bricks[i] = {
                        Width: $.data(brickElements[i], "width"),
                        Title: "Title",
                        Html: "Html",
                        Type: 1
                    };
                }

                $.ajax({
                    url: form.action,
                    type: "POST",
                    contentType: "application/json",
                    data: jQuery.toJSON(data),
                    success: function (result) {
                        $("wallScene").replaceWith(result);
                        initBricks();
                    },
                    error: function (result) {
                    }
                });
            }
        });

    });

    function initBricks() {
        var brickElements = $("#wallScene").children();
        for (var i = 0; i < brickElements.length; i++) {
            makeBrickResizable($(brickElements[i]));
        }
    }


    function onAddRowButtonClicked() {

        var wall = $("#wallScene");
        var brick = $("<div class='brick-wrapper'><div class='brick'/></div>");
        wall.append(brick);

        makeBrickResizable(brick);
    }

    function makeBrickResizable(brick) {
        updateBrickText(brick);

        brick.resizable({
            containment: 'parent',
            handles: "e",
            start: function (event, ui) { onBrickResizeStart(event, ui, brick); },
            resize: function (event, ui) { onBrickResize(event, ui, brick); },
            stop: function (event, ui) { onBrickResizeStop(event, ui, brick); }
        });
    }

    function onBrickResizeStart(event, ui, brick) {

    }

    function onBrickResize(event, ui, brick) {
        updateBrickText(brick);
    }

    function onBrickResizeStop(event, ui, brick) {
    }

    function updateBrickText(brick) {
        var width = (brick.width() / brick.parent().width() * 100);
        $.data(brick[0], "width", width);
        brick.children().first().text(width.toFixed(2));
    }

})();