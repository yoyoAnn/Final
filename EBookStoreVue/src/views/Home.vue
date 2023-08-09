<template>
  <Categorybar />
  <v-container>
    <h2>Home</h2>
  </v-container>
</template>
    
<script>
import Categorybar from "../components/Categorybar.vue";

export default {
  name: "App",
  components: {
    Categorybar,
  },
  data: () => ({}),
};
</script>
    
<style>
</style>