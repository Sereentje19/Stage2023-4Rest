<template>
    <body>
        <Header ref="Header"></Header>
        <div class="overview-container">
            <div id="topside">
                <h1 id="h1-overview">Medewerkers</h1>

                <input id="searchfield-overview" v-model="searchField" type="search" placeholder="Zoek"
                    @input="getAllCustomers" />
            </div>

            <div v-if="displayedDocuments.length > 0">
                <div id="titles-overview-employee">
                    <h3></h3>
                    <h3 id="urgentie">Naam</h3>
                    <h3 id="klantnaam">Email</h3>
                    <h3 id="klantnaam">Geschiedenis</h3>
                </div>

                <div v-for="(customer, i) in displayedDocuments">
                    <div @click="goToInfoPage(customer)" id="item-employees">
                        <div></div>
                        <div id="klantnaamTekst">{{ customer.name }}</div>
                        <div id="geldigVanTekst">{{ customer.email }}</div>
                        <button id="button-history" @click="goToHistory(customer)"><svg
                                xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor"
                                class="bi bi-hourglass" viewBox="0 0 16 16">
                                <path
                                    d="M2 1.5a.5.5 0 0 1 .5-.5h11a.5.5 0 0 1 0 1h-1v1a4.5 4.5 0 0 1-2.557 4.06c-.29.139-.443.377-.443.59v.7c0 .213.154.451.443.59A4.5 4.5 0 0 1 12.5 13v1h1a.5.5 0 0 1 0 1h-11a.5.5 0 1 1 0-1h1v-1a4.5 4.5 0 0 1 2.557-4.06c.29-.139.443-.377.443-.59v-.7c0-.213-.154-.451-.443-.59A4.5 4.5 0 0 1 3.5 3V2h-1a.5.5 0 0 1-.5-.5zm2.5.5v1a3.5 3.5 0 0 0 1.989 3.158c.533.256 1.011.791 1.011 1.491v.702c0 .7-.478 1.235-1.011 1.491A3.5 3.5 0 0 0 4.5 13v1h7v-1a3.5 3.5 0 0 0-1.989-3.158C8.978 9.586 8.5 9.052 8.5 8.351v-.702c0-.7.478-1.235 1.011-1.491A3.5 3.5 0 0 0 11.5 3V2h-7z" />
                            </svg></button>
                    </div>
                </div>

                <div id="paging">
                    <Pagination :currentPage="pager.currentPage" :totalPages="pager.totalPages"
                        @page-changed="handlePageChange" />
                </div>
            </div>
            <div v-else>
                <br> Nog geen geldige documenten bekend
            </div>

            <br><br><br>

            <PopUpMessage ref="Popup" />
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
                    customerId: 0,
                    date: "",
                    customerName: "",
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
            customers: [
                {
                    customerId: 0,
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
        this.getAllCustomers();

        if (this.$route.query.activePopup && localStorage.getItem('popUpSucces') === 'true') {
            this.$refs.Popup.popUpError("Document is geupload!");
        }
    },
    methods: {
        goToInfoPage(cus) {
            setTimeout(() => {
                if (this.toHistory == false) {
                    this.$router.push("/info/medewerker/" + cus.customerId);
                }
            }, 100);
        },
        goToHistory(cus) {
            this.toHistory = true;
            this.$router.push("/geschiedenis/medewerker/" + cus.customerId);
        },
        handlePageChange(newPage) {
            this.pager.currentPage = newPage;
            this.getAllCustomers();
        },
        getAllCustomers() {
            axios.get("Customer", {
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
                    this.customers = res.data.customers
                    this.pager = res.data.pager;
                }).catch((error) => {
                    this.$refs.Popup.popUpError(error.response.data);
                });
        },
        formatDate(date) {
            return moment(date).format("DD-MM-YYYY");
        },
    },
    computed: {
        displayedDocuments() {
            return this.customers.slice(0, this.pager.pageSize);
        },
    },
};
</script>
  
  
<style>
@import '../assets/Css/Overview.css';
@import '../assets/Css/Main.css';
</style>
  