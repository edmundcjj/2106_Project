var map;

/* initMap() */
function initMap() {
    map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: 1.3, lng: 103.8 },
        zoom: 11
    });
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