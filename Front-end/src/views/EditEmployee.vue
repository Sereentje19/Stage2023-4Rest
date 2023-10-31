<template>
    <div>
        <Header></Header>
        <div class="uploadContainerEdit">
            <h1 id="h1">Edit {{ this.route }}</h1>

            <form action="/action_page.php">
                <div class="gegevensEdit">
                    <input class="Email" v-model="this.customer.name" @input="filterDocuments" />
                    <input class="Email" v-model="this.customer.email" @input="filterDocuments" />
                </div>
            </form>
            <button @click="editCustomer()" class="verstuurEdit">Aanpassen</button>
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
        };
    },
    mounted() {
        this.getCustomer();
    },
    methods: {
        getCustomer() {
            axios.get("Customer/" + this.id, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                    console.log(res.data)
                    this.customer = res.data;
                }).catch((error) => {
                    this.$refs.Popup.popUpError(error.response.data);
                });
        },
        editCustomer() {
            console.log(this.customer)

            axios.put("Customer", this.customer,{
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                },
            })
                .then((res) => {
                    localStorage.setItem('popUpSucces', 'true');
                    this.$router.push({ path: '/info/medewerker/' + this.id, query: { activePopup: true } });
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
  