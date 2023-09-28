<template>
  <div class="header">
    <a href="/overzicht"><img id="logoHeader" src="@/assets/Pictures/Logo-4-rest-IT.png" alt="does not work" /></a>
    <div id="buttonsHeader">
      <router-link to="/overzicht">Overzicht</router-link>
      <router-link to="/uploaden">Document uploaden</router-link>
      <router-link to="/">Uitloggen</router-link>
    </div>
  </div>
  <div class="overviewContainer">
    <div class="info">
      <ul>
        <h1>Info</h1>
        <br>
        <div id="DocumentTitle">
          Document:
        </div>
        <div id="documentInfo">
          Documenttype: &nbsp;&nbsp; {{ this.document.type }}
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
          Klantemail: &nbsp;&nbsp;&nbsp;&nbsp; {{ this.customer.email }}
        </div>
      </ul>
    </div>
    <div class="foto">
      <img class="foto" :src="this.document.image" alt="image not shown" />
    </div>
  </div>
</template>

<script>
import axios from '../../axios-auth.js';
import moment from 'moment';

export default {
  name: "Quiz",
  props: {
    id: Number,
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
      axios.get("Document/" + this.id)
        .then((res) => {
          this.document = res.data.document;
          this.document.type = res.data.type
          console.log(this.document)

          this.getCustomerName(this.document.customerId);
        }).catch((error) => {
          alert(error.response.data);
        });
    },
    getCustomerName(id) {
      axios.get("Customer/" + id)
        .then((res) => {
          this.customer = res.data;
        }).catch((error) => {
          alert(error.response.data);
        });
    },
    formatDate(date) {
      return moment(date).format("DD-MM-YYYY");
    },
  }
}

</script>

<style scoped>
.overviewContainer {
  width: 85%;
  margin: auto;
  margin-top: 80px;
  display: flex;
}

.foto {
  max-height: 500px;
  max-width: 500px;
  margin-left: auto;
}

#documentInfo {
  font-size: 20px;
}

#DocumentTitle {
  font-size: 30px;
  font-weight: 500;
}

</style>