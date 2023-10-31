<template>
    <div class="pagination">
        <button @click="prevPage" :disabled="currentPage === 1">
            <ArrowLeft />
        </button>
        <span>Pagina {{ currentPage }} van {{ totalPages }}</span>
        <button id="button-pages" @click="nextPage" :disabled="currentPage === totalPages">
            <ArrowRight />
        </button>
    </div>
</template>
  
<script>
import ArrowLeft from "../components/icons/iconArrowLeft.vue";
import ArrowRight from "../components/icons/iconArrowRight.vue";
export default {
    name: "Overview",
    components: {
        ArrowLeft,
        ArrowRight
    },
    props: {
        currentPage: {
            type: Number,
            required: true,
        },
        totalPages: {
            type: Number,
            required: true,
        },
    },
    methods: {
        prevPage() {
            if (this.currentPage > 1) {
                this.$emit("page-changed", this.currentPage - 1);
            }
        },
        nextPage() {
            if (this.currentPage < this.totalPages) {
                this.$emit("page-changed", this.currentPage + 1);
            }
        },
    },
};
</script>
  
  
<style>
@import '../assets/Css/Main.css';
@import '../assets/Css/Pagination.css';
</style>
