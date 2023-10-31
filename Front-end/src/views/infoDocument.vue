<template>
  <Header></Header>
  <div class="info-container">
    <h1>Info</h1>

    <PopupChoice ref="PopupChoice" @delete="deleteDocument"/>

    <div id="leftside">
      <div id="loan-title">
        Document
      </div>
      <div id="loan-info">
        <div id="loan-info-leftside">
          Type: <br>
          Verval datum: <br>
        </div>
        <div>
          {{ this.document.type }} <br>
          {{ formatDate(this.document.date) }} <br>
        </div>
      </div>
      <div id="box">
        <button @click="toEdit()" id="edit-button">Edit</button>
        <button @click="toPopUpDelete()" id="delete-button">Delete</button>
      </div>
    </div>

    <div id="leftside">
      <div id="loan-title">
        Medewerker
      </div>
      <div id="loan-info">
        <div id="loan-info-leftside">
          Naam: <br>
          Email: <br>
        </div>
        <div>
          {{ this.customer.name }} <br>
          {{ this.customer.email }} <br>
        </div>
      </div>
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

  <PopUpMessage ref="Popup" />
</template>

<script>
import axios from '../../axios-auth.js';
import moment from 'moment';
import PopUpMessage from '../views/PopUpMessage.vue';
import PopupChoice from '../views/PopUpChoice.vue';
import Header from '../views/Header.vue';

export default {
  name: "Info",
  props: {
    id: Number,
  },
  components: {
    PopUpMessage,
    Header,
    PopupChoice
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

    if (this.$route.query.activePopup && localStorage.getItem('popUpSucces') === 'true') {
      this.$refs.Popup.popUpError("Data is bijgewerkt.");
    }
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
    deleteDocument() {
      axios.delete("Document/" + this.id, {
        headers: {
          Authorization: "Bearer " + localStorage.getItem("jwt")
        },
      })
        .then((res) => {
          localStorage.setItem('popUpSucces', 'true');
          this.$router.push({ path: '/overzicht/document', query: { activePopup: true } });
        }).catch((error) => {
          this.$refs.Popup.popUpError(error.response.data);
        });
    },
    toPopUpDelete() {
      this.emitter.emit('isPopUpTrue', { 'eventContent': true })
      this.emitter.emit('text', { 'eventContent': "Weet je zeker dat je " + this.document.type.toLowerCase() + " van medewerker " + this.customer.name + " wilt verwijderen?" })
    },
    toEdit() {
      this.$router.push("/edit/document/" + this.id);
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
