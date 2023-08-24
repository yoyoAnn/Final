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

export default {
  name: "App",
  components: {
    Books,
    AllBooks,
  },
  data: () => ({}),
};
</script>
    
<style>
</style>