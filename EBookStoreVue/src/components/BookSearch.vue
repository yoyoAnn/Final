<template>
  <NavbarC />

  <div class="row">
    <div class="col-3"></div>
    <div class="col-6"></div>
    <div class="col-3"></div>
  </div>
  <!-- 注目新書 -->

  <!-- 全部書籍 -->

  <div class="container">
    <h2>全部書籍</h2>
    <el-row class="button-row">
      <el-col :span="1">
        <i
          class="fa-solid fa-circle-chevron-left fa-beat-fade fa-2xl"
          @click="prevPage"
          :disabled="currentPage === 1"
        ></i>
      </el-col>
      <el-col :span="18">
        <div class="card-container">
          <el-col
            v-for="(book, index) in displayedBooks"
            :key="index"
            :span="6"
          >
            <el-card
              :body-style="{ padding: '0px' }"
              style="margin-right: 10px"
            >
              <a :href="`/books/${book.id}`">
                <img
                  :src="`/src/BooksImage/${book.bookImage}`"
                  style="height: 300px; width: auto; max-width: 100%"
                />
              </a>
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
        <i
          class="fa-solid fa-circle-chevron-right fa-beat-fade fa-2xl"
          @click="nextPage"
          :disabled="currentPage === totalPages"
        ></i>
      </el-col>
    </el-row>
  </div>
</template>
    
  
  
  <script setup lang='ts'>
import { ref, computed, onMounted, watch } from "vue";
import { useRoute } from "vue-router";
import "@fortawesome/fontawesome-free/css/all.min.css";

const route = useRoute();
const books = ref([]);
const currentPage = ref(1);
const itemsPerPage = 4;
const category = ref("");
category.value = route.params.category;

const newBooks = computed(() => {
  const sortedDisplayedBooks = filteredBooks.value
    .slice()
    .sort((a, b) => new Date(b.publishDate) - new Date(a.publishDate));
  const startIndex = (currentPage.value - 1) * itemsPerPage;
  const endIndex = startIndex + itemsPerPage;
  return sortedDisplayedBooks.slice(startIndex, endIndex);
});

const loadBooks = async () => {
  try {
    const response = await fetch("https://localhost:7261/api/Books");
    if (!response.ok) {
      throw new Error(`Network response was not ok: ${response.status}`);
    }
    const datas = await response.json();
    books.value = datas;
    // console.log(books.value);
    console.log(category.value);
  } catch (error) {
    console.error("Error loading books:", error);
  }
};

onMounted(() => {
  loadBooks();
});

watch(route, () => {
  category.value = route.params.category;
  loadBooks();
  currentPage.value = 1;
});
//篩選分類書籍
const filteredBooks = computed(() => {
  if (!category.value) {
    return books.value;
  }
  return books.value.filter((book) => book.categoryName === category.value);
});

//分頁邏輯
const totalPages = computed(() =>
  Math.ceil(filteredBooks.value.length / itemsPerPage)
);

const displayedBooks = computed(() => {
  const startIndex = (currentPage.value - 1) * itemsPerPage;
  const endIndex = startIndex + itemsPerPage;
  return filteredBooks.value.slice(startIndex, endIndex);
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
    
<script lang="ts">
import { defineComponent } from "vue";
import NavbarC from "./Categorybar.vue";

export default defineComponent({
  components: {
    NavbarC,
  },
});
</script>
  
  <style src="../BookCSS/BookCSS.css">
.cuscard {
  margin: 10px;
}
</style>
    