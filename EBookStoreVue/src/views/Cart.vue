<template>
    <div style="overflow-y: scroll;">
        <div v-if="books.length" class="container">
            <h5 class="mt-2">
                <router-link class="text-body" to="/"><i class="fas fa-long-arrow-alt-left me-2"></i>繼續購物</router-link>
            </h5>

            <div>
                <p>
                    您的購物車裡有 {{ books.length }} 項商品
                </p>
            </div>

            <table class="table fixed-cell">
                <thead>
                    <tr>
                        <th style="width: 60px;text-align:center; " class="checkWidth">
                            全選：<input type="checkbox" v-model="Allcheck" @change="allcheck" />
                        </th>
                        <!-- <th>編號</th> -->
                        <th>書籍名稱</th>
                        <th>書籍簡介</th>
                        <th>價格</th>
                        <th>購買數量</th>
                        <th>小計</th>
                        <th>操作</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(item, index) in books" :key="item.userId">
                        <td><input type="checkbox" v-model="item.check" /></td>
                        <!-- <td>{{ item.userId }}</td> -->
                        <td>
                            <v-dialog v-model="showImageModal" max-width="70%">
                                <v-card>
                                    <v-card-actions class="justify-end">
                                        <v-btn icon @click="closeModal">
                                            <v-icon>mdi-close</v-icon>
                                        </v-btn>
                                    </v-card-actions>
                                    <v-card-text>
                                        <img :src="`src/BooksImage/${selectedImage}`" alt="書籍圖片" class="full-image" />
                                    </v-card-text>
                                </v-card>
                            </v-dialog>
                            <img :src="`src/BooksImage/${item.image}`" alt="書籍圖片" class="book-image body-sm"
                                @click="showModal(item.image)" />
                        </td>

                        <td>{{ item.name }}</td>
                        <td>{{ getprice(item.price) }}</td>
                        <td style="white-space: nowrap;">
                            <button @click="decreaseQuantity(item)" :disabled="item.qty <= 1"
                                class="quantityButton">-</button>
                            {{ item.qty }}
                            <button @click="increaseQuantity(item)" class="quantityButton">+</button>
                        </td>
                        <td>{{ getprice(item.price * item.qty) }}</td>
                        <td><button @click="removeItem(index)">移除</button></td>
                    </tr>
                </tbody>
            </table>
            <h2>總計：{{ getprice(totalprice) }}</h2>
        </div>
        <div v-else class="container">
            <h2>購物車無內容</h2>
        </div>
    </div>
</template>
  
<script setup>
import 'bootstrap/dist/js/bootstrap.bundle.js';
import { ref, computed, onMounted } from 'vue';
import axios from 'axios';


const books = ref([]);

const Allcheck = ref(true);

const totalprice = computed(() => {
    return books.value.reduce((total, item) => {
        return item.check ? total + item.price * item.qty : total;
    }, 0);
});

const fetchCartData = async () => {
    try {
        const response = await axios.get('https://localhost:7261/api/Carts');
        books.value = response.data;

        // 將每個項目的check屬性設置為true
        books.value.forEach((item) => {
            item.check = true;
        });
    } catch (error) {
        console.error('Error fetching cart data:', error);
    }
};

const removeItem = (index) => {
    books.value.splice(index, 1);
};

const allcheck = () => {
    books.value.forEach((element) => {
        element.check = Allcheck.value;
    });
};

const decreaseQuantity = (item) => {
    if (item.qty > 1) {
        item.qty--;
    }
};

const increaseQuantity = (item) => {
    item.qty++;
};

const getprice = (val) => {
    return '$' + val.toFixed(2);
};

onMounted(() => {
    fetchCartData();
});

const showImageModal = ref(false);
const selectedImage = ref('');

const showModal = (image) => {
    selectedImage.value = image;
    showImageModal.value = true;
};

const closeModal = () => {
    showImageModal.value = false;
};
</script>
  
<style scoped>
/* 你的CSS樣式 */
table {
    border: 1px solid #e9e9e9;
    border-collapse: collapse;
    border-spacing: 0;
}

#app {
    margin: 0 auto;
}

th,
td {
    padding: 8px 16px;
    border: 1px solid #e9e9e9;
    text-align: left;
    width: 100px;
    text-align: center;
}

th {
    background-color: #f7f7f7;
    color: #5c6b77;
    font-weight: 600;
    white-space: nowrap;
}

.checkWidth {
    width: 60px !important;
}

.quantityButton {
    padding: 5px 10px;
    background-color: #007bff;
    color: #ffffff;
    border: none;
    cursor: pointer;
    font-size: 16px;
}

.quantityButton:hover {
    background-color: #0056b3;
}

img {
    object-fit: cover;
    width: 50%;
}

.full-image {
    width: 100%;
    height: auto;
    max-height: 100vh;
    object-fit: contain;
    cursor: pointer;
}
</style>