<template>
    <div class="wrapper login-3">
      <div class="container">
        <div class="col-left">
          <div class="login-text">
            <h3>歡迎回來</h3>
            <h2>布可</h2>
            <p>
              請先登入您的帳號<br />
              以繼續使用服務
            </p>
            <!-- <a class="btn" href="">Read More</a> -->
          </div>
        </div>
        <div class="col-right">
  
          <div class="login-form">
            <!-- <h2>登入</h2>      -->
  
            <div class="errormessage">
              <a2 v-if="errorMessage" class="error-message">{{ errorMessage }}</a2>
            </div>
  
            <form @submit.prevent="login">
              <p>
                <input v-model="account" type="text" placeholder="帳號" required>
              </p>
              <p>
                <input v-model="password" type="password" placeholder="密碼" required>
  
                <!-- <div class="form-floating">
                      <input :type="isActive ? 'password' : 'text'" v-model="password" class="form-control" placeholder="密碼" required>
                      <label for="floatingPassword">密碼</label>
                      <i :class="[isActive ? 'fa-eye' : 'fa-eye-slash', 'fas']" @click="isActive = !isActive"></i>
                  </div> -->
  
                <!-- <div class="form-floating">
                      <input  :type="isActive ? 'password' : 'text'" v-model="password" class="form-control" placeholder="密碼" required>
                      <label for="floatingPassword">密碼</label>
                      <i :class="[isActive ? 'fa-eye' : 'fa-eye-slash', 'fas', 'eye-icon']" @click="isActive = !isActive"></i>
                  </div> -->
  
              </p>
  
  
              <div class="input-section">
                <div class="captcha-section">
                  <input v-model="captchaInput" type="text" placeholder="請輸入驗證碼" required>
                  <s-identify :identifyCode="identifyCode" :contentWidth="120" :contentHeight="40"></s-identify>
                </div>
              </div>
  
  
              <p>
                <!-- <button class="btn" type="submit" :disabled="!isCaptchaValid">Sign In</button> -->
                <button class="btn" type="submit">Sign In</button>
              </p>
  
              <!-- <p>
                  <button class="btn" type="submit">Sign In</button>
                </p> -->
  
              <p>
                <button class="btn" @click="fillDefaultLogin">Demo用 一鍵輸入</button>
              </p>
  
  
  
  
              <!-- <hr> -->
  
              <p class="googleloginicon">
                <GoogleLogin :callback="callback" prompt style="width: 100%;"></GoogleLogin>
              </p>
  
  
              <p>
                <a href="/forgetpassword">Forget password?</a>
                <a href="/register">新增帳號</a>
              </p>
            </form>
          </div>
        </div>
      </div>
      <div class="credit">
        <!-- © 2023 布可 版權所有。 -->
      </div>
    </div>
  </template>
    
  <script>
  import GoogleLogin from "../components/GoogleLogin.vue";
  import SIdentify from "../components/SIdentify.vue";
  
  export default {
    components: {
      GoogleLogin,
      SIdentify
    },
    data() {
      return {
        account: '',
        password: '',
        id: null,
        isLoggedIn: false,
        errorMessage: '',
  
        identifyCode: "",
        identifyCodes: "0123456789abcdwerwshdjeJKDHRJHKOOPLMKQ",
        captchaInput: '',
      };
    },
    computed: {
      isCaptchaValid() {
        return this.captchaInput.toLowerCase() === this.identifyCode.toLowerCase();
      },
    },
    created() {
      this.refreshCode();
    },
    methods: {
      async login() {
        if (!this.isCaptchaValid) {
          this.errorMessage = '驗證碼錯誤';
          return;
        }
        const response = await fetch(`https://localhost:7261/api/Login`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            Account: this.account,
            Password: this.password,
            Id: this.id,
          }),
        });
  
  
        if (response.ok) {
          const data = await response.json();
  
          const userInfo = {
            account: this.account,
            // password: this.password,
            id: data.userId,
          };
  
          //   this.isLoggedIn = true;
          //   console.log(this.isLoggedIn)
  
            localStorage.setItem('userInfo', JSON.stringify(userInfo));
    
            this.$router.push('/');
  
            
          } else if (response.status === 400) {
              const errorData = await response.text();
              console.log(response)
              this.errorMessage = errorData; 
          } else {
              console.error('Login failed');
          }
        },
  
        // 驗證碼
        refreshCode() {
              this.identifyCode = "";
              this.makeCode(this.identifyCodes,4);
          },
          randomNum (min, max) {
              max = max + 1
              return Math.floor(Math.random() * (max - min) + min)
          },
          
          makeCode (data, len) {
              for (let i = 0; i < len; i++) {
                  this.identifyCode += data[this.randomNum(0, data.length - 1)]
          }
        },
  
        fillDefaultLogin() {
          this.account = 'yoyo2023'; 
          this.password = '12345678'; 
        },
         
  
      // 驗證碼
      refreshCode() {
        this.identifyCode = "";
        this.makeCode(this.identifyCodes, 4);
      },
      randomNum(min, max) {
        max = max + 1
        return Math.floor(Math.random() * (max - min) + min)
      },
  
      makeCode(data, len) {
        for (let i = 0; i < len; i++) {
          this.identifyCode += data[this.randomNum(0, data.length - 1)]
        }
      },
  
      fillDefaultLogin() {
        this.account = 'yoyo2023';
        this.password = '12345678';
      },
  
    },
  };
  </script>
    
  
  <style scoped>
  @import "@/assets/loginStyle.css";
  
  .container a2 {
    color: red;
  }
  
  .errormessage {
    margin-bottom: 20px;
  }
  
  .captcha-section {
    display: flex;
    align-items: center;
    margin-bottom: 20px;
  }
  </style>