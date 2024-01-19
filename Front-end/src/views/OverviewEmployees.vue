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
                        <button id="button-history" @click="goToHistory(employee)">
                            <History />
                        </button>
                        <div></div>
                        <button id="button-history" @click="toggleCheckbox(employee)">
                            <Archive />
                        </button>
                    </div>
                </div>

                <div id="paging">
                    <Pagination :currentPage="pager.currentPage" :totalPages="pager.totalPages"
                        @page-changed="handlePageChange" />
                </div>
            </div>
            <div v-else>
                <br> Nog geen medewerkers bekend.
            </div>

            <br><br><br>

            <PopUpMessage ref="PopUpMessage" />
        </div>

    </body>
</template>
  
<script>
import axios from '../../axios-auth.js';
import moment from 'moment';
import Pagination from '@/components/pagination/Pagination.vue';
import PopUpMessage from '@/components/notifications/PopUpMessage.vue';
import Header from '@/components/layout/Header.vue';
import History from '@/components/icons/IconHistory.vue';
import Archive from '@/components/icons/IconArchive.vue';


export default {
  name: "OverviewEmployees",
    components: {
        Pagination,
        PopUpMessage,
        Header,
        Archive,
        History
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
                if (cus.isArchived == false && this.toHistory == false) {
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
                    this.getAllEmployees();
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
@import '/assets/css/Overview.css';
@import '/assets/css/Main.css';
</style>
  