<template>
  <Header></Header>
  <div class="info-document-container">
    <div id="info-edit-delete">
      <h1>Info</h1>

      <PopupChoice ref="PopupChoice" @delete="deleteDocument" />
      <div id="leftside-documents">
        <div id="loan-title">
          Document
        </div>
        <div id="loan-info">
          <div id="loan-info-leftside">
            Type: <br>
            Verval datum: <br>
          </div>
          <div>
            {{ this.document.type.name }} <br>
            {{ formatDate(this.document.date) }} <br>
          </div>
        </div>
        <div id="box">
          <button @click="toEdit()" id="edit-button">Edit</button>
          <button v-if="this.type == 'archief'" @click="toPopUpDelete()" id="delete-button">Delete</button>
        </div>
      </div>

      <div id="leftside-documents">
        <div id="loan-title">
          Medewerker
        </div>
        <div id="loan-info">
          <div id="loan-info-leftside">
            Naam: <br>
            Email: <br>
          </div>
          <div>
            {{ this.employee.name }} <br>
            {{ this.employee.email }} <br>
          </div>
        </div>
      </div>
    </div>

    <div class="foto">
      <div v-if="this.document.fileType == null"> <br><br><br> Er is geen afbeelding aanwezig bij dit document</div>
      <embed v-else-if="this.document.fileType === 'application/pdf'"
        :src="'data:' + this.document.fileType + ';base64,' + this.document.file" frameborder="0" width="100%"
        height="500px">
      <img v-else-if="this.document.fileType != 'application/pdf'" class="foto"
        :src="'data:' + this.document.fileType + ';base64,' + this.document.file"
        alt="Afbeelding kan niet worden laten zien." />
    </div>
  </div>

  <PopUpMessage ref="PopUpMessage" />
</template>

<script>
import axios from '../../axios-auth.js';
import moment from 'moment';
import PopUpMessage from '../components/notifications/PopUpMessage.vue';
import PopupChoice from '../components/notifications/PopUpChoice.vue';
import Header from '../components/layout/Header.vue';

export default {
  name: "Info",
  props: {
    id: Number,
    type: String
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
        employeeId: 0,
        date: "",
        employeeName: "",
        type: {
          name: ""
        },
        file: "",
        fileType: ""
      },
      employee: [],
    }
  },
  mounted() {
    this.getDocument();

    if (this.$route.query.activePopup && localStorage.getItem('popUpSucces') === 'true') {
      this.$refs.PopUpMessage.popUpError("Data is bijgewerkt.");
    }
  },
  methods: {
    getDocument() {
      axios.get("document/" + this.id, {
        headers: {
          Authorization: "Bearer " + localStorage.getItem("jwt")
        }
      })
        .then((res) => {
          this.employee = res.data.employee;
          this.document = res.data;
        }).catch((error) => {
          this.$refs.PopUpMessage.popUpError(error.response.data);
        });
    },
    deleteDocument() {
      axios.delete("document/" + this.id, {
        headers: {
          Authorization: "Bearer " + localStorage.getItem("jwt")
        },
      })
        .then((res) => {
          localStorage.setItem('popUpSucces', 'true');
          this.$router.push({ path: '/overzicht/documenten/archief', query: { activePopup: true } });
        }).catch((error) => {
          this.$refs.PopUpMessage.popUpError(error.response.data);
        });
    },
    toPopUpDelete() {
      this.emitter.emit('isPopUpTrue', { 'eventContent': true })
      this.emitter.emit('text', { 'eventContent': "Weet je zeker dat je " + this.document.type.name.toLowerCase() + " van medewerker " + this.employee.name + " wilt verwijderen?" })
    },
    toEdit() {
      this.$router.push("/edit/document/" + this.type + "/" + this.id);
    },
    formatDate(date) {
      return moment(date).format("DD-MM-YYYY");
    },
  }
}

</script>

<style>
@import '../assets/css/Info.css';
@import '../assets/css/Main.css';
</style>
