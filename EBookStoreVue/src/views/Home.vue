<template>
  <Categorybar />
  <v-container>
    <CarouselForHome />
  </v-container>
</template>
    
<script>
import Categorybar from "../components/Categorybar.vue";
import CarouselForHome from "../components/CarouselForBook.vue";

export default {
  name: "App",
  components: {
    Categorybar,
    CarouselForHome,
  },
  data: () => ({}),
};
</script>
    
<style>
</style>