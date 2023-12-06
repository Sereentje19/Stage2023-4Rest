<template>
    <body>
        <Header ref="Header"></Header>
        <div id="body-profile">
            <div id="nav-profile">
                <a href="/profiel/persoonsgegevens" class="nav-item" id="current-nav-item">
                    <profileFill style="color: #ffffff;" /> Persoonsgegevens &nbsp;&nbsp;&nbsp;&nbsp;
                    <ArrowRight />
                </a>
                <a href="/profiel/wachtwoord-wijzigen" class="nav-item">
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
                    <div id="input-edit"><input v-bind:readonly="true" class="inputfield-personal-data" placeholder="Serena Kenter"> <button
                            id="button-personal-data" @click="editNameField()">edit</button></div>
                    <div v-if="this.editName" id="input-edit"><input class="inputfield-personal-data" placeholder="Nieuwe naam"> <button
                            id="button-personal-data">bevestig</button></div>
                    <div class="email-personal-data" id="input-edit"><input v-bind:readonly="true" class="inputfield-personal-data" placeholder="Serena.kenter@gmail.com"> <button
                            id="button-personal-data" @click="editEmailField()">edit</button></div>
                    <div v-if="this.editEmail" id="input-edit"><input class="inputfield-personal-data" placeholder="Nieuwe email"> <button
                            id="button-personal-data">bevestig</button></div>
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
import profileFill from "../components/icons/iconLoginProfileFill.vue";
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
            editName: false,
            editEmail: false

        };
    },
    mounted() {


    },
    methods: {
        editNameField() {
            this.editName = !this.editName;
        },
        editEmailField() {
            this.editEmail = !this.editEmail;
        }
    },
};
</script>
  
  
<style>@import '../assets/css//Profile.css';
@import '../assets/css/Main.css';</style>
  