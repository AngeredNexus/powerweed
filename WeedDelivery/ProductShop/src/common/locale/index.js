import ru from './locales/ru.json';
import en from './locales/en.json';
import zh from './locales/zh.json';
import th from './locales/th.json';

const messages = {
    ru,
    en,
    zh,
    th
};

import { createI18n } from 'vue-i18n';

const i18n = createI18n({
    legacy: false,
    locale: 'en', // Язык по умолчанию
    fallbackLocale: 'ru', // Резервный язык
    messages,
});

export default i18n;