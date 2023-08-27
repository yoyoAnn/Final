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
                        密碼更新成功
                    </v-alert><br />
                    <v-alert v-model="errorAlert" type="error" closable dismissible>
                        {{ errorMessage }}
                    </v-alert>

                    <h2 class="title" color="success">更改密碼</h2><hr/><br />
            
                    <div class="input-group">
                    <label for="OriginalPassword" class="input-label">原始密碼</label>
                    <input id="OriginalPassword" type="password" v-model="originalPassword" class="input-field" required/>
                    </div>

                    <div class="input-group">
                    <label for="Password" class="input-label">新的密碼</label>
                    <input id="Password" type="password" v-model="newPassword" class="input-field" required minlength="8"/>
                    </div>

                    <div class="input-group">
                    <label for="ConfirmPassword" class="input-label">確認密碼</label>
                    <input id="ConfirmPassword" type="password" v-model="confirmedPassword" class="input-field" required minlength="8"/>
                    </div>

                    
                    <v-btn @click="updatePassword" color="success">修改密碼</v-btn>
                </div>

              </v-sheet>
            </v-col>
          </v-row>
        </v-container>
      </v-main>
    </v-app>
</template>

<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import axios from 'axios';

const menuItems = [
    { label: '會員資訊', page: 'UserProfile' },
    { label: '更改密碼', page: 'ChangePassword' },
];

const successAlert = ref(false);
const errorAlert = ref(false);
const errorMessage = ref('');
const originalPassword = ref('');
const newPassword = ref('');
const confirmedPassword = ref('');

const userInfo = JSON.parse(localStorage.getItem('userInfo'));
const router = useRouter();

const navigateToPage = (page) => {
    router.push(`/${page}`);
};


const updatePassword = () => {
    successAlert.value = false;
    errorAlert.value = false;
    errorMessage.value = '';

    if (newPassword.value !== confirmedPassword.value) {
        errorMessage.value = '輸入的密碼不一致，請再重新確認';
        errorAlert.value = true;
        return;
    }
    if (!originalPassword.value || !newPassword.value || !confirmedPassword.value) {
        errorMessage.value = '請填寫所有欄位';
        errorAlert.value = true;
        return;
    }
    if (newPassword.value.length < 8) {
        errorMessage.value = '新的密碼長度至少需要 8 碼';
        errorAlert.value = true;
        return;
    }

    // console.log(originalPassword.value)
    // console.log(newPassword.value)
    // console.log(confirmedPassword.value)

    const data = {
        id: userInfo.id,
        originalPassword: originalPassword.value,
        newPassword: newPassword.value,
        confirmedPassword: confirmedPassword.value
    };
    // console.log(data)
    axios
        .post(`https://localhost:7261/api/Users/ChangePassword`, data)
        .then(response => {
            successAlert.value = true;
            errorMessage.value = '';
            // console.log(response.data)
            // console.log(response)

            originalPassword.value = '';
            newPassword.value = '';
            confirmedPassword.value = '';
    })
    .catch(error => {
        errorMessage.value = error.response.data;
        errorAlert.value = true;
    });
};

</script>

<style scoped>
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
  width: 35%; 
  padding: 8px; 
  font-size: 14px; 
}

.gender-label {
  margin-top: 20px; 
  font-size: 20px; 
}

.gender-button{
    margin-left: 20px;
}

</style>
