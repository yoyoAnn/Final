<template>
  <v-container class="d-flex flex-column mt-3 text-center" style="width: 900px">
    <h2>常見問題</h2>
    <v-expansion-panels variant="accordion">
      <v-expansion-panel
        class="text-start"
        v-for="question in questions"
        :key="question"
        :title="`${question.title}`"
        :text="`${question.text}`"
      ></v-expansion-panel>
    </v-expansion-panels>
  </v-container>
</template>
    
<script setup>
import { reactive } from "vue";

const questions = reactive([
  {
    title: "購物需加入會員嗎？",
    text: "金石堂網路書店採用會員制，必須成為會員後方可購物；申請會員完全不需任何費用，因此在開始購物之前，請您先申請會員。請點選網站上方【加入會員】，填入自訂的帳號、密碼及基本資料即可完成申請。",
  },
  {
    title: "如何找商品？",
    text: `會員可於網頁上方的「搜尋」方格內，輸入商品名稱、作者、出版社、關鍵字、isbn任一項目，即可找到您需要的商品。若不確定完整商品名稱、作者、出版社，可改輸入幾個關鍵字、詞，資料庫將列出相關商品清單，提供您參考。提醒您：金石堂網路書店所販售的商品，皆以網站顯示為主；若網站上無法找到您所需要的商品，即代表供應商無法供應，所以無法開放訂購與代訂的服務。請您見諒!`,
  },
  {
    title: "",
    text: "",
  },
  {
    title: "",
    text: "",
  },
  {
    title: "",
    text: "",
  },
  {
    title: "",
    text: "",
  },
  {
    title: "",
    text: "",
  },
  {
    title: "",
    text: "",
  },
  {
    title: "",
    text: "",
  },
]);
console.log(questions[1]);
</script>
    
<style></style>