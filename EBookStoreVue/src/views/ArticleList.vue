<template>
  <h2>專欄文章列表</h2>
  <v-card
    class="mx-auto"
    max-width="900"
    variant="outlined"
    v-for="(item, index) in articles"
    :key="index"
  >
    <v-card-item>
      <div>
        <div class="text-overline mb-1">{{ item.createdTime }}</div>
        <div class="text-h6 mb-1">{{ item.title }}</div>
        <div class="text-caption">
          {{ item.content.substring(0, 31) }}
        </div>
      </div>
    </v-card-item>

    <v-card-actions>
      <v-btn variant="outlined"> 收藏 </v-btn>
    </v-card-actions>
  </v-card>
</template>
    
<script setup>
import { ref, onMounted, watch } from "vue";
import { useRoute } from "vue-router";

const articles = ref([]);

//取得WebAPI資料
const loadArticle = async () => {
  try {
    const response = await fetch(`https://localhost:7261/api/Articles/`);
    if (!response.ok) {
      throw new Error(`Network response was not ok: ${response.status}`);
    }
    const datas = await response.json();
    articles.value = datas;
    // article.value.createdTime = article.value.createdTime.substring(0, 10);
    // console.log(articles.value);
  } catch (error) {
    console.error("Error loading books:", error);
  }
};

onMounted(async () => {
  await loadArticle();
});
</script>
    
<style>
</style>