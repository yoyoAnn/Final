import { createRouter, createWebHistory } from "vue-router";
import Home from '../views/Home.vue'
import Member from '../views/Member.vue'

//路由設定
const routes = [
    {
        //http://localhost/
        path: '/',
        component: Home
    },
    {
        //http://localhost/member
        path: '/Member',
        component: Member
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes: routes
})

export default router