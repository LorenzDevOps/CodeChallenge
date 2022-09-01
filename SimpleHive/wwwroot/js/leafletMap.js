var map = null;
let markers = {};

window.leafletMap = {
    create: function (mapId) {
        map = L.map(mapId, {
            center: [55.353267, 10.407906],
            zoom: 18
        });

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            maxNativeZoom: 19,
            maxZoom: 24,
            attribution: false
        }).addTo(map);
    },
    addOrUpdateMarker: function (id, lat, lng, objReference) {
        var marker = markers[id];
        if (marker) {
            marker.setLatLng([lat, lng]);
        } else {
            marker = L.marker([lat, lng], {
                draggable: true,
                icon: L.divIcon({
                    iconSize: L.point(18, 18),
                    iconAnchor: L.point(9, 9),
                    className: 'droneIcon'
                })
            });
            marker.bindTooltip("Drone: " + id).openTooltip();
            marker.on('dragend', function (eventArgs) {
                var latLng = eventArgs.target.getLatLng();
                if (latLng) {
                    objReference.invokeMethodAsync("NotifyDragEnd", latLng.lat, latLng.lng);
                }
            });
            marker.addTo(map);
        }

        markers[id] = marker;
    }
}