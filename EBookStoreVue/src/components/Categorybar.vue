<template>
  <v-layout style="height: 56px">
    <v-bottom-navigation
      bg-color="green-darken-3"
      v-model="value"
      color="light"
      horizontal
    >
      <button
        class="link-button"
        v-for="category in categories"
        :key="category"
        @click="updateCategory(category)"
      >
        <v-btn>{{ category }}</v-btn>
      </button>
      <!-- <router-link
        class="link-button"
        v-for="category in categories"
        :key="category"
        @click="updateCategory(category)"
        :to="getCategoryLink(category)"
      >
        <v-btn>{{ category }}</v-btn>
      </router-link> -->
    </v-bottom-navigation>
  </v-layout>
</template>
    
<script setup>
import { ref, computed } from "vue";
import { useRoute, useRouter } from "vue-router";

const route = useRoute();
const router = useRouter();
const categories = [
  "文學小說",
  "商業理財",
  "藝術設計",
  "人文社科",
  "心理勵志",
  "宗教命理",
  "自然科普",
  "醫療保健",
  "飲食文化",
  "生活風格",
];

const getCategoryLink = (category) => ({
  name: "book-search",
  params: { category },
  // query: { category },
});

const updateCategory = (category) => {
  router.push({ name: "book-search", params: { category } });
};

const value = ref("");
</script>
    
<style scoped>
.link-button {
  display: flex;
  align-items: center;
  justify-content: center;
}

.v-application a {
  color: white;
  text-decoration: none;
}
.v-btn span {
  vertical-align: top;
}
</style>