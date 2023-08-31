<template>
  <div class="chatroom">
    <div v-for="(item, i) in messageList">
      <div class="user remote" v-if="item.userName !== '客服'">
        <div class="avatar">
          <div class="pic">
            <img src="https://picsum.photos/100/100?random=12" />
          </div>
          <div class="name">{{ item.userName }}</div>
        </div>
        <div class="text">{{ item.message }}</div>
      </div>
      <div class="user local" v-else>
        <div class="avatar">
          <div class="pic">
            <img src="https://picsum.photos/100/100?random=50" />
          </div>
          <div class="name">我</div>
        </div>
        <div class="text">{{ item.message }}</div>
      </div>
    </div>

    <div class="message">
      <input type="text" v-model="content" placeholder="請輸入內容" @keyup.enter="send" />
    </div>
  </div>
</template>

<script setup>
import { ref, nextTick, onMounted } from "vue";

var content = ref("");
var messageList = ref([]);

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
      userName: '客服',
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
  console.log(messageList.value);
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

})
</script>

<style>
.chatroom {
  z-index: 100;
  position: fixed;
  padding-top: 5px;
  width: 500px;
  opacity: 1;
  text-align: center;
  color: #2e7d32;
  font-size: 20px;
  font-weight: bold;
  padding: 20px;
  box-shadow: 0 0 10px #000;
  background-color: #f4f5f7;
  border-radius: 20px;
  max-height: 400px;
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
    width: 80px;
    height: auto;
    text-align: center;
    flex-shrink: 0;
  }

  .pic {
    border-radius: 50%;
    overflow: hidden;
    max-width: 80px;
    max-height: 80px;

    img {
      max-width: 80px;
      max-height: 80px;
      vertical-align: middle;
    }
  }

  .name {
    color: #333;
  }

  .text {
    margin-top: 20px;
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
  width: 100%;
  border: 2px solid gray;
  border-radius: 30px;
  text-align: center;
  font-size: 20px;
  line-height: 30px;
}
</style>
