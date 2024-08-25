<template>
  <div class="relative w-64 h-96 flex items-center justify-center bg-gray-200 rounded-lg overflow-hidden shadow-lg">
    <!-- Левая область для пролистывания -->
    <div
        class="absolute left-0 top-0 bottom-0 w-1/4 flex items-center justify-center cursor-pointer bg-gradient-to-r from-gray-800/30 to-transparent hover:bg-gray-800/50 transition duration-300"
        @click="prevImage"
    >
      <span class="text-white text-2xl font-bold">&lt;</span>
    </div>

    <!-- Изображение -->
    <img :src="currentImage" alt="Image" class="object-cover h-full w-full" />

    <!-- Правая область для пролистывания -->
    <div
        class="absolute right-0 top-0 bottom-0 w-1/4 flex items-center justify-center cursor-pointer bg-gradient-to-l from-gray-800/30 to-transparent hover:bg-gray-800/50 transition duration-300"
        @click="nextImage"
    >
      <span class="text-white text-2xl font-bold">&gt;</span>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue';

const props = defineProps({
  images: {
    type: Array,
    required: true
  }
});

const currentIndex = ref(0);

const currentImage = computed(() => props.images[currentIndex.value]);

const prevImage = () => {
  currentIndex.value = (currentIndex.value - 1 + props.images.length) % props.images.length;
};

const nextImage = () => {
  currentIndex.value = (currentIndex.value + 1) % props.images.length;
};
</script>