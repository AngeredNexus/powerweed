// store/modules/cart.js
const state = {
    cart: {},
};

const getters = {
    cartItems: (state, getters, rootState) => {
        return Object.entries(state.cart).map(([productId, quantity]) => {
            const product = rootState.products.items.find(p => p.id === productId);
            return { ...product, quantity };
        });
    },
    totalPrice: (state, getters) => {
        return getters.cartItems.reduce((sum, item) => sum + item.price * item.quantity, 0);
    },
    cartBase: (state, getters, rootState) => {
        return Object.entries(state.cart).map(([productId, quantity]) => {
            return {id: productId, amount: quantity}
        })
    }
};

const mutations = {
    addToCart(state, { productId, quantity }) {
        if (quantity === 0) {
            const cart = {...state.cart};
            delete cart[productId];
            state.cart = cart;
        } else {
            state.cart = {...state.cart, [productId]: quantity};
        }
    },
    clearCart(state) {
        state.cart = {};
    }
};

const actions = {
    updateCart({ commit }, payload) {
        commit('addToCart', payload);
    },
    clearCart({ commit }) {
        commit('clearCart');
    }
};

export const cart = {
    namespaced: true,
    state,
    getters,
    mutations,
    actions
};
