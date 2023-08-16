<template>
  <v-container>
    <Books />
  </v-container>
</template>
    
<script>
import Books from "../components/ChosenBook.vue";

export default {
  name: "App",
  components: {
    Books,
  },
  data: () => ({}),
};
</script>
    
<style>
</style>