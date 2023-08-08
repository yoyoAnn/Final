<template>
    <h2>會員中心</h2>
</template>
    
<script setup >
    import { ref, reactive, onMounted } from 'vue'
    
const products = ref([])
const loadProducts = async () => {
    //const response = await fetch(`https://localhost:7124/api/Products`)
    const response = await fetch(`https://localhost:7261/api/Users`)
    const datas = await response.json()
    products.value = datas;
    console.log(products.value);
}
onMounted(() => {
    loadProducts();
})
</script>
    
<style>
    
</style>