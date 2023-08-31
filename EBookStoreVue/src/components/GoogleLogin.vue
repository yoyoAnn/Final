<template>
    <div>
      <GoogleLogin :callback="callback"  :class="customGoogleLoginClass" style="width: 200px;"/>

      <!-- <p>
        {{ data }}
      </p> -->
    </div>

    <div class="errormessage">
        <a2 v-if="errorMessage" class="error-message">{{ errorMessage }}</a2>
    </div>

    <!-- <div v-if="loggedIn">
        <button @click="logout">Logout</button>
        <h2>name:{{ user.name }}</h2>
        <h2>email:{{ user.email }}</h2>
        <img :src="user.picture" />
    </div>
    <div v-else>
        <GoogleLogin :callback="callback"></GoogleLogin>
    </div> -->


</template>
      
<script>
import axios from 'axios';
import { googleOneTap } from 'vue3-google-login'
import { decodeCredential, googleLogout } from 'vue3-google-login';


export default {
  data() {
    return {
      loggedIn: false, 
      user: null,
      errorMessage: '',  

      //response -> token
      callback: async (response) => {

        console.log("logged in", response);
        this.loggedIn = true;

        // let user = decodeCredential(response.credential); //解碼
        this.user = decodeCredential(response.credential);
        console.log(this.user);
        
        // const googleLogin = {
        //   email: this.user.email,
        //   name: this.user.name,
        // };

        await axios
            .post(
            "https://localhost:7261/api/Login/GoogleLogin",
                response.credential,
                {
                    withCredentials: true,
                    headers: {
                    "Content-Type": "application/json",
                    },                    
                }
            )
            .then((Response) => {
                // console.log(Response);
                // console.log(Response.data); //{userId: 195}
                if(Response.status == 200){
                    const userInfo = {
                        account: this.user.email,
                        id: Response.data.userId,
                    };

                    localStorage.setItem('userInfo', JSON.stringify(userInfo));
  
                    this.$router.push('/');

                }
            })
            .catch((error) => {
                console.error(error);
                this.errorMessage = " *此Email已使用一般用戶註冊";
                // alert("Email已註冊一般用戶")
        });
      },
    };
  },
};

</script>


<style scoped>
.container a2 {
    color: red;
    font-size: medium;
}
.errormessage{
    margin-bottom: 20px;
}



</style>