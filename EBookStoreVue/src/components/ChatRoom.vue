<template>
    <div class="chatroom" v-show="isShow">
        <div v-for="(item, i) in messageList">
            <div class="user remote" v-if="item.userName == '客服'">
                <div class="avatar">
                    <div class="pic">
                        <!-- <img src="https://picsum.photos/100/100?random=12" /> -->
                        <img src="https://localhost:7261/api/Users/GetImage/customerservice.png">
                    </div>
                    <div class="name">客服</div>
                </div>
                <div class="text">{{ item.message }}</div>
            </div>
            <div class="user local" v-else>
                <div class="avatar">
                    <div class="pic">
                        <img :src="userPhoto" />
                    </div>
                    <div class="name">我</div>
                </div>
                <div class="text">{{ item.message }}</div>
            </div>
        </div>

        <div class="message">
            <input type="text" v-model="content" placeholder="請輸入內容並按Enter發送" @keyup.enter="send" />
        </div>
    </div>
    <button class="chatbtn" @click="showChatRoom">
        <v-icon right icon="mdi:mdi-chat-question" />
    </button>
</template>
    
<script setup>
import { ref, nextTick, onMounted, onUpdated } from "vue";

var isShow = ref(false);
var content = ref("");
var messageList = ref([]);

//抓取user ID,帳號
const userId = ref(0);
const userAccount = ref("");
if (localStorage.getItem("userInfo") == null) {
    userAccount.value = "匿名";
} else {
    userId.value = JSON.parse(localStorage.getItem("userInfo")).id;
    userAccount.value = JSON.parse(localStorage.getItem("userInfo")).account;
}

//取得user 大頭貼
const userPhoto = ref("");
const loadUserPhoto = async () => {
    try {
        const response = await fetch(`https://localhost:7261/api/Users/${userId.value}`);
        if (!response.ok) {
            throw new Error(`Network response was not ok: ${response.status}`);
        }
        const data = await response.json();
        userPhoto.value = `https://localhost:7261/api/Users/GetImage/${data.photo}`;
    } catch (error) {
        console.error("Error loading books:", error);
        userPhoto.value = "https://localhost:7261/api/Users/GetImage/defaultUser.jpg";
    }
};

//聊天室開關設定
var showChatRoom = () => {
    isShow.value = !isShow.value;
};

//建立websocket連線
var wsUrl = "wss://localhost:7261/ws";
var socket = new WebSocket(wsUrl);
socket.onopen = (event) => {
    console.log("connected");
};

//將輸入的訊息傳自WebAPI
var send = () => {
    if (socket && socket.readyState == WebSocket.OPEN) {
        var data = {
            userName: userAccount.value,
            message: content.value,
        };
        socket.send(JSON.stringify(data));
    }
    content.value = "";
};

//將輸入的訊息顯示出來
socket.onmessage = (event) => {
    processMessage(event.data);
};

var processMessage = (data) => {
    var json = JSON.parse(data);
    messageList.value.push({ userName: json.userName, message: json.message });
    scrollToBottom();
};

//每當新訊息送出，將訊息框捲軸置底
const chatRoom = ref(null);
function scrollToBottom() {
    nextTick(() => {
        chatRoom.value.scrollTop = chatRoom.value.scrollHeight - chatRoom.value.offsetHeight;
    });

}

onMounted(async () => {
    await nextTick;
    chatRoom.value = document.getElementsByClassName("chatroom")[0];
    await scrollToBottom();
    await loadUserPhoto();

})

onUpdated(() => {
    if (localStorage.getItem("userInfo") == null) {
        userAccount.value = "匿名";
    } else {
        userId.value = JSON.parse(localStorage.getItem("userInfo")).id;
        userAccount.value = JSON.parse(localStorage.getItem("userInfo")).account;
    }
    loadUserPhoto();
})
</script>
    
<style>
.chatroom {
    z-index: 99999;
    position: fixed;
    padding-top: 5px;
    bottom: 30px;
    right: 100px;
    width: 400px;
    opacity: 1;
    text-align: center;
    color: #2e7d32;
    font-size: 16px;
    font-weight: bold;
    padding: 20px;
    box-shadow: 0 0 10px #000;
    background-color: #f4f5f7;
    border-radius: 20px;
    min-height: 80px;
    max-height: 300px;
    overflow-y: overlay;

    &::-webkit-scrollbar {
        width: 7px;
    }

    &::-webkit-scrollbar-button {
        background: transparent;
        border-radius: 4px;
    }

    &::-webkit-scrollbar-track-piece {
        background: transparent;
    }

    &::-webkit-scrollbar-thumb {
        border-radius: 4px;
        background-color: rgba(0, 0, 0, 0.4);
        border: 1px solid slategrey;
    }

    &::-webkit-scrollbar-track {
        box-shadow: transparent;
    }
}

.user {
    display: flex;
    align-items: flex-start;
    margin-bottom: 20px;

    .avatar {
        width: 60px;
        text-align: center;
        flex-shrink: 0;
    }

    .pic {
        border-radius: 50%;
        overflow: hidden;

        img {
            width: 60px;
            vertical-align: middle;
        }
    }

    .name {
        color: #333;
    }

    .text {
        margin-top: 10px;
        display: flex;
        align-items: center;
        min-height: 40px;
        height: auto;
        background-color: #aaa;
        padding: 10px;
        border-radius: 16px;
        position: relative;
    }
}

.remote {
    .text {
        margin-left: 20px;
        margin-right: 60px;
        color: #eee;
        background-color: #4179f1;
        text-align: start;

        &::before {
            border-right: 18px solid #4179f1;
            position: absolute;
            left: -12px;
        }
    }
}

.local {
    justify-content: flex-end;

    .text {
        margin-right: 20px;
        margin-left: 60px;
        order: -1;
        background-color: #fff;
        color: #333;
        text-align: start;

        &::before {
            border-left: 18px solid #fff;
            right: -12px;
        }
    }
}

.remote,
.local {
    & .text::before {
        content: "";
        position: absolute;
        top: 20px;
        border-top: 10px solid transparent;
        border-bottom: 10px solid transparent;
    }

    .text {
        font-weight: 300;
        box-shadow: 0 0 10px #888;
    }
}

.message input {
    margin-top: 5px;
    width: 100%;
    border: 2px solid gray;
    border-radius: 30px;
    text-align: center;
}

.chatbtn {
    z-index: 99999;
    position: fixed;
    bottom: 30px;
    right: 15px;
    height: 80px;
    width: 80px;
    background-color: #fff;
    opacity: 1;
    /* box-shadow: var(--el-box-shadow-lighter); */
    box-shadow: 0 0 10px #000;
    text-align: center;
    line-height: 40px;
    color: #2e7d32;
    border-radius: 50%;
    font-size: 36px;
    font-weight: bold;
}

.chatbtn:hover {
    color: #fff;
    background-color: rgba(46, 125, 50, 0.7);
    border: 3px solid #fff;
    box-shadow: 0 0 10px #000;
}
</style>