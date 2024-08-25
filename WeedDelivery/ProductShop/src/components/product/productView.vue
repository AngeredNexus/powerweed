<template>
  <div class="flex w-full flex-col items-center justify-center min-h-screen bg-gray-100 p-6">
    <!-- Общая обертка -->
    <div class="flex flex-col items-center">
      <!-- Компонент фотографий -->
      <div class="relative mb-6">
        <PhotoGallery :images="product.images"/>

        <!-- Полупрозрачная плашка с именем продукта -->
        <div
            class="absolute w-full top-0 left-0 bg-black/50 text-amber-200 py-2 px-4 rounded-br-lg text-center text-lg font-semibold">
          {{ product.name }}
        </div>
      </div>

      <!-- Цена продукта -->
      <div class="text-2xl font-bold text-gray-800 mb-4 text-center">
        {{ product.price }} ₽
      </div>

      <!-- Таблица характеристик -->
      <div class="w-full bg-white rounded-lg shadow-lg p-4 mb-4">
        <table class="w-full text-left">
          <tbody>
          <tr v-for="(value, key) in reducedProductSpecifications" :key="key" class="border-b">
            <td class="py-2 px-4 font-medium text-gray-600">{{ key }}</td>
            <td class="py-2 px-4 text-gray-800">{{ value }}</td>
          </tr>
          </tbody>
        </table>
      </div>

      <div class="relative w-full h-1/2 bg-white">
        <ProductCounter :product-id="product.id" :key="product.id"
                        @update-quantity="this.$emit('update-quantity', { product })"/>
      </div>

      <!-- Кнопка "Назад" -->
      <button
          @click="goBack"
          class="bg-gray-800 text-white px-6 py-2 rounded-lg hover:bg-gray-700 transition duration-300 shadow-lg"
      >
        {{ $t('general.back') }}
      </button>
    </div>
  </div>
</template>

<script>
import PhotoGallery from './photoViewer.vue';
import ProductCounter from "../product/productCounter.vue";

export default {
  name: 'ProductView',
  props: {
    product: {
      type: Object,
      required: true
    }
  },
  components: {
    ProductCounter,
    PhotoGallery
  },
  computed: {
    reducedProductSpecifications() {
      
      let spec = this.product.specification[0];
      
      return {
        "THC": spec.THC,
        "TGK": spec.TGK,
        "STRAIN": spec.Strain,
        "EFFECT": spec.Effekt,
        "USE TIME": spec.Time
      }
    }
  },
  methods: {
    goBack() {
      this.$emit('backView'); // Исправлено: использовать $emit для события
    }
  }
}
</script>
