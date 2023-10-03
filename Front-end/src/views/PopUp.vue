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
              <button id="buttonClose" @click="togglePopup('popup2')" :class="{ 'SuccessPopup': Message === 'Document is geupload!' }">
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



<style scoped>
.SuccessPopup {
  background-color: #10b473 !important; 
}

#CrossCircle {
  display: flex;
  color: white;
  font-weight: 500;
  font-size: 21px;
  min-width: 300px
}

#ErrorItem {
  margin-top: auto;
  margin-bottom: auto;
  display: flex;
}

#ErrorItemCross {
  margin-left: auto;
  margin-bottom: -5px;
}


#buttonClose {
  background-color: #d85353;
    border: none;
}

.Error {
  background-color: #d85353;
  padding: 10px;
  border-radius: 7px;
}

.popup-container {
  padding-right: 10px;
  position: fixed;
  bottom: 10px;
  right: -600px;
  transition: right 0.3s ease-in-out;
}

.popup-container.active {
    right: 0;
}

#message {
  margin-left: 45px;
  margin-top: 8px;
  color: white;
}
</style>
