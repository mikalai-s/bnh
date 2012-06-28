/// <reference path="jquery-1.7.1-vsdoc.js" />
/// <reference path="jquery-ui-1.8.18.js" />
/// <reference path="jquery.json-2.3.js" />
/// <reference path="jquery.validate.js" />
/// <reference path="Reorderable.js"/>


(function () {
    "use strict";

    var lockWallsCheckbox,
        lockBricksCheckbox,
        hideBricksContentCheckbox,
        originalSceneData,
        $isTemplateCheckbox,
        $titleTextbox;


    $(function () {

        lockWallsCheckbox = $("#lockWallsCheckbox");
        lockBricksCheckbox = $("#lockBricksCheckbox");
        hideBricksContentCheckbox = $("#hideBricksContentCheckbox");
        $isTemplateCheckbox = $("#IsTemplate");
        $titleTextbox = $("#Title");

        $("#addBrickButton").click(onAddBrickButtonClicked);
        $("#addWallButton").click(onAddWallButtonClicked);
        $("#viewScene").click(onViewSceneButtonClicked);
        $("#applyTemplateButton").click(onApplyTemplateButtonClicked);
        $("#saveSceneButton").click(onSaveSceneButton);

        lockWallsCheckbox.click(onLockWallsCheckbox);
        lockBricksCheckbox.click(onLockBricksCheckbox);
        hideBricksContentCheckbox.click(onHideBricksContentCheckbox);

        // trigger scene initialization
        initScene();
    });

    function initScene() {
        initWalls();

        originalSceneData = jQuery.toJSON(getSceneData());
    }

    function onSaveSceneButton() {
        var data = getSceneData();

        $.ajax({
            url: "/Scene/Save", //form.action,
            type: "POST",
            contentType: "application/json",
            async: false,
            data: jQuery.toJSON(data),
            success: function (result) {
                getScene().replaceWith(result);
                initScene();
            },
            error: function (result) {
                if (window.console) { window.console.error(result); }
            }
        });
    }

    // collect form data to submit
    function getSceneData() {
        var walls = [];

        getScene().children().each(function (i, wall) {
            wall = $(wall);

            // get entity
            var entity = wall.data("entity");
            walls[walls.length] = entity;
            
            entity.bricks = [];
            entity.order = i;

            wall.find(".brick-wrapper").each(function (j, brick) {
                brick = $(brick);

                var entity2 = brick.data("entity");
                entity.bricks[entity.bricks.length] = entity2;

                entity2.order = j;
            });
        });

        return {
            id: $("#sceneId").val(),
            ownerGuidId: $("#ownerId").val(),
            isTemplate: $isTemplateCheckbox.attr("checked") === "checked",
            title: $titleTextbox.val(),
            walls: walls
        };
    }

    function getScene() {
        return $("#scene");
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

        // reflect lock checkbox value
        //    onLockWallsCheckbox();
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
            //containment: "parent",
            handle: ".brick > .header",
            tolerance: "pointer",
            connectWith: ".wall > .content",
            placeholder: "brick-placeholder",
            forcePlaceholderSize: true,
            dropOnEmpty: true,
            helper: function (e, brick) {
                var width = brick.width();
                brick.css({ width: width + "px" });
                return brick;
            }
        })
        .disableSelection();
    }

    function onAddWallButtonClicked() {
        var wallTitle = $("#wallTitle");
        var title = wallTitle.val();

        if (!title) {
            alert("Wall title is required!");
            wallTitle.focus();
            return;
        }

        // create brick from prototype and add into DOM
        var wall = createWall();



        // initialize brick object
        initWall(wall, {
            title: title
        });

        wallTitle.val("");
    }

    function onAddBrickButtonClicked() {
        var brickTitle = $("#brickTitle");
        var title = brickTitle.val();

        if (!title) {
            alert("Brick title is required!");
            brickTitle.focus();
            return;
        }

        if (getScene().find(".wall > .content").length === 0) {
            // add wall if there no any
            initWall(createWall(), {
                title: "No Title"
            });
        }

        // create brick from prototype and add into DOM
        var brick = createBrick();

        // initialize brick object
        initializeBrick(brick, {
            typeName: $("#brickType").val(),
            title: brickTitle.val()
        });

        brickTitle.val("");
    }

    // creates new brick element from prototype
    function createWall() {
        return $("#wallPrototype").children().first().clone().appendTo(getScene());
    }

    // creates new brick element from prototype
    function createBrick() {
        return $("#brickPrototype").children().first().clone().appendTo(getScene().find(".wall > .content").first());
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
    }

    function onWallResize(event, ui, wall) {
        var width = (wall.width() / wall.parent().width() * 100);
        wall.data("entity").width = width;

        updateWallTitle(wall);
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
        $(this).closest(".wall-wrapper").remove();
    }

    function onDeleteBrickButtonClicked() {
        var brick = $(this).closest(".brick-wrapper");
        $.ajax({
            url: "/Scene/CanDeleteBrick",
            type: "POST",
            contentType: "application/json",
            async: true,
            data: jQuery.toJSON(brick.data("entity")),
            success: function (result) {
                if (result === true) {
                    brick.remove();
                } else {
                    alert("Unable to delete brick because it's in use!");
                }
            },
            error: function (result) {
                if (window.console) { window.console.error(result); }
            }
        });
    }

    function ensureSceneSaved() {
        var newData = jQuery.toJSON(getSceneData());
        var modified = (originalSceneData !== newData);

        if (modified) {
            if (!confirm("There are unsaved changes on the scene! Do you want to save them before processing?")) {
                return false;
            }

            // save wall
            onSaveSceneButton();
        }
        return true;
    }

    function onEditBrickButtonClicked() {
        var brick = $(this).closest(".brick-wrapper");
        var wall = brick.closest(".wall-wrapper");

        // get brick index to identify
        var brickIndex = brick.index();
        var wallIndex = wall.index();

        if (!ensureSceneSaved())
            return false;

        // change currently clicked a.href to redirect to correct URL
        var href = getScene()
            .find(".wall-wrapper:eq(" + wallIndex + ") .brick-wrapper:eq(" + brickIndex + ") a.edit")
            .attr("href");

        // replace clicked (old) brick href to proceed with correct url
        brick.find("a.edit").attr("href", href);

        // allow processing a.href
        return true;
    }

    function onViewSceneButtonClicked() {
        return ensureSceneSaved();
    }

    function onLockWallsCheckbox() {
        var checked = lockWallsCheckbox.attr("checked") === "checked";
        $(".wall-wrapper")
            .toggleClass("locked", checked)
            .resizable("option", "disabled", checked);
    }

    function onLockBricksCheckbox() {
        var checked = lockBricksCheckbox.attr("checked") === "checked";
        $(".brick-wrapper")
            .toggleClass("locked", checked)
            .resizable("option", "disabled", checked);
    }

    function onHideBricksContentCheckbox() {
        var checked = hideBricksContentCheckbox.attr("checked") === "checked";
        $(".brick-wrapper")
            .toggleClass("hide-content", checked);
    }

    function onApplyTemplateButtonClicked() {
        if (!confirm("Are you sure you want to replace entire scene with template data?"))
            return false;

        var data = {
            sceneId: $("#sceneId").val(),
            templateSceneId: $("#templateSceneId").val()
        };

        $.ajax({
            url: "/Scene/ApplyTemplate",
            type: "POST",
            contentType: "application/json",
            data: jQuery.toJSON(data),
            success: function (result) {
                getScene().replaceWith(result);
                initScene();
            },
            error: function (result) {
                if (window.console) { window.console.error(result); }
            }
        });
    }

})();