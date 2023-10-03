<template>
  <div class="popup-container" :class="{ 'active': activePopup === 'popup2' }">
    <div class="Error" :class="{ 'SuccessPopup': Message === 'Document is geupload!' }">
      <div>
        <div id="CrossCircle">
          <div v-if="this.Message == 'Document is geupload!'" id="ErrorItem" class="SuccesPopup">
            <Check /> &nbsp;&nbsp; Succes
          </div>
          <div v-else id="ErrorItem" class="ErrorPopup">
            <CrossCircle /> &nbsp;&nbsp; Error
          </div>
          <div id="ErrorItemCross">
            <button id="buttonClose" @click="togglePopup('popup2')"
              :class="{ 'SuccessPopup': Message === 'Document is geupload!' }">
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

      setTimeout(() => {
        this.closePopup();
      }, 4000);
    },
    closePopup() {
      this.activePopup = null;
    },
  },
};
</script>



<style>
@import '../assets/Css/Popup.css';
</style>
