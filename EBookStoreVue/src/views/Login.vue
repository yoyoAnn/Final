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
            <h2>登入</h2>     
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
              <p>
                <button class="btn" type="submit">Sign In</button>
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
        © 2023 布可 版權所有。
      </div>
    </div>
  </template>
  
  <script>
  export default {
    data() {
      return {
        account: '',
        password: '',
        id: null,
        isLoggedIn: false, 
        errorMessage: '',   
      };
    },
    methods: {
      async login() {
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

          
        } else {
          console.error('Login failed');
        }
      },
    },
  };
  </script>
  

  <style scoped>

  @import "@/assets/loginStyle.css";
  
  </style>
  