<template>
  <body>
    <Header ref="Header"></Header>
    <div class="overview-container">
      <div id="topside">
        <h1 id="h1-overview">Bruikleen</h1>


        <select v-model="dropdown" id="filter-dropdown" @change="getProducts">
          <option value="0">Selecteer type...</option>
          <option value="1">Laptop</option>
          <option value="2">Monitor</option>
          <option value="3">Stoel</option>
        </select>
        <input id="searchfield-overview" v-model="searchField" type="search" placeholder="Zoek" @input="getProducts" />
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
        </div>

        <div v-for="(product, i) in displayedProducts">
          <div @click="goToInfoPage(product)" id="item-products">
            <div></div>
            <div id="klantnaamTekst">{{ product.type }}</div>
            <div id="geldigVanTekst">{{ formatDate(product.purchaseDate) }}</div>
            <div id="geldigTotTekst">{{ formatDate(product.expirationDate) }}</div>
            <div id="typeTekst">{{ product.serialNumber }}</div>
            <div id="typeTekst" v-if="product.returnDate == null || product.returnDate == ''">Ja</div>
            <div id="typeTekst" v-else>Nee</div>
            <button id="button-history" @click="goToHistory(product)"><svg xmlns="http://www.w3.org/2000/svg"
                width="20" height="20" fill="currentColor" class="bi bi-hourglass" viewBox="0 0 16 16">
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

      <PopUpMessage ref="PopUpMessage" />
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
      product: [
        {
          purchaseDate: "",
          returnDate: "",
          type: "",
          serialNumber: "",
          productId: 0,
          expirationDate: ""
        }
      ],
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

    if (this.$route.query.activePopup && localStorage.getItem('popUpSucces') === 'true') {
      this.$refs.PopUpMessage.popUpError("Data is bijgewerkt.");
    }
  },
  methods: {
    goToInfoPage(pro) {
      setTimeout(() => {
        if (this.toHistory == false) {
          this.$router.push("/info/bruikleen/" + pro.productId);
        }
      }, 100);
    },
    goToHistory(pro) {
      this.toHistory = true;
      this.$router.push("/geschiedenis/product/" + pro.productId);
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
          console.log(res.data)
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
          console.log(res.data)
          this.product[index].returnDate = res.data;
        }).catch((error) => {
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
@import '../assets/Css/Overview.css';
@import '../assets/Css/Main.css';
</style>
