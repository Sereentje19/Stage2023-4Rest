<template>
  <Header></Header>
  <div class="InfoContainer">
    <div class="info">
      <ul>
        <h1>Info</h1>
        <br>
        <div id="DocumentTitle">
          Document
        </div>
        <div id="documentInfo">
          Type: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; {{ this.document.type }}
          <br>
          Verval datum: &nbsp;&nbsp; {{ formatDate(this.document.date) }}
        </div>
        <button @click="toEdit('document')" id="EditButton">Edit</button>
        <br><br><br>
        <div id="DocumentTitle">
          Medewerker
        </div>
        <div id="documentInfo">
          Naam: &nbsp; {{ this.customer.name }}
          <br>
          Email: &nbsp;&nbsp; {{ this.customer.email }}
        </div>
        <button @click="toEdit('klant')" id="EditButton">Edit</button>
      </ul>
    </div>
    <div class="foto">

      <div v-if="this.document.fileType == null"> <br><br><br> Er is geen afbeelding aanwezig bij dit document</div>
      <embed v-else-if="this.document.fileType === 'application/pdf'"
        :src="'data:' + this.document.fileType + ';base64,' + this.document.file" frameborder="0" width="100%"
        height="500px">
      <img v-else-if="this.document.fileType != 'application/pdf'" class="foto"
        :src="'data:' + this.document.fileType + ';base64,' + this.document.file" alt="image not shown" />




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
  name: "Info",
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
        file: "",
        fileType: ""
      },
      customer: [],
    }
  },
  mounted() {
    this.getDocument();
  },
  methods: {
    getDocument() {
            axios.get("Document/" + this.id, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                    console.log(res.data)
                    this.customer = res.data.document.customer;
                    this.document = res.data.document;
                    this.document.type = res.data.type
                }).catch((error) => {
                    this.$refs.Popup.popUpError(error.response.data);
                });
        },
    toEdit(route) {
      this.$router.push("/edit/" + route + "/" + this.id);
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
