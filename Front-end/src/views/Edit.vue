<template>
    <div>
        <Header></Header>
        <div class="uploadContainerEdit">
            <h1 id="h1">Edit {{ this.route }}</h1>

            <form action="/action_page.php">
                <div class="gegevensEdit" v-if="this.route == 'klant'">
                    <input v-model="this.customer.Name" :placeholder="this.customer.name" class="NaamEdit" />
                    <input v-model="this.customer.Email" :placeholder="this.customer.email" class="Email" />
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
                    <input v-model="formattedDate" type="date" class="Date" name="Date" />

                </div>
            </form>
            <button @click="route === 'document' ? putDocument() : putCustomer()" class="verstuurEdit">Verstuur
                document</button>
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
                DocumentId: parseInt(this.id, 10),
                Type: 0,
                Date: "",
                CustomerId: 0,
            },
            filteredCustomers: []
        };
    },
    mounted() {
        this.getCustomerId();
    },
    methods: {
        getCustomerId() {
            axios.get("Document/" + this.id, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                    this.customer.CustomerId = res.data.document.customerId;
                    this.document.CustomerId = res.data.document.customerId;
                    this.document.Date = res.data.document.date;
                    this.document.Type = res.data.document.type;
                    console.log(this.document.Date)
                    this.getCustomerInfo(res.data.document.customerId);
                }).catch((error) => {
                    this.$refs.Popup.popUpError(error.response.data);
                });
        },
        getCustomerInfo(customerId) {
            axios.get("Customer/" + customerId, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                    this.customer.Email = res.data.email;
                    this.customer.Name = res.data.name;
                    console.log(this.customer);

                }).catch((error) => {
                    this.$refs.Popup.popUpError(error.response.data);
                });
        },
        putCustomer() {
            console.log(this.customer);
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
        putDocument() {
            this.document.Type = parseInt(this.document.Type, 10);
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
  