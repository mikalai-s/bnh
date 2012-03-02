﻿/// <reference path="jquery-1.5.1-vsdoc.js" />
/// <reference path="jquery-ui-1.8.11.js" />
/// <reference path="jquery.json-2.3.js" />
/// <reference path="jquery.validate.js" />


(function () {

    $(function () {

        $("#addBrickButton").click(onAddBrickButtonClicked);
        $("#viewWall").click(onViewWallButtonClicked);

        initBricks();

        $("form").validate({
            submitHandler: function (form) {
                saveWall();
            }
        });
    });

    function saveWall(done) {
        var form = $("form")[0];
        var data = getSceneData(form);

        $.ajax({
            url: form.action,
            type: "POST",
            contentType: "application/json",
            async: false,
            data: jQuery.toJSON(data),
            success: function (result) {
                getScene().replaceWith(result);
                initBricks();

                if (done) {
                    done();
                }
            },
            error: function (result) {
                document.write(result.responseText);
                document.close();
            }
        });
    }

    // collect form data to submit
    function getSceneData(form) {
        var data = {
            wall: {
                Id: form["Id"].value,
                OwnerId: form["OwnerId"].value,
                Title: form["Title"].value
            },
            added: [],
            edited: [],
            deleted: []
        };

        getScene().children().each(function (i, brick) {
            // get entity
            var entity = $(brick).data("entity");

            // check for changed order
            if (entity.order != i) {
                entity.order = i;
                onProcessBrickAction($(brick), "edited");
            }

            // put brick into appropriate action collection if need
            if (entity.action) {
                var bricks = data[entity.action];
                bricks[bricks.length] = entity;
            }
        });

        return data;
    }

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
            helper: function (e, brick) {
                var width = brick.width();
                brick.css({ width: width + "px" });
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

        // move deleted bricks to the end of the wall
        getScene().find(".brick-wrapper:hidden").appendTo(getScene());

        // handle edit button specifically for non-saved bricks
        //brick.find("a.edit").click(onEditNonSavedBrickClicked);

        onProcessBrickAction(brick, "added");
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

        // handle action buttons
        brick.find("a.edit").click(onEditBrickButtonClicked);
        brick.find("a.delete").click(onDeleteBrickButtonClicked);

        // make brick resizable
        brick.resizable({
            containment: 'parent',
            handles: "e",
            resize: function (event, ui) { onBrickResize(event, ui, brick); }
        });
    }

    function onBrickResize(event, ui, brick) {
        var width = (brick.width() / brick.parent().width() * 100);
        brick.data("entity").width = width;

        updateBrickTitle(brick);

        onProcessBrickAction(brick, "edited");
    }

    function updateBrickTitle(brick) {
        var entity = brick.data("entity");
        brick.find(".title").text(entity.title + " (" + entity.width.toFixed(2) + ")");
    }

    function onDeleteBrickButtonClicked() {
        onProcessBrickAction($(this).closest(".brick-wrapper"), "deleted");
    }

    function onProcessBrickAction(brick, action) {
        var entity = brick.data("entity");

        // if brick is being deleted
        if (action == "deleted") {
            // if it was added recently - delete it from DOM
            if (entity.action == "added") {
                brick.remove();
                return;
            }

            // otherwise mark as deleted 
            entity.action = action;

            // movee brick to the end of the list (to make brick orders to be correct)
            brick.appendTo(brick.parent());

            // hide brick (we need it to submit action to the server)
            brick.hide();

            return;
        }

        // if there is no action set yet - just set it
        if (!entity.action) {
            entity.action = action;
            return;
        }
    }

    function onEditBrickButtonClicked() {
        var brick = $(this).closest(".brick-wrapper");
        var entity = brick.data("entity");

        if (entity.action == "added" || entity.action == "edited")
            if (!confirm("This brick is no saved yet! Do you want to save entire wall before processing?"))
                return false;

        // get brick index to identify
        var index = brick.index();

        // we are sure that saving is not async
        saveWall(function () {
            // change currently clicked a.href to redirect to correct URL
            var href = $(getScene().find(".brick-wrapper")[index]).find("a.edit").attr("href");
            brick.find("a.edit").attr("href", href);
        });

        // allow processing a.href
        return true;
    }

    function onViewWallButtonClicked() {
        var entity = getSceneData($("form")[0]);
        if ((entity.added != null && entity.added.length > 0)
            || (entity.edited != null && entity.edited.length > 0)
            || (entity.deleted != null && entity.deleted.length > 0)) {
            
            // in there are unsaved changes
            if (!confirm("There are unsaved changes on the wall! Do you want to save them before processing?"))
                return false;

            // save wall
            saveWall();
        }
        return true;
    }

})();