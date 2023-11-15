<template>
    <div>
        <Header></Header>
        <div class="upload-container-edit">
            <h1 id="h1">Edit Document</h1>

            <form>
                <div class="gegevens-edit">
                    <select v-model="this.document.Type" class="Type">
                        <option value="0">Selecteer type...</option>
                        <option value="1">Vog</option>
                        <option value="2">Contract</option>
                        <option value="3">Paspoort</option>
                        <option value="4">ID kaart</option>
                        <option value="5">Diploma</option>
                        <option value="6">Certificaat</option>
                        <option value="7">Lease auto</option>
                    </select>
                    <input v-model="formattedDate" type="date" class="Date" />

                </div>
            </form>
            <button @click="editDocument()" class="verstuur-edit">Aanpassen</button>
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
    props: {
        id: Number
    },
    data() {
        return {
            document: {
                DocumentId: 0,
                Type: 0,
                Date: "",
            },
            employeeDocument: {
                employeeId: 0,
                Name: '',
                Email: '',
                DocumentId: 0,
            },
            filteredEmployees: []
        };
    },
    mounted() {
        this.getDocument();
    },
    methods: {
        getDocument() {
            axios.get("document/" + this.id, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                    this.document = res.data.document;
                    this.document.Date = res.data.document.date;
                    this.document.Type = res.data.document.type;
                }).catch((error) => {
                    this.$refs.PopUpMessage.popUpError(error.response.data);
                });
        },
        editDocument() {
            this.document.Type = parseInt(this.document.Type, 10);
            this.document.DocumentId = this.id;

            axios.put("document", this.document, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                    localStorage.setItem('popUpSucces', 'true');
                    this.$router.push({ path: '/info/document/' + this.id, query: { activePopup: true } });
                }).catch((error) => {
                    this.$refs.PopUpMessage.popUpError(error.response.data);
                });
        },
    },
    computed: {
        formattedDate: {
            get() {
                return this.document.Date.split('T')[0];
            },
            set(newDate) {
                this.document.Date = newDate;
            },
        }
    },
};
</script>
  
<style>
@import '../assets/Css/Edit.css';
@import '../assets/Css/Main.css';
</style>
  