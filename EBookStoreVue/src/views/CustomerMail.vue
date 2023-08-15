<template>
  <h2>客服信箱</h2>
</template>
    
<script setup>
import { ref, reactive, onMounted } from "vue";

const mail = ref([]);
const loadProducts = async () => {
  //const response = await fetch(`https://localhost:7124/api/Products`)
  const response = await fetch(
    `https://localhost:7261/api/CustomerServiceMails`
  );
  const datas = await response.json();
  mail.value = datas;
  console.log(mail.value);
};
onMounted(() => {
  loadProducts();
});
</script>
    
<style>
</style>