<template>
  <v-container class="d-flex justify-content-center align-items-center">
    <div class="container-fluid m-3 w-50">
      <form
        class="form-group w-100 d-flex flex-column justify-content-center align-items-center"
        action=""
      >
        <input
          class="form-control mt-3 w-100"
          type="text"
          v-model="form.UserAccount"
          placeholder="會員帳號"
        />
        <input
          class="form-control mt-3 w-100"
          type="email"
          v-model="form.Email"
          placeholder="Email"
        />
        <input
          class="form-control mt-3 w-100"
          type="text"
          v-model="form.OrderId"
          placeholder="您的訂單編號(如沒有可不填)"
        />
        <select class="form-control mt-3 w-100" v-model="form.ProblemTypeId">
          <option value="" disabled selected>問題種類</option>
          <option v-for="(item, i) in problemTypes" :value="i + 1">
            {{ item }}
          </option>
        </select>
        <textarea
          class="form-control mt-3 w-100"
          v-model="form.ProblemStatement"
          placeholder="問題敘述"
        ></textarea>
        <input
          class="btn btn-primary mt-3"
          type="button"
          name="submit"
          id=""
          @click="submitMail"
          value="送出"
        />
      </form>
    </div>
  </v-container>
</template>
    
<script setup>
import { useVuelidate } from "@vuelidate/core";
import { email, required } from "@vuelidate/validators";
import "bootstrap/dist/js/bootstrap.bundle.js";
import { ref, reactive, onMounted } from "vue";
import axios from "axios";

const baseAddress = `https://localhost:7261`;
const problemTypes = ref([]);
// problemTypes.push("問題種類");
const loadProducts = async () => {
  const response = await fetch(`${baseAddress}/api/CustomerServiceMails`);
  const datas = await response.json();
  for (let i = 0; i < datas.length; i++) {
    problemTypes.value[i] = datas[i].name;
  }
};

const form = {
  Id: 0,
  UserAccount: "",
  Email: "",
  OrderId: null,
  ProblemTypeId: "",
  ProblemStatement: "",
};

const submitMail = () => {
  console.log(form);
  axios
    .post(`${baseAddress}/api/CustomerServiceMails`, form)
    .then((response) => {
      alert("成功");
    })
    .catch((err) => {
      alert(err);
    });
};

onMounted(() => {
  loadProducts();
});
</script>
    
<style>
</style>