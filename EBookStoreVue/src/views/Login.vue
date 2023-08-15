<template>
    <br />
    <v-container>
      <v-form @submit.prevent="login">
        <v-card>
          <v-card-title>
            <h3>Login</h3>
          </v-card-title>
  
          <v-card-text>
            <v-text-field
              v-model="account"
              label="帳號"
              outlined
              placeholder="請輸入帳號"
            ></v-text-field>
  
            <v-text-field
              v-model="password"
              label="密碼"
              type="password"
              outlined
              placeholder="請輸入密碼"
            ></v-text-field>
          </v-card-text>
  
          <v-card-actions>
            <v-btn depressed color="blue" block @click="login">登入</v-btn>
          </v-card-actions>
        </v-card>
      </v-form>
    </v-container>
  </template>
  
  <script>

  export default {
    data() {
      return {
        account: '',
        password: '',
        id: null,
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
            password: this.password,
            // Id: this.id,
            id: data.userId,
          };
        
        //   console.log(userInfo)
          localStorage.setItem('userInfo', JSON.stringify(userInfo));

          this.$router.push('/');
        } else {
        //   console.error('Login failed');
        }
      },
    },
  };
  </script>
  