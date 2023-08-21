<template>
    <v-app id="inspire" class="bg-grey-lighten-3">
      <v-main class="bg-grey-lighten-3" style="margin-top: 25px;">
        <v-container>
          <v-row>
            <v-col cols="2">
              <v-sheet rounded="lg" class="text-center">
                <v-list rounded="lg">
                  <v-list-item color="grey-lighten-4" title="會員中心"></v-list-item>
                  <v-divider class="my-2"></v-divider>
                  <v-list-item v-for="(item, index) in menuItems" :key="index" link @click="navigateToPage(item.page)">
                    {{ item.label }}
                  </v-list-item>
                </v-list>
              </v-sheet>
            </v-col>
  
            <v-col>
              <v-sheet min-height="70vh" rounded="lg">
                <div class="form-container">

                    <v-alert v-model="successAlert" type="success" closable dismissible>
                        資料更新成功
                    </v-alert><br />
                    <v-alert v-model="errorAlert" type="error" closable dismissible>
                        {{ errorMessage }}error
                    </v-alert>
                  
                    <h2 class="title" color="success">會員資訊</h2><hr/><br />

                    <div class="input-group">
                    <label for="email" class="input-label">信箱</label>
                    <input id="email" type="text" v-model="email" class="input-field-email" readonly />
                    </div>

                    <div class="input-group">
                    <label for="account" class="input-label">帳號</label>
                    <input id="account" type="text" v-model="account" class="input-field" />
                    </div>

                    <div class="input-group">
                    <label for="name" class="input-label">姓名</label>
                    <input id="name" type="text" v-model="name" class="input-field" />
                    </div>

                    <div class="input-group">
                    <label for="phone" class="input-label">手機</label>
                    <input id="phone" type="text" v-model="phone" class="input-field" />
                    </div>

                    <div class="input-group">
                    <label for="address" class="input-label">地址</label>
                    <input id="address" type="text" v-model="address" class="input-field" />
                    </div>

                    <!-- <div class="input-group"> -->
                    <p class="gender-label">性別 
                    <input class="gender-button" type="radio" name="gender" v-model="gender" :value="false">男 
                    <input class="gender-button" type="radio" name="gender" v-model="gender" :value="true">女
                    </p>
                    <!-- </div> -->

                    
                    <v-btn @click="updateUserInfo" color="success">更新</v-btn>

                </div>

              </v-sheet>
            </v-col>
          </v-row>
        </v-container>
      </v-main>
    </v-app>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import axios from 'axios';

const menuItems = [
    { label: '會員資訊', page: 'UserProfile' },
    { label: '更改密碼', page: 'ChangePassword' },
];

const email = ref('');
const account = ref('');
const name = ref('');
const phone = ref('');
const address = ref('');
const gender = ref(false);

const successAlert = ref(false);
const errorAlert = ref(false);
const errorMessage = ref('');

const router = useRouter();

const navigateToPage = (page) => {
    router.push(`/${page}`);
};

const userInfo = JSON.parse(localStorage.getItem('userInfo'));
if (userInfo) {
    axios.get(`https://localhost:7261/api/Users/GetUserProfileById?id=${userInfo.id}`)
    .then(response => {
        const userProfile = response.data;
        email.value = userProfile.email;
        account.value = userProfile.account;
        name.value = userProfile.name;
        phone.value = userProfile.phone;
        address.value = userProfile.address;
        gender.value = userProfile.gender;
    })
    .catch(error => {
        console.error(error);
    });
}
// console.log('gender1:', gender.value);
const updateUserInfo = () => {
    const updatedUser = {
        id: userInfo.id,
        account: account.value,
        name: name.value,
        email: email.value,
        phone: phone.value,
        address: address.value,
        gender: gender.value
    };
    // console.log('gender2:', gender.value);

    axios.put(`https://localhost:7261/api/Users/EditProfile`, updatedUser)
    .then(response => {
      console.log(response.data);
      successAlert.value = true; 
      errorAlert.value = false;

    })
    .catch(error => {
      console.error(error);
      successAlert.value = false; 
      errorAlert.value = true;
    });
};

</script>

<style>
.form-container {
/* display: flex; */
flex-direction: column;
align-items: center;
padding: 50px;
}

.title {
  color: #148118; 
  font-weight: bold; 
  margin-bottom: 20px; 
}

input {
  border: 1px solid #ccc;
  background-color: white;
}

.input-group {
  margin-bottom: 20px;
}

.input-label {
  display: block;
  margin-bottom: 5px; 
  margin-right: 20px; 
  font-size: 20px; 
  /* font-weight: bold; */
}

.input-field {
  width: 50%; 
  padding: 8px; 
  font-size: 14px; 
}

.input-field-email{
  width: 35%; 
  padding: 8px; 
  font-size: 14px; 
  background-color: #f2f2f2;
}

.gender-label {
  margin-top: 20px; 
  font-size: 20px; 
}

.gender-button{
  margin-left: 20px;
}


</style>
