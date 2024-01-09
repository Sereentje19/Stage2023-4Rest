<template>
    <body>
        <Header ref="Header"></Header>
        <PopupChoice ref="PopupChoice" @delete="deleteUser()" />
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

            <div id="profile-page-users">
                <div id="profile-users">
                    <div id="table-users">
                        <table id="top-table">
                            <tr>
                                <td class="first-row-users"><b>Naam</b></td>
                                <td id="second-row-users"><b>Email</b></td>
                                <td id="third-row-users">
                                    <ProfileDelete /> &nbsp;
                                </td>
                            </tr>
                        </table>

                        <div id="table-info-users">
                            <table id="bottom-table-users">
                                <tr v-for="(user, i) in this.users">
                                    <td class="first-row-users">{{ user.name }}</td>
                                    <td id="second-row-users">{{ user.email }}</td>
                                    <td id="third-row-users">
                                        <Trash @click="popUp(user)" id="trash" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <button id="profile-add-button">
                    <ProfileAdd @click="addUser()" />
                </button>
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
import ProfileAdd from "../components/icons/IconProfileAdd.vue";
import Trash from "../components/icons/IconTrash.vue";
import ProfileDelete from "../components/icons/IconProfileDelete.vue";
import PopupChoice from '../components/notifications/PopUpChoice.vue';


export default {
    components: {
        Pagination,
        PopUpMessage,
        Header,
        profileFill,
        lockClosed,
        ProfilePeople,
        ArrowRight,
        ProfileAdd,
        Trash,
        ProfileDelete,
        PopupChoice
    },

    data() {
        return {
            users: {
                name: "",
                email: ""
            },
            email: ""
        };
    },
    mounted() {
        this.getUsers();
    },
    methods: {
        addUser() {
            this.$router.push('/profiel/gebruikers-toevoegen');
        },
        getUsers() {
            axios.get("user", {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                    this.users = res.data;
                }).catch((error) => {
                    this.$refs.PopUpMessage.popUpError(error.response.data);
                });
        },
        deleteUser() {
            axios.delete("user", {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                },
                params: {
                    email: this.email
                }
            })
                .then((res) => {
                    this.users = res.data;
                    this.getUsers();
                }).catch((error) => {
                    this.$refs.PopUpMessage.popUpError(error.response.data);
                });
        },
        popUp(user) {
            this.email = user.email;
            this.emitter.emit('isPopUpTrue', { 'eventContent': true });
            this.emitter.emit('text', { 'eventContent': `Weet je zeker dat je ${user.name} wilt verwijderen?` });
        },
    },
};
</script>
  
  
<style>
@import '../assets/css//Profile.css';
@import '../assets/css/Main.css';
</style>
  