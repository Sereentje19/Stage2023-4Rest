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
                    <div id="inputfield-title">Naam</div> 
                    <div id="input-edit"><input v-bind:readonly="true" class="inputfield-personal-data"
                            :placeholder="this.currentUser.name"> <button id="button-personal-data"
                            @click="editNameField()">edit</button></div>
                    <div v-if="this.editName" id="input-edit"><input v-model="this.user.name"
                            class="inputfield-personal-data" placeholder="Nieuwe naam"> <button @click="updateUser(true)"
                            id="button-personal-data">bevestig</button></div>
                    <div class="email-personal-data" id="inputfield-title">Email</div>
                    <div id="input-edit"><input v-bind:readonly="true" class="inputfield-personal-data"
                            :placeholder="this.currentUser.email"> <button id="button-personal-data"
                            @click="editEmailField()">edit</button></div>
                    <div v-if="this.editEmail" id="input-edit"><input v-model="this.user.email1" class="inputfield-personal-data"
                            placeholder="Nieuwe email"></div>
                    <div v-if="this.editEmail" id="input-edit"><input v-model="this.user.email2"
                            class="inputfield-personal-data" placeholder="Nieuwe email"> <button @click="updateUser(false)"
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
import Pagination from '@/components/pagination/Pagination.vue';
import PopUpMessage from '@/components/notifications/PopUpMessage.vue';
import Header from '@/components/layout/Header.vue';
import profileFill from "@/components/icons/iconProfileFill.vue";
import lockClosed from "@/components/icons/iconLockClosed.vue";
import ProfilePeople from "@/components/icons/IconProfilePeople.vue";
import ArrowRight from "@/components/icons/iconArrowRight.vue";


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
            editEmail: false,
            currentUser: JSON.parse(localStorage.getItem("currentUser")),
            user: {
                email1: "",
                name: "",
                userId: JSON.parse(localStorage.getItem("currentUser")).userId,
                email2: "",
                updateName: false
            },
        };
    },
    methods: {

        
        updateUser(bool) {
            this.user.updateName = bool;

            axios.put("user", this.user, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                    localStorage.setItem('popUpSucces', 'true');
                    this.$refs.PopUpMessage.popUpError("Data is bijgewerkt. Log opnieuw in om de nieuwe data in te zien.");
                }).catch((error) => {
                    this.$refs.PopUpMessage.popUpError(error.response.data);
                });
        },
        editNameField() {
            this.editName = !this.editName;
        },
        editEmailField() {
            this.editEmail = !this.editEmail;
        }
    },
};
</script>
  
  
<style>
@import '/assets/css//Profile.css';
@import '/assets/css/Main.css';
</style>
  