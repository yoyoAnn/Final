<template>
  <v-container>
    <AllBooks />
  </v-container>
  <v-container>
    <Books />
  </v-container>
</template>
    
<script>
import Books from "../components/ChosenBook.vue";
import AllBooks from "../components/AllBook.vue";
import { useCartStore } from '../stores/cart';

export default {
  name: "App",
  components: {
    Books,
    AllBooks,
  },
  data: () => ({}),
  created() {
    const { updateCartItemsCount } = useCartStore();
    updateCartItemsCount();
  },
};


</script>
    
<style></style>