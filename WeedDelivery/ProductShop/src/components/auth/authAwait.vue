<template>

  <div>
    <!-- Шапка -->
    <header class="fixed top-0 left-0 w-full bg-gray-800 text-white shadow-md z-50">
      <div class="flex justify-between items-center p-4 max-w-screen-xl mx-auto">
        <div class="text-xl font-bold flex items-center">
          <Header/>
        </div>
      </div>
    </header>

    <div class="flex flex-col items-center justify-center min-h-screen bg-gray-100 p-6">
      <!-- Уведомление -->
      <transition name="fade" @after-leave="hideNotification">
        <div v-if="notificationVisible"
             class="fixed bottom-4 left-1/2 transform -translate-x-1/2 bg-green-600 text-white p-3 rounded-lg shadow-lg">
          {{ $t('loginWait.notification') }}
        </div>
      </transition>

      <!-- Поле с кодом -->
      <div class="bg-white shadow-md rounded-lg p-6 mb-4 max-w-sm w-full">
        <input
            type="text"
            :value="code"
            readonly
            @click="copyCode"
            class="text-center text-lg font-bold bg-gray-200 border border-gray-300 rounded-lg p-4 cursor-pointer"
        />
      </div>

      <!-- Инструкция -->
      <p class="text-center text-gray-700 mb-4 px-4">
        {{ $t('loginWait.instructions') }}
      </p>

      <!-- Блок иконок-ссылок -->
      <div class="flex space-x-6 mt-4">
        <a href="https://t.me/your_bot" target="_blank" class="text-blue-600 hover:text-blue-800 transition-colors">
          <i class="fab fa-telegram fa-2x"></i>
        </a>
        <a href="https://wa.me/your_number" target="_blank"
           class="text-green-600 hover:text-green-800 transition-colors">
          <i class="fab fa-whatsapp fa-2x"></i>
        </a>
        <a href="https://line.me/R/ti/p/@your_line_id" target="_blank"
           class="text-blue-400 hover:text-blue-600 transition-colors">
          <i class="fab fa-line fa-2x"></i>
        </a>
      </div>
    </div>
  </div>
</template>

<script>
import {mapActions} from 'vuex';
import router from "@/components/router/router";
import Header from "@/components/header.vue";

export default {
  components: {Header},
  data() {
    return {
      code: '', // Код авторизации из URL
      notificationVisible: false,
      loading: true,
      error: null,
    };
  },
  methods: {
    ...mapActions('auth', ["setToken"]),

    async fetchToken(code) {
      try {

        this.$store.dispatch("auth/login", code).then(
            async () => {
              await router.push({name: "store"});
            },
            (error) => {
              this.loading = false;
              this.message =
                  (error.response &&
                      error.response.data &&
                      error.response.data.message) ||
                  error.message ||
                  error.toString();
            }
        );
      } catch (err) {
        this.error = err.response?.data?.message || err;
      } finally {
        this.loading = false;
      }
    },
    copyCode() {
      navigator.clipboard.writeText(this.code).then(() => {
        this.notificationVisible = true;
        setTimeout(() => {
          this.notificationVisible = false;
        }, 3000); // Уведомление пропадет через 3 секунды
      });
    },
    hideNotification() {
      this.notificationVisible = false;
    },
  },
  async mounted() {

    const code = this.$route.query.code;

    if (code) {
      this.code = code;
      await this.fetchToken(code);
    } else {
      this.error = 'Некорректный код в URL';
      this.loading = false;
    }
  },
};
</script>

<style>
.fade-enter-active, .fade-leave-active {
  transition: opacity 0.5s;
}

.fade-enter, .fade-leave-to {
  opacity: 0;
}
</style>
