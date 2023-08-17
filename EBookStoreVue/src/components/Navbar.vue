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
          <el-autocomplete
            class=""
            v-model="searchInput.value"
            :fetch-suggestions="querySearch"
            placeholder="搜尋"
            @keydown.enter="goToSearchPage"
            @select="handleSelect"
          />
        </v-responsive>
      </v-col>
      <v-col class="d-flex justify-end" cols="3">
        <v-btn
          flat
          color="grey"
          router
          v-if="!$route.path.includes('/Login')"
          :to="cartRoute"
        >
          <v-icon right icon="mdi:mdi-cart" />
        </v-btn>
        <v-menu open-on-hover>
          <template v-slot:activator="{ props }">
            <v-btn
              color="grey"
              v-bind="props"
              v-if="!$route.path.includes('/Login')"
            >
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
        <v-btn
          flat
          color="grey"
          @click="logout"
          v-if="!$route.path.includes('/Login')"
        >
          <v-icon right icon="mdi:mdi-exit-to-app" />
        </v-btn>
      </v-col>

      <!-- <v-spacer></v-spacer> -->
    </v-row>
  </v-container>
</template>


<script setup lang="ts">
import { useRouter } from "vue-router";
import axios from "axios";
import { ref, onMounted } from "vue";
import { ElAutocomplete } from "element-plus";

const useritems = [
  { title: "會員中心", route: "/Users" },
  { title: "歷史訂單", route: "/" },
  { title: "收藏專欄", route: "/" },
];

const cartRoute = "/cart";
const homeRoute = "/";
const isLoggedIn = ref(false);
const searchInput = ref("");

const userInfo = JSON.parse(localStorage.getItem("userInfo"));
isLoggedIn.value = userInfo && userInfo.id;

//查詢邏輯

function handleKeyDown(event) {
  if (event.key === "Enter") {
    goToSearchPage();
  }
}

const router = useRouter();
function goToSearchPage() {
  router.push({ name: "book-searchall" });
}
</script>




<!-- <script lang="ts">
import { useRouter } from "vue-router";
import axios from "axios";
import { ref } from "vue";

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
    searchInput: "",
  }),
  created() {
    const userInfo = JSON.parse(localStorage.getItem("userInfo"));
    this.isLoggedIn = userInfo && userInfo.id;
  },
  methods: {
    logout() {
      const userInfo = JSON.parse(localStorage.getItem("userInfo"));
      if (userInfo && userInfo.id) {
        localStorage.removeItem("userInfo");
        this.$router.push("/Login");
      } else {
        this.$router.push("/Login");
      }
    },
    async querySearch(query) {
      try {
        const apiUrl = "https://localhost:7261/api/Books";
        const response = await axios.get(apiUrl);

        if (response.status === 200) {
          console.log(response.data);

          const allBooks = response.data;
          const filteredBooks = allBooks.filter((book) => {
            return (
              book.name?.includes(query) ||
              book.isbn?.includes(query) ||
              book.categoryName?.includes(query) ||
              book.publisherName?.includes(query)
            );
          });
          return filteredBooks;
        }
      } catch (error) {
        console.error("Error fetching suggestions:", error);
      }
      return [];
    },
    goToSearchPage() {
      const router = useRouter();
      router.push({ name: "book-searchall" });
    },
    handleKeyDown(event) {
      if (event.key === "Enter") {
        this.goToSearchPage();
      }
    },
  },
};
</script> -->

    
<style></style>