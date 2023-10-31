<template>
    <div>
        <Header></Header>
        <div class="uploadContainerEdit">
            <h1 id="h1">Edit {{ this.route }}</h1>

            <form action="/action_page.php">
                <div class="gegevensEdit">
                    <select v-model="this.document.Type" class="Type" name="Type">
                        <option value="0">Selecteer type...</option>
                        <option value="1">Vog</option>
                        <option value="2">Contract</option>
                        <option value="3">Paspoort</option>
                        <option value="4">ID kaart</option>
                        <option value="5">Diploma</option>
                        <option value="6">Certificaat</option>
                        <option value="7">Lease auto</option>
                    </select>
                    <input v-model="formattedDate" type="date" class="Date" name="Date" />

                </div>
            </form>
            <button @click="editDocument()" class="verstuurEdit">Aanpassen</button>
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
            customer: {
                CustomerId: 0,
                Name: '',
                Email: '',
            },
            document: {
                DocumentId: 0,
                Type: 0,
                Date: "",
            },
            customerDocument: {
                CustomerId: 0,
                Name: '',
                Email: '',
                DocumentId: 0,
            },
            filteredCustomers: []
        };
    },
    mounted() {
        this.getDocument();
    },
    methods: {
        getDocument() {
            axios.get("Document/" + this.id, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                    this.customer = res.data.customer;
                    this.document = res.data.document;
                    this.document.Date = res.data.document.date;
                    this.document.Type = res.data.document.type;
                }).catch((error) => {
                    this.$refs.Popup.popUpError(error.response.data);
                });
        },
        editDocument() {
            this.document.Type = parseInt(this.document.Type, 10);
            console.log(this.document.DocumentId)
            this.document.DocumentId = this.id;
            axios.put("Document", this.document, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                    localStorage.setItem('popUpSucces', 'true');
                    this.$router.push({ path: '/infopage/document/' + this.id, query: { activePopup: true } });
                }).catch((error) => {
                    this.$refs.Popup.popUpError(error.response.data);
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
  