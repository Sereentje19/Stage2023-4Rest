<template>
  <div>
    <Header></Header>
    <div class="upload-container">
      <div class="leftside">
        <h1 id="h1">Document uploaden</h1>
        <div id="drop" ref="dropArea" class="drop-area" @dragover.prevent="handleDrag(true)"
          @dragleave="handleDrag(false)" @drop.prevent="handleDrop">
          <p id="p" ref="pElement"></p>
          <input type="file" class="file" @change="handleFileChange" style="display: none" />
        </div>

        <div v-if="this.selectedFile != null">Geselecteerde bestand: <b>{{ this.selectedFile.name }}</b></div>
        <div v-else>Nog geen bestand geselecteerd</div>


        <label class="overlay">
          <div id="select-document"> Selecteer document</div>
          <img id="folder-image" src="../assets/pictures/folder.png">
          <input type="file" class="file" accept=".jpg, .jpeg, .png, .gif, .pdf" @change="handleFileChange" />
        </label>
      </div>

      <div class="rightside">
        <ul>
          <form class="gegevens">
            <input @input="filterEmployee" @focus="isFocused = true" @blur="onBlur" v-model="searchField" type="search"
              class="Zoek" placeholder="Zoek klant" name="Naam" />

            <ul id="ul" v-show="isFocused && filteredEmployees.length > 0">
              <li v-for="employee in filteredEmployees" :key="employee.id">
                <div id="searchList" @click="fillEmployee(employee)"> {{ employee.name }}</div>
              </li>
            </ul>

            <input v-model="this.employee.Name" type="text" class="name" placeholder="Naam" name="Zoek" />
            <input v-model="this.employee.Email" type="email" class="email" placeholder="Email" name="email" />
            <select v-model="this.document.Type" class="type" name="Type">
              <option value="0">Selecteer document...</option>
              <option v-for="(type, index) in documentTypes" :key="index" :value="index + 1">
                {{ type }}
              </option>
            </select>
            <input v-model="this.document.Date" :placeholder="this.document.Date" type="date" class="date" name="Date" />
          </form>
        </ul>

        <button @click="this.postDocument()" class="verstuur">
          Verstuur document
        </button>
      </div>
    </div>

    <PopUpMessage ref="PopUpMessage" />

  </div>
</template>

<script>
import axios from '../../axios-auth.js'
import PopUpMessage from '../components/notifications/PopUpMessage.vue';
import Header from '../components/layout/Header.vue';

export default {
  components: {
    PopUpMessage,
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
      employee: {
        employeeId: 0,
        Name: '',
        Email: '',
      },
      documentTypes: [],
      document: {
        DocumentId: 0,
        Type: 0,
        Date: new Date().toISOString().split('T')[0],
        employeeId: 0,
        fileType: ""
      },
      filteredEmployees: []
    };
  },
  mounted() {
    this.getDocumentTypes();
  },
  methods: {
    postDocument() {
      let formData = this.CreateFromData();

      axios.post("document", formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
          Authorization: "Bearer " + localStorage.getItem("jwt")
        }
      })
        .then((res) => {
          localStorage.setItem('popUpSucces', 'true');
          this.$router.push({ path: '/Overzicht/documenten', query: { activePopup: true } });
        }).catch((error) => {
          this.$refs.PopUpMessage.popUpError(error.response.data);
        });
    },
    CreateFromData() {
      let formData = new FormData();

      if (this.selectedFile != null) {
        formData.append('file', this.selectedFile);
        formData.append('document.FileType', this.selectedFile.type);
      }

      formData.append('document.Type', this.document.Type);
      formData.append('document.Date', this.document.Date);
      formData.append('document.Employee.Email', this.employee.Email);
      formData.append('document.Employee.Name', this.employee.Name);

      return formData;
    },
    filterEmployee() {
      if (this.searchField != "") {
        axios.get("employee/filter", {
          headers: {
            Authorization: "Bearer " + localStorage.getItem("jwt")
          },
          params: {
            searchField: this.searchField
          }
        })
          .then((res) => {
            this.filteredEmployees = res.data;
            console.log(this.filteredEmployees)
          }).catch((error) => {
            this.$refs.PopUpMessage.popUpError(error.response.data);
          });
      }
    },
    getDocumentTypes() {
      axios
        .get("document/types", {
          headers: {
            Authorization: "Bearer " + localStorage.getItem("jwt"),
          }
        })
        .then((res) => {
          console.log(res.data)
          this.documentTypes = res.data;
        })
        .catch((error) => {
          this.$refs.PopUpMessage.popUpError(error.response.data);
        });
    },
    fillEmployee(cus) {
      this.employee.Email = cus.email;
      this.employee.Name = cus.name;
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
      }, 200);
    },
    handleDrag(e, bool) {
      e.preventDefault();
      this.dropAreaActive = bool;
    },
    handleDrop(e) {
      e.preventDefault();
      this.dropAreaActive = false;
      const files = e.dataTransfer.files;
      this.processFile(files[0]);
    },
    processFile(file) {
      if (file) {
        this.selectedFile = file;
        const reader = new FileReader();
        reader.readAsDataURL(file);
      }
    },
  },
};
</script>

<style>
@import '../assets/css/Uploaden.css';
@import '../assets/css/Main.css';
</style>
