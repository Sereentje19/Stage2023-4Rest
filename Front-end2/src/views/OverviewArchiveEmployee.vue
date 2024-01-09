<template>
    <body>
        <Header ref="Header"></Header>
        <div class="overview-container">
            <div id="topside">
                <h1 id="h1-overview">Archief</h1>
                <input id="searchfield-overview" v-model="searchField" type="search" placeholder="Zoek"
                    @input="getAllEmployees" />
            </div>

            <div v-if="displayedDocuments.length > 0">
                <div id="titles-overview-employee">
                    <h3></h3>
                    <h3 id="urgentie">Naam</h3>
                    <h3 id="klantnaam">Email</h3>
                    <h3 id="klantnaam">Geschiedenis</h3>
                    <div></div>
                    <h3 id="klantnaam">Zet terug</h3>
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
                <br> Nog geen medewerkers gearchiveerd.
            </div>

            <br><br><br>

            <PopUpMessage ref="PopUpMessage" />
        </div>

    </body>
</template>
  
<script>
import axios from '../../axios-auth.js';
import moment from 'moment';
import Pagination from '../components/pagination/Pagination.vue';
import PopUpMessage from '../components/notifications/PopUpMessage.vue';
import Header from '../components/layout/Header.vue';
import ArrowRight from "../components/icons/iconArrowRight.vue";
import Archive from '../components/icons/IconArchive.vue';
import History from '../components/icons/IconHistory.vue';


export default {
    name: "Overview",
    components: {
        Pagination,
        Header,
        PopUpMessage,
        ArrowRight,
        Archive,
        History
    },

    data() {
        return {
            employees: [
                {
                    employeeId: 0,
                    name: "",
                    email: ""
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
                if (cus.isArchived == true && this.toHistory == false) {
                    this.$router.push("/info/medewerker/archief/" + cus.employeeId);
                }
                else {
                    this.getAllEmployees();
                }
            }, 100);
        },
        goToHistory(cus) {
            this.toHistory = true;
            this.$router.push("/geschiedenis/medewerker/" + cus.employeeId);
        },
        toggleCheckbox(doc) {
            doc.isArchived = false;

            axios.put("employee/archive", doc, {
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
        handlePageChange(newPage) {
            this.pager.currentPage = newPage;
            this.getAllEmployees();
        },
        getAllEmployees() {
            axios
                .get("employee/archive", {
                    headers: {
                        Authorization: "Bearer " + localStorage.getItem("jwt"),
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
                })
                .catch((error) => {
                    this.$refs.PopUpMessage.popUpError(error.response.data);
                });
        },
        formatDate(date) {
            return moment(date).format("DD-MM-YYYY");
        },
        caculationDays(date) {
            const documentDate = new Date(date);
            const currentDate = new Date();
            const ageInDays = Math.floor((currentDate - documentDate) / (1000 * 60 * 60 * 24));
            return (ageInDays - (ageInDays + ageInDays));
        },
        daysAway(date) {
            const ageInDays = this.caculationDays(date);
            const week = 7;
            const month = 30;
            const year = 365;

            let unit, value;

            if (ageInDays >= year) {
                unit = "jaar";
                value = Math.floor(ageInDays / year);
            } else if (ageInDays >= month * 2) {
                unit = "maanden";
                value = Math.floor(ageInDays / month);
            } else if (ageInDays >= week * 2) {
                unit = "weken";
                value = Math.floor(ageInDays / week);
            } else {
                unit = "dagen";
                value = ageInDays;
            }

            value = this.toOrFromArchive(value);
            return `${value} ${unit}`;
        },
        toOrFromArchive(value) {
            if (value < 0) {
                value = Math.abs(value);
            }
            else if (value > 0) {
                value = -value
            }
            return value
        },
        documentDaysFromExpiration(document, days) {
            const ageInDays = this.caculationDays(document.date);
            return (ageInDays <= days)
        },
    },
    computed: {
        displayedDocuments() {
            return this.employees.slice(0, this.pager.pageSize);
        },
    },
};
</script>
  
  
<style>@import '../assets/css/Overview.css';
@import '../assets/css/Main.css';</style>
  