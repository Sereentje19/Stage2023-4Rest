
<template>
    <Header></Header>
    <div class="history-container">
        <div class="info">
            <h1>Leen geschiedenis</h1>
            <div id="item-data">
                <div v-if="loanHistory.length > 0" id="loan-data">
                    <div>
                        <div><b> Type:</b> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; {{ this.loanHistory[0].type.name }}</div>
                        <div><b> Serie nummer:</b> &nbsp; {{ this.loanHistory[0].serialNumber }}</div>
                    </div>
                    <div id="loan-item-data">
                        <div><b> Gekocht op:</b> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; {{ formatDate(this.loanHistory[0].purchaseDate) }}
                        </div>
                        <div><b> Gebruiken tot:</b> &nbsp;&nbsp; {{ formatDate(this.loanHistory[0].expirationDate) }}</div>
                    </div>
                </div>

                <div v-if="displayedDocuments.length > 0">
                    <div id="titles-overview-history">
                        <div></div>
                        <h3 id="klantnaam">Medewerker</h3>
                        <h3 id="geldigVan">Geleend op</h3>
                        <h3 id="geldigTot">Geleend tot</h3>
                    </div>

                    <div v-for="(loanHistory, i) in displayedDocuments">
                        <div @click="goToInfoPage(loanHistory)" id="item-history">
                            <div></div>
                            <div id="klantnaamTekst">{{ loanHistory.name }}</div>
                            <div id="geldigVanTekst">{{ formatDate(loanHistory.loanDate) }}</div>
                            <div id="geldigTotTekst">{{ formatDate(loanHistory.returnDate) }}</div>
                        </div>
                    </div>

                    <div id="paging">
                        <Pagination :currentPage="pager.currentPage" :totalPages="pager.totalPages"
                            @page-changed="handlePageChange" />
                    </div>
                </div>
                <div v-else>
                    Dit product is nog niet uitgeleend.
                </div>
            </div>
        </div>
        <PopUpMessage ref="PopUpMessage" />
    </div>
</template>

<script>
import axios from '../../axios-auth.js';
import moment from 'moment';
import PopUpMessage from '../components/notifications/PopUpMessage.vue';
import Header from '../components/layout/Header.vue';
import Pagination from '../components/pagination/Pagination.vue';

export default {
    name: "Info",
    props: {
        id: Number,
    },
    components: {
        PopUpMessage,
        Header,
        Pagination
    },
    data() {
        return {
            loanHistory: [
                {
                    type: {
                        name: ""
                    },
                    serialNumber: "",
                    productId: 0,
                    expirationDate: "",
                    purchaseDate: "",
                    loanDate: "",
                    returnDate: "",
                    name: ""
                }
            ],
            pager: {
                currentPage: 1,
                totalItems: 0,
                totalPages: 0,
                pageSize: 5,
            },
        }
    },
    mounted() {
        this.getLoanHistory();
    },
    methods: {
        handlePageChange(newPage) {
            this.pager.currentPage = newPage;
            this.getLoanHistory();
        },
        getLoanHistory() {
            axios.get("loan-history/product/" + this.id, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                },
                params: {
                    page: this.pager.currentPage,
                    pageSize: this.pager.pageSize
                },
            })
                .then((res) => {
                    this.loanHistory = res.data.loanHistory;
                    this.pager = res.data.pager;

                }).catch((error) => {
                    this.$refs.PopUpMessage.popUpError(error.response.data);
                });
        },
        formatDate(date) {
            if (date == null) {
                return "Heden";
            }
            return moment(date).format("DD MMM YYYY");
        },
    },
    computed: {
        displayedDocuments() {
            return this.loanHistory.slice(0, this.pager.pageSize);
        },
    },
}

</script>

<style>
@import '../assets/css/History.css';
@import '../assets/css/Main.css';
</style>
  