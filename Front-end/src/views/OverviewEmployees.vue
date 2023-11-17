<template>
    <body>
        <Header ref="Header"></Header>
        <div class="overview-container">
            <div id="topside">
                <h1 id="h1-overview">Medewerkers</h1>

                <input id="searchfield-overview" v-model="searchField" type="search" placeholder="Zoek"
                    @input="getAllEmployees" />

                <button @click="toArchive()" id="button-archive">Archief</button>
            </div>

            <div v-if="displayedDocuments.length > 0">
                <div id="titles-overview-employee">
                    <h3></h3>
                    <h3 id="urgentie">Naam</h3>
                    <h3 id="klantnaam">Email</h3>
                    <h3 id="klantnaam">Geschiedenis</h3>
                    <div></div>
                    <h3 id="klantnaam">Archiveer</h3>
                </div>

                <div v-for="(employee, i) in displayedDocuments">
                    <div @click="goToInfoPage(employee)" id="item-employees">
                        <div></div>
                        <div id="klantnaamTekst">{{ employee.name }}</div>
                        <div id="geldigVanTekst">{{ employee.email }}</div>
                        <button id="button-history" @click="goToHistory(employee)"><svg xmlns="http://www.w3.org/2000/svg"
                                width="20" height="20" fill="currentColor" class="bi bi-hourglass" viewBox="0 0 16 16">
                                <path
                                    d="M2 1.5a.5.5 0 0 1 .5-.5h11a.5.5 0 0 1 0 1h-1v1a4.5 4.5 0 0 1-2.557 4.06c-.29.139-.443.377-.443.59v.7c0 .213.154.451.443.59A4.5 4.5 0 0 1 12.5 13v1h1a.5.5 0 0 1 0 1h-11a.5.5 0 1 1 0-1h1v-1a4.5 4.5 0 0 1 2.557-4.06c.29-.139.443-.377.443-.59v-.7c0-.213-.154-.451-.443-.59A4.5 4.5 0 0 1 3.5 3V2h-1a.5.5 0 0 1-.5-.5zm2.5.5v1a3.5 3.5 0 0 0 1.989 3.158c.533.256 1.011.791 1.011 1.491v.702c0 .7-.478 1.235-1.011 1.491A3.5 3.5 0 0 0 4.5 13v1h7v-1a3.5 3.5 0 0 0-1.989-3.158C8.978 9.586 8.5 9.052 8.5 8.351v-.702c0-.7.478-1.235 1.011-1.491A3.5 3.5 0 0 0 11.5 3V2h-7z" />
                            </svg></button>
                        <div></div>
                        <button id="button-history" @click="toggleCheckbox(employee)">
                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor"
                                class="bi bi-archive" viewBox="0 0 16 16">
                                <path
                                    d="M0 2a1 1 0 0 1 1-1h14a1 1 0 0 1 1 1v2a1 1 0 0 1-1 1v7.5a2.5 2.5 0 0 1-2.5 2.5h-9A2.5 2.5 0 0 1 1 12.5V5a1 1 0 0 1-1-1V2zm2 3v7.5A1.5 1.5 0 0 0 3.5 14h9a1.5 1.5 0 0 0 1.5-1.5V5H2zm13-3H1v2h14V2zM5 7.5a.5.5 0 0 1 .5-.5h5a.5.5 0 0 1 0 1h-5a.5.5 0 0 1-.5-.5z" />
                            </svg></button>
                    </div>
                </div>

                <div id="paging">
                    <Pagination :currentPage="pager.currentPage" :totalPages="pager.totalPages"
                        @page-changed="handlePageChange" />
                </div>
            </div>
            <div v-else>
                <br> Nog geen medewerkers bekend
            </div>

            <br><br><br>

            <PopUpMessage ref="PopUpMessage" />
        </div>

    </body>
</template>
  
<script>
import axios from '../../axios-auth.js';
import moment from 'moment';
import Pagination from '../views/Pagination.vue';
import PopUpMessage from '../views/PopUpMessage.vue';
import Header from '../views/Header.vue';


export default {
    name: "Overview",
    components: {
        Pagination,
        PopUpMessage,
        Header,
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
                    isArchived: null
                }
            ],
            pager: {
                currentPage: 1,
                totalItems: 0,
                totalPages: 0,
                pageSize: 5,
            },
            employees: [
                {
                    employeeId: 0,
                    name: "",
                    email: ""
                }
            ],
            searchField: "",
            dropdown: "0",
            toHistory: false
        };
    },
    mounted() {
        this.getAllEmployees();

        if (this.$route.query.activePopup && localStorage.getItem('popUpSucces') === 'true') {
            this.$refs.PopUpMessage.popUpError("Data is bijgewerkt.");
        }
    },
    methods: {
        goToInfoPage(cus) {
            setTimeout(() => {
                if (cus.isArchived == false) {
                    this.$router.push("/info/medewerker/huidig/" + cus.employeeId);
                }
                else {
                    this.getAllEmployees();
                }
            }, 100);
        },
        toArchive() {
            this.$router.push("/overzicht/medewerkers/archief");
        },
        toggleCheckbox(employee) {
            employee.isArchived = true;

            axios.put("employee/archive", employee, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                }).catch((error) => {
                    this.$refs.PopUpMessage.popUpError(error.response.data);
                });
        },
        goToHistory(cus) {
            this.toHistory = true;
            this.$router.push("/geschiedenis/medewerker/" + cus.employeeId);
        },
        handlePageChange(newPage) {
            this.pager.currentPage = newPage;
            this.getAllEmployees();
        },
        getAllEmployees() {
            axios.get("employee", {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                },
                params: {
                    searchfield: this.searchField,
                    page: this.pager.currentPage,
                    pageSize: this.pager.pageSize
                },
            })
                .then((res) => {
                    console.log(res.data)
                    this.employees = res.data.employees
                    this.pager = res.data.pager;
                }).catch((error) => {
                    this.$refs.PopUpMessage.popUpError(error.response.data);
                });
        },
        formatDate(date) {
            return moment(date).format("DD-MM-YYYY");
        },
    },
    computed: {
        displayedDocuments() {
            return this.employees.slice(0, this.pager.pageSize);
        },
    },
};
</script>
  
  
<style>
@import '../assets/Css/Overview.css';
@import '../assets/Css/Main.css';
</style>
  