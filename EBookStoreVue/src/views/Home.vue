<template>
  <Categorybar />
  <v-container>
    <CarouselForHome />
  </v-container>
  <v-container>
    <Books />
  </v-container>
</template>
    
<script>
import Categorybar from "../components/Categorybar.vue";
import CarouselForHome from "../components/CarouselForBook.vue";
import Books from "../views/Book.vue";

export default {
  name: "App",
  components: {
    Categorybar,
    CarouselForHome,
    Books,
  },
  data: () => ({}),
};
</script>
    
<style>
</style>