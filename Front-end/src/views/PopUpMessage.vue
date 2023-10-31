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
  import Cross from '../components/icons/IconCross.vue';
  import CrossCircle from '../components/icons/IconCrossCircle.vue';
  import Check from '../components/icons/IconCheck.vue';
  
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
    mounted() {
  
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
  
        setTimeout(() => {
          this.closePopup();
          localStorage.setItem('popUpSucces', 'false');
        }, 4000);
  
      },
      closePopup() {
        this.activePopup = null;
      },
    },
  };
  </script>
  
  
  <style>
  @import '../assets/Css/Main.css';
  @import '../assets/Css/Popup.css';
  </style>
  