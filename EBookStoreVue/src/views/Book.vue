<template>
  <h2>本日推薦</h2>
  <div class="row">
    <div class="col-3"></div>
    <div class="col-6"></div>
    <div class="col-3"></div>
  </div>

  <div>
    <el-row class="button-row">
      <el-col :span="1">
        <el-button @click="prevPage" :disabled="currentPage === 1"
          >上一頁</el-button
        >
      </el-col>
      <el-col :span="18">
        <div class="card-container">
          <el-col
            v-for="(book, index) in displayedBooks"
            :key="index"
            :span="6"
          >
            <el-card :body-style="{ padding: '0px' }" class="custom-card">
              <img
                :src="`src/BooksImage/${book.bookImage}`"
                style="height: 300px; width: auto; max-width: 100%"
              />
              <div style="padding: 14px">
                <span class="book-title">{{ book.name }}</span>
                <div class="bottom">
                  <span class="pricecolor">{{ book.price }} 元</span>
                  <el-button type="text" class="button">加入購物車</el-button>
                </div>
              </div>
            </el-card>
          </el-col>
        </div>
      </el-col>
      <el-col :span="3">
        <el-button @click="nextPage" :disabled="currentPage === totalPages"
          >下一頁</el-button
        >
        <i class="fa-solid fa-chevron-right fa-fade"></i>
      </el-col>
    </el-row>
  </div>
</template>
  


<script setup lang='ts'>
import { ref, computed, onMounted } from "vue";

const books = ref([]);
// const randomBooks = ref([]);
const currentPage = ref(1);
const itemsPerPage = 4;
const numberOfRandomBooks = 10;

const loadBooks = async () => {
  try {
    const response = await fetch("https://localhost:7261/api/Books");
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

onMounted(() => {
  loadBooks();
});

//隨機選擇指定數量書籍

const getRandomBooks = computed(() => {
  const shuffledBooks = books.value.sort(() => 0.5 - Math.random());
  return shuffledBooks.slice(0, numberOfRandomBooks);
});

//分頁邏輯
const totalPages = computed(() =>
  Math.ceil(getRandomBooks.value.length / itemsPerPage)
);

const displayedBooks = computed(() => {
  const startIndex = (currentPage.value - 1) * itemsPerPage;
  const endIndex = startIndex + itemsPerPage;
  return getRandomBooks.value.slice(startIndex, endIndex);
});

const prevPage = () => {
  if (currentPage.value > 1) {
    currentPage.value--;
  }
};
const nextPage = () => {
  if (currentPage.value < totalPages.value) {
    currentPage.value++;
  }
};
</script>
  


<style src="../BookCSS/BookCSS.css">
</style>
  