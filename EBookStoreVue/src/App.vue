<template>
  <v-app>

    <!-- <v-navigation-drawer app>
    </v-navigation-drawer> -->

    <Navbar />

    <v-main>
      <v-container fluid>
        <router-view></router-view>
      </v-container>
    </v-main>

    <v-footer app>
      <!-- -->
    </v-footer>
  </v-app>
</template>

<script>
import Navbar from './components/Navbar.vue'

export default {
  name: 'App',
  components: {
    Navbar,
  },
  data: () => ({

  }),
}
</script>
