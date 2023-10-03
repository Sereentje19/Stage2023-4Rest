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

<style scoped>
#myUL {
  list-style-type: none;
  padding: 0;
  z-index: 10;
  position: absolute;
  margin-top: 49px;
  margin-left: 128px;
  width: 350px;
}

#myUL li #searchList {
  border: 1px solid rgb(155, 155, 155);
  border-top: none;
  background-color: #f6f6f6;
  padding: 10px;
  font-size: 18px;
  color: black;
  margin: auto;
  cursor: pointer;
}

.leftSide {
  margin-right: auto;
}

.rightSide {
  display: flex;
  flex-direction: column;
  margin-top: 117px;
}

.uploadContainer {
  width: 85%;
  margin: auto;
  margin-top: 50px;
  display: flex;
  margin-bottom: 50px;
}

.dropArea.active {
  border: 2px dashed #007bff;
  background-color: #f5f5f5;
}

.dropArea {
  position: relative;
  height: 400px;
  background-color: white;
  color: #717171;
  text-align: center;
  display: flex;
  align-items: center;
  justify-content: center;
  overflow: hidden;
  max-width: 400px;
  max-height: 380px;
  transition: background-color 0.3s, border 0.3s;
  background-repeat: no-repeat;
  background-size: cover;
  border-radius: 5px;
  border-color: black;
  border-width: 2px;
  border-style: solid;

}

#p {
  width: 100px;
  height: 100px;
  background-size: cover;
  display: block;
  background-image: url(../assets/Pictures/move.png);
}

body {
  background-color: #d9d9d9;
  font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
}

a {
  color: white;
  text-decoration: none;
}

#folderImage {
  width: 50px;
  padding: 3px;
  margin-left: auto;
}

#selectDocument {
  margin-top: auto;
  padding: 12px;
}

.overlay {
  position: relative;
  top: 30px;
  background: #22421f;
  color: white;
  font-size: 25px;
  width: 400px;
  border-radius: 5px;
  display: flex;
  cursor: pointer;
}

.file {
  display: none;
}

.gegevens {
  display: flex;
  flex-direction: column;
  position: relative;
  margin-top: 2px;
}


#searchOutput {
  background-color: white;
  display: flex;
  flex-direction: column;
  width: 350px;
  margin-left: auto;
  z-index: 20;
}

#searchOutputButtons {
  background-color: white;
}

.Naam,
.Email,
.Date,
.Type {
  right: 350px;
  background-color: #f4f4f4;
  font-size: 20px;
  padding: 12px;
  width: 450px;
  margin-bottom: 15px;
  border-radius: 5px;
  border: black solid 1px;
}

.Type {
  width: 477px;
}

.Zoek {
  margin-left: auto;
  width: 350px;
  background-color: #f4f4f4;
  font-size: 20px;
  padding: 12px;
  border: black solid 1px;
  border-radius: 5px;
}

.Naam {
  margin-top: 90px;
}


.verstuur {
  margin-left: auto;
  font-size: 20px;
  background-color: #22421f;
  color: white;
  padding: 12px;
  font-size: 25px;
  border-radius: 5px;
  border: none;
}
</style>
