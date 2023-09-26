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
        <h1 class="title">{{ this.document.type }}</h1>
        <br /><br />
        <h2 class="text">
          {{ formatDate(this.document.date) }}
          <br />
          {{ "Over " + daysAway(this.document.date) + " dagen vervalt het document!" }}
        </h2>
        <br /><br />
        <h2 class="text">
          {{this.customer.name}}
          <br />
          {{this.customer.email}}
        </h2>
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
    daysAway(date) {
      const documentDate = new Date(date);
      const currentDate = new Date();
      const ageInDays = Math.floor((currentDate - documentDate) / (1000 * 60 * 60 * 24));
      return Math.abs(ageInDays)
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

/* .text {
  position: relative;
  left: 400px;
  top: 80px;
  text-align: left;
  font-size: 30px;
}

.title {
  position: relative;
  top: 80px;
  left: 400px;
  text-align: left;
  font-size: 50px;

}

.foto {
  position: fixed;
  right: 300px;
  top: 200px;
  width: 600px;
  height: 700px;
} */
</style>