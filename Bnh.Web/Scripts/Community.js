function ZoneViewModel() {
    var self = this;
    self.name = ko.observable();
    self.communities = ko.observableArray();
}

function CommuntiyViewModel(id, name, urlId, gpsLocation, gpsBounds) {
    var self = this;
    self.id = ko.observable(id);
    self.name = ko.observable(name);
    self.urlId = ko.observable(urlId);
    self.hasLake = ko.observable();
    self.distanceToCityCenter = ko.observable();
    self.deleteUrl = ko.computed(function () {
        return "/Calgary/Community/" + self.id() + "/Delete";
    });
    self.deleteTitle = ko.computed(function () {
        return "Delete " + self.name();
    });
    self.detailsUrl = ko.computed(function () {
        return "/Calgary/Community/" + self.urlId();
    });
    if (gpsBounds != null) {     
        self.gpsBounds = $.map(gpsBounds, Global.Map.deserializeCoordinates);
        self.associatedMapObject = Global.Map.addPolygon(self.gpsBounds, '<a href="' + self.detailsUrl() + '">' + self.name() + '</a>');
    }
    self.gpsLocation = Global.Map.deserializeCoordinates(gpsLocation);

    self.communityMouseover = function () {
        Global.Map.highlightPolygon(self.associatedMapObject);
        
    };
    self.communityMouseout = function () {
        Global.Map.dehighlightPolygon(self.associatedMapObject);
    };
}

ko.bindingHandlers.infoPopUpBinding = {
    init: function (element, valueAccessor) {
        $(element).balloon({ position: "right", contents: valueAccessor(),
           css: {
                borderRadius: 0,
                 backgroundColor: 'white'
           } });
    }
 };

 ko.bindingHandlers.slideVisible = {
     update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
         // First get the latest data that we're bound to
         var value = valueAccessor();

         // Next, whether or not the supplied model property is observable, get its current value
         var valueUnwrapped = ko.utils.unwrapObservable(value);

         element = $(element);

         // Now manipulate the DOM element
         if (valueUnwrapped == true)
             element.show(); // Make the element visible
         else
             element.hide(); // Make the element invisible
         if (viewModel.associatedMapObject != null) {
             viewModel.associatedMapObject.setVisible(valueUnwrapped);
         }
     }
 };

 function CommunityFilterViewModel(config) {
     var self = this;
     self.remoteness = ko.observable(false);
     self.hasLake = ko.observable(false);
     self.hasWaterFeature = ko.observable(false);
     self.hasClubOrFacility = ko.observable(false);
     self.hasMountainView = ko.observable(false);
     self.hasParksAndPathways = ko.observable(false);
     self.hasShoppingPlaza = ko.observable(false);

     this.isCommunityVisible = function(cp) {
         var visible =
             (!(self.remoteness()) || self.remoteness() <= cp.distanceToCityCenter()) &&
                 (!(self.hasLake()) || self.hasLake() === cp.hasLake()) &&
                     (!(self.hasWaterFeature()) || self.hasWaterFeature() === cp.hasWaterFeature()) &&
                         (!(self.hasClubOrFacility()) || self.hasClubOrFacility() === cp.hasClubOrFacility()) &&
                             (!(self.hasMountainView()) || self.hasMountainView() === cp.hasMountainView()) &&
                                 (!(self.hasParksAndPathways()) || self.hasParksAndPathways() === cp.hasParksAndPathways()) &&
                                     (!(self.hasShoppingPlaza()) || self.hasShoppingPlaza() === cp.hasShoppingPlaza()) &&
                                         true;
         return visible;
     };
 }

function CommunityPageViewModel() {
    var self = this;
    self.zones = ko.observableArray();
    self.filter = ko.observable(new CommunityFilterViewModel());
    self.initialize = function () {
        $.getJSON("/Community/Zones", function (data) {
            var mappedZones = $.map(data, function (item) {
                var zone = new ZoneViewModel();
                zone.name(item.Name);
                zone.communities($.map(item.Communities, function (item) {
                    var community = new CommuntiyViewModel(item.Id, item.Name, item.UrlId, $.parseJSON(item.GpsLocation), $.parseJSON(item.GpsBounds));
                    community.hasLake(item.HasLake);
                    community.distanceToCityCenter(item.Remoteness);
                    return community;
                }));
                return zone;
            });
            self.zones(mappedZones);
        });
    };
    self.initialize();
}

