<template>
  <body class="overviewBody">
    <Header ref="Header"></Header>
    <div class="overviewContainer">
      <div id="h1AndButton">
        <h1 id="h1Overzicht">{{ overviewType }}</h1>


        <select v-model="dropBoxType" id="filterDropDown" @change="filterDocuments">
          <option value="0">Selecteer document...</option>
          <option value="1">Vog</option>
          <option value="2">Contract</option>
          <option value="3">Paspoort</option>
          <option value="4">ID kaart</option>
          <option value="5">Diploma</option>
          <option value="6">Certificaat</option>
          <option value="7">Lease auto</option>
        </select>
        <input id="SearchFieldOverview" v-model="searchField" type="search" placeholder="Zoek" @input="filterDocuments" />

      </div>

      <div v-if="displayedDocuments.length > 0">
        <div id="titlesOverview">
          <h3 id="urgentie">Urgentie</h3>
          <h3 id="klantnaam">Klantnaam</h3>
          <h3 id="geldigVan">Geldig tot</h3>
          <h3 v-if="overviewType == 'Archief'" id="geldigTot">Verstreken tijd</h3>
          <h3 v-else id="geldigTot">Verloopt over</h3>
          <h3 id="Type">Type document</h3>
          <h3 v-if="overviewType == 'Archief'" id="Type">Zet terug</h3>
          <h3 v-else id="Type">Archiveer</h3>
        </div>

        <div class="overview" v-for="(document, i) in displayedDocuments">
          <div @click="goToInfoPage(document)" id="item">
            <img v-if="documentDaysFromExpiration(document, 35)" id="urgentieSymbool"
              src="../assets/Pictures/hogeUrgentie.png" alt="does not work" />
            <img v-else-if="documentDaysFromExpiration(document, 42)" id="urgentieSymbool"
              src="../assets/Pictures/middelUrgentie.png" alt="does not work" />
            <img v-else id="urgentieSymbool" src="../assets/Pictures/lageUrgentie.png" alt="does not work" />
            <div id="klantnaamTekst">{{ document.customerName }}</div>
            <div id="geldigVanTekst">{{ formatDate(document.date) }}</div>
            <div id="geldigTotTekst">{{ daysAway(document.date) }}</div>
            <div id="typeTekst">{{ document.type }}</div>
            <div id="checkboxArchive"><input type="checkbox" id="checkboxA" v-model="document.isChecked"
                @change="toggleCheckbox(document)"></div>
          </div>
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
import Pagination from '../views/Pagination.vue';
import Popup from '../views/Popup.vue';
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
      customers: [],
      searchField: "",
      dropBoxType: "0",
      overviewType: localStorage.getItem("overviewType")
    };
  },
  mounted() {
    this.filterDocuments();

    if (this.$route.query.activePopup && localStorage.getItem('popUpSucces') === 'true') {
      this.$refs.Popup.popUpError("Document is geupload!");
      localStorage.setItem('popUpSucces', 'false');
    }
  },
  methods: {
    goToInfoPage(doc) {
      setTimeout(() => {
        if (doc.isArchived == null) {
          this.$router.push("/infopage/document/" + doc.documentId);
        }
        else {
          this.filterDocuments();
        }
      }, 100);
    },
    toggleCheckbox(doc) {
      doc.isArchived = this.overviewType === 'Archief' ? false : true;

      axios.put("Document/IsArchived", doc, {
        headers: {
          Authorization: "Bearer " + localStorage.getItem("jwt")
        }
      })
        .then((res) => {
        }).catch((error) => {
          this.$refs.Popup.popUpError(error.response.data);
        });
    },
    handlePageChange(newPage) {
      this.pager.currentPage = newPage;
      this.filterDocuments();
    },
    filterDocuments() {
      console.log(this.overviewType)
      axios
        .get("Document/Filter", {
          headers: {
            Authorization: "Bearer " + localStorage.getItem("jwt"),
          },
          params: {
            searchfield: this.searchField,
            overviewType: this.overviewType,
            dropBoxType: this.dropBoxType,
            page: this.pager.currentPage,
            pageSize: this.pager.pageSize
          },
        })
        .then((res) => {
          this.documents = res.data.documents;
          this.pager = res.data.pager;
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
    toOrFromArchive(value){
      if (this.overviewType === 'Archief' && value < 0) {
        value = Math.abs(value);
      }
      else if(this.overviewType === 'Archief' && value > 0)
      {
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
      return this.documents.slice(0, this.pager.pageSize);
    },
  },
};
</script>


<style>
@import '../assets/Css/Overview.css';
@import '../assets/Css/Main.css';
</style>
