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
                  {{ errorMessage }}
                </v-alert>

                <h2 class="title" color="success">會員資訊</h2>
                <hr /><br />

                <v-container class="vertical-split">
                  <div class="right-pane">
                    <!-- <p>右邊</p> -->

                    <!-- <div>
                                <div class="image-container">  
                                    <img :src="uploadedImage || defaultImage" alt="Uploaded Image" class="image-preview" /> 原
                                    <img :src="uploadedImage || defaultImageUrl" alt="Uploaded Image" class="image-preview" />
                                </div>
                                <input type="file" ref="fileInput" @change="uploadImage" />
                            </div> 
                                                      -->

                    <!-- 1 -->
                    <div class="profile-pic-wrapper">
                      <div class="pic-holder">
                        <img :src="uploadedImage || defaultImageUrl" alt="Uploaded Image" id="profilePic"
                          class="image-preview" />
                        <!-- <img id="profilePic" class="pic" src="https://img.88icon.com/download/jpg/20201201/43e87c7a222f94f7a2439fc39b5e4369_512_512.jpg!bg"> -->
                        <!-- <img id="profilePic" class="pic" src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR2o5ggIYeeru5fvV3xzW4O8SL0OS9FYNyfew&usqp=CAU"> -->

                        <Input class="uploadProfileInput" type="file" name="profile_pic" id="newProfilePhoto"
                          accept="image/*" style="opacity: 0;" @change="uploadImage" />
                        <label for="newProfilePhoto" class="upload-file-block">
                          <div class="text-center">
                            <div class="mb-2">
                              <i class="fa fa-camera fa-2x"></i>
                            </div>
                            <div class="text-uppercase">
                              上傳 <br /> 圖片
                            </div>
                          </div>
                        </label>
                      </div>
                    </div>



                  </div>

                  <div class="left-pane">
                    <div class="input-group">
                      <label for="email" class="input-label">信箱</label>
                      <input id="email" type="text" v-model="email" class="input-field-email" readonly />
                    </div>

                    <div class="input-group">
                      <label for="account" class="input-label">帳號</label>
                      <input id="account" type="text" v-model="account" class="input-field" />
                      <!-- <a2 v-if="errorMessage" class="error-message">{{ errorMessage }}</a2> -->
                    </div>

                    <div class="input-group">
                      <label for="name" class="input-label">姓名</label>
                      <input id="name" type="text" v-model="name" class="input-field" />
                    </div>

                    <div class="input-group">
                      <label for="phone" class="input-label">手機</label>
                      <input id="phone" type="text" v-model="phone" class="input-field" pattern="[0-9]{10}" />
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
                    <v-btn class="btn2 ml-3" @click="fillDefaultInfo" color="success">Demo用</v-btn>
                  </div>

                </v-container>

                <!-- <v-divider vertical class="mx-4"></v-divider>
                    <div class="uploadpage">
                        2
                    </div> -->
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
const photo = ref('');

const successAlert = ref(false);
const errorAlert = ref(false);
const errorMessage = ref('');

const fileInput = ref(null);
// const uploadedImage = ref("");
const uploadedImage = ref(null);
const defaultImage = ref("");
const photoUrl = ref("");
const defaultImageFileName = ref(""); // 預設圖片檔名
const defaultImageUrl = ref(""); // 完整預設圖片URL

const router = useRouter();

const navigateToPage = (page) => {
  router.push(`/${page}`);
};

const fillDefaultInfo = () => {
    name.value = '王小明';
    phone.value = '09123456789';
    address.value = '桃園市中壢區新生路二段421號';
    gender.value = false;
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


      // 預設圖片API 0824
      axios.post(`https://localhost:7261/api/Users/GetDefaultImage?id=${userInfo.id}`)
        .then(defaultImageResponse => {
          defaultImageFileName.value = defaultImageResponse.data.photo;

          defaultImageUrl.value = `https://localhost:7261/api/Users/GetImage/${defaultImageFileName.value}`;
          console.log(defaultImageUrl.value);
        })
        .catch(defaultImageError => {
          console.error(defaultImageError);
        });
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

  if (phone.value && !/^\d{10}$/.test(phone.value)) {
    errorMessage.value = '手機號碼應為 10 位數字';
    errorAlert.value = true;
    return;
  }

  if (address.value && address.value.length > 255) {
    errorMessage.value = '地址不能超過 255 個字';
    errorAlert.value = true;
    return;
  }

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
      errorMessage.value = error.response.data;
    });
};


const uploadImage = async (event) => {
  const file = event.target.files[0];
  if (file) {
    const formData = new FormData();
    formData.append("image", file);

    try {
      const response = await axios.post(
        "https://localhost:7261/api/Users/UploadImage",
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );

      // 提取檔名部分（從最後一個斜線 / 開始到字串結束）
      const fileName = response.data.substring(response.data.lastIndexOf("/") + 1);

      const userInfo = JSON.parse(localStorage.getItem('userInfo'));
      const id = userInfo.id;

      try {
        await axios.post(
          "https://localhost:7261/api/Users/UpdateUserPhoto",
          { id, photo: fileName }
        );
        console.log('圖片資料庫更新成功');
        history.go(0);
      } catch (error) {
        console.error('更新圖片資料庫失敗:', error);
      }

      uploadedImage.value = "https://localhost:7261/api/Users/GetImage/" + fileName;

    } catch (error) {
      console.error("上傳失敗:", error);
    }
  }
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
  /* 0828 */
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

.input-field-email {
  width: 50%;
  padding: 8px;
  font-size: 14px;
  background-color: #f2f2f2;
}

.gender-label {
  margin-top: 20px;
  font-size: 20px;
}

.gender-button {
  margin-left: 20px;
}

/* 左右 */

.vertical-split {
  display: flex;
  align-items: stretch;
}

.left-pane,
.right-pane {
  /* flex: 1; */
  padding: 0px;
}

.left-pane {
  /* border-right: 1px solid #ccc; */
  /* flex: 1.5; 原 */
  flex: 2.5;
  /* 左右交換後  */
  /* background-color: #f0f0f0; */
}

.right-pane {
  margin-left: 20px;
  flex: 1;
  /* background-color: #e0e0e0; */
}

/* 圖 */
.profile-pic-wrapper {
  height: 100vh;
  width: 100%;
  position: relative;
  display: flex;
  flex-direction: column;
  /* justify-content: center; */
  /* align-items: center; */
}

.pic-holder {
  text-align: center;
  position: relative;
  border-radius: 50%;
  width: 150px;
  height: 150px;
  overflow: hidden;
  display: flex;
  justify-content: center;
  align-items: center;
  margin-bottom: 20px;
}

/* .pic-holder .pic {
  height: 100%;
  width: 100%;
  -o-object-fit: cover;
  object-fit: cover;
  -o-object-position: center;
  object-position: center;
} */

.pic-holder .image-preview {
  height: 100%;
  width: 100%;
  -o-object-fit: cover;
  object-fit: cover;
  -o-object-position: center;
  object-position: center;
}

.pic-holder .upload-file-block,
.pic-holder .upload-loader {
  position: absolute;
  top: 0;
  left: 0;
  height: 100%;
  width: 100%;
  background-color: rgba(90, 92, 105, 0.7);
  color: #f8f9fc;
  font-size: 12px;
  font-weight: 600;
  opacity: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.2s;
}

.pic-holder .upload-file-block {
  cursor: pointer;
}

.pic-holder:hover .upload-file-block,
.uploadProfileInput:focus~.upload-file-block {
  opacity: 1;
}

.pic-holder.uploadInProgress .upload-file-block {
  display: none;
}

.pic-holder.uploadInProgress .upload-loader {
  opacity: 1;
}

/* Snackbar css */
.snackbar {
  visibility: hidden;
  min-width: 250px;
  background-color: #333;
  color: #fff;
  text-align: center;
  border-radius: 2px;
  padding: 16px;
  position: fixed;
  z-index: 1;
  left: 50%;
  bottom: 30px;
  font-size: 14px;
  transform: translateX(-50%);
}

.snackbar.show {
  visibility: visible;
  -webkit-animation: fadein 0.5s, fadeout 0.5s 2.5s;
  animation: fadein 0.5s, fadeout 0.5s 2.5s;
}

@-webkit-keyframes fadein {
  from {
    bottom: 0;
    opacity: 0;
  }

  to {
    bottom: 30px;
    opacity: 1;
  }
}

@keyframes fadein {
  from {
    bottom: 0;
    opacity: 0;
  }

  to {
    bottom: 30px;
    opacity: 1;
  }
}

@-webkit-keyframes fadeout {
  from {
    bottom: 30px;
    opacity: 1;
  }

  to {
    bottom: 0;
    opacity: 0;
  }
}

@keyframes fadeout {
  from {
    bottom: 30px;
    opacity: 1;
  }

  to {
    bottom: 0;
    opacity: 0;
  }
}

/* 圖預覽 */
.image-preview {
  width: 200px;
  /* 圖寬 */
  height: auto;
  /* 讓高度按照比例自適應 */
  max-height: 200px;
  /* 設置最大高度，防止圖片過大 */
}

a2 {
  color: red;
}
</style>
