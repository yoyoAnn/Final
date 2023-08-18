<template>
  <el-button class="button" style="background-color: #f5900d; color: #ebeff4"
    ><i
      class="fa-solid fa-cart-shopping fa-xl"
      style="color: #ebeff4; margin-right: 10px"
      @click="addToCart"
    ></i>
    加入購物車</el-button
  >
</template>


<script setup lang="ts">
import "@fortawesome/fontawesome-free/css/all.min.css";
import { ref, nextTick } from "vue";
import { useRouter } from "vue-router";
//驗證登入

const userInfo = JSON.parse(localStorage.getItem("userInfo"));
const isLoggedIn = ref(userInfo && userInfo.id !== undefined);
const router = useRouter();

//點擊事件
const addToCart = () => {
  if (isLoggedIn.value) {
    // 購物車操作
    console.log("router");
  } else {
    console.log("aaa");
    nextTick(() => {
      router.push("/Login");
    });
  }
};
</script>


<style>
</style>