<template>
  <body class="overviewBody">
    <Header></Header>
    <div class="overviewContainer">
      <div id="h1AndButton">
        <h1 id="h1Overzicht">Overzicht</h1>
        <button @click="toArchive" id="buttonArchief"> Archief</button>
      </div>
      <div id="titlesOverview">
        <h3 id="urgentie">Urgentie</h3>
        <h3 id="klantnaam">Klantnaam</h3>
        <h3 id="geldigVan">Geldig tot</h3>
        <h3 id="geldigTot">Aantal dagen</h3>
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

      <br><br><br>

      <Popup ref="Popup" />
    </div>

  </body>
</template>

<script>
import axios from '../../axios-auth.js';
import moment from 'moment';
import Pagination from '../views/Pagination.vue';
import Popup from '../views/popUp.vue';
import Header from '../views/Header.vue';


export default {
  name: "Overview",
  components: {
    Pagination,
    Popup,
    Header
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
    };
  },
  mounted() {
    this.getDocuments();

    if (this.$route.query.activePopup) {
      this.$refs.Popup.popUpError("Document is geupload!");
    }
  },
  methods: {
    handlePageChange(newPage) {
      this.pager.currentPage = newPage;
      this.getDocuments();
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
      return ageInDays
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
#paging {
  display: flex;
  margin-right: auto;

}

#h1AndButton {
  display: flex;
  height: 70px;
}

#buttonArchief {
  font-size: 25px;
  height: fit-content;
  padding: 10px 20px 10px 20px;
  margin: 0;
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
#typeTekst {
  max-width: 350px;
  white-space: nowrap; 
  overflow: hidden;
  text-overflow: ellipsis;
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
  border-radius: 3px;
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

#h1Overzicht {
  margin: auto auto auto 0;
}



body {
  background-color: #afaeae;
  font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
}
</style>