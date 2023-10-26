<template>
    <Header></Header>
    <div class="InfoContainer">
        <div class="info">
            <ul>
                <h1>Info</h1>
                <br>
                <div id="DocumentTitle">
                    Medewerker
                </div>
                <div id="documentInfo">
                    Naam: &nbsp; {{ this.customer.name }}
                    <br>
                    Email: &nbsp;&nbsp; {{ this.customer.email }}
                </div>
                <button @click="toEdit('document')" id="EditButton">Edit</button>
                <button id="deleteButton">Delete </button>
            </ul>
        </div>
    </div>

    <Popup ref="Popup" />
</template>
  
<script>
import axios from '../../axios-auth.js';
import moment from 'moment';
import Popup from '../views/popUp.vue';
import Header from '../views/Header.vue';

export default {
    name: "Info",
    props: {
        id: Number,
    },
    components: {
        Popup,
        Header
    },
    data() {
        return {
            customer: {
                name: "",
                email: ""
            },
        }
    },
    mounted() {
        this.getAllCustomers();

        if (this.$route.query.activePopup && localStorage.getItem('popUpSucces') === 'true') {
            this.$refs.Popup.popUpError("Data is bijgewerkt.");
        }
    },
    methods: {
        getAllCustomers() {
            axios.get("Customer/" + this.id, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                },
            })
                .then((res) => {
                    console.log(res.data)
                    this.customer = res.data
                }).catch((error) => {
                    this.$refs.Popup.popUpError(error.response.data);
                });
        },
        toEdit(){
            this.$router.push("/edit/Medewerker/" + this.id);
        },
        formatDate(date) {
            return moment(date).format("DD-MM-YYYY");
        },
    }
}

</script>
  
<style>
#deleteButton {
    margin-top: 50px;
    font-size: 20px;
    background-color: rgb(11, 92, 17);
    color: white;
    padding: 12px;
    border-radius: 5px;
    border: none;
}

@import '../assets/Css/Info.css';
@import '../assets/Css/Main.css';
</style>
  