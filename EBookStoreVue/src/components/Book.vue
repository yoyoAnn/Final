<template>
  <h2>Book111</h2>
</template>
  
  <script setup lang='ts'>
import { ref, onMounted } from "vue";

const books = ref([]);

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
</script>
  
  <style>
</style>
  