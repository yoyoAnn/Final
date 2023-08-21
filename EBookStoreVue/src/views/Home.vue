<template>
  <Categorybar />
  <v-container>
    <CarouselForBook />
  </v-container>
  <v-container>
    <Books />
  </v-container>
  <v-container>
    <CarouselForArticle />
  </v-container>
</template>
    
<script>
import Categorybar from "@/components/Categorybar.vue";
import CarouselForArticle from "@/components/CarouselForArticle.vue";
import CarouselForBook from "@/components/CarouselForBook.vue";
import Books from "@/views/Book.vue";

export default {
  name: "App",
  components: {
    Categorybar,
    CarouselForArticle,
    CarouselForBook,
    Books,
  },
  data: () => ({}),
};
</script>
    
<style></style>