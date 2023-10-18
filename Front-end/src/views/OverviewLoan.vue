<template>
  <body class="overviewBody">
    <Header ref="Header"></Header>
    <div class="overviewContainer">
      <div id="h1AndButton">
        <h1 id="h1Overzicht">Bruikleen</h1>


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
        <div id="titlesOverviewLoan">
          <h3></h3>
          <h3 id="urgentie">Type</h3>
          <h3 id="klantnaam">Gekocht op</h3>
          <h3 id="geldigVan">Geldig tot</h3>
          <h3 id="geldigTot">Serie nummer</h3>
          <h3 id="geldigTot">Geschiedenis</h3>
        </div>

        <div class="overviewLoan" v-for="(document, i) in displayedDocuments">
          <div @click="goToInfoPage(document)" id="itemLoan">
            <div></div>
            <div id="klantnaamTekst">{{ document.customerName }}</div>
            <div id="geldigVanTekst">{{ formatDate(document.date) }}</div>
            <div id="geldigTotTekst">{{ daysAway(document.date) }}</div>
            <div id="typeTekst">{{ document.type }}</div>
            <button id="buttonGeschiedenis" @change="toggleCheckbox(document)" ><svg xmlns="http://www.w3.org/2000/svg" width="20" height="20"
                fill="currentColor" class="bi bi-hourglass" viewBox="0 0 16 16">
                <path
                  d="M2 1.5a.5.5 0 0 1 .5-.5h11a.5.5 0 0 1 0 1h-1v1a4.5 4.5 0 0 1-2.557 4.06c-.29.139-.443.377-.443.59v.7c0 .213.154.451.443.59A4.5 4.5 0 0 1 12.5 13v1h1a.5.5 0 0 1 0 1h-11a.5.5 0 1 1 0-1h1v-1a4.5 4.5 0 0 1 2.557-4.06c.29-.139.443-.377.443-.59v-.7c0-.213-.154-.451-.443-.59A4.5 4.5 0 0 1 3.5 3V2h-1a.5.5 0 0 1-.5-.5zm2.5.5v1a3.5 3.5 0 0 0 1.989 3.158c.533.256 1.011.791 1.011 1.491v.702c0 .7-.478 1.235-1.011 1.491A3.5 3.5 0 0 0 4.5 13v1h7v-1a3.5 3.5 0 0 0-1.989-3.158C8.978 9.586 8.5 9.052 8.5 8.351v-.702c0-.7.478-1.235 1.011-1.491A3.5 3.5 0 0 0 11.5 3V2h-7z" />
              </svg></button>
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
    toOrFromArchive(value) {
      if (this.overviewType === 'Archief' && value < 0) {
        value = Math.abs(value);
      }
      else if (this.overviewType === 'Archief' && value > 0) {
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
@import '../assets/Css/OverviewLoan.css';
@import '../assets/Css/Main.css';
</style>
