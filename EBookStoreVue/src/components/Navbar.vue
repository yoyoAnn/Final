<template>
  <v-container class="py-0 fill-height">
    <v-row no-gutters>
      <v-col cols="3">
        <v-btn class="fill-height" flat color="white" router :to="homeRoute">
          <v-img
            class=""
            :width="60"
            aspect-ratio="4/3"
            src="https://blog.flamingtext.com/blog/2023/08/08/flamingtext_com_1691517616_586896794.png"
          ></v-img>
          <span
            class="text-subtitle-1 text-grey-darken-3"
            style="display: flex; vertical-align: text-bottom"
            >網路書店</span
          >
        </v-btn>
      </v-col>
      <v-col class="d-flex justify-center" cols="6">
        <v-responsive max-height="100" max-width="500">
          <v-text-field
            class=""
            placeholder="搜尋"
            solo
            density
            flat
            hide-details
            rounded
            append-inner-icon="mdi:mdi-magnify"
          >
          </v-text-field>
        </v-responsive>
      </v-col>
      <v-col class="d-flex justify-end" cols="3">
        <v-btn flat color="grey" router v-if="!$route.path.includes('/Login')" :to="cartRoute">
          <v-icon right icon="mdi:mdi-cart" />
        </v-btn>
        <v-menu open-on-hover>
          <template v-slot:activator="{ props }" >
            <v-btn color="grey" v-bind="props" v-if="!$route.path.includes('/Login')"> 
              <v-icon right icon="mdi:mdi-account" />
            </v-btn>
          </template>
          <v-list>
            <v-list-item
              v-for="(useritem, index) in useritems"
              :key="index"
              router
              :to="useritem.route"
            >
              <v-list-item-title>{{ useritem.title }}</v-list-item-title>
            </v-list-item>
          </v-list>
        </v-menu>
        <v-btn flat color="grey" @click="logout" v-if="!$route.path.includes('/Login')">
          <v-icon right icon="mdi:mdi-exit-to-app" />
        </v-btn>
      </v-col>

      <!-- <v-spacer></v-spacer> -->
    </v-row>
  </v-container>
</template>
<script>
export default {
  data: () => ({
    useritems: [
      { title: "會員中心", route: "/Users" },
      { title: "歷史訂單", route: "/" },
      { title: "收藏專欄", route: "/" },
    ],
    cartRoute: "/cart",
    homeRoute: "/",
    isLoggedIn: false, 
  }),
  created() {
    const userInfo = JSON.parse(localStorage.getItem('userInfo'));
    this.isLoggedIn = userInfo && userInfo.id;
  },
  methods: {
    logout() {
      const userInfo = JSON.parse(localStorage.getItem('userInfo'));
      if (userInfo && userInfo.id) {
        localStorage.removeItem('userInfo');
        this.$router.push('/Login');
      } else {
        this.$router.push('/Login'); 
      }
    },
  },
};
</script>
    
<style></style>