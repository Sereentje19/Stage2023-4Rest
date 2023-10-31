<template>
    <div>
        <Header></Header>
        <div class="upload-container-edit">
            <h1 id="h1">Edit {{ this.route }}</h1>

            <form>
                <div class="gegevens-edit">
                    <select v-model="this.product.type" class="Type">
                        <option value="0">Selecteer type...</option>
                        <option value="1">Laptop</option>
                        <option value="2">Monitor</option>
                        <option value="3">Stoel</option>
                    </select>
                    <input v-model="this.product.purchaseDate" type="date" class="Date"/>
                    <input v-model="this.product.expirationDate" type="date" class="Date"/>
                    <input class="Email" v-model="this.product.serialNumber" @input="filterDocuments" />
                </div>
            </form>
            <button @click="editProduct()" class="verstuur-edit">Aanpassen</button>
        </div>

        <PopUpMessage ref="Popup" />

    </div>
</template>
  
<script>
import axios from '../../axios-auth.js'
import PopUpMessage from '../views/PopUpMessage.vue';
import Header from '../views/Header.vue';

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
        };
    },
    mounted() {
        this.getProduct();
    },
    methods: {
        getProduct() {
            axios.get("Product/" + this.id, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                    console.log(res.data)
                    this.product = res.data;
                    this.product.type = res.data.type
                    this.formatDates();
                }).catch((error) => {
                    this.$refs.Popup.popUpError(error.response.data);
                });
        },
        editProduct() {
            console.log(this.product)
            this.product.type = parseInt(this.product.type, 10);

            axios.put("Product", this.product,{
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                },
                params: {
                    customer: this.customer,
                },
            })
                .then((res) => {
                    localStorage.setItem('popUpSucces', 'true');
                    this.$router.push({ path: '/info/bruikleen/' + this.id, query: { activePopup: true } });
                }).catch((error) => {
                    this.$refs.Popup.popUpError(error.response.data);
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
@import '../assets/Css/Edit.css';
@import '../assets/Css/Main.css';
</style>
  