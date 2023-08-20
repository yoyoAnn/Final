<template>
    <v-container class="d-flex flex-column" style="width: 900px;">
        <!-- <img class="titleImage" :src="`src/ArticleImage/${article.image}`" /> -->
        <p class="text-h4">{{ article.title }}</p>
        <p class="text-subtitle-1">{{ article.createdTime }}</p>
    </v-container>
</template>
    
<script setup >
import { ref, computed, onMounted, watch } from "vue";
import { useRoute } from "vue-router";


const article = ref([]);
const articleId = ref("");
// 取得Article ID
const route = useRoute();

watch(
    () => {
        articleId.value = route.params.articleid;
    },
    { immediate: true }
);

// 取得Article資料

const loadArticle = async () => {
    try {
        const response = await fetch(`https://localhost:7261/api/Articles/${articleId.value}`);
        if (!response.ok) {
            throw new Error(`Network response was not ok: ${response.status}`);
        }
        const datas = await response.json();
        article.value = datas;
        console.log(article.value);
    } catch (error) {
        console.error("Error loading books:", error);
    }
    //article.value.createdTime = article.value.createdTime.substring(0, 10);

};

onMounted(() => {
    loadArticle();

});

</script>
    
<style>
.titleImage {
    margin-bottom: 30px;
    height: 400px;
    width: auto;
    max-width: 100%;
}
</style>