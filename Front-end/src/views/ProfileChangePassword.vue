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
                    <div id="input-edit"><input class="inputfield-personal-data" placeholder="Huidige wachtwoord"> </div>
                    <div id="input-edit"><input class="inputfield-personal-data" placeholder="Nieuw wachtwoord">
                    </div>
                    <div id="input-edit"><input class="inputfield-personal-data" placeholder="Nieuw wachtwoord"></div>
                    <button id="button-personal-data">bevestig</button>
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
            documents: [
                {
                    documentId: 0,
                    employeeId: 0,
                    date: "",
                    employeeName: "",
                    type: "",
                    isArchived: false
                }
            ],
            pager: {
                currentPage: 1,
                totalItems: 0,
                totalPages: 0,
                pageSize: 5,
            },
            employees: [],
            searchField: "",
            dropdown: "0",
        };
    },
    mounted() {


    },
    methods: {

    },
};
</script>
  
  
<style>
@import '../assets/css//Profile.css';
@import '../assets/css/Main.css';
</style>
  