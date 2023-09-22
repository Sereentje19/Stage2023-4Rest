<template>
  <div>
    <div class="header">
      <a href="/overzicht"><img id="logoHeader" src="@/assets/Pictures/Logo-4-rest-IT.png" alt="does not work" /></a>
      <div id="buttonsHeader">
        <router-link to="/overzicht">Overzicht</router-link>
        <router-link to="/uploaden">Document uploaden</router-link>
        <router-link to="/">Uitloggen</router-link>
      </div>
    </div>

    <div class="uploadContainer">
      <div class="leftSide">
        <h1 id="h1">Document uploaden</h1>
        <div id="drop" ref="dropArea" class="dropArea" @dragover.prevent="handleDragOver" @dragleave="handleDragLeave"
          @drop.prevent="handleDrop">
          <p id="p" ref="pElement"></p>
          <input type="file" class="file" @change="handleFileChange" style="display: none" />
        </div>

        <label class="overlay">
          Selecteer document
          <input type="file" class="file" @change="handleFileChange" />
        </label>
      </div>

      <div class="rightSide">
        <ul>
          <form class="gegevens" action="/action_page.php">
            <input type="text" class="Zoek" placeholder="Zoek klant" name="Naam" />
            <input v-model="this.customer.Name" type="text" class="Naam" placeholder="Naam klant" name="Zoek" />
            <input v-model="this.customer.Email" type="text" class="Email" placeholder="Email klant" name="Email" />
            <select v-model="this.document.Type" class="Type" placeholder="Type bestand" name="Type">
              <option value="0">Vog</option>
              <option value="1">Contract</option>
              <option value="2">Paspoort</option>
              <option value="3">id kaart</option>
              <option value="4">Diploma</option>
              <option value="5">Certificaat</option>
              <option value="6">Lease auto</option>
            </select>
            <input v-model="this.document.Date" type="date" class="Date" name="Date" />
          </form>
        </ul>

        <a @click="handleVerstuurClick" class="verstuur" type="button">
          Verstuur document
        </a>
      </div>
    </div>


    <div class="popup-container" :class="{ 'active': activePopup === 'popup2' }">
      <div class="Error">
        <img class="Errorimage" src="../assets/Pictures/cancel.png">
        <p>
          {{ errorMessage }}
        </p>
        <button @click="togglePopup('popup2')">Close</button>
      </div>
    </div>

  </div>
</template>

<script>
import axios from '../../axios-auth.js'

export default {
  data() {
    return {
      activePopup: null,
      selectedFile: null,
      dropAreaActive: false,
      displayImage: false,
      uploadedFileName: '',
      imageContainerWidth: 400,
      imageContainerHeight: 410,
      errorMessage: '',
      customer: {
        CustomerId: 0,
        Name: '',
        Email: '',
      },
      document: {
        DocumentId: 0,
        Type: 0,
        Date: "",
        CustomerId: 0,
      }
    };
  },


  methods: {
    CreateDocument() {

      axios.post("Customer", this.customer)
        .then((res) => {
          let customerId = res.data;
          this.postDocument(customerId);

        }).catch((error) => {
          alert(error.response.data);
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
    postDocument(customerId) {
      let formData = this.CreateFromData(customerId);

      axios.post("Document", formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        }
      })
        .then((res) => {
          console.log(res.data)
        }).catch((error) => {
          alert(error.response.data);
        });
    },
    handleFileChange(event) {
      this.selectedFile = event.target.files[0]

      const reader = new FileReader();
      reader.onload = () => {
        this.fileContents = reader.result;
      };
      reader.readAsBinaryString(this.selectedFile)
    },
    togglePopup(popupName) {
      if (this.activePopup === popupName) {
        this.activePopup = null;
      } else {
        this.activePopup = popupName;
      }
    },
    handleVerstuurClick() {
      if (this.selectedFile || this.displayImage) {
        if (this.displayImage) {
          this.$router.push({ name: 'overview', query: { popup1: true } }); // Set popup1 to true
          // alert('Verstuur document clicked. Image is uploaded.');
        } else {
          // Set an upload success message
          this.errorMessage = `Upload successful. File "${this.selectedFile.name}" is uploaded.`;
        }
      } else {
        // Set an error message for no file selected
        this.errorMessage = 'Error: Selecteer een bestand.';
      }
      this.CreateDocument();

      // Toggle "popup2"
      this.togglePopup('popup2');
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

    calculateImageContainerDimensions() {
      this.imageContainerWidth = 400;
      this.imageContainerHeight = 410;
    },
    handleImageLoad(e) {
      const reader = e.target;
      const image = new Image();
      image.src = reader.result;
      image.onload = () => {
        let width = image.width;
        let height = image.height;
        const maxWidth = 400;
        const maxHeight = 410;
        const aspectRatio = width / height;
        if (width > maxWidth) {
          width = maxWidth;
          height = width / aspectRatio;
        }
        if (height > maxHeight) {
          height = maxHeight;
          width = height * aspectRatio;
        }
        this.$refs.pElement.style.display = 'none'; // Hide the pElement
        this.displayImage = true; // Set displayImage to true
        this.$refs.dropArea.style.width = `${width}px`;
        this.$refs.dropArea.style.height = `${height}px`;
        this.$refs.dropArea.style.backgroundImage = `url(${reader.result})`;
        this.$refs.dropArea.style.backgroundSize = 'cover';
        this.$refs.dropArea.style.backgroundRepeat = 'no-repeat';
        this.$refs.dropArea.style.backgroundPosition = 'center center';
      };
    },
  },
};
</script>

<style scoped>
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

.Error {
  color: black;
  padding: 10px;
  background-color: #F56C6C;
  font-size: 20px;
  text-align: left;
}

.Errorimage {
  width: 60px;
  height: 60px;
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
  border-radius: 25px;
  border-color: black;
  border-width: 12x;
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

/* #logoHeader {
  width: 130px;
  height: 80px;
  margin-left: 20px;
  padding: 0.5% 0% 0% 1%;
} */

/* .header {
  width: 103%;
  height: 120px;
  background-color: #153912;
  margin-left: -1.5%;
  margin-top: -1.5%;
  display: flex;
  flex-direction: row;
} */

/* #buttonsHeader {
  position: absolute;
  right: 0px;
  padding: 50px 40px 0px 0px;
}

 */

/* .input {
  position: fixed;
  top: 300px;
  left: 200px;
  font-size: 20px;
} */


a {
  color: white;
  text-decoration: none;
}

.overlay {
  position: relative;
  display: block;
  top: 30px;
  background: #22421f;
  background-image: url(../assets/Pictures/folder.png);
  background-repeat: no-repeat;
  background-position: 330px;
  background-size: 60px 50px;
  color: white;
  padding: 12px;
  font-size: 25px;
  width: 378px;
  border-radius: 25px;
}

.file {
  display: none;
}

.gegevens {
  display: flex;
  flex-direction: column;
}

.Zoek,
.Naam,
.Email,
.Date,
.Type {
  right: 350px;
  background-color: #f4f4f4;
  font-size: 23px;
  padding: 12px;
  width: 450px;
  margin-bottom: 15px;
  border-radius: 25px;
  border: black solid 2px;
}

.Type {
  width: 477px;
}

.Zoek {
  margin-bottom: 20px;
  margin-left: auto;
  margin-bottom: 70px;
  width: 350px;
}


.verstuur {
  margin-left: auto;
  font-size: 20px;
  background-color: #22421f;
  color: white;
  padding: 12px;
  font-size: 25px;
  border-radius: 25px;
}
</style>
