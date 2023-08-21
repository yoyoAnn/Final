<template>
  <!-- Footer -->
  <MDBFooter class="footer" :text="['center', 'lg-start', 'muted']">
    <!-- Section: Links  -->
    <section class="main">
      <MDBContainer class="text-center text-md-start mt-0">
        <!-- Grid row -->
        <MDBRow class="pt-3">
          <!-- Grid column -->
          <MDBCol md="3" lg="4" xl="3" class="mx-auto mb-1">
            <!-- Content -->
            <h5 class="text-uppercase fw-bold mb-4">關於我們</h5>
            <p>
              <a href="#!" class="text-reset">關於布可</a>
            </p>
            <p>
              <a href="/articleList/0" class="text-reset">精選專欄文章</a>
            </p>
            <p>
              <a href="#!" class="text-reset">隱私權政策</a>
            </p>
            <p>
              <a href="#!" class="text-reset">服務條款</a>
            </p>
            <p>
              <a href="#!" class="text-reset">資訊安全通報</a>
            </p>
          </MDBCol>
          <!-- Grid column -->

          <!-- Grid column -->
          <MDBCol md="2" lg="2" xl="2" class="mx-auto mb-1">
            <!-- Links -->
            <h5 class="text-uppercase fw-bold mb-4">友站連結</h5>
            <p>
              <a href="https://www.books.com.tw/?loc=tw_logo_001" class="text-reset">博客來</a>
            </p>
            <p>
              <a href="https://www.kingstone.com.tw/" class="text-reset">金石堂</a>
            </p>
            <p>
              <a href="https://www.taaze.tw/index.html" class="text-reset">TAAZE讀冊生活</a>
            </p>
            <p>
              <a href="https://readmoo.com/" class="text-reset">Readmoo讀墨</a>
            </p>
          </MDBCol>
          <!-- Grid column -->

          <!-- Grid column -->
          <MDBCol md="3" lg="2" xl="2" class="mx-auto mb-1">
            <!-- Links -->
            <h5 class="text-uppercase fw-bold mb-4">客服中心</h5>
            <p>
              <a href="/QA" class="text-reset">常見問題</a>
            </p>
            <p>
              <a href="#!" class="text-reset">線上客服</a>
            </p>
            <p>
              <a href="/customerMail" class="text-reset">客服信箱</a>
            </p>
          </MDBCol>
          <!-- Grid column -->

          <!-- Grid column -->
          <MDBCol md="4" lg="3" xl="3" class="mx-auto mb-md-0 mb-1">
            <!-- Links -->
            <h5 class="text-uppercase fw-bold mb-4">聯絡我們</h5>
            <p>
              <i class="fas fa-home me-3 link-secondary"></i>桃園市中壢區新生路二段421號(聖德基督學院)
            </p>
            <p>
              <i class="fas fa-envelope me-3 link-secondary"></i>
              yoyoann2023@gmail.com
            </p>
            <p>
              <MDBIcon icon="phone" class="me-3 link-secondary" />
              (03) 453-3013
            </p>
          </MDBCol>
          <!-- Grid column -->
        </MDBRow>
        <!-- Grid row -->
      </MDBContainer>
    </section>
    <!-- Section: Links  -->
    <!-- Section: Social media -->
    <section class="d-flex justify-content-center justify-content-lg-between p-2">
      <!-- Left -->
      <div class="me-5 d-none d-lg-block">
        <span>Get connected with us on social networks:</span>
      </div>
      <!-- Left -->

      <!-- Right -->
      <div>
        <a href="" class="me-4 text-reset">
          <MDBIcon class="link-secondary" iconStyle="fab" icon="facebook-f"></MDBIcon>
        </a>
        <a href="" class="me-4 text-reset">
          <MDBIcon class="link-secondary" iconStyle="fab" icon="twitter"></MDBIcon>
        </a>
        <a href="" class="me-4 text-reset">
          <MDBIcon class="link-secondary" iconStyle="fab" icon="google"></MDBIcon>
        </a>
        <a href="" class="me-4 text-reset">
          <MDBIcon class="link-secondary" iconStyle="fab" icon="instagram"></MDBIcon>
        </a>
        <a href="" class="me-4 text-reset">
          <MDBIcon class="link-secondary" iconStyle="fab" icon="linkedin"></MDBIcon>
        </a>
        <a href="" class="me-4 text-reset">
          <MDBIcon class="link-secondary" iconStyle="fab" icon="github"></MDBIcon>
        </a>
      </div>
      <!-- Right -->
    </section>
    <!-- Section: Social media -->

    <!-- Copyright -->
    <div class="text-center p-2 copyright" style="background-color: rgba(0, 0, 0, 0.05)">
      © 2021 Copyright:
      <a class="text-reset fw-bold" href="https://mdbootstrap.com/">MDBootstrap.com</a>
    </div>
    <!-- Copyright -->
  </MDBFooter>
  <!-- Footer -->
</template>
    
<script>
import {
  MDBFooter,
  MDBIcon,
  MDBRow,
  MDBCol,
  MDBContainer,
} from "mdb-vue-ui-kit";

export default {
  components: {
    MDBFooter,
    MDBIcon,
    MDBRow,
    MDBCol,
    MDBContainer,
  },
};
</script>
    
<style>
.footer {
  margin: 0;
  padding: 0;
  width: 100%;
}

section {
  background-color: #2e7d32;
  color: #e0e0e0;
}

.copyright {
  background-color: #2e7d32;
}

i.fab {
  color: #e0e0e0;
  background-blend-mode: #e0e0e0;
}

.MDBIcon {
  color: #e0e0e0;
}
</style>