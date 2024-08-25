<template>
  <div>
    <input
        id="autocomplete"
        type="text"
        placeholder="Введите адрес"
        class="border p-2 w-full"
    />
    <div id="map" class="w-full h-64 mt-2"></div>
  </div>
</template>

<script setup>

import { onMounted } from 'vue';

onMounted(() => {
  const input = document.getElementById('autocomplete');
  const map = new google.maps.Map(document.getElementById('map'), {
    center: { lat: 7.8804, lng: 98.3923 }, // Координаты острова Пхукет
    zoom: 13,
  });

  const autocomplete = new google.maps.places.Autocomplete(input, {
    bounds: new google.maps.LatLngBounds(
        new google.maps.LatLng(7.7680, 98.2820), // Границы острова Пхукет
        new google.maps.LatLng(8.1000, 98.4950)
    ),
    strictBounds: true, // Ограничение поиска только на Пхукет
  });

  autocomplete.bindTo('bounds', map);
  autocomplete.addListener('place_changed', () => {
    const place = autocomplete.getPlace();
    if (place.geometry) {
      map.setCenter(place.geometry.location);
      map.setZoom(15);
    }
  });
});
</script>

<style scoped>
/* Ваши стили для карты и ввода */
#map {
  height: 200px;
}
</style>