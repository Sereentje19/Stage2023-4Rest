<template>
    <body>
        <Header ref="Header"></Header>
        <div id="body-profile">
            <div id="nav-profile">
                <a href="/profiel/persoonsgegevens" class="nav-item">
                    <profileFill style="color: #ffffff;" /> Persoonsgegevens &nbsp;&nbsp;&nbsp;&nbsp;
                    <ArrowRight />
                </a>
                <a href="/profiel/wachtwoord-wijzigen" class="nav-item" id="current-nav-item">
                    <lockClosed style="color: #ffffff;" /> Wachtwoord wijzigen
                    <ArrowRight />
                </a>
                <a href="/profiel/gebruikers-beheren" class="nav-item">
                    <ProfilePeople style="color: #ffffff;" /> Gebruikers beheren &nbsp;&nbsp;&nbsp;
                    <ArrowRight />
                </a>
            </div>

            <div id="profile-page">
                <div id="inputfields">
                    <div id="inputfield-title">Huidig wachtwoord</div>
                    <div id="input-edit"><input v-model="this.currentUser.password1" class="inputfield-personal-data"> </div><br>
                    <div id="inputfield-title">Nieuwe wachtwoord</div>
                    <div id="input-edit"><input v-model="this.currentUser.password2" class="inputfield-personal-data">
                    </div>
                    <div id="inputfield-title">Nieuwe wachtwoord</div>
                    <div id="input-edit"><input v-model="this.currentUser.password3" class="inputfield-personal-data"></div>
                    <button @click="changePassword()" id="button-personal-data">bevestig</button>
                </div>
            </div>

        </div>
        <PopUpMessage ref="PopUpMessage" />
    </body>
</template>
  
<script>
import axios from '../../axios-auth.js';
import moment from 'moment';
import Pagination from '../components/pagination/Pagination.vue';
import PopUpMessage from '../components/notifications/PopUpMessage.vue';
import Header from '../components/layout/Header.vue';
import profileFill from "../components/icons/IconLoginProfileFill.vue";
import lockClosed from "../components/icons/iconLoginLockClosed.vue";
import ProfilePeople from "../components/icons/IconProfilePeople.vue";
import ArrowRight from "../components/icons/iconArrowRight.vue";


export default {
    components: {
        Pagination,
        PopUpMessage,
        Header,
        profileFill,
        lockClosed,
        ProfilePeople,
        ArrowRight
    },

    data() {
        return {
            currentUser: {
                name: JSON.parse(localStorage.getItem("currentUser")).name,
                email: JSON.parse(localStorage.getItem("currentUser")).email,
                userId: JSON.parse(localStorage.getItem("currentUser")).userId,
                password1: "",
                password2: "",
                password3: ""

            },
        };
    },
    methods: {
        changePassword() {
            console.log(this.currentUser)
            axios.put("forgot-password", this.currentUser, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                    localStorage.setItem('popUpSucces', 'true');
                    this.$refs.PopUpMessage.popUpError("Data is bijgewerkt.");
                }).catch((error) => {
                    this.$refs.PopUpMessage.popUpError(error.response.data);
                });
        },
    },
};
</script>
  
  
<style>
@import '../assets/css//Profile.css';
@import '../assets/css/Main.css';
</style>
  