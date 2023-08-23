<template>
  <NavbarC />
  <Bookbt />

  <div class="book-details container">
    <div class="image divimage">
      <img :src="`/src/BooksImage/${book.bookImage}`" />
    </div>
    <div class="info">
      <h3>書名 : {{ book.name }}</h3>
      <p>出版社 : {{ book.publisherName }}</p>
      <P>出版日期 : {{ book.publisherDateTxt }}</P>
      <p>作者 : {{ book.author }}</p>
      <p>ISBN : {{ book.isbn }}</p>
      <p>價格 : {{ book.price }} 元</p>
      <p>分類 : {{ book.categoryName }}</p>
    </div>
    <div class="stock-info">
      <p>庫存尚餘 : {{ book.stock }} ， 限購 {{ book.stock }} 份</p>
      <div class="center-button">
        <BookCartbtn @click="checkout" @add-to-cart="addToCart" :book="book" />
      </div>
      <div class="center-button">
        <BookCheckout @add-to-cart="addToCart" :book="book" />
      </div>
    </div>
  </div>
  <div class="container">
    <h3>內容簡介</h3>
    <hr />
  </div>
  <div class="container" style="margin-bottom: 50px">
    <div class="summary formatted-text">{{ book.summary }}</div>
  </div>
  <v-container>
    <Books />
  </v-container>
</template>




<script setup>
import { ref, inject, onMounted, watch } from "vue";
import { useRoute } from "vue-router";

const book = ref([]);
const bookId = ref("");
const addToCart = inject("addToCart");
// 獲取參數
const route = useRoute();
// const router = useRouter();

watch(
  () => {
    bookId.value = route.params.bookid; //參數從router/index.js設定
  },
  { immediate: true }
);

const loadBookDetails = async () => {
  try {
    const apiUrl = `https://localhost:7261/api/Books/${bookId.value}`;
    const response = await fetch(apiUrl);
    const data = await response.json();
    book.value = data;
    console.log(book.value);
  } catch (error) {
    console.error("Error loading book details:", error);
  }
};

onMounted(() => {
  loadBookDetails();
});

const checkout = () => {
  if (book.value.stock > 0) {
    book.value.stock--;
  }
};
</script>

<script>
import { defineComponent } from "vue";
import Books from "./ChosenBook.vue";
import NavbarC from "./Categorybar.vue";
import Bookbt from "./Bookbacktop.vue";
import BookCartbtn from "./BookCartBtn.vue";
import BookCheckout from "./BookCheckout.vue";
export default defineComponent({
  components: {
    Books,
    NavbarC,
    Bookbt,
    BookCartbtn,
    BookCheckout,
  },
});
</script>

<style>
.formatted-text {
  white-space: pre-line;
}

.button-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-top: 20px;
}

.button-container el-button {
  width: 100%;
  margin-top: 10px;
}

.center-button {
  display: flex;
  justify-content: center;
  align-items: center;
  margin-top: 20px;
  width: 150px;
}

.stock-info {
  background-color: #ebe4e4;
  border: 1px solid #ccc;
  padding: 10px;
  width: 200px;
  height: 170px;
  box-sizing: border-box;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.book-details {
  display: flex;
  align-items: flex-start;
  gap: 1px;
  margin-top: 50px;
  width: 500px;
  height: 400px;
}

.book-details .info {
  flex: 1;
  min-width: 200px;
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  justify-content: center;
  padding: 10px;
}

.image {
  margin-left: auto;
}

.image img {
  height: 300px;
  width: auto;
  max-width: 100%;
  border: 2px solid #ccc;
}

.divimage {
  width: 348px;
  height: 348px;
  margin-left: auto;
}
</style>