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

            <div id="profile-page-users">
                <div id="profile-users">
                    <div id="table-users">
                        <input @input="getAllFilteredEmployees" v-model="searchField" type="search"
                            class="searchfield-loanhistory" placeholder="Zoek klant" />

                        <table id="top-table">
                            <tr>
                                <td class="first-row-users"><b>Name</b></td>
                                <td id="second-row-users"><b>Email</b></td>
                                <td id="third-row-users"><ProfileDelete/> &nbsp;</td>
                            </tr>
                        </table>
                        
                        <div id="table-info-users">
                            <table id="bottom-table-users">
                                <tr @click="selectEmployee(employee)">
                                    <td class="first-row-users">vf</td>
                                    <td id="second-row-users">vfvfd</td>
                                    <td id="third-row-users"><Trash id="trash"/></td>
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
import profileFill from "../components/icons/iconLoginProfileFill.vue";
import lockClosed from "../components/icons/iconLoginLockClosed.vue";
import ProfilePeople from "../components/icons/IconProfilePeople.vue";
import ArrowRight from "../components/icons/iconArrowRight.vue";
import ProfileAdd from "../components/icons/IconProfileAdd.vue";
import Trash from "../components/icons/IconTrash.vue";
import ProfileDelete from "../components/icons/IconProfileDelete.vue";


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
        ProfileDelete
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
        addUser() {
            this.$router.push('/profiel/gebruikers-toevoegen');
        }
    },
};
</script>
  
  
<style>
@import '../assets/css//Profile.css';
@import '../assets/css/Main.css';
</style>
  