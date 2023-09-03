<template>
  <el-col class="hidden-sm-and-down">
    <h2 class="text-center text-white bg-red rounded-shaped">暢銷排行</h2>
    <el-carousel :interval="10000" type="card" height="600px">
      <el-carousel-item v-for="(book, i) in books" :key="i">
        <div class="item">
          <el-badge :value="'TOP ' + (i + 1)" style="font: 50px sans-serif">
            <a :href="`/books/${book.id}`">
              <img :src="`/src/BooksImage/${book.bookImage}`" style="height: 400px; width: auto; max-width: 100%" />
            </a>
          </el-badge>
        </div>
      </el-carousel-item>
    </el-carousel>
  </el-col>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { CaretBottom } from "@element-plus/icons-vue";
import "element-plus/theme-chalk/display.css";

const books = ref([]);

const loadBooks = async () => {
  try {
    const response = await fetch("https://localhost:7261/api/Books/Top5Order");
    if (!response.ok) {
      throw new Error(`Network response was not ok: ${response.status}`);
    }
    const datas = await response.json();
    books.value = datas;
    console.log(books.value);
  } catch (error) {
    console.error("Error loading books:", error);
  }
};

onMounted(async () => {
  await loadBooks();
});
</script>
    
<style>
.el-carousel__item h3 {
  color: #475669;
  opacity: 0.75;
  line-height: 200px;
  margin: 0;
  /* display: flex; */
  /* text-align: center; */
  /* align-content: center; */
}

.el-carousel__item {
  height: 600px;
  display: flex;
  align-content: center;
  justify-items: center;
}

.el-carousel__item img {
  width: auto;
  margin: auto;
  display: flex;
  align-content: center;
  justify-items: center;
}

.item {
  margin-top: 50px;
  margin-left: 250px;
}

.el-dropdown {
  margin-top: 1.1rem;
}
</style>
