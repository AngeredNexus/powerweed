<template>
  <div class="min-h-screen flex flex-col">
    <header class="flex justify-between items-center p-4 bg-gray-800 text-white shadow-md">
      <div class="flex text-xl font-bold justify-between">
        <img src="/powerweed.logo.png" alt="Logo" class="h-8">
        <LanguageSwitcher class="ml-8 mt-1" />
      </div>
      <div class="flex items-center space-x-4">
        <div v-if="isLoaded && isHaveDiscount">
          <span class="line-through text-base"> {{ cartPrice }} </span>
          <span class="text-lg ml-2 text-amber-300"> {{ discountedPrice }}฿</span>
        </div>
        <div v-else>
          <span v-if="isLoaded"> {{ cartPrice }}฿</span>
        </div>
        <button @click="toggleCart" class="bg-gray-700 p-2 rounded-full hover:bg-gray-600 transition duration-300">
          🛒
        </button>
      </div>
    </header>

    <main class="flex-1 p-4 overflow-hidden">
      <transition name="fade" mode="out-in">
        <div :key="currentViewComponent">
          <ProductList v-if="currentView === 'ProductList' && isLoaded" :products.sync="products" @select-product="selectProduct"/>
          <ProductView v-if="currentView === 'ProductView'" :product.sync="selectedProduct" @update-quantity="updateQuantity" @backView="selectedProductExit"/>
        </div>
      </transition>
    </main>

    <transition name="fade">
      <Cart v-if="isCartVisible && isLoaded" :cartItems.sync="cartItems" :cartPrice.sync="isHaveDiscount ? discountedPrice : cartPrice" @update-quantity="updateQuantity" @close-cart="toggleCart" @checkout="proceedToCheckout"/>
    </transition>
  </div>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
import ProductList from '../product/productList.vue';
import ProductView from '../product/productView.vue';
import Cart from '../order/basketView.vue';
import router from "../router/router";
import api from "../../common/services/api";
import LanguageSwitcher from "@/components/languageSwitcher.vue";

export default {
  name: 'shopView',
  components: {
    LanguageSwitcher,
    ProductList,
    ProductView,
    Cart
  },
  data() {
    return {
      currentView: 'ProductList',
      selectedProduct: null,
      isCartVisible: false,
      isLoaded: false,
      discounted: []
    };
  },
  computed: {
    ...mapGetters('cart', {
      cartPrice: "totalPrice",
      cartItems: "cartItems",
      cartForDiscount: "cartBase"
    }),
    ...mapGetters('products', {
      products: "allProducts"
    }),
    currentViewComponent() {
      return this.currentView === 'ProductList' ? ProductList : ProductView;
    },
    discountedPrice() {
      return this.discounted.reduce((sum, item) => sum + item.discountedPrice * item.productOrder.Amount, 0);
    },
    isHaveDiscount() {
      return this.discountedPrice < this.cartPrice;
    }
  },
  methods: {
    ...mapActions('cart', ['updateCart']),
    ...mapActions('products', ['fetchProducts']),
    toggleCart() {
      this.isCartVisible = !this.isCartVisible;
    },
    selectProduct(product) {
      this.selectedProduct = { ...product };
      this.currentView = 'ProductView';
    },
    selectedProductExit() {
      this.selectedProduct = null;
      this.currentView = 'ProductList';
    },
    async updateQuantity({ productId, quantity }) {
      await this.updateCart({ productId, quantity });
      const resp = await api.post("order/lookup", this.cartForDiscount);
      this.discounted = resp.data;
    },
    proceedToCheckout() {
      router.push({ name: 'order' });
    },
    async loadProducts() {
      await this.fetchProducts();
    }
  },
  async created() {
    await this.loadProducts();
    this.currentView = 'ProductList';
    this.isLoaded = true;
  }
};
</script>
