<template>
  <v-app>
    <!-- <v-navigation-drawer app>
    </v-navigation-drawer> -->
    <v-app-bar app>
      <Navbar />
    </v-app-bar>

    <v-main class="grey">
      <v-container fluid class="px-0 py-0">
        <router-view></router-view>
      </v-container>
    </v-main>

    <v-footer style="padding: 0%;">
      <Footer></Footer>
    </v-footer>
  </v-app>
</template>

<script>
import Navbar from "./components/Navbar.vue";
import Footer from "./components/Footer.vue";

export default {
  name: "App",
  components: {
    Navbar, Footer
  },
  data: () => ({}),
};
</script>
<style>
#app {
  font-family: Roboto, Helvetica, Arial, sans-serif;
}
</style>
