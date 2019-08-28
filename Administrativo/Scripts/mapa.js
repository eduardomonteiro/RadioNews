var Mapas = {
    map: null,
    geocoder: null,
    markersArray: [],
    resetMarkers: function () {
        var that = this;

        if (that.map.clearOverlays) {
            that.map.clearOverlays();
        }

        that.map.setZoom(14);
    },

    init: function (marker) {
        var markers = [];
        var that = this;
        var markerMap;
        if (marker && marker.latitude && marker.longitude) {
            markerMap = marker;
        } else {
            //-23.219496, -44.723656
            markerMap = {
                latitude: "-19.8910292",
                longitude: "-43.9918571",
                draggable: true
            };
        }
        markers.push(markerMap);
        // Inicializa os mapas
        that.initMap(markers)

    },
    initMap: function (markers) {
        var that = this;
        google.maps.event.addDomListener(window, 'load', that.configureMaps(markers));
        that.resetMarkers();
    },
    configureMaps: function (markers) {
        var that = this;
        var latDefault = -23.219496;
        var lngDefault = -44.723656;
        if (markers.length > 0) {
            latDefault = markers[0].latitude;
            lngDefault = markers[0].longitude;
        }
        // Latitude e Longitude inicial do mapa
        var myLatlng = new google.maps.LatLng(latDefault, lngDefault);
        // Opções do mapa - Zoom, latiude longitude, tipo do mapa
        var mapOptions = { zoom: 12, center: myLatlng, mapTypeId: google.maps.MapTypeId.ROADMAP };
        // Cria o mapa
        that.map = new google.maps.Map(document.getElementById('mapa'), mapOptions);

        that.geocoder = new google.maps.Geocoder();

        if (markers.length > 0) {
            that.addMakers(markers);
        }


        ////define method to clear all markers.
        //google.maps.Map.prototype.clearOverlays = function () {
        //    for (var i = 0; i < that.markersArray.length; i++) {
        //        that.markersArray[i].setMap(null);
        //    }
        //    that.markersArray.length = 0;
        //}

    },
    addMakers: function (markers) {
        var that = this;
        var infowindow = new google.maps.InfoWindow();
        $(markers).each(function (index, item) {
            var latlng = new google.maps.LatLng(item.latitude, item.longitude);
            var txtEndereco = document.getElementById('Endereco');
            that.getAddress(latlng, function (endereco) {
                $(txtEndereco).val(endereco);
            });

            var marker = new google.maps.Marker({
                position: latlng,
                map: that.map,
                draggable: item.draggable
            });

            //autocomplete com listener que ao trocar o endereço, seta ele no mapa..

            autocomplete = new google.maps.places.Autocomplete(txtEndereco);

            google.maps.event.addListener(
                autocomplete, 'place_changed', function () {
                    var place = autocomplete.getPlace();
                    if (place.geometry) {
                        var location = place.geometry.location;
                        that.map.panTo(location);
                        that.map.setZoom(12);
                        marker.setMap(that.map);
                        marker.setPosition(location);
                        console.log(marker.getPosition().lat());

                    }
                });

            //listener que ao trocar o marcador de posição, seta a lat e long aos campos e joga o endereço no campo pro usuário visualizar.
            google.maps.event.addListener(marker, 'dragend', function (event) {

                $("#Latitude").val(marker.getPosition().lat());
                $("#Longitude").val(marker.getPosition().lng());
                //                    $("#Long_Final").val(marker.getPosition().lng());

                that.getAddress(event.latLng, function (endereco) {
                    $(txtEndereco).val(endereco);
                });

                that.markersArray.push(marker);

            });
        });
        //that.autoCenter(that.markersArray);

        //var mcOptions = { gridSize: 50, maxZoom: 15 };
        //var mc = new MarkerClusterer(that.map, that.markersArray, mcOptions);

    },
    getAddress: function (latLng, callback) {

        this.geocoder.geocode({ latLng: latLng }, function (results, status) {
            if (callback) {
                callback(results[0].formatted_address);
            }
        });
    },
    autoCenter: function (markers) {
        var that = this;
        var bounds = new google.maps.LatLngBounds();

        $.each(markers, function (index, marker) {
            bounds.extend(marker.position);

        });
        that.map.fitBounds(bounds);

    }

}