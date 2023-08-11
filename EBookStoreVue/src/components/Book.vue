<template>
  <h2>Products Page</h2>
  <div class="row">
    <div class="col-3"></div>
    <div class="col-6"></div>
    <div class="col-3"></div>
  </div>
  <table class="table table-bordered">
    <thead>
      <tr>
        <th>產品名稱</th>
        <th>產品價格</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="book in books" :key="book.id">
        <img
          :src="`src/BooksImage/${book.bookImage}`"
          style="height: 300px; width: 300px"
        />
        <td>{{ book.name }}</td>
        <td>{{ book.price }}</td>
      </tr>
    </tbody>
  </table>
</template>
  


<script setup lang='ts'>
import { ref, onMounted } from "vue";

const books = ref([]);
const booksImage = ref([]);

const loadBooks = async () => {
  try {
    const response = await fetch("https://localhost:7261/api/Books");
    if (!response.ok) {
      throw new Error(`Network response was not ok: ${response.status}`);
    }
    const datas = await response.json();
    books.value = datas;

    // console.log(books.value);
    console.log(books.value);
  } catch (error) {
    console.error("Error loading books:", error);
  }
};

onMounted(() => {
  loadBooks();
});
</script>
  
<style>
.book-image {
  width: 100px;
  height: 150px;
}
</style>
  