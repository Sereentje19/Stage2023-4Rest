<template>
    <Header></Header>
    <div class="info-container">
        <h1>Info</h1>

        <PopupChoice ref="PopupChoice" @delete="deleteEmployee"/>

        <div id="leftside">
            <div id="loan-title">
                Medewerker
            </div>
            <div id="loan-info">
                <div id="loan-info-leftside">
                    Name: <br>
                    Email: <br>
                </div>
                <div>
                    {{ this.employee.name }} <br>
                    {{ this.employee.email }} <br>
                </div>
            </div>
            <div id="box">
                <button @click="toEdit()" id="edit-button">Edit</button>
                <button @click="toPopUpDelete()" id="delete-button">Delete</button>
            </div>
        </div>
    </div>

    <PopupMessage ref="PopUpMessage" />
</template>
  
<script>
import axios from '../../axios-auth.js';
import moment from 'moment';
import PopupMessage from '../views/PopUpMessage.vue';
import PopupChoice from '../views/PopUpChoice.vue';
import Header from '../views/Header.vue';

export default {
    name: "Info",
    props: {
        id: Number,
    },
    components: {
        PopupMessage,
        Header,
        PopupChoice
    },
    data() {
        return {
            employee: {
                employeeId: 0,
                name: "",
                email: ""
            },
        }
    },
    mounted() {
        this.getAllEmployees();

        if (this.$route.query.activePopup && localStorage.getItem('popUpSucces') === 'true') {
            this.$refs.PopUpMessage.popUpError("Data is bijgewerkt.");
        }
    },
    methods: {
        getAllEmployees() {
            axios.get("employee/" + this.id, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                },
            })
                .then((res) => {
                    this.employee = res.data
                }).catch((error) => {
                    this.$refs.PopUpMessage.popUpError(error.response.data);
                });
        },
        toPopUpDelete() {
            this.emitter.emit('isPopUpTrue', {'eventContent': true})
            this.emitter.emit('text', {'eventContent': "Weet je zeker dat je " + this.employee.name + " wilt verwijderen?"})
        },
        deleteEmployee() {
            this.isPopUpDelete = false;
            axios.delete("employee/" + this.id, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                },
            })
                .then((res) => {
                    localStorage.setItem('popUpSucces', 'true');
                    this.$router.push({ path: '/overzicht/medewerkers', query: { activePopup: true } });
                }).catch((error) => {
                    this.$refs.PopUpMessage.popUpError(error.response.data);
                });
        },
        cancel() {
            this.isPopUpDelete = false;
        },
        toEdit() {
            this.$router.push("/edit/medewerker/" + this.id);
        },
        formatDate(date) {
            return moment(date).format("DD-MM-YYYY");
        },
    }
}

</script>
  
<style>
@import '../assets/Css/Info.css';
@import '../assets/Css/Main.css';
</style>
  