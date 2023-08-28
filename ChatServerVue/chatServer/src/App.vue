<script setup>
import HelloWorld from "./components/HelloWorld.vue";
import TheWelcome from "./components/TheWelcome.vue";

import { ref } from "vue";

var content = ref("");
var messageList = ref([]);

//建立websocket連線
var wsUrl = "wss://localhost:7261/ws";
var socket = new WebSocket(wsUrl);
socket.onopen = (event) => {
  //alert("open");
  console.log("connected");
};
socket.onmessage = (event) => {
  processMessage(event.data);
};

var send = () => {
  if (socket && socket.readyState == WebSocket.OPEN) {
    var data = {
      userName: "Ann",
      message: content.value,
    };
    socket.send(JSON.stringify(data));
  }
};

var processMessage = (data) => {
  var json = JSON.parse(data);
  messageList.value.push({ userName: json.userName, message: json.message });
};
</script>

<template>
  <header>
    <div class="wrapper">
      <div class="p-2 chat">
        <p v-for="message in messageList">{{ message }}</p>
      </div>
      <div class="message">
        <input
          type="text"
          v-model="content"
          placeholder="請輸入內容"
          @keyup.enter="send"
        />
      </div>
    </div>
  </header>

  <main>
    <!-- <TheWelcome /> -->
  </main>
</template>

<style scoped>
header {
  line-height: 1.5;
}

.logo {
  display: block;
  margin: 0 auto 2rem;
}

@media (min-width: 1024px) {
  header {
    display: flex;
    place-items: center;
    padding-right: calc(var(--section-gap) / 2);
  }

  .logo {
    margin: 0 2rem 0 0;
  }

  header .wrapper {
    display: flex;
    place-items: flex-start;
    flex-wrap: wrap;
  }
}
</style>
