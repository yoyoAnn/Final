<template>
    <div class="chatroom" v-show="isShow">
        <!-- <div class="user remote">
            <div class="avatar">
                <div class="pic">
                    <img src="https://picsum.photos/100/100?random=12" />
                </div>
                <div class="name">You</div>
            </div>
            <div class="text">這是妳的對話文字</div>
        </div>
        <div class="user local">
            <div class="avatar">
                <div class="pic">
                    <img src="https://picsum.photos/100/100?random=16" />
                </div>
                <div class="name">Me</div>
            </div>
            <div class="text">這是我的對話文字</div>
        </div> -->
        <div class="p-2 chat">
            <p v-for="message in messageList">{{ message }}</p>
        </div>
        <div class="message">
            <input type="text" v-model="content" placeholder="請輸入內容" @keyup.enter="send">
        </div>
    </div>
    <button class="chatbtn" @click="showChatRoom">
        <v-icon right icon="mdi:mdi-chat-question" />
    </button>
</template>
    
<script setup>
import { ref } from "vue";

var isShow = ref(false);
var content = ref("");
var messageList = ref([]);

//聊天室開關設定
var showChatRoom = () => {
    isShow.value = !isShow.value;
};

//建立websocket連線
var wsUrl = "wss://localhost:7261/ws";
var socket = new WebSocket(wsUrl);
socket.onopen = (event) => {
    //alert("open");
    console.log("connected");
}
socket.onmessage = (event) => {
    processMessage(event.data)
};


var send = () => {
    if (socket && socket.readyState == WebSocket.OPEN) {
        var data = {
            userName: "Ann",
            message: content.value
        }
        socket.send(JSON.stringify(data));
    }
};

var processMessage = (data) => {
    // var ul = document.getElementById("#chatContent");
    // var li = document.createElement("li");
    var json = JSON.parse(data);
    // li.textContent = `${json["userName"]}說:${json["message"]}`;
    // ul.append(li);
    messageList.value.push(`${json.userName}說:${json.message}`);
};
</script>
    
<style>
.chatroom {
    z-index: 100;
    position: fixed;
    bottom: 30px;
    right: 100px;
    width: 500px;
    opacity: 1;
    text-align: center;
    color: #2e7d32;
    font-size: 36px;
    font-weight: bold;
    padding: 20px;
    box-shadow: 0 0 10px #000;
    background-color: #f4f5f7;
    border-radius: 20px;
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
        background-color: #aaa;
        padding: 16px;
        border-radius: 10px;
        position: relative;
    }
}

.remote {
    .text {
        margin-left: 20px;
        margin-right: 80px;
        color: #eee;
        background-color: #4179f1;

        &::before {
            border-right: 10px solid #4179f1;
            left: -10px;
        }
    }
}

.local {
    justify-content: flex-end;

    .text {
        margin-right: 20px;
        margin-left: 80px;
        order: -1;
        background-color: #fff;
        color: #333;

        &::before {
            border-left: 10px solid #fff;
            right: -10px;
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

.chatbtn {
    z-index: 100;
    position: fixed;
    bottom: 30px;
    right: 15px;
    height: 80px;
    width: 80px;
    background-color: #fff;
    opacity: 1;
    box-shadow: var(--el-box-shadow-lighter);
    text-align: center;
    line-height: 40px;
    color: #2e7d32;
    border-radius: 50%;
    font-size: 36px;
    font-weight: bold;
}

.chatbtn:hover {
    color: #fff;
    background-color: #2e7d32;
    border: 3px solid #fff;
}
</style>