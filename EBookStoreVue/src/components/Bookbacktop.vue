<template>
  <!-- <el-backtop :bottom="30" right="15">
    <div style="
        height: 100%;
        width: 100%;
        background-color: #fff;
        box-shadow: var(--el-box-shadow-lighter);
        text-align: center;
        line-height: 40px;
        color: #2e7d32;   /*#1989fa;*/
        border-radius: 50%;
        font-size: 18px;
        font-weight: bold;
      ">
      TOP
    </div>
  </el-backtop> -->
  <div class="top" v-show="showBackToTop">
    <button class="goTop" @click="goTop">TOP</button>
  </div>
</template>
      
<script>
export default {
  name: 'scrollTop',
  data() {
    return {
      scrollY: 0, // 当前滚动距离
      showBackToTop: false, // 是否显示返回顶部按钮
      threshold: 300 // px 滚动距离阈值
    };
  },
  created() {
    window.addEventListener("scroll", this.handleScroll);
  },
  computed: {
    // 计算属性，返回当前是否应该显示返回顶部按钮
    shouldShowBackToTop() {
      return this.scrollY >= this.threshold; // 假设滚动距离超过 500px 时显示按钮
    }
  },
  watch: {
    // 监听 scrollY 属性的变化，更新 showBackToTop 属性
    scrollY(newValue) {
      this.showBackToTop = this.shouldShowBackToTop;
    }
  },
  methods: {
    handleScroll() {
      this.scrollY = window.scrollY;
    },
    goTop() {
      window.scrollTo({
        top: 0,
        behavior: "smooth"
      });
    },
  }
};
</script>
      
<style>
.goTop {
  z-index: 100;
  position: fixed;
  bottom: 120px;
  right: 15px;
  height: 80px;
  width: 80px;
  background-color: #fff;
  box-shadow: var(--el-box-shadow-lighter);
  text-align: center;
  line-height: 40px;
  color: #2e7d32;
  border-radius: 50%;
  font-size: 18px;
  font-weight: bold;
}

.goTop:hover {
  color: #fff;
  background-color: rgba(46, 125, 50, 0.7);
}

.goTop.hide {
  opacity: 0;
  filter: Alpha(opacity=0);
  transform: translateY(150%);
}
</style>