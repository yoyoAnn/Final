<template>
    <div style="overflow-y: scroll;">
        <div v-if="books.length" class="container">
            <v-card>
                <v-tabs v-model="tab" color="deep-purple-accent-4" align-tabs="center">
                    <v-tab :value="1">購物車</v-tab>
                    <v-tab :value="2">訂購人資訊</v-tab>
                </v-tabs>
                <v-window v-model="tab">
                    <v-window-item :value="1">
                        <v-row class="mt-2">
                            <v-col class="text-start">
                                <v-btn text to="/"><span class="fas fa-long-arrow-alt-left me-2"></span>繼續購物</v-btn>
                            </v-col>
                            <v-col class="text-end">
                                您的購物車裡有 {{ books.length }} 項商品
                            </v-col>
                        </v-row>

                        <table id="cartItem" class="table fixed-cell">
                            <thead>
                                <tr>
                                    <th style="width: 60px;text-align:center; " class="checkWidth">
                                        全選：<input type="checkbox" v-model="Allcheck" @change="allcheck" />
                                    </th>
                                    <th class="hidden-column">Id</th>
                                    <th class="hidden-column">會員Id</th>
                                    <th class="hidden-column">書本Id</th>
                                    <th>書籍封面</th>
                                    <th>書籍名稱</th>
                                    <th>價格</th>
                                    <th>購買數量</th>
                                    <th>小計</th>
                                    <th>動作</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(item, index) in books" :key="item.userId">
                                    <td><input type="checkbox" v-model="item.check" /></td>
                                    <td class="hidden-column">{{ item.id }}</td>
                                    <td class="hidden-column">{{ item.userId }}</td>
                                    <td class="hidden-column">{{ item.bookId }}</td>
                                    <td>
                                        <v-dialog v-model="showImageModal" max-width="70%">
                                            <v-card>
                                                <v-card-actions class="justify-end">
                                                    <v-btn icon @click="closeModal">
                                                        <v-icon>mdi-close</v-icon>
                                                    </v-btn>
                                                </v-card-actions>
                                                <v-card-text>
                                                    <img :src="`src/BooksImage/${selectedImage}`" alt="書籍圖片"
                                                        class="full-image" />
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
                                    <td> <button type="button" class="btn btn-danger" @click="removeItem(index)">移除
                                        </button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <h2>總計：{{ getprice(totalprice) }}</h2>

                    </v-window-item>

                    <v-window-item :value="2">
                        <v-row class="mb-3">

                            <v-col cols="12" md="3">
                                <v-text-field label="收件人姓名" v-model="ReceiverName" color="primary" variant="underlined"
                                    @blur="validate"></v-text-field>
                                <div class="text-danger">{{ receiverNameError }}</div>
                            </v-col>

                            <v-col cols="12" md="3">
                                <v-text-field label="收件人連絡電話" v-model="ReceiverPhone" color="primary" variant="underlined"
                                    @blur="validate"></v-text-field>
                                <div class="text-danger">{{ receiverPhoneError }}</div>
                            </v-col>

                            <v-col cols="12" md="5">
                                <v-text-field label="收件地址" v-model="ReceiverAddress" color="primary" variant="underlined"
                                    @blur="validate"></v-text-field>
                                <div class="text-danger">{{ receiverAddressError }}</div>
                            </v-col>

                            <v-col cols="12" md="3">
                                <v-text-field label="手機載具" v-model="TaxIdNum" color="primary" variant="underlined"
                                    placeholder="請以/開頭" @blur="validate"></v-text-field>
                                <div class="text-danger">{{ taxIdNumError }}</div>
                            </v-col>

                            <v-col cols="12" md="5">
                                <v-text-field label="備註" v-model="Remark" color="primary" variant="underlined"
                                    @blur="validate"></v-text-field>
                                <div class="text-danger">{{ remarkError }}</div>
                            </v-col>
                            <v-col cols="12" md="11" class="text-end">
                                <v-btn @click="handleCheckout"><span class="fa-solid fa-credit-card" beat-fade
                                        style="color: #252da7;"></span>結帳</v-btn>

                            </v-col>
                        </v-row>
                    </v-window-item>
                </v-window>
            </v-card>
        </div>

        <div v-else class="container">
            <h2>購物車無內容</h2>
        </div>
    </div>
</template>

  
<script setup>

import { Delete, Edit, Search, Share, Upload } from '@element-plus/icons-vue'
import 'bootstrap/dist/js/bootstrap.bundle.js';
import { ref, computed, onMounted } from 'vue';
import axios from 'axios';

const tab = ref(null);

const books = ref([]);

const Allcheck = ref(true);

const ReceiverName = ref('');
const ReceiverNameError = ref('');
const ReceiverPhone = ref('');
const receiverPhoneError = ref('');
const TaxIdNum = ref('');
const taxIdNumError = ref('');
const ReceiverAddress = ref('');
const receiverAddressError = ref('');
const Remark = ref('');
const remarkError = ref('');

const validate = () => {
    let valid = true;

    // 收件人姓名驗證
    if (ReceiverName.value.length > 255) {
        ReceiverNameError.value = '收件人姓名不能超過255個字';
        valid = false;
    } else {
        ReceiverNameError.value = '';
    }

    // 手機載具驗證
    if (!/^[A-Za-z0-9!@#$%^&*()_+{}\[\]:;<>,.?~\\/-]{8}$/.test(TaxIdNum.value)) {
        taxIdNumError.value = '手機載具僅能8碼的英數字符號';
        valid = false;
    } else {
        taxIdNumError.value = '';
    }

    // 手機號碼驗證
    if (ReceiverPhone.value.length !== 10) {
        receiverPhoneError.value = '手機號碼僅能十碼';
        valid = false;
    } else {
        receiverPhoneError.value = '';
    }

    // 收件地址驗證
    if (ReceiverAddress.value.length > 255) {
        receiverAddressError.value = '收件地址不能超過255個字';
        valid = false;
    } else {
        receiverAddressError.value = '';
    }

    // 備註驗證
    if (Remark.value.length > 50) {
        remarkError.value = '備註不能超過50個字';
        valid = false;
    } else {
        remarkError.value = '';
    }

    return valid;
};

const handleCheckout = () => {
    // 在此可以進行任何其他結帳相關的邏輯，例如數據驗證等。

    // 如果驗證失敗，則終止函數
    if (!validate()) {
        return;
    }

    const cartInfoData = {
        ReceiverName: ReceiverName.value,
        ReceiverPhone: ReceiverPhone.value,
        TaxIdNum: TaxIdNum.value,
        ReceiverAddress: ReceiverAddress.value,
        Remark: Remark.value
    };

    // 將資料儲存到localStorage
    localStorage.setItem('cartInfo', JSON.stringify(cartInfoData));

    // 假設想導航到另一頁面
    // router.push('/somepath'); 
};


const totalprice = computed(() => {
    return books.value.reduce((total, item) => {
        return item.check ? total + item.price * item.qty : total;
    }, 0);
});

const fetchCartData = async () => {
    try {
        const userInfo = JSON.parse(localStorage.getItem('userInfo'));
        console.log(userInfo.id);
        const data = {
            userId: userInfo.id,
        };

        const response = await axios.post('https://localhost:7261/GetCartsList/', data);
        books.value = response.data;

        // 將每個項目的check屬性設置為true
        books.value.forEach((item) => {
            item.check = true;
        });
    } catch (error) {
        console.error('Error fetching cart data:', error);
    }
};

const updateQuantity = async (item) => {
    const data = {
        id: item.id, // Assuming your item has an 'id' property
        userId: item.userId,
        qty: item.qty
    };

    try {
        await axios.put(`https://localhost:7261/api/Carts/`, data);
    } catch (error) {
        console.error('Error updating quantity:', error);
        // Rollback the change if update fails
        item.qty--;
    }
};

const removeItem = async (index) => {
    const itemId = books.value[index].id;
    console.log(itemId);
    try {
        await axios.delete(`https://localhost:7261/api/Carts/${itemId}`);
        books.value.splice(index, 1);
    }
    catch (error) {
        console.error('Error deleting item:', error);
    }
};

const allcheck = () => {
    books.value.forEach((element) => {
        element.check = Allcheck.value;
    });
};

const decreaseQuantity = (item) => {
    if (item.qty > 1) {
        item.qty--;
        updateQuantity(item);
    }
};

const increaseQuantity = (item) => {
    item.qty++;
    updateQuantity(item);
};

const getprice = (val) => {
    return '$' + val//.toFixed(2);
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


.hidden-column {
    display: none;
}

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
    width: 100%;
}

.full-image {
    width: 100%;
    height: auto;
    max-height: 100vh;
    object-fit: contain;
    cursor: pointer;
}
</style>