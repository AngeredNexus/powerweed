// store/modules/products.js
import api from "../services/api";

const state = {
    items: [], // Здесь хранятся продукты
};

const getters = {
    allProducts: state => state.items.slice(),
    getProductById: state => id => state.items.find(p => p.id === id)
};

const mutations = {
    setProducts(state, products) {
        state.items = [...products];
        console.log("Items setted!");
    },
    addProduct(state, product) {
        state.items = [...state.items, product];
    },
    updateProduct(state, updatedProduct) {
        const index = state.items.findIndex(product => product.id === updatedProduct.id);
        if (index !== -1) {
            state.items.splice(index, 1, updatedProduct);
        }
    },
    removeProduct(state, productId) {
        state.items = state.items.filter(product => product.id !== productId);
    }
};

const actions = {
    async fetchProducts({ commit }) {
        try {
            const response = await api.get('/product/all');
            commit('setProducts', response.data.products);
        } catch (error) {
            console.error('Ошибка загрузки продуктов:', error);
        }
    },
    addProduct({ commit }, product) {
        commit('addProduct', product);
    },
    updateProduct({ commit }, product) {
        commit('updateProduct', product);
    },
    removeProduct({ commit }, productId) {
        commit('removeProduct', productId);
    }
};

export const products = {
    namespaced: true,
    state,
    getters,
    mutations,
    actions
};
