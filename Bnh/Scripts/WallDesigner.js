/// <reference path="jquery-1.5.1-vsdoc.js" />
/// <reference path="jquery-ui-1.8.11.js" />
/// <reference path="jquery.json-2.3.js" />
/// <reference path="jquery.validate.js" />


(function () {

    $(function () {

        $("#addBrickButton").click(onAddBrickButtonClicked);
        $("#viewWall").click(onViewWallButtonClicked);

        $("form").validate({
            submitHandler: function (form) {
                saveWall();
            }
        });

        // trigger scene initialization
        initScene();
    });

    function initScene() {
        initWalls();
    }

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
                initScene();

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

    function initWalls() {
        getScene().children().each(function (i, wall) {
            // initialize brick element
            initWall($(wall));
        });

        getScene().sortable({
            containment: "parent",
            handle: ".wall > .header",
            tolerance: "pointer",
            helper: function (e, brick) {
                var width = brick.width();
                brick.css({ width: width + "px" });
                return brick;
            }
        })
        .disableSelection();
    }

    function initWall(wall, customData) {
        // parse and assign entity object to brick
        var data = $.parseJSON(wall.attr("entity-data"));

        // extend result data with custom one
        $.extend(data, customData);

        // set brick entity data
        wall.data("entity", data);

        // update brick text
        updateWallTitle(wall);

        // handle action buttons
        wall.find(".wall > .header > a.delete").click(onDeleteWallButtonClicked);

        // make brick resizable
        wall.resizable({
            containment: 'parent',
            handles: "e",
            animateDuration: "0",
            resize: function (event, ui) { onWallResize(event, ui, wall); }
        });

        var wallContent = wall.find(".wall > .content");
        wallContent.find(".brick-wrapper").each(function (i, brick) {
            initializeBrick($(brick));
        });

        wallContent.sortable({
            containment: "parent",
            handle: ".brick > .header",
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
            resize: function (event, ui) {
                onBrickResize(event, ui, brick);
            },
            stop: function (event, ui) {
                var data = brick.data("entity");
                brick.width(data.width + "%");
            }
        });
    }

    function onBrickResize(event, ui, brick) {
        var width = (brick.width() / brick.parent().width() * 100);
        brick.data("entity").width = width;

        updateBrickTitle(brick);

        onProcessBrickAction(brick, "edited");
    }

    function onWallResize(event, ui, wall) {
        var width = (wall.width() / wall.parent().width() * 100);
        wall.data("entity").width = width;

        updateWallTitle(wall);

        // onProcessBrickAction(brick, "edited");
    }

    function updateWallTitle(wall) {
        var entity = wall.data("entity");
        wall.find(".wall > .header > .title").text(entity.title + " (" + entity.width.toFixed(2) + ")");
    }

    function updateBrickTitle(brick) {
        var entity = brick.data("entity");
        brick.find(".title").text(entity.title + " (" + entity.width.toFixed(2) + ")");
    }


    function onDeleteWallButtonClicked() {
        alert("No handler specified");
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

    function ensureWallSaved() {
        var entity = getSceneData($("form")[0]);
        var modified = (entity.added != null && entity.added.length > 0)
            || (entity.edited != null && entity.edited.length > 0)
            || (entity.deleted != null && entity.deleted.length > 0);

        if (modified && !confirm("There are unsaved changes on the wall! Do you want to save them before processing?"))
            return false;

        // save wall
        saveWall();

        return true;
    }

    function onEditBrickButtonClicked() {
        var brick = $(this).closest(".brick-wrapper");

        // get brick index to identify
        var index = brick.index();

        if (!ensureWallSaved())
            return false;

        // change currently clicked a.href to redirect to correct URL
        var href = $(getScene().find(".brick-wrapper")[index]).find("a.edit").attr("href");
        brick.find("a.edit").attr("href", href);

        // allow processing a.href
        return true;
    }

    function onViewWallButtonClicked() {
        return ensureWallSaved();
    }

})();