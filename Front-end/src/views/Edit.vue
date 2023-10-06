<template>
    <div>
        <Header></Header>
        <div class="uploadContainerEdit">
            <h1 id="h1">Edit {{ this.route }}</h1>

            <form action="/action_page.php">
                <div class="gegevensEdit" v-if="this.route == 'klant'">
                    <input v-model="this.customer.Name" type="text" class="NaamEdit" placeholder="Naam klant" name="Zoek" />
                    <input v-model="this.customer.Email" type="text" class="Email" placeholder="Email klant" name="Email" />
                </div>
                <div class="gegevensEdit" v-else>
                    <select v-model="this.document.Type" class="Type" name="Type">
                        <option value="0">Selecteer type...</option>
                        <option value="1">Vog</option>
                        <option value="2">Contract</option>
                        <option value="3">Paspoort</option>
                        <option value="4">id kaart</option>
                        <option value="5">Diploma</option>
                        <option value="6">Certificaat</option>
                        <option value="7">Lease auto</option>
                    </select>
                    <input v-model="this.document.Date" :placeholder="this.document.Date" type="date" class="Date"
                        name="Date" />
                </div>
            </form>

            <button @click="this.getCurrectDocument()" class="verstuurEdit" type="button">
                Verstuur document
            </button>
        </div>

        <Popup ref="Popup" />

    </div>
</template>
  
<script>
import axios from '../../axios-auth.js'
import Popup from '../views/popUp.vue';
import Header from '../views/Header.vue';

export default {
    components: {
        Popup,
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
                Date: new Date().toISOString().split('T')[0],
                CustomerId: 0,
            },
            filteredCustomers: []
        };
    },
    methods: {
        Edit(customerId) {

            console.log(customerId)
            if (this.route == 'document') {
                this.putDocument(customerId);
            }
            else {
                this.putCustomer(customerId);
            }
        },
        getCurrectDocument() {
            axios.get("Document/" + this.id, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                    this.Edit(res.data.document.customerId);
                }).catch((error) => {
                    this.$refs.Popup.popUpError(error.response.data);
                });
        },
        putCustomer(customerId) {
            this.customer.CustomerId = customerId;
            axios.put("Customer", this.customer, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {

                    this.$router.push("/infopage/" + this.id);
                }).catch((error) => {
                    this.$refs.Popup.popUpError(error.response.data);
                });
        },
        putDocument(customerId) {
            this.document.DocumentId = parseInt(this.id, 10);
            this.document.CustomerId = customerId;
    this.document.Type = parseInt(this.document.Type, 10);
            console.log(this.document)
            axios.put("Document", this.document, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                    this.$router.push("/infopage/" + this.id);
                }).catch((error) => {
                    this.$refs.Popup.popUpError(error.response.data);
                });
        },
    },
};
</script>
  
<style>
@import '../assets/Css/Edit.css';
@import '../assets/Css/Main.css';
</style>
  