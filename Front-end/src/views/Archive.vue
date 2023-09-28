<template>
    <body class="overviewBody">
        <div class="header">
            <a href="/overzicht"><img id="logoHeader" src="../assets/Pictures/Logo-4-rest-IT.png" alt="does not work" /></a>
            <div id="buttonsHeader">
                <router-link to="/overzicht">Overzicht</router-link>
                <router-link to="/uploaden">Document uploaden</router-link>
                <router-link to="/">Uitloggen</router-link>
            </div>
        </div>
        <div class="overviewContainer">
            <h1 id="h1Overzicht">Archief</h1>
            <div id="titlesArchive">
                <h3 id="klantnaam">Klantnaam</h3>
                <h3 id="geldigVan">Verlopen op</h3>
                <h3 id="geldigTot">Dagen vervallen </h3>
                <h3 id="Type">Type document</h3>
            </div>

            <div class="overview" v-for="(document, i) in displayedDocuments">
                <router-link :to="{ path: '/infopage/' + document.documentId }" id="itemArchive">
                    <div id="klantnaamTekst">{{ document.customerName }}</div>
                    <div id="geldigVanTekst">{{ formatDate(document.date) }}</div>
                    <div id="geldigTotTekst">{{ daysAway(document.date) }}</div>
                    <div id="typeTekst">{{ document.type }}</div>
                </router-link>
            </div>

            <div id="paging">
                <Pagination :currentPage="pager.currentPage" :totalPages="pager.totalPages"
                    @page-changed="handlePageChange" />
            </div>

            <div class="popup-container" :class="{ 'active': activePopup === 'popup1' || popup1 === 'true' }">
                <div class="Succes">
                    <img class="Succesimage" src="../assets/Pictures/Checked.png">
                    <p id="message">Upload successful.</p>
                    <button id="buttonClose" @click="closePopup('popup1')"><b>x</b></button>
                </div>
            </div>
        </div>
        <br><br><br>

    </body>
</template>
  
<script>
import axios from '../../axios-auth.js';
import moment from 'moment';
import Pagination from '../views/Pagination.vue';


export default {
    name: "Overview",
    components: {
        Pagination,
    },

    data() {
        return {
            documents: [
                {
                    documentId: 0,
                    customerId: 0,
                    date: "",
                    customerName: "",
                    type: ""
                }
            ],
            pager: {
                currentPage: 1,
                totalItems: 0,
                totalPages: 0,
                pageSize: 5,
            },
            customers: [],
            activePopup: null,
            popup1: this.$route.query.popup1,
        };
    },
    mounted() {
        this.getDocuments();

        setTimeout(() => {
            this.closePopup();
        }, 3000);
    },
    methods: {
        handlePageChange(newPage) {
            this.pager.currentPage = newPage;
            this.getDocuments();
        },
        getDocuments() {
            axios.get("Document", {
                params: {
                    page: this.pager.currentPage,
                    pageSize: this.pager.pageSize,
                    isArchived: true
                }
            })
                .then((res) => {
                    this.documents = res.data.documents;
                    this.pager = res.data.pager;

                    for (let index = 0; index < this.documents.length; index++) {
                        const customerId = this.documents[index].customerId;
                        this.getCustomerName(customerId, index);
                    }
                }).catch((error) => {
                    alert(error.response.data);
                });
        },
        getCustomerName(id, index) {
            axios.get("Customer/" + id)
                .then((res) => {
                    this.documents[index].customerName = res.data.name;
                }).catch((error) => {
                    alert(error.response.data);
                });
        },
        formatDate(date) {
            return moment(date).format("DD-MM-YYYY");
        },
        daysAway(date) {
            const documentDate = new Date(date);
            const currentDate = new Date();
            const ageInDays = Math.floor((currentDate - documentDate) / (1000 * 60 * 60 * 24));
            return ageInDays
        },
        closePopup() {
            this.popup1 = false;
        },
    },
    computed: {
        displayedDocuments() {
            return this.documents.slice(0, this.pager.pageSize);
        },
    },
};
</script>
  
  
<style>
#paging {
    display: flex;
    margin-right: auto;

}

.Succes {
    color: black;
    text-align: left;
    background-color: #90F587;
    font-size: 17px;
    padding: 10px;
    display: flex;
}

.popup-container {
    width: fit-content;
}

#message {
    margin: auto 20px auto 20px;
}

.Succesimage {
    width: 30px;
    height: 30px;
    margin: auto;
}

.popup-container {
    position: fixed;
    bottom: 0;
    right: -600px;
    width: fit-content;
    box-shadow: -5px 0 15px rgba(0, 0, 0, 0.3);
    transition: right 0.3s ease-in-out;
}

#buttonClose {
    font-size: 25px;
    background-color: #90F587;
    color: rgb(63, 63, 63);
    border: none;
}

.popup-container.active {
    right: 0;
}

#h1AndButton {
    display: flex;
    margin: auto;
}

#buttonArchief {
    font-size: 25px;
    height: fit-content;
    padding: 10px 20px 10px 20px;
    margin: auto 0 auto auto;
    background-color: #22421f;
    color: white;
    border: none;
    border-radius: 4px;
}

.popup-button {
    display: block;
    text-align: center;
    background-color: #007bff;
    color: #fff;
    padding: 10px;
    cursor: pointer;
}

#urgentieSymbool {
    width: 50px;
    margin-top: 8px;
    margin-left: 8px;
}

#klantnaamTekst,
#geldigVanTekst,
#geldigTotTekst,
#typeTekst {
    margin-top: auto;
    margin-bottom: auto;
    font-size: large;
}

#titlesArchive {
    display: grid;
    grid-template-columns: 30% 15% 20% 25%;
}

#pageNavigator {
    margin-top: 20px;
}


#itemArchive {
    width: 100%;
    height: 55px;
    background-color: white;
    margin-bottom: 6px;
    border-color: #868686;
    border-width: 1px;
    border-style: solid;
    display: grid;
    grid-template-columns: 30% 15% 20% 25%;
    color: black;
    margin-left: auto;
    border-radius: 3px;
}

#itemArchive :first-child {
    margin-left: 20px;
}

#titlesArchive :first-child {
    margin-left: 20px;
}

a {
    text-decoration: none;
}

.overviewContainer {
    width: 85%;
    margin: auto;
    margin-top: 80px;
}

h1 {
    font-size: 50px;
}

#logoHeader {
    width: 130px;
    height: 80px;
    margin-left: 3px;
    margin-top: 28px;
    padding: 0.5% 0% 0% 1%;
}

a {
    font-size: 28px;
    color: white;
    margin-left: 30px;
}

#buttonsHeader {
    position: absolute;
    right: 0px;
    padding: 50px 40px 0px 0px;
}

.header {
    width: 103%;
    height: 120px;
    background-color: #153912;
    margin-left: -1.5%;
    margin-top: -1.5%;
    display: flex;
    flex-direction: row;
}

body {
    background-color: #afaeae;
    font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
}
</style>