<template>
    <form ref="paymentForm" method="POST" action="https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5">
        <div v-for="(value, key) in ecpayData" :key="key">
            <input type="hidden" :name="key" :value="value">
        </div>
    </form>
    <div class="common-layout">
        <el-container>
            <el-header>
                <el-tabs v-model="activeTab" @tab-click="handleClick">
                    <el-tab-pane label="未付款" name="not-paid"></el-tab-pane>
                    <el-tab-pane label="處理中" name="paid"></el-tab-pane>
                    <el-tab-pane label="交易完成" name="final"></el-tab-pane>
                    <el-tab-pane label="已取消" name="cancel"></el-tab-pane>
                </el-tabs>
            </el-header>
            <el-main>
                <el-table stripe v-if="activeTab === 'not-paid'" :data="notPaidOrders" style="width: 100%"
                    @expand-change="handleExpand">
                    <el-table-column type="expand">
                        <template #default="{ row }">
                            <el-table :data="expandOrderItems[row.id]" style="width: 100%">
                                <el-table-column width="200" prop="image" label="封面">
                                    <template #default="{ row }">
                                        <img :src="getImagePath(row.image)" alt="Book Cover"
                                            style="max-width: 100%; height: auto;">
                                    </template>
                                </el-table-column>
                                <el-table-column width="500" prop="name" label="書名"></el-table-column>
                                <el-table-column prop="price" label="價格"></el-table-column>
                                <el-table-column prop="qty" label="數量"></el-table-column>
                                <el-table-column label="小計">
                                    <template #default="{ row }">
                                        {{ computeSubtotal(row.price, row.qty) }}
                                    </template>
                                </el-table-column>
                            </el-table>
                        </template>
                    </el-table-column>

                    <el-table-column v-for="header in headers" :key="header.value" :prop="header.value" :label="header.text"
                        :width="getColumnWidth(header.value)"></el-table-column>
                    <el-table-column label="訂單日期" prop="shippingTime" width="180">
                        <template #default="{ row }">
                            {{ formatDate(row.shippingTime) }}
                        </template>
                    </el-table-column>

                    <el-table-column label="操作" width="220">
                        <template #default="{ row }">
                            <el-button type="primary" @click="completePayment(row)">完成付款</el-button>
                            <el-button type="danger" @click="promptCancelOrder(row)">取消訂單</el-button>
                        </template>
                    </el-table-column>
                </el-table>

                <div v-if="activeTab === 'paid'">
                    <el-table :data="displayedPaidOrders" style="width: 100%" @expand-change="handleExpand"
                        :row-class-name="tableRowClassName">
                        <el-table-column type="expand">
                            <template #default="{ row }">
                                <el-table :data="expandOrderItems[row.id]" style="width: 100%">
                                    <el-table-column width="200" prop="image" label="封面">
                                        <template #default="{ row }">
                                            <img :src="getImagePath(row.image)" alt="Book Cover"
                                                style="max-width: 100%; height: auto;">
                                        </template>
                                    </el-table-column>
                                    <el-table-column width="500" prop="name" label="書名"></el-table-column>
                                    <el-table-column prop="price" label="價格"></el-table-column>
                                    <el-table-column prop="qty" label="數量"></el-table-column>
                                    <el-table-column label="小計">
                                        <template #default="{ row }">
                                            {{ computeSubtotal(row.price, row.qty) }}
                                        </template>
                                    </el-table-column>
                                </el-table>
                            </template>
                        </el-table-column>

                        <el-table-column v-for="header in paidheaders" :key="header.value" :prop="header.value"
                            :label="header.text" :width="getColumnWidth(header.value)"></el-table-column>
                        <el-table-column label="訂單日期" prop="orderTime" width="180">
                            <template #default="{ row }">
                                {{ formatDate(row.orderTime) }}
                            </template>
                        </el-table-column>
                        <el-table-column label="出貨日期" prop="shippingTime" width="180">
                            <template #default="{ row }">
                                {{ formatDate(row.shippingTime) }}
                            </template>
                        </el-table-column>
                        <el-table-column label="操作" width="220">
                            <template #default="{ row }">
                                <el-button v-if="canReturn(row.shippingTime, row.shippingStatusName)" type="warning"
                                    @click="returnPayment(row)">我要退貨</el-button>
                            </template>
                        </el-table-column>

                    </el-table>
                    <div style="text-align: right;">
                        <el-select v-model="pageSize" placeholder="選擇每頁數量" style="margin: 15px 0;">
                            <el-option label="10" :value="10"></el-option>
                            <el-option label="20" :value="20"></el-option>
                            <el-option label="30" :value="30"></el-option>
                        </el-select>
                    </div>
                    <div style="display: flex; justify-content: flex-end;">
                        <el-pagination :total="paidOrders.length" :page-size="pageSize" v-model:current-page="currentPage"
                            layout="total, prev, pager, next">
                        </el-pagination>
                    </div>
                </div>

                <div v-if="activeTab === 'final'">
                    <el-table stripe :data="displayedFinalOrders" style="width: 100%" @expand-change="handleExpand">
                        <el-table-column type="expand">
                            <template #default="{ row }">
                                <el-table :data="expandOrderItems[row.id]" style="width: 100%">
                                    <el-table-column width="200" prop="image" label="封面">
                                        <template #default="{ row }">
                                            <img :src="getImagePath(row.image)" alt="Book Cover"
                                                style="max-width: 100%; height: auto;">
                                        </template>
                                    </el-table-column>
                                    <el-table-column width="500" prop="name" label="書名"></el-table-column>
                                    <el-table-column prop="price" label="價格"></el-table-column>
                                    <el-table-column prop="qty" label="數量"></el-table-column>
                                    <el-table-column label="小計">
                                        <template #default="{ row }">
                                            {{ computeSubtotal(row.price, row.qty) }}
                                        </template>
                                    </el-table-column>
                                </el-table>
                            </template>
                        </el-table-column>

                        <el-table-column v-for="header in finalheaders" :key="header.value" :prop="header.value"
                            :label="header.text" :width="getColumnWidth(header.value)"></el-table-column>
                        <el-table-column label="訂單日期" prop="orderTime" width="180">
                            <template #default="{ row }">
                                {{ formatDate(row.orderTime) }}
                            </template>
                        </el-table-column>
                        <el-table-column label="出貨日期" prop="shippingTime" width="180">
                            <template #default="{ row }">
                                {{ formatDate(row.shippingTime) }}
                            </template>
                        </el-table-column>
                    </el-table>
                    <div style="text-align: right;">
                        <el-select v-model="pageSize" placeholder="選擇每頁數量" style="margin: 15px 0;">
                            <el-option label="10" :value="10"></el-option>
                            <el-option label="20" :value="20"></el-option>
                            <el-option label="30" :value="30"></el-option>
                        </el-select>
                    </div>
                    <div style="display: flex; justify-content: flex-end;">
                        <el-pagination :total="finalOrders.length" :page-size="pageSize" v-model:current-page="currentPage"
                            layout="total, prev, pager, next">
                        </el-pagination>
                    </div>
                </div>

                <div v-if="activeTab === 'cancel'">
                    <el-table stripe :data="displayedcancelOrders" style="width: 100%" @expand-change="handleExpand">
                        <el-table-column type="expand">
                            <template #default="{ row }">
                                <el-table :data="expandOrderItems[row.id]" style="width: 100%">
                                    <el-table-column width="200" prop="image" label="封面">
                                        <template #default="{ row }">
                                            <img :src="getImagePath(row.image)" alt="Book Cover"
                                                style="max-width: 100%; height: auto;">
                                        </template>
                                    </el-table-column>
                                    <el-table-column width="500" prop="name" label="書名"></el-table-column>
                                    <el-table-column prop="price" label="價格"></el-table-column>
                                    <el-table-column prop="qty" label="數量"></el-table-column>
                                    <el-table-column label="小計">
                                        <template #default="{ row }">
                                            {{ computeSubtotal(row.price, row.qty) }}
                                        </template>
                                    </el-table-column>
                                </el-table>
                            </template>
                        </el-table-column>

                        <el-table-column v-for="header in cancelheaders" :key="header.value" :prop="header.value"
                            :label="header.text" :width="getColumnWidth(header.value)"></el-table-column>
                    </el-table>

                    <div style="text-align: right;">
                        <el-select v-model="pageSize" placeholder="選擇每頁數量" style="margin: 15px 0;">
                            <el-option label="10" :value="10"></el-option>
                            <el-option label="20" :value="20"></el-option>
                            <el-option label="30" :value="30"></el-option>
                        </el-select>
                    </div>
                    <div style="display: flex; justify-content: flex-end;">
                        <el-pagination :total="cancelOrders.length" :page-size="pageSize" v-model:current-page="currentPage"
                            layout="total, prev, pager, next">
                        </el-pagination>
                    </div>
                </div>
            </el-main>
        </el-container>
    </div>


    <el-dialog v-model="showCancelDialog" title="確認" icon="warning">
        <span>您確定要取消這筆訂單嗎？</span><br>
        <span>此操作無法復原</span>
        <template v-slot:footer>
            <span class="dialog-footer">
                <el-button @click="showCancelDialog = false">取 消</el-button>
                <el-button type="primary" @click="confirmCancelOrder">確 定</el-button>
            </span>
        </template>
    </el-dialog>

    <el-dialog v-model="showReturnDialog" title="確認" icon="warning">
        <span>您確定要退貨這筆訂單嗎？</span><br>
        <span>此操作無法復原</span>
        <template v-slot:footer>
            <span class="dialog-footer">
                <el-button @click="showReturnDialog = false">取 消</el-button>
                <el-button type="primary" @click="confirmReturnOrder">確 定</el-button>
            </span>
        </template>
    </el-dialog>
</template>
  
<script setup>
import { ref, onMounted, computed } from 'vue';
import axios from "axios";
import { nextTick } from 'vue';

const activeTab = ref("not-paid");
const notPaidOrders = ref([]);
const paidOrders = ref([]);
const finalOrders = ref([]);
const cancelOrders = ref([]);
const formattedItems = ref([]);
const expandOrderItems = ref({});  // Data for expanded rows
const showEcpayForm = ref(false);
const paymentForm = ref(null)
const ecpayData = ref({});
const showCancelDialog = ref(false);
const showReturnDialog = ref(false);
const orderToCancel = ref(null);
const orderToReturn = ref(null);
const checkItemToFinal = ref([]);

const pageSize = ref(10);
const currentPage = ref(1);

const finalpageSize = ref(10);
const finalcurrentPage = ref(1);

const cancelpageSize = ref(10);
const cancelcurrentPage = ref(1);

const displayedPaidOrders = computed(() => {
    const start = (currentPage.value - 1) * pageSize.value;
    const end = currentPage.value * pageSize.value;
    return paidOrders.value.slice(start, end);
});

const displayedFinalOrders = computed(() => {
    const start = (finalcurrentPage.value - 1) * finalpageSize.value;
    const end = finalcurrentPage.value * finalpageSize.value;
    return finalOrders.value.slice(start, end);
});

const displayedcancelOrders = computed(() => {
    const start = (cancelcurrentPage.value - 1) * cancelpageSize.value;
    const end = cancelcurrentPage.value * cancelpageSize.value;
    return cancelOrders.value.slice(start, end);
});

const headers = [
    { text: "訂單編號", value: "id" },
    { text: "收件人", value: "receiverName" },
    { text: "收件地址", value: "receiverAddress" },
    { text: "聯絡電話", value: "receiverPhone" },
    // { text: "出貨狀態", value: "shippingStatusName" },
    { text: "運費", value: "shippingFee" },
    { text: "總金額", value: "totalAmount" },
];

const paidheaders = [
    { text: "訂單編號", value: "id" },
    { text: "訂單狀態", value: "orderStatusName" },
    { text: "出貨狀態", value: "shippingStatusName" },
    { text: "收件人", value: "receiverName" },
    { text: "收件地址", value: "receiverAddress" },
    { text: "聯絡電話", value: "receiverPhone" },
    { text: "運費", value: "shippingFee" },
    { text: "總金額", value: "totalAmount" },
];

const finalheaders = [
    { text: "訂單編號", value: "id" },
    { text: "訂單狀態", value: "orderStatusName" },
    { text: "出貨狀態", value: "shippingStatusName" },
    { text: "收件人", value: "receiverName" },
    { text: "收件地址", value: "receiverAddress" },
    { text: "聯絡電話", value: "receiverPhone" },
    { text: "運費", value: "shippingFee" },
    { text: "總金額", value: "totalAmount" },
];

const cancelheaders = [
    { text: "訂單編號", value: "id" },
    //{ text: "出貨狀態", value: "shippingStatusName" },
    { text: "收件人", value: "receiverName" },
    { text: "收件地址", value: "receiverAddress" },
    { text: "聯絡電話", value: "receiverPhone" },
    { text: "運費", value: "shippingFee" },
    { text: "總金額", value: "totalAmount" },
];

const getColumnWidth = (value) => {
    if (value === 'id') return '200';
    if (value === 'receiverAddress') return '275';
    if (value === 'shippingFee') return '60';
    if (value === 'receiverPhone') return '110';
    if (value === 'shippingStatusName') return '100';
    if (value === 'orderStatusName') return '100';

    return undefined;
};

const tableRowClassName = ({ row }) => {
    if (row.shippingStatusName === '退貨處理中') {
        return 'warning-color';
    }

    if (row.orderStatusName === '已付款' && row.shippingStatusName === '已取貨') {
        return 'success-color';
    }

    return '';
};

const getImagePath = (imageName) => {
    return `/src/BooksImage/${imageName}`;
};

const computeSubtotal = (price, qty) => {
    return price * qty;
};

const getNotPaidOrders = async () => {
    const userInfo = JSON.parse(localStorage.getItem('userInfo'));
    try {
        const response = await axios.post("https://localhost:7261/GetOrderNotPayList", {
            id: userInfo.id
        });
        notPaidOrders.value = response.data;


        const responsePaid = await axios.post("https://localhost:7261/GetOrderPaidList", {
            id: userInfo.id
        });

        paidOrders.value = responsePaid.data;

        checkItemToFinal.value = paidOrders.value.map(item => ({
            id: item.id,
            orderDateTime: item.shippingTime
        }));
        const checkToFinal = await axios.post("https://localhost:7261/CheckOrderToFinal", checkItemToFinal.value);



        const responseFinal = await axios.post("https://localhost:7261/GetFinalOrder", {
            id: userInfo.id
        });

        finalOrders.value = responseFinal.data;


        const responseCancel = await axios.post("https://localhost:7261/GetCancelOrder", {
            id: userInfo.id
        });

        cancelOrders.value = responseCancel.data;
    } catch (error) {
        console.error("Error fetching not paid orders:", error);
    }
};

const fetchOrderItems = async (orderId) => {
    try {
        const response = await axios.post("https://localhost:7261/GetOrderItemList", { orderId });
        expandOrderItems.value[orderId] = response.data;
    } catch (error) {
        console.error("Error fetching order items:", error);
    }
};

const handleExpand = (row, expanded) => {
    if (expanded && !expandOrderItems.value[row.id]) {
        fetchOrderItems(row.id);
    }
};

const formatDate = (dateString) => {
    const date = new Date(dateString);
    return date.toLocaleDateString("zh-TW") + " " + date.toLocaleTimeString("zh-TW");
};

onMounted(getNotPaidOrders);

const completePayment = async (order) => {
    let items = expandOrderItems.value[order.id];

    // 如果子行資料還沒有，那就先去載入
    if (!items || items.length === 0) {
        await fetchOrderItems(order.id);  // 等待資料載入完成
        items = expandOrderItems.value[order.id];
    }

    // 再次確認資料是否載入成功
    if (!items || items.length === 0) {
        console.error("找不到訂單的子行資料：", order.id);
        return;
    }

    formattedItems.value = items.map(item => ({
        orderId: order.id,
        name: item.name,
        price: item.price,
        qty: item.qty
    }));

    try {
        const cartItemsresponseResult = await axios.post("https://localhost:7261/api/EcpayAgain/EcpayAgain", formattedItems.value);
        console.log(cartItemsresponseResult);
        console.log("正在生成訂單資料：", order.id);

        ecpayData.value = cartItemsresponseResult.data;

        await nextTick();

        console.log(ecpayData.value.MerchantID);

        paymentForm.value.submit()


    } catch (error) {
        console.error("生成訂單資料錯誤：", error);
    }

};

const confirmCancelOrder = async () => {
    if (!orderToCancel.value) return;

    try {
        const response = await axios.post("https://localhost:7261/CancelOrder", {
            orderId: orderToCancel.value
        });

        if (response.status === 200) {

            console.log("訂單已取消");
            getNotPaidOrders();
        } else {
            console.error("取消訂單失敗：", response.data);
        }
    } catch (error) {
        console.error("取消訂單時出錯：", error);
    }
    console.log("訂單已取消");
    showCancelDialog.value = false;
};

const confirmReturnOrder = async () => {
    if (!orderToReturn.value) return;

    try {
        const requestData = {
            id: orderToReturn.value.id,
            orderDateTime: orderToReturn.value.shippingTime
        };

        const response = await axios.post("https://localhost:7261/RetornOrder", requestData);

        const userInfo = JSON.parse(localStorage.getItem('userInfo'));
        const responsemail = await axios.get("https://localhost:7261/api/Users/GetUserProfileById", { params: { id: userInfo.id } });
        const response2 = await axios.post("https://localhost:7261/RetornOrderMailSent", {
            id: responsemail.data.id,
            name: responsemail.data.name,
            email: responsemail.data.email
        });


        getNotPaidOrders();
        showReturnDialog.value = false;

    } catch (error) {
        console.error("退貨申請發生錯誤", error);
    }
};

const returnPayment = async (order) => {
    orderToReturn.value = order;  // 保存要退款的訂單的資訊
    showReturnDialog.value = true;
};

const promptCancelOrder = (order) => {
    orderToCancel.value = order.id;  // 保存要取消的訂單的編號
    showCancelDialog.value = true;
};

const canReturn = (ShippingTime, shippingStatusName) => {
    const orderDate = new Date(ShippingTime);
    const currentDate = new Date();
    const diffTime = Math.abs(currentDate - orderDate);
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

    return diffDays <= 7 && shippingStatusName === '已取貨';
};
</script>
  
<style>
.el-header,
.el-main {
    background-color: white;
}

.el-header {
    display: flex;
    justify-content: center;
    align-items: center;
}

.warning-color {
    background-color: #e8ca99 !important;
}

.success-color {
    background-color: #f0f9eb !important;
}
</style>