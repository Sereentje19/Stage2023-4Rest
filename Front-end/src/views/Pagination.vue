<template>
    <div class="pagination">
        <button @click="prevPage" :disabled="currentPage === 1">Previous</button>
        <span>Page {{ currentPage }} of {{ totalPages }}</span>
        <button @click="nextPage" :disabled="currentPage === totalPages">Next</button>
    </div>
</template>
  
<script>
export default {
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
    margin: 0 5px;
    padding: 5px 10px;
    cursor: pointer;
}

button[disabled] {
    opacity: 0.5;
    cursor: not-allowed;
}
</style>
  