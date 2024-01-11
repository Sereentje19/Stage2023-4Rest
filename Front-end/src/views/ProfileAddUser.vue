<template>
    <body>
        <Header ref="Header"></Header>
        <div id="body-profile">
            <div id="nav-profile">
                <a href="/profiel/persoonsgegevens" class="nav-item">
                    <profileFill style="color: #ffffff;" /> Persoonsgegevens &nbsp;&nbsp;&nbsp;&nbsp;
                    <ArrowRight />
                </a>
                <a href="/profiel/wachtwoord-wijzigen" class="nav-item">
                    <lockClosed style="color: #ffffff;" /> Wachtwoord wijzigen
                    <ArrowRight />
                </a>
                <a href="/profiel/gebruikers-beheren" class="nav-item" id="current-nav-item">
                    <ProfilePeople style="color: #ffffff;" /> Gebruikers beheren &nbsp;&nbsp;&nbsp;
                    <ArrowRight />
                </a>
            </div>

            <div id="profile-page-add-users">
                <div id="table-add-users">
                    <div id="inputfields">
                        <div id="inputfield-title">Naam</div>
                        <div id="input-edit"><input v-model="this.user.name" class="inputfield-personal-data">
                        </div>
                        <div id="inputfield-title">Email</div>
                        <div id="input-edit"><input v-model="this.user.email" class="inputfield-personal-data"></div>
                        <button @click="addUser()" id="button-personal-data">bevestig</button>
                    </div>
                    <button id="profile-back-button">
                        <ProfileBack @click="back()" />
                    </button>
                </div>
            </div>


        </div>
        <PopUpMessage ref="PopUpMessage" />
    </body>
</template>
  
<script>
import axios from '../../axios-auth.js';
import moment from 'moment';
import Pagination from '@/components/pagination/Pagination.vue';
import PopUpMessage from '@/components/notifications/PopUpMessage.vue';
import Header from '@/components/layout/Header.vue';
import profileFill from "@/components/icons/IconProfileFill.vue";
import lockClosed from "@/components/icons/iconLoginLockClosed.vue";
import ProfilePeople from "@/components/icons/IconProfilePeople.vue";
import ArrowRight from "@/components/icons/iconArrowRight.vue";
import ProfileBack from "@/components/icons/IconBack.vue";
import Trash from "@/components/icons/IconTrash.vue";


export default {
    components: {
        Pagination,
        PopUpMessage,
        Header,
        profileFill,
        lockClosed,
        ProfilePeople,
        ArrowRight,
        ProfileBack,
        Trash
    },

    data() {
        return {
            user:{
                name: "",
                email: ""
            }
        };
    },
    methods: {
        back() {
            this.$router.push('/profiel/gebruikers-beheren');
        },
        addUser() {
            axios.post("user", this.user, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }})
                .then((res) => {
                    this.$router.push("/profiel/gebruikers-beheren");
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
  