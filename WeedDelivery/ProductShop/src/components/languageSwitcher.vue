<template>
  <div class="relative inline-block text-left">
    <div class="flex items-center space-x-2 cursor-pointer" @click="toggleDropdown">
      <font-awesome-icon icon="globe" class="text-xl" />
      <img :src="currentFlag" alt="Current Language" class="w-6 h-6 rounded-full" />
    </div>
    <transition name="fade">
      <div
          v-if="isOpen"
          class="origin-top-right absolute right-0 mt-2 w-40 rounded-md shadow-lg bg-white ring-1 ring-black ring-opacity-5"
      >
        <div class="py-1">
          <button
              v-for="lang in languages"
              :key="lang.code"
              @click="setLanguage(lang)"
              class="flex items-center w-full px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
          >
            <img :src="lang.flag" alt="" class="w-6 h-6 rounded-full mr-2" />
            {{ lang.name }}
          </button>
        </div>
      </div>
    </transition>
  </div>
</template>

<script>
import { mapActions } from 'vuex';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

export default {
  components: {
    FontAwesomeIcon,
  },
  data() {
    return {
      isOpen: false,
      languages: [
        { code: 'en', name: 'English', flag: 'https://flagcdn.com/w20/us.png' },
        { code: 'ru', name: 'Русский', flag: 'https://flagcdn.com/w20/ru.png' },
        { code: 'zh', name: '中文', flag: 'https://flagcdn.com/w20/cn.png' },
        { code: 'th', name: 'ไทย', flag: 'https://flagcdn.com/w20/th.png' },
      ],
    };
  },
  computed: {
    currentFlag() {
      const currentLang = this.languages.find(lang => lang.code === this.$i18n.locale);
      return currentLang ? currentLang.flag : '';
    },
  },
  methods: {
    ...mapActions('i18n', ['setLanguage']),
    toggleDropdown() {
      this.isOpen = !this.isOpen;
    },
    setLanguage(lang) {
      this.$i18n.locale = lang.code;
      this.isOpen = false;
    },
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
