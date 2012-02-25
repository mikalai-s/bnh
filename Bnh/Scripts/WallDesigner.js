/// <reference path="jquery-1.5.1-vsdoc.js" />
/// <reference path="jquery-ui-1.8.11.js" />
/// <reference path="jquery.json-2.3.js" />
/// <reference path="jquery.validate.js" />


(function () {

    $(function () {

        $("#addBrickButton").click(onAddBrickButtonClicked);

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

                $("#wallScene").children().each(function(i, brick) {
                    data.bricks[i] = $(brick).data("entity");
                });

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
        $("#wallScene").children().each(function (i, brick) {
            // initialize brick element
            initializeBrick($(brick));
        });
    }

    function onAddBrickButtonClicked() {
        // create brick from prototype and add into DOM
        var brick = createBrick();

        // initialize brick object
        initializeBrick(brick, {
            type: parseInt($("#brickType").val()),
            title: $("#brickTitle").val()
        });
    }

    // creates new brick element from prototype
    function createBrick() {
        return $("#brickPrototype").children().first().clone().appendTo($("#wallScene"));
    }

    function initializeBrick(brick, customData) {
        // parse and assign entity object to brick
        var data = $.parseJSON(brick.attr("entity-data"));

        // extend result data with custom one
        $.extend(data, customData);

        // set brick entity data
        brick.data("entity", data);

        // update brick text
        updateBrickText(brick);

        // make brick resizable
        brick.resizable({
            containment: 'parent',
            handles: "e",
            resize: function (event, ui) { onBrickResize(event, ui, brick); },
        });
    }

    function onBrickResize(event, ui, brick) {
        var width = (brick.width() / brick.parent().width() * 100);
        brick.data("entity").width = width;

        updateBrickText(brick);
    }

    function updateBrickText(brick) {
        var width = brick.data("entity").width;
        brick.find(".brick-content").text(width.toFixed(2));
    }

})();