<template>
  <v-row class="d-flex flex-row mt-3">
    <v-col cols="3"></v-col>
    <v-col cols="6">
      <h2 class="text-center" v-if="writerId != 0">{{ writerName }}</h2>
      <v-card class="card mx-auto my-3" max-width="900" variant="outlined" v-for="(item, index) in articles" :key="index">
        <v-row>
          <v-col class="col-9">
            <v-card-item>
              <div>
                <div class="text-overline mb-1">發表於 {{ item.createdTime.substring(0, 10) }} By
                  <a href="/#">{{ item.writerName }}</a>
                </div>
                <a :href="`/article/${item.id}`" class="text-black">
                  <div class="text-h4 font-weight-bold mb-1">{{ item.title }}</div>
                  <div class="text-h6">
                    {{ item.content.substring(0, 30) }}...
                  </div>
                </a>
              </div>
            </v-card-item>

            <!-- <v-card-actions>
          <v-btn variant="outlined"> 收藏 </v-btn>
        </v-card-actions> -->
          </v-col>
          <v-col class="col-3">
            <img :src="`/src/ArticleImage/${item.image}`" />
          </v-col>
        </v-row>
      </v-card>
    </v-col>
    <v-col cols="3" class="mb-3">
      <v-text-field class="search" v-model="search" append-inner-icon="mdi:mdi-magnify" label="文章搜尋" single-line
        hide-details></v-text-field>
    </v-col>
  </v-row>
</template>
    
<script setup>
import { ref, onMounted, watch } from "vue";
import { useRoute } from "vue-router";

const baseAddress = ref("");
const articles = ref([]);
const writerId = ref("");
const writerName = ref("");

// 取得Writer ID
const route = useRoute();
watch(
  () => {
    writerId.value = route.params.writerid;
  },
  { immediate: true }
);

// 搜尋條件
if (writerId.value == 0) {
  baseAddress.value = `https://localhost:7261/api/Articles`;
}
else {
  baseAddress.value = `https://localhost:7261/api/Articles?writerId=${writerId.value}`;
}


//取得WebAPI資料
const loadArticle = async () => {
  try {
    console.log(baseAddress.value);
    const response = await fetch(`${baseAddress.value}`);
    if (!response.ok) {
      throw new Error(`Network response was not ok: ${response.status}`);
    }
    const datas = await response.json();
    articles.value = datas;
    writerName.value = articles.value[0].writerName;
    console.log(articles.value);
  } catch (error) {
    console.error("Error loading books:", error);
  }
};

onMounted(async () => {
  await loadArticle();
});
</script>
    
<style>
.card {
  border: 1px solid silver;

}

img {
  width: 100%;
  height: 100%;
  object-fit: fill !important;
}

.search {
  width: 300px;
  line-height: 20px;
}
</style>