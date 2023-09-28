<template>
    <div class="pagination">
        <button @click="prevPage" :disabled="currentPage === 1">
            <ArrowLeft />
        </button>
        <span>Pagina {{ currentPage }} van {{ totalPages }}</span>
        <button id="button2" @click="nextPage" :disabled="currentPage === totalPages">
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
  
<style scoped>
.pagination {
    display: flex;
    justify-content: center;
    align-items: center;
}

button {
    margin-right: 10px;
    margin-top: 2px;
    display: flex;
    padding: 5px 10px;
    cursor: pointer;
    background-color: #afaeae;
    color: rgb(14, 12, 12);
    border-radius: 3px;
    border: none;
}

#button2 {
    margin-left: 10px;
}

button[disabled] {
    opacity: 0.5;
    cursor: not-allowed;
}</style>
  