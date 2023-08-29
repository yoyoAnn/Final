import { defineStore } from 'pinia';
import axios from 'axios';
import { ref } from 'vue';

//const books = ref([]);
//const userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
//console.log(userInfo.id);
//const data = {
//    userId: userInfo.id,
//};

//const response = await axios.post('https://localhost:7261/GetCartsList/', data);
//books.value = response.data;

export const useCartStore = defineStore({
    id: 'cart',
    state: () => ({
        cartItemsCount: 0,
    }),
    actions: {
        updateCartItemsCount(count: number) {
            const userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
            console.log(userInfo.id);
            const data = {
                userId: userInfo.id,
            };
            fetch('https://localhost:7261/GetCartsList/', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(data),
            })
                .then(response => response.json())
                .then(responseData => {
                    this.cartItemsCount = responseData.length;
                })
        },
    },
});