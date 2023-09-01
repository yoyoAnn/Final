<template>
    <div class="profile-pic-wrapper">
        <div class="pic-holder">
            <img :src="uploadedImage || defaultImageUrl" alt="Uploaded Image" id="profilePic" class="image-preview" />

        </div>
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import axios from 'axios';

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
        })
        .catch(defaultImageError => {
            console.error(defaultImageError);
        });
    })
    .catch(error => {
        console.error(error);
    });
}

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



<script>
import { ref } from 'vue';

export default {
  name: "UserPicture",
};
</script>

<style scoped>

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
  /* width: 150px;
  height: 150px; */
  overflow: hidden;
  display: flex;
  justify-content: center;
  align-items: center;
  margin-bottom: 20px;
  margin-top: 8px;
}
/* .pic-holder {
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
} */

/* .pic-holder .pic {
  height: 100%;
  width: 100%;
  -o-object-fit: cover;
  object-fit: cover;
  -o-object-position: center;
  object-position: center;
} */

/* .pic-holder .image-preview {
  height: 100%;
  width: 100%;
  -o-object-fit: cover;
  object-fit: cover;
  -o-object-position: center;
  object-position: center;
} */

/* .pic-holder .upload-file-block,
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
} */

.pic-holder .upload-file-block {
  cursor: pointer;
}

.pic-holder:hover .upload-file-block,
.uploadProfileInput:focus ~ .upload-file-block {
  opacity: 1;
}

.pic-holder.uploadInProgress .upload-file-block {
  display: none;
}

.pic-holder.uploadInProgress .upload-loader {
  opacity: 1;
}


/* 圖預覽 */
.image-preview {
  width: 20px; 
  height: auto;
  max-height: 50px;
  max-width: 50px; 
}

a2 {
    color: red;
}

</style>