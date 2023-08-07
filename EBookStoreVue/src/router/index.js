import { createRouter, createWebHistory } from "vue-router";
import Member from '../views/Member.vue'

//路由設定
const routes = [
    {
        //http://localhost/member
        path: '/member',
        component: Member
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes: routes
})

export default router