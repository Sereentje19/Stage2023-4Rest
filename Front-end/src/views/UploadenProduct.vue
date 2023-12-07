<template>
    <div>
        <Header></Header>
        <div class="upload-container">
            <div class="leftside">
                <h1 id="h1">Product uploaden</h1>
                <div id="drop" ref="dropArea" class="drop-area" @dragover.prevent="handleDrag(true)"
                    @dragleave="handleDrag(false)" @drop.prevent="handleDrop">
                    <p id="p" ref="pElement"></p>
                    <input type="file" class="file" @change="handleFileChange" style="display: none" />
                </div>

                <div v-if="this.selectedFile != null">Geselecteerde bestand: <b>{{ this.selectedFile.name }}</b></div>
                <div v-else>Nog geen bestand geselecteerd</div>


                <label class="overlay">
                    <div id="select-document"> Selecteer document</div>
                    <img id="folder-image" src="../assets/pictures/folder.png">
                    <input type="file" class="file" accept=".jpg, .jpeg, .png, .gif, .pdf" @change="handleFileChange" />
                </label>
            </div>


            <div class="rightside" id="rightside-product">
                <ul>
                    <form class="gegevens">
                        <select v-model="this.product.type" class="Type">
                            <option value="0">Selecteer type...</option>
                            <option v-for="(type, index) in productTypes" :key="index" :value="index + 1">
                                {{ type }}
                            </option>
                        </select>
                        <input v-model="this.product.purchaseDate" type="date" class="Date" />
                        <input v-model="this.product.expirationDate" type="date" class="Date" />
                        <input class="Email" v-model="this.product.serialNumber" placeholder="Serie nummer" />
                    </form>
                </ul>

                <button @click="this.PostEmployee()" class="verstuur" id="verstuur-product">
                    Verstuur document
                </button>
            </div>
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
    data() {
        return {
            product: {
                purchaseDate: new Date().toISOString().split('T')[0],
                expirationDate: new Date().toISOString().split('T')[0],
                type: 0,
                serialNumber: "",
            },
            productTypes: [],
        };
    },
    mounted() {
        this.getProductTypes();
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
    },
};
</script>
  
<style>
@import '../assets/css/Uploaden.css';
@import '../assets/css/Main.css';
</style>
  