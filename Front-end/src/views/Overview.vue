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
      <h1>Overzicht</h1>

      <div id="titlesOverview">
        <h3 id="urgentie">Urgentie</h3>
        <h3 id="klantnaam">Klantnaam</h3>
        <h3 id="geldigVan">Geldig tot</h3>
        <h3 id="geldigTot">Aantal dagen</h3>
        <h3 id="Type">Type document</h3>
      </div>

      <div class="overview" v-for="(document, i) in displayedDocuments">
        <router-link :to="{ path: '/infopage' }" id="item">
          <img v-if="documentDaysFromExpiration(document, 14)" id="urgentieSymbool"
            src="../assets/Pictures/hogeUrgentie.png" alt="does not work" />
          <img v-else-if="documentDaysFromExpiration(document, 30)" id="urgentieSymbool"
            src="../assets/Pictures/middelUrgentie.png" alt="does not work" />
          <img v-else id="urgentieSymbool" src="../assets/Pictures/lageUrgentie.png" alt="does not work" />
          <div id="klantnaamTekst">{{ document.customerName }}</div>
          <div id="geldigVanTekst">{{ formatDate(document.date) }}</div>
          <div id="geldigTotTekst">{{ daysAway(document.date) }}</div>
          <div id="typeTekst">{{ document.type }}</div>
        </router-link>
      </div>

      <div id="paging">
        <Pagination :currentPage="pager.currentPage" :totalPages="pager.totalPages" @page-changed="handlePageChange" />
      </div>

      <div id="pageNavigator">
        <div>
          <div>
            <div>
              <div class="popup-container" :class="{ 'active': activePopup === 'popup1' || popup1 === 'true' }">
                <div class="Succes">
                  <img class="Succesimage" src="../assets/Pictures/Checked.png">
                  <p>Succes!<br> Het document is succes geupload.</p>
                  <button @click="closePopup('popup1')">Close</button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

  </body>
</template>

<script>
import axios from '../../axios-auth.js';
import moment from 'moment';
import ArrowLeft from "../components/icons/iconOverviewArrowLeft.vue";
import ArrowRight from "../components/icons/iconOverviewArrowRight.vue";
import Pagination from '../views/Pagination.vue';


export default {
  name: "overview",
  components: {
    ArrowLeft,
    ArrowRight,
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
        startIndex: 0,
        endIndex: 0,
        startPage: 0,
        endPage: 0,
      },
      customers: [],
      activePopup: null,
      sidebarOpen: true,
      popup1: this.$route.query.popup1 || false,
    };
  },
  mounted() {
    this.getDocuments();
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
          pageSize: this.pager.pageSize
        }
      })
        .then((res) => {
          this.documents = res.data.documents;
          this.pager = res.data.pager;
          console.log(res.data.documents)
          console.log(res.data.pager)

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
    caculationDays(date){
      const documentDate = new Date(date);
      const currentDate = new Date();
      const ageInDays = Math.floor((currentDate - documentDate) / (1000 * 60 * 60 * 24));
      return ageInDays;
    },
    daysAway(date) {
      const ageInDays = this.caculationDays(date);
      return Math.abs(ageInDays)
    },
    documentDaysFromExpiration(document, days) {
      const ageInDays = this.caculationDays(document.date);
      return (Math.abs(ageInDays) <= days)
    },
    closePopup(popupName) {
      if (popupName === 'popup1') {
        this.popup1 = false;
      }
    },
  },
  computed: {
    displayedDocuments() {
      const startIndex = (this.pager.currentPage - 1) * this.pager.pageSize;
      const endIndex = startIndex + this.pager.pageSize;

      return this.documents.slice(0, 5);
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
  padding: 10px;
  text-align: left;
  background-color: #90F587;
  font-size: 20px;
}

.Error {
  color: black;
  padding: 10px;
  background-color: #F56C6C;
  font-size: 20px;
  text-align: left;
}

.Errorimage {
  width: 60px;
  height: 60px;
}

.Succesimage {
  width: 60px;
  height: 60px;
}

.popup-container {
  position: fixed;
  bottom: 0;
  right: -300px;
  width: 300px;
  box-shadow: -5px 0 15px rgba(0, 0, 0, 0.3);
  transition: right 0.3s ease-in-out;
}

.popup-container.active {
  right: 0;
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

#titlesOverview {
  display: grid;
  grid-template-columns: 10% 30% 15% 15% 30%;
}

#pageNavigator {
  margin-top: 20px;
}

#item {
  width: 100%;
  height: 55px;
  background-color: white;
  margin-bottom: 6px;
  border-color: #868686;
  border-width: 1px;
  border-style: solid;
  display: grid;
  grid-template-columns: 10% 30% 15% 15% 30%;
  color: black;
  margin-left: auto;
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
  background-color: #d9d9d9;
  font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
}
</style>