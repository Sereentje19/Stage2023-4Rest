<template>
  <Header></Header>
  <div class="InfoContainer">
    <div class="info">
      <ul>
        <h1>Info</h1>
        <br>
        <div id="DocumentTitle">
          Document:
        </div>
        <div id="documentInfo">
          Documenttype: &nbsp; {{ this.document.type }}
          <br>
          VervalDatum: &nbsp;&nbsp;&nbsp;&nbsp; {{ formatDate(this.document.date) }}
        </div>
        <br><br><br>
        <div id="DocumentTitle">
          klant:
        </div>
        <div id="documentInfo">
          Klantnaam: &nbsp;&nbsp; {{ this.customer.name }}
          <br>
          Klantemail: &nbsp;&nbsp; {{ this.customer.email }}
        </div>
      </ul>
    </div>
    <div class="foto">
      <img class="foto" :src="'data:image/jpeg;base64,' + this.document.image" alt="image not shown" />
    </div>
  </div>

  <Popup ref="Popup" />
</template>

<script>
import axios from '../../axios-auth.js';
import moment from 'moment';
import Popup from '../views/popUp.vue';
import Header from '../views/Header.vue';

export default {
  name: "Quiz",
  props: {
    id: Number,
  },
  components: {
    Popup,
    Header
  },
  data() {
    return {
      document:
      {
        documentId: 0,
        customerId: 0,
        date: "",
        customerName: "",
        type: "",
        image: "",
      },
      customer: [],
    }
  },
  mounted() {
    this.getDocuments();
  },
  methods: {
    getDocuments() {
      axios.get("Document/" + this.id, {
        headers: {
          Authorization: "Bearer " + localStorage.getItem("jwt")
        }
      })
        .then((res) => {
          this.document = res.data.document;
          this.document.type = res.data.type

          this.getCustomerName(this.document.customerId);
        }).catch((error) => {
          this.$refs.Popup.popUpError(error.response.data);
        });
    },
    getCustomerName(id) {
      axios.get("Customer/" + id, {
        headers: {
          Authorization: "Bearer " + localStorage.getItem("jwt")
        }
      })
        .then((res) => {
          this.customer = res.data;
        }).catch((error) => {
          this.$refs.Popup.popUpError(error.response.data);
        });
    },
    formatDate(date) {
      return moment(date).format("DD-MM-YYYY");
    },
  }
}

</script>

<style>
@import '../assets/Css/Info.css';
@import '../assets/Css/Main.css';
</style>
