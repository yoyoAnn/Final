<template>
  <v-layout style="height: 56px">
    <v-bottom-navigation
      bg-color="green-darken-3"
      v-model="value"
      color="light"
      horizontal
    >
      <v-btn
        v-for="category in categories"
        :key="category.title"
        :to="category.route"
        >{{ category.title }}</v-btn
      >
    </v-bottom-navigation>
  </v-layout>
</template>
    
<script>
export default {
  data: () => ({
    value: 1,
    categories: [
      { title: "暢銷排行", route: "/" },
      { title: "文學小說", route: "/" },
      { title: "商業理財", route: "/" },
      { title: "藝術設計", route: "/" },
      { title: "人文社科", route: "/" },
      { title: "心理勵志", route: "/" },
    ],
  }),
};
</script>
    
<style>
</style>