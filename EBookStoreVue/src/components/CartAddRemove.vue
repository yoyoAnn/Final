<template>
    <div v-if="product" class="input-group plus-minus">
        <button class="btn btn-outline-secondary " :class="loading ? 'disabled' : ''" @click="addOrRemove(-1)" type="button"
            id="button-addon1">-</button>
        <input type="number" v-model="qty" disabled class="form-control form-control-sm" placeholder=""
            aria-label="Example text with button addon" aria-describedby="button-addon1">
        <button class="btn btn-outline-secondary" :class="loading ? 'disabled' : ''" @click="addOrRemove(1)" type="button"
            id="button-addon1">+</button>
    </div>
</template>
    
<script setup>

</script>
    
<style>
.plus-minus input {
    max-width: 50px;
}
</style>