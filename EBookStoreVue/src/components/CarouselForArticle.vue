<template>
    <div class="block text-center" m="t-4">
        <span class="articlecarouseltitle">《精選專欄文章》</span>
        <el-carousel trigger="click" height="300px">
            <el-carousel-item class="article" v-for="(article, index) in articles" :key="index">
                <a :href="`/article/${article.id}`">
                    <div class="imgdiv">
                        <img :src="`/src/ArticleImage/${article.image}`"
                            style="height: 300px; width: 100%; max-width: 100%" />
                        <p>{{ article.title }}</p>
                    </div>
                </a>
            </el-carousel-item>
        </el-carousel>
    </div>
</template>
    
<script setup>
import { ref, reactive, onMounted, watch } from "vue";
const articles = ref([]);

const loadArticles = async () => {
    try {
        const response = await fetch("https://localhost:7261/api/Articles");
        if (!response.ok) {
            throw new Error(`Network response was not ok: ${response.status}`);
        }
        const datas = await response.json();
        articles.value = datas;
    } catch (error) {
        console.error("Error loading books:", error);
    }
};

onMounted(() => {
    loadArticles();
});
</script>
    
<style>
.articlecarouseltitle {
    color: black;
    font-size: 30px;
}

.article {
    position: relative;
}

.imgdiv {
    position: relative;
}

.el-carousel__item p {
    width: 100%;
    height: 50px;
    position: absolute;
    bottom: 20px;
    left: 0;
    background-color: #fff;
    opacity: 0.60;
    color: black;
    font-size: 2em;
}
</style>