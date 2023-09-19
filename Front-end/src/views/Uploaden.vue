<template>
  <div>
    <div class="header">
      <img
        id="logoHeader"
        src="@/assets/Pictures/Logo-4-rest-IT.png"
        alt="does not work"
      />
      <div id="buttonsHeader">
        <router-link to="/overzicht">Overzicht</router-link>

        <a href="/">Uitloggen</a>
      </div>
    </div>

    <div class="input">
      <h1 id="h1">Document uploaden</h1>
      <div
        id="drop"
        ref="dropArea"
        class="dropArea"
        @dragover.prevent="handleDragOver"
        @dragleave="handleDragLeave"
        @drop.prevent="handleDrop"
      >
        <p id="p" ref="pElement"></p>
        <input
          type="file"
          class="file"
          @change="handleFileChange"
          style="display: none"
        />
      </div>

      <label class="overlay">
        Selecteer document
        <input type="file"  class="file" @change="handleFileChange" />
      </label>

      <div class="Klant">
        <ul>
          <form class="gegevens" action="/action_page.php">
            <input
              type="text"
              class="Zoek"
              placeholder="Zoek klant"
              name="Naam"
            />
            <input
              type="text"
              class="Naam"
              placeholder="Naam klant"
              name="Zoek"
            />
            <input
              type="text"
              class="Email"
              placeholder="Email klant"
              name="Email"
            />
            <input
              type="text"
              class="Type"
              placeholder="Type bestand"
              name="Type"
            />
          </form>
        </ul>
        <button @click="handleVerstuurClick" class="verstuur" type="button">
          Verstuur document
        </button>

        <div class="popup-container" :class="{ 'active': activePopup === 'popup2' }">
  <div class="Error">
    <img class="Errorimage" src="../components/icons/cancel.png">
    <p>
      {{ errorMessage }}
    </p>
    <button @click="togglePopup('popup2')">Close</button>
  </div>
</div>

      </div>
    </div>
  </div>
</template>

<script>
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
  };
},

  methods: {

    togglePopup(popupName) {
      if (this.activePopup === popupName) {
        this.activePopup = null;
      } else {
        this.activePopup = popupName;
      }
    },
  


    handleFileChange(e) {
      const files = e.target.files || e.dataTransfer.files;
      this.processFile(files[0]);
    },
    

    handleVerstuurClick() {
  if (this.selectedFile || this.displayImage) {
    if (this.displayImage) {
      this.$router.push({ name: 'overview', query: { popup1: true } }); // Set popup1 to true
      alert('Verstuur document clicked. Image is uploaded.');
    } else {
      // Set an upload success message
      this.errorMessage = `Upload successful. File "${this.uploadedFileName}" is uploaded.`;
    }
  } else {
    // Set an error message for no file selected
    this.errorMessage = 'Error: Selecteer een bestand.';
  }

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

    processFile(file) {
      if (file) {
        const reader = new FileReader();
        reader.onload = this.handleImageLoad;
        reader.readAsDataURL(file);
      }
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

.Error{
  color: black;
  padding: 10px;
  background-color: #F56C6C;
  font-size: 20px;
  text-align: left;
}

.Errorimage{
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
  left: 150px;
  background-color: white;
  color: #717171;
  text-align: center;
  display: flex;
  align-items: center;
  justify-content: center;
  overflow: hidden;
  max-width: 400px;
  max-height: 410px;
  transition: background-color 0.3s, border 0.3s;
  background-repeat: no-repeat;
  background-size: cover;
}

#p {
  width: 100px;
  height: 100px;
  background-size: cover;
  display: block;
  background-image: url(../components/icons/move.png);
}

body {
  background-color: #d9d9d9;
  font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
}

#logoHeader {
  width: 130px;
  height: 80px;
  margin-left: 20px;
  margin-top: 20px;
  padding: 0.5% 0% 0% 1%;
}

.header {
  width: 103%;
  height: 120px;
  background-color: #153912;
  margin-left: -1.5%;
  margin-top: -1.5%;
  display: flex;
  flex-direction: row;
}

#buttonsHeader {
  position: absolute;
  right: 0px;
  padding: 50px 40px 0px 0px;
}

a {
  font-size: 28px;
  color: white;
  margin-left: 30px;
  text-decoration: none;
}

.input {
  position: fixed;
  top: 300px;
  left: 200px;
  font-size: 20px;
}

#h1 {
  position: fixed;
  top: 200px;
  left: 350px;
  font-size: 40px;
}

.overlay {
  position: relative;
  display: block;
  top: 30px;
  left: 150px;
  background: #22421f;
  background-image: url(../components/icons/folder.png);
  background-repeat: no-repeat;
  background-position: 330px;
  background-size: 60px 50px;
  color: white;
  padding: 12px;
  font-size: 25px;
  width: 378px;
}

.file {
  display: none;
}

.Zoek {
  position: fixed;
  top: 226px;
  right: 350px;
  background-image: url(../components/icons/lens.png);
  background-position: 375px 10px;
  background-size: 45px 45px;
  background-color: #f4f4f4;
  background-repeat: no-repeat;
  border-color: black;
  padding: 12px;
  font-size: 23px;
  width: 350px;
}

.Naam {
  position: fixed;
  top: 362px;
  right: 350px;
  background-color: #f4f4f4;
  border-color: black;
  font-size: 23px;
  padding: 12px;
  width: 450px;
}

.Email {
  position: fixed;
  top: 440px;
  right: 350px;
  background-color: #f4f4f4;
  border-color: black;
  font-size: 23px;
  padding: 12px;
  width: 450px;
}

.Type {
  position: fixed;
  top: 515px;
  right: 350px;
  background-color: #f4f4f4;
  border-color: black;
  font-size: 23px;
  padding: 12px;
  width: 450px;
}

.verstuur {
  position: fixed;
  top: 745px;
  right: 350px;
  font-size: 20px;
  background-color: #22421f;
  color: white;
  padding: 12px;
}
</style>
