<template>
    <div>
        <!-- Tabs Component -->
        <v-tabs v-model="activeTab">
            <v-tab key="not-paid">未付款</v-tab>
            <v-tab key="paid">已付款</v-tab>
        </v-tabs>

        <v-tabs-items v-model="activeTab">
            <!-- Not Paid Orders Table -->
            <v-tab-item key="not-paid">
                <v-data-table :headers="headers" :items="notPaidOrders" class="elevation-1">
                    <template #orderTime="{ item }">
                        {{ formatDate(item.orderTime) }}
                    </template>
                </v-data-table>
            </v-tab-item>

            <!-- Paid Orders Table -->
            <v-tab-item key="paid">
                <!-- This will be populated later when you have data for paid orders -->
            </v-tab-item>
        </v-tabs-items>
    </div>
</template>
  
<script setup>
import { ref, onMounted } from 'vue';
import axios from "axios";

const activeTab = ref("not-paid");
const notPaidOrders = ref([]);
const headers = [
    { text: "訂單ID", value: "id" },
    { text: "姓名", value: "name" },
    { text: "收件人", value: "receiverName" },
    { text: "地址", value: "receiverAddress" },
    { text: "電話", value: "receiverPhone" },
    // ... (you can continue with other columns you want to show in the table)
    { text: "訂單時間", value: "orderTime" },
    { text: "總金額", value: "totalAmount" },
];

const getNotPaidOrders = async () => {
    const userInfo = JSON.parse(localStorage.getItem('userInfo'));
    try {
        const response = await axios.post("https://localhost:7261/GetOrderNotPayList", {
            id: userInfo.id
        });
        notPaidOrders.value = response.data;
    } catch (error) {
        console.error("Error fetching not paid orders:", error);
    }
};

const formatDate = (dateString) => {
    const date = new Date(dateString);
    return date.toLocaleDateString("zh-TW") + " " + date.toLocaleTimeString("zh-TW");
};

onMounted(getNotPaidOrders);
</script>
  
<style scoped>
/* Add your CSS styles here */
</style>