<template>
  <el-carousel :interval="5000" trigger="click" type="card" height="400px">
    <el-carousel-item v-for="(item, i) in items" :key="i">
      <img :src="item.src" :alt="item.title" />
    </el-carousel-item>
  </el-carousel>
</template>

<script>
export default {
  data() {
    return {
      items: [
        {
          title: "神的棲息地",
          src: "https://im2.book.com.tw/image/getImage?i=https://www.books.com.tw/img/001/095/68/0010956899.jpg&v=646de789k&w=348&h=348",
        },
        {
          title: "護理師",
          src: "https://im1.book.com.tw/image/getImage?i=https://www.books.com.tw/img/001/096/23/0010962314.jpg&v=64abdef5k&w=348&h=348",
        },
        {
          title: "秘密：東野圭吾之所以成為東野圭吾的完美傑作！",
          src: "https://im1.book.com.tw/image/getImage?i=https://www.books.com.tw/img/001/096/04/0010960476.jpg&v=648d8c4bk&w=348&h=348",
        },
        {
          title: "困惑的心",
          src: "https://im1.book.com.tw/image/getImage?i=https://www.books.com.tw/img/001/096/03/0010960332.jpg&v=648c3a1ek&w=348&h=348",
        },
        {
          title: "顯微鏡下的大明",
          src: "https://im1.book.com.tw/image/getImage?i=https://www.books.com.tw/img/001/092/69/0010926947.jpg&w=348&h=348&v=62b440ab",
        },
      ],
    };
  },
};
</script>
    
<style>
.el-carousel__item h3 {
  color: #475669;
  opacity: 0.75;
  line-height: 200px;
  margin: 0;
  display: flex;
  text-align: center;
  align-content: end;
}
.el-carousel__item {
  height: 600px;
  display: flex;
  align-content: center;
  justify-items: center;
}
.el-carousel__item img {
  width: auto;
  margin: auto;
  display: flex;
  align-content: center;
  justify-items: center;
}
</style>