<template>
    <v-card>
        <v-tabs v-model="tab" bg-color="light">
            <v-tab value="Profile">作者</v-tab>
            <v-tab value="Articles">最新文章</v-tab>
        </v-tabs>

        <v-card-text>
            <v-window v-model="tab">
                <v-window-item value="Profile">
                    <p class="text-h6">作者{{ writerId }}</p>
                    <img class="writerPhoto" :src="`/src/WriterImage/${writerPhoto}`" />
                </v-window-item>

                <v-window-item value="Articles">
                    文章列表
                </v-window-item>
            </v-window>
        </v-card-text>
    </v-card>
</template>
    
<script setup>
import { ref, reactive, onMounted, watch } from "vue";

// 取得父元件之WriterId
const props = defineProps({
    writerId: Number
})

const tab = ref(null)
const writerPhoto = ref("");
const writer = ref([]);
// console.log(props.writerId);
const loadWriter = async () => {
    try {
        // console.log(props.writerId);
        const response = await fetch(`https://localhost:7261/api/Articles/writer/${props.writerId}`);
        if (!response.ok) {
            throw new Error(`Network response was not ok: ${response.status}`);
        }
        const datas = await response.json();
        writer.value = datas;
        console.log(datas);
    } catch (error) {
        console.error("Error loading books:", error);
    }
};

onMounted(() => {
    loadWriter();

});
</script>
    
<style>
.writerPhoto {
    border-radius: 40px;
    width: 80px;
    height: 80px;
}
</style>