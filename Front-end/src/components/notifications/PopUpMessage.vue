<template>
  <div class="popup-container" :class="{ 'active': activePopup === 'popup2' }">
    <div class="error" :class="{ 'success-popup': colorOfMessage === 'true' }">
      <div>
        <div id="cross-circle">
          <div v-if="this.colorOfMessage == 'true'" id="error-item" class="success-popup">
            <Check /> &nbsp;&nbsp; Succes
          </div>
          <div v-else id="error-item">
            <CrossCircle /> &nbsp;&nbsp; Error
          </div>
          <div id="error-item-cross">
            <button id="button-close" @click="togglePopup('popup2')"
              :class="{ 'success-popup': colorOfMessage === 'true' }">
              <Cross />
            </button>
          </div>
        </div>
        <div id="message">
          {{ Message }}
        </div>
      </div>
    </div>
  </div>
</template>
  
<script>
import Cross from '../icons/IconCross.vue';
import CrossCircle from '../icons/IconCrossCircle.vue';
import Check from '../icons/IconCheck.vue';

export default {
  name: "PopUpMessage",
  components: {
    Cross,
    CrossCircle,
    Check
  },
  props: {
    Message: String,
  },
  data() {
    return {
      activePopup: null,
      Message: '',
      colorOfMessage: localStorage.getItem('popUpSucces')
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
      this.colorOfMessage = localStorage.getItem('popUpSucces');
      this.activePopup = null;
      this.Message = error;
      this.togglePopup('popup2');

      setTimeout(() => {
        this.closePopup();
      }, 4000);
    },
    closePopup() {
      this.activePopup = null;
      localStorage.setItem('popUpSucces', 'false');
    },
  },
};
</script>
  
  
<style>
@import '/assets/css/Main.css';
@import '/assets/css/Popup.css';
</style>
  