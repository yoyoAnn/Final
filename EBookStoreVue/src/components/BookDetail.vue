<template>
  <div>
    <h2>書本詳細訊息</h2>
    <p>ID: {{ bookId }}</p>
    <p>書名: {{ book.name }}</p>
    <p>價格: {{ book.price }} 元</p>
  </div>
</template>




<script setup lang='ts'>
import { ref, computed, onMounted, watch } from "vue";
import { useRoute } from "vue-router";

const book = ref([]);
const bookId = ref("");

// 獲取參數
const route = useRoute();

watch(
  () => {
    bookId.value = route.params.id;
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
</script>


<style></style>