<template>
  <div class="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
    <!-- Корзина -->
    <div class="bg-white rounded-lg shadow-lg w-full sm:w-3/4 lg:w-1/2">
      <!-- Заголовок корзины -->
      <header class="flex justify-between items-center p-4 bg-gray-800 text-white rounded-t-lg">
        <h1 class="text-xl font-bold">{{ $t('basket.title') }}</h1>
        <button @click="closeCart" class="text-white hover:text-gray-300">&times;</button>
      </header>

      <!-- Список продуктов в корзине -->
      <main class="p-4 overflow-auto max-h-96">
        <div
            v-for="product in cartItems"
            :key="product.id"
            class="flex items-center justify-between p-4 mb-2 bg-white rounded-lg shadow"
        >
          <div>
            <h2 class="text-lg font-semibold">{{ product.name }}</h2>
            <p class="text-gray-700">{{ product.price }}฿ / 1gr</p>
          </div>
          <ProductCounter
              :product-id="product.id"
              :initial-quantity="product.quantity"
              @updateQuantity="updateQuantity"
          />
        </div>
      </main>

      <!-- Итоговая сумма -->
      <div class="flex justify-between items-center p-4 border-t border-gray-300 bg-gray-100">
        <span class="text-lg font-semibold">{{ $t('basket.total') }}</span>
        <span class="text-xl font-bold">{{ cartPrice }} ฿</span>
      </div>

      <!-- Нижняя панель с кнопками -->
      <footer class="flex justify-between p-4 bg-gray-200 rounded-b-lg shadow-inner">
        <button
            @click="closeCart"
            class="bg-gray-500 text-white px-6 py-2 rounded hover:bg-gray-600 transition duration-300"
        >
          {{ $t('basket.close') }}
        </button>
        <button
            @click="checkout"
            class="bg-green-500 text-white px-6 py-2 rounded hover:bg-green-600 transition duration-300"
        >
          {{ $t('basket.proceed') }}
        </button>
      </footer>
    </div>
  </div>
</template>

<script>
import ProductCounter from '../product/productCounter.vue';

export default {
  name: "CartView",
  components: {
    ProductCounter
  },
  props: {
    cartItems: {
      type: Array,
      required: true
    },
    cartPrice: {
      type: Number,
      required: true
    }
  },
  methods: {
    updateQuantity({ productId, quantity }) {
      
      if (quantity === 0) {
        if (confirm('Вы уверены, что хотите удалить этот товар из корзины?')) {
          this.$emit('update-quantity', { productId, quantity });
        }
      } else {
        this.$emit('update-quantity', { productId, quantity });
      }
    },
    closeCart() {
      this.$emit('close-cart');
    },
    checkout() {
      this.$emit('checkout');
    }
  }
}
</script>