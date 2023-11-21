<template>
    <div>
        <Header></Header>
        <div class="upload-container-edit">
            <h1 id="h1">Edit {{ this.route }}</h1>

            <form>
                <div class="gegevens-edit">
                    <select v-model="this.product.type" class="Type">
                        <option value="0">Selecteer type...</option>
                        <option v-for="(type, index) in productTypes" :key="index" :value="index + 1">
                            {{ type }}
                        </option>
                    </select>
                    <input v-model="this.product.purchaseDate" type="date" class="Date" />
                    <input v-model="this.product.expirationDate" type="date" class="Date" />
                    <input class="Email" v-model="this.product.serialNumber" @input="filterDocuments" />
                </div>
            </form>
            <button @click="editProduct()" class="verstuur-edit">Aanpassen</button>
        </div>

        <PopUpMessage ref="PopUpMessage" />

    </div>
</template>
  
<script>
import axios from '../../axios-auth.js'
import PopUpMessage from '../components/notifications/PopUpMessage.vue';
import Header from '../components/layout/Header.vue';

export default {
    components: {
        PopUpMessage,
        Header
    },
    props: {
        route: String,
        id: Number
    },
    data() {
        return {
            product: {
                type: 0,
                purchaseDate: "",
                expirationDate: "",
                serialNumber: ""
            },
            productTypes: [],
        };
    },
    mounted() {
        this.getProduct();
        this.getProductTypes();
    },
    methods: {
        getProduct() {
            axios.get("product/" + this.id, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                    console.log(res.data)
                    this.product = res.data.product;
                    this.formatDates();
                }).catch((error) => {
                    this.$refs.PopUpMessage.popUpError(error.response.data);
                });
        },
        editProduct() {
            console.log(this.product)
            this.product.type = parseInt(this.product.type, 10);

            axios.put("product", this.product, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                },
                params: {
                    employee: this.employee,
                },
            })
                .then((res) => {
                    localStorage.setItem('popUpSucces', 'true');
                    this.$router.push({ path: '/info/bruikleen/' + this.id, query: { activePopup: true } });
                }).catch((error) => {
                    this.$refs.PopUpMessage.popUpError(error.response.data);
                });
        },
        getProductTypes() {
            axios
                .get("product/types", {
                    headers: {
                        Authorization: "Bearer " + localStorage.getItem("jwt"),
                    }
                })
                .then((res) => {
                    console.log(res.data)
                    this.productTypes = res.data;
                })
                .catch((error) => {
                    this.$refs.PopUpMessage.popUpError(error.response.data);
                });
        },
        formatDates() {
            this.product.purchaseDate = this.product.purchaseDate.split('T')[0];
            this.product.expirationDate = this.product.expirationDate.split('T')[0];
        }
    }
};
</script>
  
<style>
@import '../assets/css/Edit.css';
@import '../assets/css/Main.css';
</style>
  