import { createRouter, createWebHistory } from "vue-router";
import Home from '../views/Home.vue'
import Users from '../views/Users.vue'
import Cart from '../views/Cart.vue'
import Article from '../views/Article.vue'
import Books from '../components/BookDetail.vue'
import Login from '../views/Login.vue'
import CustomerMail from '../views/CustomerMail.vue'
import BookSearch from '../components/BookSearch.vue'
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
        path: '/books/:bookid',
        component: Books
    },
    {
        path: '/booksearch/:category',
        name: 'book-search',
        component: BookSearch

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