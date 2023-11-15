<template>
    <div>
        <Header></Header>
        <div class="upload-container">
            <div id="upload-employee">
                <h1 id="h1">Product uploaden</h1>

                <ul>
                    <form class="gegevens">
                        <select v-model="this.product.type" class="Type">
                            <option value="0">Selecteer type...</option>
                            <option value="1">Laptop</option>
                            <option value="2">Monitor</option>
                            <option value="3">Stoel</option>
                        </select>
                        <input v-model="this.product.purchaseDate" type="date" class="Date" />
                        <input v-model="this.product.expirationDate" type="date" class="Date" />
                        <input class="Email" v-model="this.product.serialNumber" placeholder="Serie nummer" />
                    </form>
                </ul>

                <button @click="this.PostEmployee()" class="verstuur-employee">
                    Verstuur document
                </button>
            </div>
        </div>

        <PopUpMessage ref="PopUpMessage" />

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
    data() {
        return {
            product: {
                purchaseDate: new Date().toISOString().split('T')[0],
                expirationDate: new Date().toISOString().split('T')[0],
                type: 0,
                serialNumber: "",
            },
        };
    },
    methods: {
        PostEmployee() {
            this.product.type = parseInt(this.product.type, 10);
            axios.post("product", this.product, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                    localStorage.setItem('popUpSucces', 'true');
                    this.$router.push({ path: '/Overzicht/bruikleen', query: { activePopup: true } });
                }).catch((error) => {
                    this.$refs.PopUpMessage.popUpError(error.response.data);
                });
        },
    },
};
</script>
  
<style>
@import '../assets/Css/Uploaden.css';
@import '../assets/Css/Main.css';
</style>
  