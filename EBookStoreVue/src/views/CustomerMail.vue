<template>
  <v-container class="d-flex justify-content-center align-items-center">
    <div class="container-fluid m-2" style="width: 600px">
      <h2 class="text-center mb-3">客服信箱</h2>
      <div class="p-2 border">
        <p class="mb-0">
          親愛的顧客，您好：
          <br />
          若您對我們提供的產品、服務內容有任何問題，建議您先參考<a href="/QA"
            >常見問題</a
          >，以快速找到解答。若您仍有更多疑問，請填寫下方表格，我們在收到您的信件後會盡快處理。
        </p>
      </div>
      <form
        class="form-group mt-2 w-100 d-flex flex-column justify-content-center align-items-center"
        action=""
      >
        <div class="row col-12">
          <div class="col-3 p-0">
            <label
              class="form-label m-3 d-flex justify-content-start"
              for="Email"
              >Email：</label
            >
          </div>
          <div class="col-9">
            <input
              class="form-control m-3"
              type="email"
              id="Email"
              v-model="form.Email"
              placeholder="(*必填*)"
              @blur="validate"
            />
          </div>
        </div>
        <div class="m-0 p-0 text-red">{{ errorMsg.emailErr }}</div>
        <div class="row col-12">
          <div class="col-3 p-0">
            <label
              class="form-label m-3 d-flex justify-content-start"
              for="Account"
              >會員帳號：</label
            >
          </div>
          <div class="col-9">
            <input
              class="form-control m-3"
              type="text"
              id="Account"
              v-model="form.UserAccount"
              placeholder="(非必填)"
            />
          </div>
        </div>
        <div class="row col-12">
          <div class="col-3 p-0">
            <label
              class="form-label m-3 d-flex justify-content-start"
              for="OrderId"
              >訂單編號：</label
            >
          </div>
          <div class="col-9">
            <input
              class="form-control m-3"
              type="text"
              v-model="form.OrderId"
              placeholder="(非必填)"
            />
          </div>
        </div>
        <select
          class="form-control m-3 w-100"
          v-model="form.ProblemTypeId"
          @blur="validate"
        >
          <option value="" disabled selected>問題種類</option>
          <option v-for="(item, i) in problemTypes" :value="i + 1">
            {{ item }}
          </option>
          <!-- <option v-for="(item, i) in problemTypes.items" :value="i + 1">
            {{ item }} 
          </option>-->
        </select>
        <div class="m-0 p-0 text-red">{{ errorMsg.problemTypeIdErr }}</div>
        <textarea
          class="form-control m-3 w-100"
          style="height: 100px"
          maxlength="200"
          v-model="form.ProblemStatement"
          placeholder="問題敘述(請勿超過200字)"
          @blur="validate"
        ></textarea>
        <div class="m-0 p-0 text-red">{{ errorMsg.problemStatementErr }}</div>
        <div class="row col-12 d-flex justify-content-center">
          <input
            class="btn btn-primary mt-3 col-2 mr-5"
            type="button"
            name="submit"
            id=""
            @click="submitMail"
            value="送出"
          />
          <input
            class="btn btn-primary-outline mt-3 col-2"
            type="button"
            name="submit"
            id=""
            @click="clear"
            value="清除"
          />
        </div>
      </form>
    </div>
  </v-container>
</template>
    
<script setup>
import { useVuelidate } from "@vuelidate/core";
import { maxLength, email, required } from "@vuelidate/validators";
import "bootstrap/dist/js/bootstrap.bundle.js";
import { ref, reactive, onMounted, watch } from "vue";
import axios from "axios";

//抓取user帳號
const userAccount = ref("");
if (localStorage.getItem("userInfo") == null) {
  userAccount.value = null;
} else {
  userAccount.value = JSON.parse(localStorage.getItem("userInfo")).account;
}

//串接WebAPI的ProblemTypes
const baseAddress = `https://localhost:7261`;
//const problemTypes = reactive({ items: [] });
const problemTypes = ref([]);
const loadProducts = async () => {
  const response = await fetch(`${baseAddress}/api/CustomerServiceMails`);
  const datas = await response.json();
  for (let i = 0; i < datas.length; i++) {
    problemTypes.value[i] = datas[i].name;
    //problemTypes.items.push(datas[i].name);
  }
};

//表單屬性
const form = reactive({
  Id: 0,
  UserAccount: userAccount.value ? userAccount.value : "",
  Email: "",
  OrderId: null,
  ProblemTypeId: "",
  ProblemStatement: "",
});
//Error屬性
const errorMsg = reactive({
  emailErr: "",
  problemTypeIdErr: "",
  problemStatementErr: "",
});

//表單驗證
const validate = () => {
  let valid = true;

  // 信箱驗證
  let emailrule =
    /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z]+$/;
  if (!form.Email.trim()) {
    errorMsg.emailErr = "*信箱未填寫*";
    valid = false;
  } else if (emailrule.test(form.Email) == false) {
    errorMsg.emailErr = "*信箱格式不正確*";
    valid = false;
  } else {
    errorMsg.emailErr = "";
  }

  // 問題種類驗證
  if (!form.ProblemTypeId) {
    errorMsg.problemTypeIdErr = "*問題種類未選擇*";
    valid = false;
  } else {
    errorMsg.problemTypeIdErr = "";
  }

  // 問題敘述驗證
  if (!form.ProblemStatement.trim()) {
    errorMsg.problemStatementErr = "*問題敘述未填寫*";
    valid = false;
  } else if (form.ProblemStatement.length > 200) {
    errorMsg.problemStatementErr = "*問題敘述不能超過200個字*";
    valid = false;
  } else {
    errorMsg.problemStatementErr = "";
  }
  return valid;
};

//客服信件送出
const submitMail = () => {
  if (!validate()) {
    return;
  }
  axios
    .post(`${baseAddress}/api/CustomerServiceMails`, form)
    .then((response) => {
      alert("成功");
    })
    .catch((err) => {
      alert(err);
    });
};

//資料清除
const clear = () => {
  form.UserAccount = "";
  form.Email = "";
  form.OrderId = null;
  form.ProblemTypeId = "";
  form.ProblemStatement = "";
};

onMounted(() => {
  loadProducts();
});
</script>
    
<style></style>