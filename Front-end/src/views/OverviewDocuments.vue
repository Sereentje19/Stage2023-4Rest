<template>
  <body>
    <Header ref="Header"></Header>
    <div class="overview-container">
      <div id="topside">
        <h1 id="h1-overview">Documenten</h1>


        <select v-model="dropdown" id="filter-dropdown" @change="filterDocuments">
          <option value="0">Selecteer document...</option>
          <option v-for="(type, index) in documentTypes" :key="index" :value="type.id">
            {{ type.name }}
          </option>
        </select>
        <input id="searchfield-overview" v-model="searchField" type="search" placeholder="Zoek"
          @input="filterDocuments" />

        <button @click="toArchive()" id="button-archive">Archief</button>
      </div>

      <div v-if="displayedDocuments.length > 0">
        <div id="titles-overview-documents">
          <h3 id="urgentie">Urgentie</h3>
          <h3 id="klantnaam">Klantnaam</h3>
          <h3 id="geldigVan">Geldig tot</h3>
          <h3 id="geldigTot">Verloopt over</h3>
          <h3 id="Type">Type document</h3>
          <h3 id="Type">Archiveer</h3>
        </div>

        <div v-for="(document, i) in displayedDocuments">
          <div @click="goToInfoPage(document)" id="item-documents">
            <img v-if="documentDaysFromExpiration(document, 35)" id="urgentie-symbool"
              src="@/assets/pictures/hogeUrgentie.png" alt="does not work" />
            <img v-else-if="documentDaysFromExpiration(document, 42)" id="urgentie-symbool"
              src="@/assets/pictures/middelUrgentie.png" alt="does not work" />
            <img v-else id="urgentie-symbool" src="@/assets/pictures/lageUrgentie.png" alt="does not work" />
            <div id="klantnaamTekst">{{ document.employeeName }}</div>
            <div id="geldigVanTekst">{{ formatDate(document.date) }}</div>
            <div id="geldigTotTekst">{{ daysAway(document.date) }}</div>
            <div id="typeTekst">{{ document.type.name }}</div>
            <div id="checkboxArchive">
              <button id="button-history" @click="toggleCheckbox(document)">
                <Archive />
              </button>
            </div>
          </div>
        </div>

        <div id="paging">
          <div>
            <Pagination :currentPage="pager.currentPage" :totalPages="pager.totalPages"
              @page-changed="handlePageChange" />
          </div>
          <a href="/overzicht/documenten/lang-geldig" id="long-valid-link">Documenten bekijken die langer dan 6 weken
            geldig zijn
            <ArrowRight />
          </a>

        </div>
      </div>
      <div v-else>
        <br>
        Er zijn momenteel geen documenten bekend met een datum binnen het tijdsbestek van nu tot de komende zes weken.
        <br>
        <br>
        <a href="/overzicht/documenten/lang-geldig" id="doc-link">Documenten bekijken die langer dan 6 weken
          geldig zijn
          <ArrowRight />
        </a>
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
import ArrowRight from "@/components/icons/iconArrowRight.vue";
import Archive from '@/components/icons/IconArchive.vue';


export default {
  components: {
    Pagination,
    PopUpMessage,
    Header,
    ArrowRight,
    Archive
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
          isArchived: false
        }
      ],
      documentTypes: [],
      pager: {
        currentPage: 1,
        totalItems: 0,
        totalPages: 0,
        pageSize: 5,
      },
      employees: [],
      searchField: "",
      dropdown: "0",
    };
  },
  mounted() {
    this.filterDocuments();
    this.getDocumentTypes();

    if (this.$route.query.activePopup && localStorage.getItem('popUpSucces') === 'true') {
      this.$refs.PopUpMessage.popUpError("Data is bijgewerkt.");
    }
  },
  methods: {
    goToInfoPage(doc) {
      setTimeout(() => {
        if (doc.isArchived == false) {
          this.$router.push("/info/document/huidig/" + doc.documentId);
        }
        else {
          this.filterDocuments();
        }
      }, 100);
    },
    toArchive() {
      this.$router.push("/overzicht/documenten/archief");
    },
    toggleCheckbox(doc) {
      doc.isArchived = true;

      axios.put("document/archive", doc, {
        headers: {
          Authorization: "Bearer " + localStorage.getItem("jwt")
        }
      })
        .then((res) => {
        }).catch((error) => {
          this.$refs.PopUpMessage.popUpError(error.response.data);
        });
    },
    handlePageChange(newPage) {
      this.pager.currentPage = newPage;
      this.filterDocuments();
    },
    filterDocuments() {
      axios
        .get("document", {
          headers: {
            Authorization: "Bearer " + localStorage.getItem("jwt"),
          },
          params: {
            searchfield: this.searchField,
            dropdown: this.dropdown,
            page: this.pager.currentPage,
            pageSize: this.pager.pageSize
          },
        })
        .then((res) => {
          this.documents = res.data.documents;
          this.pager = res.data.pager;
        })
        .catch((error) => {
          this.$refs.PopUpMessage.popUpError(error.response.data);
        });
    },
    getDocumentTypes() {
      axios
        .get("document/types", {
          headers: {
            Authorization: "Bearer " + localStorage.getItem("jwt"),
          }
        })
        .then((res) => {
          this.documentTypes = res.data;
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

      let unit, value;

      if (ageInDays >= week * 2) {
        unit = "weken";
        value = Math.floor(ageInDays / week);
      } else {
        unit = "dagen";
        value = ageInDays;
      }

      return `${value} ${unit}`;
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


<style>@import '../assets/css/Overview.css';
@import '../assets/css/Main.css';</style>
