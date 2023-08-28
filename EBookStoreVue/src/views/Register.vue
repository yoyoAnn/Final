<template>
    <div class="container">
      <h1>註冊帳號</h1>
      <!-- <p>提醒您點擊註冊後<br />即表示您已閱讀並同意「布可網路書店」<br />服務條款與隱私權政策</p> -->
      <!-- <router-link to="/survice">服務條款</router-link> -->
      <p>
        提醒您點擊註冊後<br />
        即表示您已閱讀並同意「布可網路書店」<br />
        <a href="/">服務條款</a>
        與
        <a href="/">隱私權政策</a>
      </p>   

      <div class="errormessage">
        <a2 v-if="registrationMessage">{{ registrationMessage }}</a2>
      </div>
      
      <form @submit.prevent="register">
        <div class="input-group">
          <span><v-icon right icon="mdi:mdi-account" /></span>
          <input v-model="registerData.account" type="text" placeholder="帳號" required>
        </div>
        <div class="input-group">
          <span><v-icon right icon="mdi:mdi-email" /></span>
          <input v-model="registerData.email" type="email" placeholder="Email" required>
        </div>
        <div class="input-group">
          <span><v-icon right icon="mdi:mdi-lock" /></span>
          <input v-model="registerData.password" type="password" placeholder="密碼" required minlength="8">
        </div>
        <div class="input-group">
          <span><v-icon right icon="mdi:mdi-lock" /></span>
          <input v-model="registerData.confirmedPassword" type="password" placeholder="確認密碼" required minlength="8">
        </div>
        <button class="btn" type="submit">註冊帳號</button>
      </form>
     
    </div>
  </template>
  
<script>
import axios from 'axios';
  
export default {
data() {
    return {
    registerData: {
        account: '',
        email: '',
        password: '',
        confirmedPassword: ''
    },
    registrationMessage: ''
    };
},
methods: {
    async register() {
        try {
            const response = await axios.post("https://localhost:7261/api/Users/Register", this.registerData);
            console.log(response)

            this.registrationMessage = response.data;
            console.log(this.registrationMessage)

            this.registerData = {
            account: '',
            email: '',
            password: '',
            confirmedPassword: ''
            };
            console.log(this.registerData)

            console.log(response.data)
            console.log(response.status)
            


            if (response.status == 200) {
                this.$router.push('/ConformRegister');
            }
            
        } catch (error) {
            // this.registrationMessage = '註冊失敗';
            this.registrationMessage = error.response.data;
            console.error(error);
        }
    }
}
};
</script>
  


<style scoped>

/* @import "@/assets/registerStyle.css"; */
body {
    font-family: Arial, sans-serif;

    margin: 0;
    padding: 0;
    background-color: #f5f5f5;
    display: flex;
    align-items: center;
    justify-content: center;
    min-height: 100vh;
}

.container {
    background-color: #fff;
    padding: 60px;
    border-radius: 8px;
    box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
    width: 100%;
    max-width: 400px;
    text-align: center;
    margin-top: 40px;
    margin-bottom: 40px;
}

.container h1 {
    font-size: 40px;
    margin-bottom: 20px;
    /* color: #333; */
    color: #4CAF50;
    font-weight: bold;
}

.container p {
    font-size: 14px;
    margin-bottom: 30px;
    color: #666;
}

.input-group {
    margin-bottom: 15px;
    position: relative;
}

.input-group span {
    position: absolute;
    left: 10px;
    top: 50%;
    transform: translateY(-50%);
    color: #999;
}

.input-group span {
    position: absolute;
    left: 10px;
    top: 50%;
    transform: translateY(-50%);
    /* color: #999; */
    color: #4CAF50;
}

.input-group input[type="email"],
.input-group input[type="text"],
.input-group input[type="password"] {
    width: 100%;
    padding: 12px 0;
    /* padding: 12px 12px 12px 35px; */
    /* padding-left: 35px; */

    border: 1px solid #ccc;
    border-radius: 6px;
    text-indent: 40px;
}


.btn {
    background-color: #4CAF50;
    ;
    color: #fff;
    padding: 12px 0;
    border: none;
    width: 100%;
    border-radius: 6px;
    font-size: 16px;
    cursor: pointer;
    transition: background-color 0.3s ease;
}

.btn:hover {
    background-color: #00b894;
    color: #fff;
}

.clear {
    clear: both;
}


.container a2 {
    color: red;
    /* margin-bottom: 30px; */
    /* margin-top: 30px; */
}

.errormessage{
    margin-bottom: 20px;
}

</style>