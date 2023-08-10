<template>
  <v-layout style="height: 56px">
    <v-bottom-navigation
      bg-color="green-darken-3"
      v-model="value"
      color="light"
      horizontal
    >
      <v-btn>暢銷排行</v-btn>
      <v-btn>文學小說</v-btn>
      <v-btn>商業理財</v-btn>
      <v-btn>藝術設計</v-btn>
      <v-btn>人文社科</v-btn>
      <v-btn>心理勵志</v-btn>
      <v-btn>宗教命理</v-btn>
      <v-btn>自然科普</v-btn>
      <v-btn>醫療保健</v-btn>
      <v-btn>飲食文化</v-btn>
      <v-btn>生活風格</v-btn>
    </v-bottom-navigation>
  </v-layout>
</template>
    
<script>
export default {
  data: () => ({
    value: 0,
  }),
};
</script>
    
<style>
</style>