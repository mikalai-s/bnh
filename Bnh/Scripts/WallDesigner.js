/// <reference path="jquery-1.5.1-vsdoc.js" />
/// <reference path="jquery-ui-1.8.11.js" />
/// <reference path="jquery.json-2.3.js" />
/// <reference path="jquery.validate.js" />


(function () {

    $(function () {

        $("#addRowButton").click(onAddRowButtonClicked);
 
        $("form").validate({
            submitHandler: function(form) {
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
                        Type: 1 };
                }

                $.ajax({
                    url: form.action,
                    type: "POST",
                    contentType: "application/json",
                    data: jQuery.toJSON(data),
                    success: function(result){
                    },
                    error: function(result) {
                    },
                });
            },
        });

    });


    function onAddRowButtonClicked() {

        var wall = $("#wallScene");
        var brick = $("<div class='brick-wrapper'><div class='brick'/></div>");
        wall.append(brick);

        updateBrickText(brick);

        brick.resizable({
            containment: 'parent',
            handles: "e",
            start: function (event, ui) { onBrickResizeStart(event, ui, wall, brick) },
            resize: function (event, ui) { onBrickResize(event, ui, wall, brick) },
            stop: function (event, ui) { onBrickResizeStop(event, ui, wall, brick) }
        });

    }

    function onBrickResizeStart(event, ui, wall, brick) {

    }

    function onBrickResize(event, ui, wall, brick) {
        updateBrickText(brick);
    }

    function onBrickResizeStop(event, ui, wall, brick) {
    }

    function updateBrickText(brick) {
        var width = (brick.width() / brick.parent().width() * 100);
        $.data(brick[0], "width", width);
        brick.children().first().text(width.toFixed(2));
    }    

})();