<template>
    <nav>
        <v-toolbar flat app>
            <v-toolbar-title class="grey--text text-h4">
                <v-img style="float: left;" :width="80" aspect-ratio="4/3"
                    src="https://blog.flamingtext.com/blog/2023/08/08/flamingtext_com_1691517616_586896794.png"></v-img>
                <!-- <span style="font-weight: lighter;">布可</span> -->
                <span style="display: flex;vertical-align: text-bottom;">網路書店</span>
            </v-toolbar-title>
            <v-spacer></v-spacer>
            <v-menu open-on-hover>
                <template v-slot:activator="{ props }">
                    <v-btn color="grey" v-bind="props">
                        <v-icon right icon="mdi-account" />
                    </v-btn>
                </template>

                <v-list>
                    <v-list-item v-for="(useritem, index) in useritems" :key="index" router :to="useritem.route">
                        <v-list-item-title>{{ useritem.title }}</v-list-item-title>
                    </v-list-item>
                </v-list>
            </v-menu>
            <v-btn flat color="grey">
                <v-icon right icon="mdi-exit-to-app" />
            </v-btn>
        </v-toolbar>
    </nav>
</template>
    <!-- 
                            </v-btn> -->
<script>
import { mergeProps } from 'vue'

export default {
    data: () => ({
        useritems: [
            { title: '會員中心', route: "/Users" },
            { title: '歷史訂單', route: "#" },
            { title: '收藏專欄', route: "#" },
        ],
    }),
}
</script>
    
<style></style>