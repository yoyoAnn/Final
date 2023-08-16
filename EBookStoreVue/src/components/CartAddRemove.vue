<template>
    <div v-if="product" class="input-group plus-minus">
        <button class="btn btn-outline-secondary" :class="loading ? 'disabled' : ''" @click="addOrRemove(-1)" type="button">
            -
        </button>
        <input type="number" :value="product.qty" disabled class="form-control form-control-sm" placeholder=""
            aria-label="Example text with button addon" aria-describedby="button-addon1">
        <button class="btn btn-outline-secondary" :class="loading ? 'disabled' : ''" @click="addOrRemove(1)" type="button">
            +
        </button>
    </div>
</template>
  
<script>
//import { toast } from 'vue3-toastify';
import 'vue3-toastify/dist/index.css';

export default {
    name: 'CartAddRemove',
    props: ['product'],
    data() {
        return {
            loading: false,
        };
    },
    methods: {
        async addOrRemove(number) {
            this.loading = true;
            if ((number === 1 && this.product.qty < 10) || (number === -1 && this.product.qty > 1)) {
                this.product.qty += number;

                this.$emit('updateQuantity', this.product.id, this.product.qty);

                await this.$store.commit('updateCart', { product: this.product });
                toast.success('cart updated', {
                    autoClose: 1000,
                });
            } else {
                toast.warning('商品數量不得小於1或大於10', {
                    autoClose: 3000,
                });
            }
            this.loading = false;
        },
    },
};
</script>
  
<style>
.plus-minus input {
    max-width: 50px;
}
</style>