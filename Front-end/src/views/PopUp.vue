<template>
<div class="popup-container" :class="{ 'active': activePopup === 'popup2' }">
      <div class="Error">
      <img v-if="this.Message == 'Document is geupload!'" class="Succesimage" src="../assets/Pictures/Checked.png">
        <img v-else class="Errorimage" src="../assets/Pictures/cancel.png">
        <div id="message">
          {{ Message }}
        </div>
        <button id="buttonClose" @click="togglePopup('popup2')"><b>x</b></button>
      </div>
    </div>
</template>



<script>
export default {
  props: {
    Message: String,
  },
  data() {
    return {
      activePopup: null,
      Message: '',
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
    popUpError(error) {
      this.closePopup();
      this.Message = error;
      this.togglePopup('popup2');

    //   setTimeout(() => {
    //     this.closePopup();
    //   }, 4000);
    },
    closePopup() {
      this.activePopup = null;
    },
  },
};
</script>



<style scoped>
#buttonClose {
  font-size: 25px;
  background-color: #535353;
  color: rgb(216, 216, 216);
  border: none;
}

.Error {
  color: rgb(216, 216, 216);
  background-color: #535353;
  font-size: 17px;
  text-align: left;
  display: flex;
  padding: 10px;
  border-radius: 5px;
}

.Errorimage {
  width: 30px;
  height: 30px;
  margin: auto;
}

.popup-container {
  position: fixed;
  bottom: 1px;

  right: -600px;
  width: fit-content;
  box-shadow: -5px 0 15px rgba(0, 0, 0, 0.3);
  transition: right 0.3s ease-in-out;
}

#message {
  margin: auto 20px auto 20px;
}
</style>
