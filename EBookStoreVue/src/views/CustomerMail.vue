<template>
  <div class="container-fluid m-3">
    <form>
      <v-select
        variant="outlined"
        v-model="state.select"
        :items="items"
        :error-messages="v$.select.$errors.map((e) => e.$message)"
        label="問題種類"
        required
        @change="v$.select.$touch"
        @blur="v$.select.$touch"
      ></v-select>

      <v-text-field
        variant="outlined"
        v-model="state.email"
        :error-messages="v$.email.$errors.map((e) => e.$message)"
        label="E-mail"
        required
        @input="v$.email.$touch"
        @blur="v$.email.$touch"
      ></v-text-field>

      <v-text-field
        variant="outlined"
        v-model="state.name"
        :error-messages="v$.name.$errors.map((e) => e.$message)"
        :counter="10"
        label="帳號"
        required
        @input="v$.name.$touch"
        @blur="v$.name.$touch"
      ></v-text-field>
      <v-textarea label="Label" variant="outlined"></v-textarea>
      <v-checkbox
        v-model="state.checkbox"
        :error-messages="v$.checkbox.$errors.map((e) => e.$message)"
        label="Do you agree?"
        required
        @change="v$.checkbox.$touch"
        @blur="v$.checkbox.$touch"
      ></v-checkbox>

      <v-btn class="me-4" @click="v$.$validate"> submit </v-btn>
      <v-btn @click="clear"> clear </v-btn>
    </form>
  </div>
</template>
    
<script setup>
import { useVuelidate } from "@vuelidate/core";
import { email, required } from "@vuelidate/validators";
import "bootstrap/dist/js/bootstrap.bundle.js";
import { ref, reactive, onMounted } from "vue";

const initialState = {
  name: "",
  email: "",
  select: null,
  checkbox: null,
};

const state = reactive({
  ...initialState,
});

const items = ["Item 1", "Item 2", "Item 3", "Item 4"];

const rules = {
  name: { required },
  email: { required, email },
  select: { required },
  items: { required },
  checkbox: { required },
};

const v$ = useVuelidate(rules, state);

function clear() {
  v$.value.$reset();

  for (const [key, value] of Object.entries(initialState)) {
    state[key] = value;
  }
}

const mail = ref([]);
const loadProducts = async () => {
  const response = await fetch(
    `https://localhost:7261/api/CustomerServiceMails`
  );
  const datas = await response.json();
  mail.value = datas;
  console.log(mail.value);
};
onMounted(() => {
  loadProducts();
});
</script>
    
<style>
</style>