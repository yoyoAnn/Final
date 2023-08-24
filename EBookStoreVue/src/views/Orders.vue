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
                <el-table v-if="activeTab === 'not-paid'" :data="notPaidOrders" style="width: 100%"
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
                    <el-table-column label="訂單時間" prop="orderTime" width="180">
                        <template #default="{ row }">
                            {{ formatDate(row.orderTime) }}
                        </template>
                    </el-table-column>
                    <el-table-column label="操作" width="220">
                        <template #default="{ row }">
                            <el-button type="primary" @click="completePayment(row)">完成付款</el-button>
                            <el-button type="danger" @click="promptCancelOrder(row)">取消訂單</el-button>
                        </template>

                    </el-table-column>
                </el-table>

                <el-table v-if="activeTab === 'paid'" :data="paidOrders" style="width: 100%" @expand-change="handleExpand">
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
                    <el-table-column label="訂單時間" prop="orderTime" width="180">
                        <template #default="{ row }">
                            {{ formatDate(row.orderTime) }}
                        </template>
                    </el-table-column>

                </el-table>
            </el-main>
        </el-container>
    </div>


    <el-dialog v-model="showCancelDialog" title="確認">
        <span>您確定要取消這筆訂單嗎？</span>
        <template v-slot:footer>
            <span class="dialog-footer">
                <el-button @click="showCancelDialog = false">取 消</el-button>
                <el-button type="primary" @click="confirmCancelOrder">確 定</el-button>
            </span>
        </template>
    </el-dialog>
</template>
  
<script setup>
import { ref, onMounted } from 'vue';
import axios from "axios";
import { nextTick } from 'vue';

const activeTab = ref("not-paid");
const notPaidOrders = ref([]);
const paidOrders = ref([]);
const formattedItems = ref([]);
const expandOrderItems = ref({});  // Data for expanded rows
const showEcpayForm = ref(false);
const paymentForm = ref(null)
const ecpayData = ref({});
const showCancelDialog = ref(false);
const orderToCancel = ref(null);

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
    { text: "出貨狀態", value: "shippingStatusName" },
    { text: "收件人", value: "receiverName" },
    { text: "收件地址", value: "receiverAddress" },
    { text: "聯絡電話", value: "receiverPhone" },
    { text: "運費", value: "shippingFee" },
    { text: "總金額", value: "totalAmount" },
];

const getColumnWidth = (value) => {
    if (value === 'id') return '200';
    if (value === 'receiverAddress') return '350';
    if (value === 'shippingFee') return '60';
    if (value === 'receiverPhone') return '110';
    if (value === 'receiverAddress') return '300';
    if (value === 'shippingStatusName') return '100';
    return undefined;
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


const promptCancelOrder = (order) => {
    orderToCancel.value = order.id;  // 保存要取消的訂單的編號
    showCancelDialog.value = true;
};
</script>
  
<style scoped>
.el-header,
.el-main {
    background-color: white;
}

.el-header {
    display: flex;
    justify-content: center;
    align-items: center;
}
</style>