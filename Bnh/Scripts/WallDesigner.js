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
                    bricks: {
                        "add": [],
                        "delete": [],
                        "edit": [],
                    }
                };

                getScene().children().each(function(i, brick) {
                    // get entity
                    var entity = $(brick).data("entity");

                    // check for changed order
                    if(entity.order != i) {
                        entity.order = i;
                        onProcessBrickAction(brick, "edit");
                    }

                    // put brick into appropriate action collection if need
                    if(entity.action) {
                        var bricks = data.bricks[entity.action];
                        bricks[bricks.length] = entity;
                    }
                });

                $.ajax({
                    url: form.action,
                    type: "POST",
                    contentType: "application/json",
                    data: jQuery.toJSON(data),
                    success: function (result) {
                        getScene().replaceWith(result);
                        initBricks();
                    },
                    error: function (result) {
                        $("html").replaceWith(result.responseText);
                    }
                });
            }
        });
    });

    function getScene() {
        return $("#wallScene");
    }

    function initBricks() {
        getScene().children().each(function (i, brick) {
            // initialize brick element
            initializeBrick($(brick));
        });

        getScene().sortable({
            containment: "parent",
            handle: ".brick .header",
            tolerance: "pointer",
            helper: function(e, brick) {
                var width = brick.width();
                brick.css({width: width + "px"})
                return brick;
            }
        })
        .disableSelection();
    }

    function onAddBrickButtonClicked() {
        // create brick from prototype and add into DOM
        var brick = createBrick();

        // initialize brick object
        initializeBrick(brick, {
            type: parseInt($("#brickType").val()),
            title: $("#brickTitle").val()
        });

        onProcessBrickAction(brick, "add");
    }

    // creates new brick element from prototype
    function createBrick() {
        return $("#brickPrototype").children().first().clone().appendTo(getScene());
    }

    function initializeBrick(brick, customData) {
        // parse and assign entity object to brick
        var data = $.parseJSON(brick.attr("entity-data"));

        // extend result data with custom one
        $.extend(data, customData);

        // set brick entity data
        brick.data("entity", data);

        // update brick text
        updateBrickTitle(brick);

        // handle delete button
        brick.find("a.delete").click(onDeleteBrickButtonClicked);

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

        updateBrickTitle(brick);

        onProcessBrickAction(brick, "edit");
    }

    function updateBrickTitle(brick) {
        var entity = brick.data("entity");
        brick.find(".title").text(entity.title + " (" + entity.width.toFixed(2) + ")");
    }

    function onDeleteBrickButtonClicked() {
        onProcessBrickAction($(this).closest(".brick-wrapper"), "delete");
    }

    function onProcessBrickAction(brick, action) {
        var entity = brick.data("entity");

        // if there is action set yet - just set it
        if(!entity.action) {
            entity.action = action;
            return;
        }

        // if brick is being deleted
        if(action == "delete") {
            // if it was added recently - delete it from DOM
            if(entity.action == "add") {
                brick.remove();
                return;
            }

            // otherwise mark as deleted 
            entity.action = action;
            return;
        }
    }

})();