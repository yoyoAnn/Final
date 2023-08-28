<template>
    <div class="container">
        <h1 class="title">帳號驗證成功!</h1>
    </div>
</template>
  
<script>
import axios from 'axios';
  
export default {
    created() {
      this.activateAccount();
    },
    methods: {
      async activateAccount() {
        const url = 'https://localhost:7261/api/Users/ActiveRegister';
        const userId = Number(this.$route.query.userid); 
        const confirmCode = this.$route.query.confirmCode; 
  
        const data = {
          Id: userId,
          ConfirmCode: confirmCode
        };
  
        try {
          const response = await axios.post(url, data);
          console.log(response.data);
          console.log(response);
        //   console.log(url);
          console.log("ok",userId);
          console.log("ok",confirmCode);

          console.log("ok",response.status);
          if (response.status == 200) {
            setTimeout(() => {
                window.location = "https://127.0.0.1:8080/Login";
            }, 1000);
          } 
        } catch (error) {
         
        //   console.error(error);
          console.log("err",userId);
          console.log("err",confirmCode);
        }
      }
    },
  };
</script>

<style scoped>
.container {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    margin-top: 40px;
}

.title {
    text-align: center;
    font-weight: bold;
    margin-bottom: 10px;
    font-size: 50px;
}
</style>
  