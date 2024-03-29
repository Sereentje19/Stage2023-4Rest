<template>
  <body>
    <Header ref="Header"></Header>
    <div class="overview-container">
      <div id="topside">
        <h1 id="h1-overview">Bruikleen</h1>


        <select v-model="dropdown" id="filter-dropdown" @change="getProducts">
          <option value="0">Selecteer type...</option>
          <option v-for="(type, index) in productTypes" :key="index" :value="type.id">
            {{ type.name }}
          </option>
        </select>
        <input id="searchfield-overview" v-model="searchField" type="search" placeholder="Zoek" @input="getProducts" />
        <button @click="toTrash()" id="button-archive">Prullenbak</button>
      </div>

      <div v-if="displayedProducts.length > 0">
        <div id="titles-overview-products">
          <h3></h3>
          <h3 id="urgentie">Type</h3>
          <h3 id="klantnaam">Gekocht op</h3>
          <h3 id="geldigVan">Gebruiken tot</h3>
          <h3 id="geldigTot">Serie nummer</h3>
          <h3 id="geldigTot">In gebruik</h3>
          <h3 id="geldigTot">Geschiedenis</h3>
          <h3 id="geldigTot">Verwijder</h3>
        </div>

        <div v-for="(product, i) in displayedProducts">
          <div @click="goToInfoPage(product)" id="item-products">
            <div></div>
            <div id="klantnaamTekst">{{ product.type.name }}</div>
            <div id="geldigVanTekst">{{ formatDate(product.purchaseDate) }}</div>
            <div id="geldigTotTekst">{{ formatDate(product.expirationDate) }}</div>
            <div id="typeTekst">{{ product.serialNumber }}</div>
            <div id="typeTekst" v-if="product.returnDate == null || product.returnDate == ''">Ja</div>
            <div id="typeTekst" v-else>Nee</div>
            <button id="button-history" @click="goToHistory(product)">
              <History />
            </button>
            <button id="button-history" @click="toggleCheckbox(product)">
              <Trash />
            </button>
          </div>
        </div>

        <div id="paging">
          <Pagination :currentPage="pager.currentPage" :totalPages="pager.totalPages" @page-changed="handlePageChange" />
        </div>
      </div>
      <div v-else>
        <br> Nog geen producten bekend.
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
import History from '@/components/icons/IconHistory.vue';
import Trash from '@/components/icons/IconTrash.vue';


export default {
  name: "Overview",
  components: {
    Pagination,
    PopUpMessage,
    Header,
    History,
    Trash
  },

  data() {
    return {
      product: [
        {
          purchaseDate: "",
          returnDate: "",
          type: {
            id: 0,
            name: ""
          },
          serialNumber: "",
          productId: 0,
          isDeleted: false,
          expirationDate: ""
        }
      ],
      productTypes: [],
      pager: {
        currentPage: 1,
        totalItems: 0,
        totalPages: 0,
        pageSize: 5,
      },
      searchField: "",
      dropdown: "0",
      toHistory: false
    };
  },
  mounted() {
    this.getProducts();
    this.getProductTypes();

    if (this.$route.query.activePopup && localStorage.getItem('popUpSucces') === 'true') {
      this.$refs.PopUpMessage.popUpError("Data is bijgewerkt.");
    }
  },
  methods: {
    goToInfoPage(pro) {
      setTimeout(() => {
        if (this.toHistory == false && pro.isDeleted == false) {
          this.$router.push("/info/bruikleen/" + pro.productId);
        }
      }, 100);
    },
    goToHistory(pro) {
      this.toHistory = true;
      this.$router.push("/geschiedenis/product/" + pro.productId);
    },
    toTrash() {
      this.$router.push("/overzicht/bruikleen/archief");
    },
    toggleCheckbox(pro) {
      pro.isDeleted = true;

      axios.put("product/delete", pro, {
        headers: {
          Authorization: "Bearer " + localStorage.getItem("jwt")
        }
      })
        .then((res) => {
          this.getProducts();
        }).catch((error) => {
          this.$refs.PopUpMessage.popUpError(error.response.data);
        });
    },
    handlePageChange(newPage) {
      this.pager.currentPage = newPage;
      this.getProducts();
    },
    getProducts() {
      axios.get("product", {
        headers: {
          Authorization: "Bearer " + localStorage.getItem("jwt")
        },
        params: {
          searchfield: this.searchField,
          dropdown: this.dropdown,
          page: this.pager.currentPage,
          pageSize: this.pager.pageSize
        },
      })
        .then((res) => {
          this.product = res.data.products;
          this.pager = res.data.pager;

          for (let i = 0; i < this.product.length; i++) {
            this.getReturnDate(this.product[i].productId, i);
          }

        }).catch((error) => {
          this.$refs.PopUpMessage.popUpError(error.response.data);
        });
    },
    getReturnDate(productId, index) {
      axios.get("loan-history/date/" + productId, {
        headers: {
          Authorization: "Bearer " + localStorage.getItem("jwt")
        }
      })
        .then((res) => {
          this.product[index].returnDate = res.data;
        }).catch((error) => {
          this.$refs.PopUpMessage.popUpError(error.response.data);
        });
    },
    getProductTypes() {
      axios
        .get("product/types", {
          headers: {
            Authorization: "Bearer " + localStorage.getItem("jwt"),
          }
        })
        .then((res) => {
          this.productTypes = res.data;
        })
        .catch((error) => {
          this.$refs.PopUpMessage.popUpError(error.response.data);
        });
    },
    formatDate(date) {
      return moment(date).format("DD-MM-YYYY");
    },
  },
  computed: {
    displayedProducts() {
      return this.product.slice(0, this.pager.pageSize);
    },
  },
};
</script>


<style>
@import '/assets/css/Overview.css';
@import '/assets/css/Main.css';
</style>
