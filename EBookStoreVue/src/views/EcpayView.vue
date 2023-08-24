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

onMounted(() => {
    const queryParams = router.currentRoute.value.query;
    for (const key in queryParams) {
        formData.value[key] = queryParams[key];
    }
});
</script>
    
<style></style>