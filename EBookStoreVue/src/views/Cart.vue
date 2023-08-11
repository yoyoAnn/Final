<template>
  <section class="h-100 h-custom">
    <div class="container py-5 h-100">
      <div class="row d-flex justify-content-center align-items-center h-100">
        <div class="col">
          <div class="card border-0">
            <div class="card-body p-4">
              <div class="row">
                <div class="col-lg-7">
                  <h5 class="mb-3">
                    <router-link class="text-body" to="/"
                      ><i class="fas fa-long-arrow-alt-left me-2"></i>Continue
                      shopping</router-link
                    >
                  </h5>
                  <hr />

                  <div
                    class="d-flex justify-content-between align-items-center mb-4"
                  >
                    <div>
                      <p class="mb-0">
                        You have {{ books.length }} items in your cart
                      </p>
                    </div>
                  </div>

                  <div
                    v-for="item in books"
                    class="card mb-3 shadow-sm border-0"
                    :key="item.id"
                  >
                    <div class="card-body">
                      <div class="d-flex justify-content-between">
                        <div class="d-flex flex-row align-items-center">
                          <div>
                            <img
                              :src="item.image"
                              class="img-fluid rounded-3"
                              alt="Shopping item"
                              style="width: 65px"
                            />
                          </div>
                          <div class="ms-3">
                            <p>{{ item.bookId }}</p>
                            <!-- Display item name -->
                          </div>
                        </div>
                        <div class="d-flex flex-row align-items-center">
                          <div>
                            <CartAddRemove :product="item" />
                          </div>
                        </div>
                        <div class="d-flex flex-row align-items-center">
                          <div>
                            <h5 class="mb-0">
                              <i class="bi bi-currency-dollar"></i
                              >{{ item.Price * item.qty }}
                            </h5>
                            <small
                              v-if="item.hasDiscount"
                              class="text-muted text-decoration-line-through"
                            >
                              <i class="bi bi-currency-dollar"></i
                              >{{ item.price }}</small
                            >
                          </div>
                          <a
                            role="button"
                            @click="removeItem(item)"
                            class="ms-4"
                            style="color: #cecece"
                          >
                            <i class="bi bi-trash3 h4"></i>
                          </a>
                        </div>
                      </div>
                    </div>
                  </div>

                  <h2>test</h2>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>
<script>
import CartAddRemove from "../components/CartAddRemove.vue";
import { ref, reactive, onMounted } from "vue";

export default {
  components: { CartAddRemove },
  setup() {
    const books = ref([]);

    const removeItem = async (item) => {
      try {
        const response = await fetch(
          `https://localhost:7261/api/Carts/${item.id}`,
          {
            method: "DELETE",
          }
        );

        if (response.ok) {
          // Remove the item from the local array
          const itemIndex = books.value.findIndex(
            (cartItem) => cartItem.id === item.id
          );
          if (itemIndex !== -1) {
            books.value.splice(itemIndex, 1);
          }
        } else {
          console.error("Failed to remove item from cart.");
        }
      } catch (error) {
        console.error("An error occurred:", error);
      }
    };

    const loadProducts = async function () {
      try {
        const response = await fetch(`https://localhost:7261/api/Carts`, {
          method: "Get",
        });
        const datas = await response.json();

        books.value = datas;
        console.log(books.value);
      } catch (error) {
        console.error("錯誤原因:", error);
      }
    };

    onMounted(function () {
      loadProducts();
    });

    return {
      books,
      removeItem,
    };
  },
};
</script>