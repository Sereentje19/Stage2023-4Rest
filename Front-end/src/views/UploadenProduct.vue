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
                    <div id="select-document"> Selecteer product</div>
                    <img id="folder-image" src="/src/assets/pictures/folder.png">
                    <input type="file" class="file" accept=".jpg, .jpeg, .png, .gif, .pdf" @change="handleFileChange" />
                </label>
            </div>


            <div class="rightside" id="rightside-product">
                <ul>
                    <form class="gegevens">
                        Type
                        <div id="new-type">
                            <select v-if="this.addProductType == false" v-model="this.product.type.name" class="type"
                                name="Type">
                                <option value="0">Selecteer product...</option>
                                <option v-for="(type, index) in productTypes" :key="index" :value="type.name">
                                    {{ type.name }}
                                </option>
                            </select>

                            <input v-else v-model="this.product.type.name" class="email" placeholder="Nieuw type"
                                name="email">
                            <div v-if="this.addProductType == false" @click="addType()" id="add-button">
                                <IconAdd />
                            </div>
                            <div v-else @click="addTypeReverse()" id="add-button">
                                <CardList />
                            </div>
                        </div>

                        Gekocht op <input v-model="this.product.purchaseDate" type="date" class="date" />
                        Gebruiken tot <input v-model="this.product.expirationDate" type="date" class="date" />
                        Serienummer <input class="email" v-model="this.product.serialNumber" />
                    </form>
                </ul>

                <button @click="this.PostEmployee()" class="verstuur" id="verstuur-product">
                    Verstuur product
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
import IconAdd from '../components/icons/IconAdd.vue';
import CardList from '../components/icons/IconCardList.vue';

export default {
    components: {
        PopUpMessage,
        Header,
        IconAdd,
        CardList
    },
    data() {
        return {
            isFocused: false,
            selectedFile: null,
            dropAreaActive: false,
            product: {
                purchaseDate: new Date().toISOString().split('T')[0],
                expirationDate: new Date().toISOString().split('T')[0],
                type: {
                    name: "0"
                },
                serialNumber: "",
            },
            productTypes: [],
            addProductType: false
        };
    },
    mounted() {
        this.getProductTypes();
    },
    methods: {
        addType() {
            this.addProductType = !this.addProductType;
            this.product.type.name = "";
        },
        addTypeReverse() {
            this.addProductType = !this.addProductType;
            this.product.type.name = "0";
        },
        PostEmployee() {
            let formData = this.CreateFromData();

            axios.post("product", formData, {
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
        CreateFromData() {
            let formData = new FormData();

            if (this.product.purchaseDate == "" || this.product.expirationDate == "") {
                this.product.purchaseDate = "2022-01-01T00:00:00Z";
                this.product.expirationDate = "2022-01-01T00:00:00Z";
            }

            console.log(this.product.purchaseDate)

            if (this.selectedFile != null) {
                formData.append('file', this.selectedFile);
                formData.append('product.FileType', this.selectedFile.type);
            }

            formData.append('product.expirationDate', this.product.expirationDate);
            formData.append('product.purchaseDate', this.product.purchaseDate);
            formData.append('product.type.name', this.product.type.name);
            formData.append('product.serialNumber', this.product.serialNumber);

            return formData;
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
        handleFileChange(event) {
            this.selectedFile = event.target.files[0]

            const reader = new FileReader();
            reader.onload = () => {
                this.fileContents = reader.result;
            };
            reader.readAsBinaryString(this.selectedFile)

        },
        onBlur() {
            setTimeout(() => {
                this.isFocused = false;
            }, 200);
        },
        handleDrag(e, bool) {
            e.preventDefault();
            this.dropAreaActive = bool;
        },
        handleDrop(e) {
            e.preventDefault();
            this.dropAreaActive = false;
            const files = e.dataTransfer.files;
            this.processFile(files[0]);
        },
        processFile(file) {
            if (file) {
                this.selectedFile = file;
                const reader = new FileReader();
                reader.readAsDataURL(file);
            }
        },
    },
};
</script>
  
<style>
@import '../assets/css/Uploaden.css';
@import '../assets/css/Main.css';
</style>
  