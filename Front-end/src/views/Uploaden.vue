<template>
  <div>
    <Header></Header>
    <div class="uploadContainer">
      <div class="leftSide">
        <h1 id="h1">Document uploaden</h1>
        <div id="drop" ref="dropArea" class="dropArea" @dragover.prevent="handleDragOver" @dragleave="handleDragLeave"
          @drop.prevent="handleDrop">
          <p id="p" ref="pElement"></p>
          <input type="file" class="file" @change="handleFileChange" style="display: none" />
        </div>

        <label class="overlay">
          <div id="selectDocument"> Selecteer document</div>
          <img id="folderImage" src="../assets/Pictures/folder.png" alt="">
          <input type="file" class="file" @change="handleFileChange" />
        </label>
      </div>

      <div class="rightSide">
        <ul>
          <form class="gegevens" action="/action_page.php">
            <input @input="filterCustomer" @focus="isFocused = true" @blur="onBlur" v-model="searchField" type="search"
              class="Zoek" placeholder="Zoek klant" name="Naam" />

            <ul id="myUL" v-show="isFocused && filteredCustomers.length > 0">
              <li v-for="customer in filteredCustomers" :key="customer.id">
                <div id="searchList" @click="fillCustomer(customer)"> {{ customer.name }}</div>
              </li>
            </ul>

            <input v-model="this.customer.Name" type="text" class="Naam" placeholder="Naam klant" name="Zoek" />
            <input v-model="this.customer.Email" type="text" class="Email" placeholder="Email klant" name="Email" />
            <select v-model="this.document.Type" class="Type" name="Type">
              <option value="0">Selecteer type...</option>
              <option value="1">Vog</option>
              <option value="2">Contract</option>
              <option value="3">Paspoort</option>
              <option value="4">id kaart</option>
              <option value="5">Diploma</option>
              <option value="6">Certificaat</option>
              <option value="7">Lease auto</option>
            </select>
            <input v-model="this.document.Date" :placeholder="this.document.Date" type="date" class="Date" name="Date" />
          </form>
        </ul>

        <button @click="this.CreateDocument()" class="verstuur" type="button">
          Verstuur document
        </button>
      </div>
    </div>

    <Popup ref="Popup" />

  </div>
</template>

<script>
import axios from '../../axios-auth.js'
import Popup from '../views/popUp.vue';
import Header from '../views/Header.vue';

export default {
  components: {
    Popup,
    Header
  },
  data() {
    return {
      isFocused: false,
      selectedFile: null,
      dropAreaActive: false,
      displayImage: false,
      searchField: "",
      uploadedFileName: '',
      customer: {
        CustomerId: 0,
        Name: '',
        Email: '',
      },
      document: {
        DocumentId: 0,
        Type: 0,
        Date: new Date().toISOString().split('T')[0],
        CustomerId: 0,
      },
      filteredCustomers: []
    };
  },
  methods: {
    CreateDocument() {
      if (this.selectedFile == null) {
        this.$refs.Popup.popUpError("Selecteer een bestand!");
      }
      else if (this.document.Type == 0) {
        this.$refs.Popup.popUpError("Selecteer een type!");
      }
      else {
        axios.post("Customer", this.customer, {
          headers: {
            Authorization: "Bearer " + localStorage.getItem("jwt")
          }
        })
          .then((res) => {
            let customerId = res.data;
            console.log(customerId)
            this.postDocument(customerId);

          }).catch((error) => {
            this.$refs.Popup.popUpError(error.response.data);
          });
      }
    },
    postDocument(customerId) {
      console.log(this.document)
      let formData = this.CreateFromData(customerId);
      console.log(this.selectedFile)

      axios.post("Document", formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
          Authorization: "Bearer " + localStorage.getItem("jwt")
        }
      })
        .then((res) => {
          this.$router.push({ path: '/Overzicht', query: { activePopup: true } });
        }).catch((error) => {
          this.$refs.Popup.popUpError(error.response.data);
        });
    },
    CreateFromData(customerId) {
      let formData = new FormData();
      formData.append('file', this.selectedFile);
      formData.append('document.Type', this.document.Type);
      formData.append('document.Date', this.document.Date);
      formData.append('document.CustomerId', customerId);
      return formData;
    },
    filterCustomer() {
      if (this.searchField != "") {
        axios.get("Customer/Filter", {
          headers: {
            Authorization: "Bearer " + localStorage.getItem("jwt")
          },
          params: {
            searchField: this.searchField
          }
        })
          .then((res) => {
            this.filteredCustomers = res.data;
            console.log(this.filteredCustomers)
          }).catch((error) => { });
      }
    },
    fillCustomer(cus) {
      this.customer.Email = cus.email;
      this.customer.Name = cus.name;
      this.searchField = "";
    },
    handleFileChange(event) {
      this.selectedFile = event.target.files[0]

      const reader = new FileReader();
      reader.onload = () => {
        this.fileContents = reader.result;
      };
      reader.readAsBinaryString(this.selectedFile)
    },
    onBlur() {
      setTimeout(() => {
        this.isFocused = false;
      }, 100);
    },
    handleDragOver(e) {
      e.preventDefault();
      this.dropAreaActive = true;
    },
    handleDragLeave(e) {
      e.preventDefault();
      this.dropAreaActive = false;
    },
    handleDrop(e) {
      e.preventDefault();
      this.dropAreaActive = false;
      const files = e.dataTransfer.files;
      this.processFile(files[0]);
    },
  },
};
</script>

<style>
@import '../assets/Css/Uploaden.css';
@import '../assets/Css/Main.css';
</style>
