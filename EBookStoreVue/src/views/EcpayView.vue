<template>
    <div>
        <div v-for="(value, key) in formData" :key="key">
            {{ key }} <input type="text" :name="key" :value="value" disabled /><br />
        </div>
    </div>
</template>
  
<script setup>
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();
const formData = ref({});

// 這裡可以根據需要進行初始化，例如將路由中的參數轉換成 formData
onMounted(() => {
    const queryParams = router.currentRoute.value.query;
    for (const key in queryParams) {
        formData.value[key] = queryParams[key];
    }
});
</script>
    
<style></style>