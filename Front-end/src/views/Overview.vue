<template>
  <body class="overviewBody">
    <Header></Header>
    <div class="overviewContainer">
      <div id="h1AndButton">
        <h1 id="h1Overzicht">Overzicht</h1>

        <input id="SearchFieldOverview" v-model="searchField" type="search" placeholder="Zoek"
          @input="filterDocumentsAndCustomers" />

        <select v-model="dropBoxType" id="filterDropDown" @change="filterDocumentsAndCustomers">
          <option value="0">Selecteer document...</option>
          <option value="1">Vog</option>
          <option value="2">Contract</option>
          <option value="3">Paspoort</option>
          <option value="4">id kaart</option>
          <option value="5">Diploma</option>
          <option value="6">Certificaat</option>
          <option value="7">Lease auto</option>
        </select>

        <button @click="toGeldig" id="buttonArchief"> Geldig</button>
        &nbsp; &nbsp; &nbsp;
        <button @click="toArchive" id="buttonArchief"> Archief</button>
      </div>


      <div v-if="displayedDocuments.length > 0">
        <div id="titlesOverview">
          <h3 id="urgentie">Urgentie</h3>
          <h3 id="klantnaam">Klantnaam</h3>
          <h3 id="geldigVan">Geldig tot</h3>
          <h3 id="geldigTot">Verloopt over</h3>
          <h3 id="Type">Type document</h3>
        </div>

        <div class="overview" v-for="(document, i) in displayedDocuments">
          <router-link :to="{ path: '/infopage/' + document.documentId }" id="item">
            <img v-if="documentDaysFromExpiration(document, 35)" id="urgentieSymbool"
              src="../assets/Pictures/hogeUrgentie.png" alt="does not work" />
            <img v-else-if="documentDaysFromExpiration(document, 42)" id="urgentieSymbool"
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
      </div>
      <div v-else>
        <br> Nog geen geldige documenten bekend
      </div>

      <br><br><br>

      <Popup ref="Popup" />
    </div>

  </body>
</template>

<script>
import axios from '../../axios-auth.js';
import moment from 'moment';
import Pagination from '../views/pagination.vue';
import Popup from '../views/popUp.vue';
import Header from '../views/Header.vue';


export default {
  name: "Overview",
  components: {
    Pagination,
    Popup,
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
      searchField: "",
      dropBoxType: "0"
    };
  },
  mounted() {
    // this.getDocuments();

    if (this.$route.query.activePopup && localStorage.getItem('popUpSucces') === 'true') {
      this.$refs.Popup.popUpError("Document is geupload!");
      localStorage.setItem('popUpSucces', 'false');
    }
  },
  methods: {
    handlePageChange(newPage) {
      this.pager.currentPage = newPage;
      // this.getDocuments();
    },
    toArchive() {
      this.$router.push("/archief");
    },
    getDocuments() {
      axios.get("Document", {
        headers: {
          Authorization: "Bearer " + localStorage.getItem("jwt")
        },
        params: {
          page: this.pager.currentPage,
          pageSize: this.pager.pageSize,
          isArchived: false
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
          this.$refs.Popup.popUpError(error.response.data);
        });
    },
    getCustomerName(id, index) {
      axios.get("Customer/" + id, {
        headers: {
          Authorization: "Bearer " + localStorage.getItem("jwt")
        }
      })
        .then((res) => {
          this.documents[index].customerName = res.data.name;
        }).catch((error) => {
          this.$refs.Popup.popUpError(error.response.data);
        });
    },
    filterDocumentsAndCustomers() {
      axios
        .get("Filter/documents-and-customers", {
          headers: {
            Authorization: "Bearer " + localStorage.getItem("jwt"),
          },
          params: {
            searchfield: this.searchField, 
            dropBoxType: this.dropBoxType

          },
        })
        .then((res) => {
          this.documents = res.data.documents;
          this.customers = res.data.customers;

          console.log(this.documents)

          for (let index = 0; index < this.documents.length; index++) {
            this.documents[index].customerName = this.customers[index].name;
          }

        })
        .catch((error) => {
          this.$refs.Popup.popUpError(error.response.data);
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
      var time;

      if (ageInDays >= year) {
        time = Math.floor(ageInDays / year) + " jaar"
      }
      else if (ageInDays >= (month * 2)) {
        time = Math.floor(ageInDays / month) + " maanden"
      }
      else if (ageInDays >= (week * 2)) {
        time = Math.floor(ageInDays / week) + " weken"
      }
      else {
        time = ageInDays + " dagen"
      }

      return time
    },
    documentDaysFromExpiration(document, days) {
      const ageInDays = this.caculationDays(document.date);
      return (ageInDays <= days)
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
@import '../assets/Css/Overview.css';
@import '../assets/Css/Main.css';
</style>
