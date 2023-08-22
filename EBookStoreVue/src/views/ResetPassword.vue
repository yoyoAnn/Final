<template>
    <div class="container">
      <h1>重設密碼</h1><hr/><br />
    
      <a2 v-if="registrationMessage">{{ registrationMessage }}</a2>

      <form @submit.prevent="activateAccount">
        <div class="input-group">
          <span><v-icon right icon="mdi:mdi-lock" /></span>
          <input v-model="resetPasswordData.NewPassword" type="password" placeholder="新密碼" required>
        </div>
        <div class="input-group">
          <span><v-icon right icon="mdi:mdi-lock" /></span>
          <input v-model="resetPasswordData.ConfirmPassword" type="password" placeholder="確認密碼" required>
        </div>
        <br>
        <button class="btn" type="submit">重設密碼</button>
      </form>
      
    </div>
  </template>
  
<script>
import axios from 'axios';
  
export default {
  data() {
    return {
        resetPasswordData: {
            NewPassword: '',
            ConfirmPassword: '',
        },
        registrationMessage: ''
    };
  },
    // created() {
    //     this.activateAccount();
    // },
    methods: {
      async activateAccount() {
        const url = 'https://localhost:7261/api/Users/ResetPassword';
        const userId = Number(this.$route.query.userid); 
        const confirmCode = this.$route.query.confirmCode; 
  
        const data = {
          id: userId,
          confirmCode: confirmCode,
          newPassword: this.resetPasswordData.NewPassword,
          confirmedPassword: this.resetPasswordData.ConfirmPassword
        };
  
        try {
          const response = await axios.post(url, data);
        //   console.log(response.data);
        //   console.log(response);
        //   console.log(url);
        //   console.log("ok",confirmCode);
        //   console.log("ok",id);
        alert("已成功重新設定密碼，請重新登入");
        this.$router.push('/Login');
        } catch (error) {
        //   console.log("err",id);
        //   console.log("err",confirmCode);
            this.registrationMessage = error.response.data;
            console.error(error);
        }
      }
    },
};
</script>
  
  

<style scoped>

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

.container a2{
    color: red;
}

</style>