import { createRouter, createWebHistory } from "vue-router";
import Home from '../views/Home.vue'
import Users from '../views/Users.vue'
import Cart from '../views/Cart.vue'
import Article from '../views/Article.vue'
import Books from '../components/BookDetail.vue'
import Login from '../views/Login.vue'
import CustomerMail from '../views/CustomerMail.vue'
import Orders from '../views/Orders.vue'

//路由設定
const routes = [
    {
        path: '/',
        component: Home
    },
    {
        path: '/login',
        component: Login
    },
    {
        path: '/users',
        component: Users
    },
    {
        path: '/cart',
        component: Cart
    },
    {
        path: '/article',
        component: Article

    },
    {
        path: '/customerMail',
        component: CustomerMail
    },
    {
        path: '/books/:id',
        component: Books

    },
    {
        path: '/orders',
        component: Orders
    }
]



const router = createRouter({
    history: createWebHistory(),
    routes: routes
})

export default router