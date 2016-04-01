var map;
var directionsService = new google.maps.DirectionsService();

/* initMap() */
function initMap() {
    var directionsService = new google.maps.DirectionsService;
    var directionsDisplay = new google.maps.DirectionsRenderer;
    map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: 1.3, lng: 103.8 },
        zoom: 11
    });
    directionsDisplay.setMap(map);

}


/* getItemMap() */
/* Provide latitude and longitude from database and show it on the map, retrieving the address to display as text as well. */
function getItemMap(latitude, longitude) {
    map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: latitude, lng: longitude },
        zoom: 8
    });
    var geocoder = new google.maps.Geocoder;
    //var infowindow = new google.maps.InfoWindow;
    var latlng = { lat: latitude, lng: longitude };
    geocoder.geocode({ 'location': latlng }, function (results, status) {
        if (status === google.maps.GeocoderStatus.OK) {
            if (results[1]) {
                map.setZoom(17);
                var marker = new google.maps.Marker({
                    position: latlng,

                    map: map
                });
                //infowindow.setContent(results[1].formatted_address);
                //infowindow.open(map, marker);
                document.getElementById('addressText').innerText = results[1].formatted_address;
            } else {
                window.alert('No results found');
            }
        } else {
            window.alert('Geocoder failed due to: ' + status);
        }
    });
}

function calcRoute(start, end) {
    var request = {
        origin: start,
        destination: end,
        travelMode: google.maps.TravelMode.DRIVING
    };
    directionsService.route(request, function (result, status) {
        if (status == google.maps.DirectionsStatus.OK) {
            directionsDisplay.setDirections(result);
        }
    });
}

function plotMap(items){
    var directionsService = new google.maps.DirectionsService();

    var renderOptions = { draggable: true };
    var directionDisplay = new google.maps.DirectionsRenderer(renderOptions);

    //set the directions display service to the map
    directionDisplay.setMap(map);
    //set the directions display panel
    //panel is usually just and empty div.
    //This is where the turn by turn directions appear.
    directionDisplay.setPanel(document.getElementById("directionsPanel"));

    //build the waypoints
    //free api allows a max of 9 total stops including the start and end address
    //premier allows a total of 25 stops.
    var waypoints = [];
    if (items.length > 2) {
        for (var i = 1; i < items.length-1; i++) {
            var address = items[i];
            if (address !== "") {
                waypoints.push({
                    location: address,
                    stopover: true
                });
            }
        }
    }
    console.log(waypoints);

    var startOrigin = items[0];
    var originCoor = startOrigin.split(",");
    console.log(originCoor[0]);
    console.log(originCoor[1]);
    var originAddress = new google.maps.LatLng(originCoor[0], originCoor[1]);

    //set the starting address and destination address
    var endOrigin = items[items.length - 1];
    var destCoor = endOrigin.split(",");
    var destinationAddress = new google.maps.LatLng(destCoor[0], destCoor[1]);

    //build directions request
    var request = {
        origin: originAddress,
        destination: destinationAddress,
        waypoints: waypoints, //an array of waypoints
        optimizeWaypoints: true, //set to true if you want google to determine the shortest route or false to use the order specified.
        travelMode: google.maps.DirectionsTravelMode.DRIVING
    };

    //get the route from the directions service
    directionsService.route(request, function (response, status) {
        console.log(request);
        if (status == google.maps.DirectionsStatus.OK) {
            directionDisplay.setDirections(response);
        }
        else {
            //handle error
        }
    });
}

function calculateDistanceTime(items) {
    var startOrigin = [];
    for (j = 0; j<items.length-1; j++){
        startOrigin.push(items[j]);
    }
    console.log(startOrigin);

    var destLocations = [];
    for (i = 1; i < items.length; i++){
        destLocations.push(items[i]);
    }
    console.log(destLocations);

    var distanceDiv = document.getElementById('distanceOutput');
    var travelDiv = document.getElementById('travelOutput');

    var service = new google.maps.DistanceMatrixService;
    service.getDistanceMatrix({
        origins: startOrigin,
        destinations: destLocations,
        travelMode: google.maps.TravelMode.DRIVING,
        unitSystem: google.maps.UnitSystem.METRIC,
        avoidHighways: false,
        avoidTolls: false
    }, function (response, status) {
        if (status !== google.maps.DistanceMatrixStatus.OK) {
            distanceDiv.innerHTML = "-";
            travelDiv.innerHTML = "-";
        } else {
            var originList = response.originAddresses;
            var destinationList = response.destinationAddresses;
            distanceDiv.innerHTML = '';
            travelDiv.innerHTML = '';

            var totalDistance = 0;
            var totalTravelTime = 0;
            for (var i = 0; i < originList.length; i++) {
                var results = response.rows[i].elements;

                //console.log(results);
                totalDistance = totalDistance + results[i].distance.value;
                totalTravelTime = totalTravelTime + results[i].duration.value;
                distanceDiv.innerHTML = (totalDistance / 1000).toFixed(2) + " km";
                travelDiv.innerHTML = Math.round(totalTravelTime / 60) + " mins";
            }
        }
    });
}
