<template>
  <Categorybar />
  <v-container>
    <div class="block text-center" m="t-4">
      <span class="demonstration">精選專欄文章</span>
      <el-carousel trigger="click" height="250px">
        <el-carousel-item class="article" v-for="item in 4" :key="item">
          <h3 class="small justify-center" text="2xl">{{ item }}</h3>
        </el-carousel-item>
      </el-carousel>
    </div>
  </v-container>
  <v-container>
    <CarouselForBook />
  </v-container>
  <v-container>
    <Books />
  </v-container>
</template>
    
<script>
import Categorybar from "@/components/Categorybar.vue";
import CarouselForBook from "../components/CarouselForBook.vue";
import Books from "../views/Book.vue";

export default {
  name: "App",
  components: {
    Categorybar,
    CarouselForBook,
    Books,
  },
  data: () => ({}),
};
</script>
    
<style>
.demonstration {
  color: var(--el-text-color-secondary);
}

.el-carousel__item h3 {
  color: #475669;
  opacity: 0.75;
  line-height: 150px;
  margin: 0;
  text-align: center;
}

.article:nth-child(2n) {
  background-color: #00ff73;
}

.article:nth-child(2n + 1) {
  background-color: #e5ff00;
}
</style>