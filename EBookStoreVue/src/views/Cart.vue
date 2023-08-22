<template>
    <div style="overflow-y: scroll;">
        <div v-if="books.length" class="container">
            <v-card>
                <v-tabs v-model="tab" color="deep-purple-accent-4" align-tabs="center">
                    <v-tab :value="1">購物車</v-tab>
                    <v-tab :value="2">收件人資訊</v-tab>
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
                                    <th class="col-check">
                                        全選：<input type="checkbox" v-model="Allcheck" @change="allcheck" />
                                    </th>
                                    <th class="hidden-column">Id</th>
                                    <th class="hidden-column">會員Id</th>
                                    <th class="hidden-column">書本Id</th>
                                    <th>書籍封面</th>
                                    <th class="col-bookName">書籍名稱</th>
                                    <th class="col-check">價格</th>
                                    <th class="col-check">購買數量</th>
                                    <th class="col-check">小計</th>
                                    <th class="col-remove">動作</th>
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
                                                        <v-icon icon="mdi:mdi-close" color="black"></v-icon>
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
                        <h2><span style="color: blue;">小計</span>+<span style="color: red;">運費</span>：<span
                                style="color: blue;">{{ getprice(totalprice)
                                }}</span> + <span style="color: red;">60</span> ={{ getprice(totalprice + 60) }} </h2>

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
                                    placeholder="(選填)請以/開頭" @blur="validate"></v-text-field>
                                <div class="text-danger">{{ taxIdNumError }}</div>
                            </v-col>

                            <v-col cols="12" md="5">
                                <v-text-field label="備註" v-model="Remark" color="primary" variant="underlined"
                                    placeholder="(選填)" @blur="validate"></v-text-field>
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


        <!-- Vuetify的Modal -->
        <v-dialog v-model="showModalcart" max-width="600px">
            <v-card>
                <v-card-title>結賬確認</v-card-title>
                <v-card-text>
                    <!-- 顯示購物車項目 -->
                    <div v-for="item in cartItems" :key="item.name">
                        名稱: {{ item.name }}<br>
                        價格: {{ item.price }}<br>
                        數量: {{ item.qty }}<br>
                        <hr>
                    </div>

                    <form ref="paymentForm" method="POST"
                        action="https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5">
                        <div v-for="(value, key) in cartItemsresponse" :key="key">
                            <input type="hidden" :name="key" :value="value">
                        </div>
                    </form>
                </v-card-text>
                <v-card-actions>
                    <v-btn color="red" @click="showModalcart = false">取消</v-btn>
                    <v-btn color="green" @click="submitForm">前往付款</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
    </div>
</template>

<script setup>

import { Delete, Edit, Search, Share, Upload } from '@element-plus/icons-vue'
import 'bootstrap/dist/js/bootstrap.bundle.js';
import router from '../router/index.js';
import { ref, computed, onMounted } from 'vue';
import axios from 'axios';
import moment from 'moment-timezone';

const tab = ref(null);

const books = ref([]);

const Allcheck = ref(true);

const showModalcart = ref(false)
const cartItemsresponse = ref([]);
const cartItems = ref([]);
let orderItems = ref([]);
const ReceiverName = ref('');
const receiverNameError = ref('');
const ReceiverPhone = ref('');
const receiverPhoneError = ref('');
const TaxIdNum = ref('');
const taxIdNumError = ref('');
const ReceiverAddress = ref('');
const receiverAddressError = ref('');
const Remark = ref('');
const remarkError = ref('');
let cartInfoData = ref({
    receivername: "",
    receiverphone: "",
    vehiclenum: "",
    receiveraddress: "",
    remark: "",

    id: "",
    userid: "",
    ordertime: "",
    orderstatusid: "",
    totalamount: "",
    totalpayment: "",
    taxIdNum: "",
    shippingFee: "80",
    shippingStatusId: ""
});

let selectedItems = ref([]);

//const cartInfoData = ref();

const validate = () => {
    let valid = true;

    // 收件人姓名驗證
    if (!ReceiverName.value.trim()) {
        receiverNameError.value = '收件人姓名為必填';
        valid = false;
    } else if (ReceiverName.value.length > 255) {
        receiverNameError.value = '收件人姓名不能超過255個字';
        valid = false;
    } else {
        receiverNameError.value = '';
    }

    // 手機載具驗證
    if (TaxIdNum.value.trim()) {
        if (!/^[A-Za-z0-9!@#$%^&*()_+{}\[\]:;<>,.?~\\/-]{8}$/.test(TaxIdNum.value)) {
            taxIdNumError.value = '手機載具僅能8碼的英數字符號';
            valid = false;
        } else {
            taxIdNumError.value = '';
        }
    }

    // 手機號碼驗證
    if (!ReceiverPhone.value.trim()) {
        receiverPhoneError.value = '手機號碼為必填';
        valid = false;
    } else if (ReceiverPhone.value.length !== 10) {
        receiverPhoneError.value = '手機號碼僅能十碼';
        valid = false;
    } else {
        receiverPhoneError.value = '';
    }

    // 收件地址驗證
    if (!ReceiverAddress.value.trim()) {
        receiverAddressError.value = '收件地址為必填';
        valid = false;
    } else if (ReceiverAddress.value.length > 255) {
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

const handleCheckout = async () => {
    // 在此可以進行任何其他結帳相關的邏輯，例如數據驗證等。

    // 如果驗證失敗，則終止函數
    if (!validate()) {
        return;
    }

    try {
        const userInfo = JSON.parse(localStorage.getItem('userInfo'));
        console.log(userInfo.id);
        const data = {
            userId: userInfo.id,
        };

        // 過濾出被勾選的項目
        selectedItems = books.value.filter(item => item.check);

        cartItems.value = selectedItems.map(item => ({
            name: item.name,
            price: item.price,
            qty: item.qty
        }));

        try {
            const cartItemsresponseResult = await axios.post("https://localhost:7261/api/Ecpay/Ecpay", cartItems.value)
            cartItemsresponse.value = cartItemsresponseResult.data

            cartInfoData = {
                receivername: ReceiverName.value,
                receiverphone: ReceiverPhone.value,
                vehiclenum: TaxIdNum.value,
                receiveraddress: ReceiverAddress.value,
                remark: Remark.value,

                id: cartItemsresponseResult.data.MerchantTradeNo,
                userid: data.userId,
                ordertime: moment(cartItemsresponseResult.data.MerchantTradeDate).tz("Asia/Taipei").format(),
                orderstatusid: '1',
                totalamount: cartItemsresponseResult.data.TotalAmount,
                totalpayment: cartItemsresponseResult.data.TotalAmount,
                shippingFee: "80",
                shippingStatusId: "1"
            };
            orderItems.value = selectedItems.map(item => ({
                OrderId: cartInfoData.id,
                BookId: item.bookId,
                Price: item.price,
                Qty: item.qty
            }));

            // 將資料儲存到localStorage
            localStorage.setItem('ReceiverInfo', JSON.stringify(cartInfoData));

            // 顯示Vuetify的Modal
            showModalcart.value = true
        } catch (error) {
            console.error("Error axios Ecpay items:", error)
        }

    } catch (error) {
        console.error("Error axios GetCartsList items:", error);
    }

    // 假設想導航到另一頁面
    router.push('');
};

const paymentForm = ref(null)

async function submitForm() {

    const response = await axios.post('https://localhost:7261/CartAddToOrderDB/', cartInfoData);
    updatePayment(selectedItems, orderItems);
    paymentForm.value.submit()
}

async function updatePayment(selectedItems, orderItems) {
    try {
        const updateRequests = selectedItems.map(item => {
            return axios.post(`https://localhost:7261/PaymentCart/${item.id}`, item.id);
        });

        await Promise.all(updateRequests);
        console.log('購物車已更新');
    } catch (error) {
        console.error('購物車更新失敗原因:', error);
    }
    try {
        const postOrderItemRequests = orderItems.value.map(item => {
            return axios.post(`https://localhost:7261/CartAddToOrderItemsDB/`, item);
        });
    } catch (error) {
        console.error('訂單項目更新失敗原因:', error);
    }
}

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

.col-check {
    width: 5%;
    text-align: center
}

.col-bookName {
    width: 50%;
    text-align: center
}

.col-remove {
    width: 10%;
    text-align: center
}
</style>