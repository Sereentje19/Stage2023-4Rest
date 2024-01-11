<template>
    <div v-if="this.isPopUp" class="popup">
        <div class="popup-content">
            <div>
                {{ this.toDelete }}
            </div>
            <div id="box">
                <button id="button-popup" @click="cancel()">Cancel</button>
                <button id="button-popup" @click="confirm()">Bevestig</button>
            </div>
        </div>
    </div>
</template>


<script>
export default {
    data: function () {
        return {
            isPopUp: false,
            toReturn: false,
            toDelete: '',
        }
    },
    created() {
        this.emitter.on('isPopUpTrue', (evt) => {
            this.isPopUp = evt.eventContent;
        })

        this.emitter.on('text', (evt) => {
            this.toDelete = evt.eventContent;
        })

        this.emitter.on('toReturn', (evt) => {
            this.toReturn = evt.eventContent;
        })
    },
    methods: {
        confirm() {
            if (this.toReturn) {
                this.$emit('return');
            }
            else {
                this.$emit('delete');
            }

            this.isPopUp = false;
        },
        cancel() {
            this.isPopUp = false;
        },
    }
}

</script>
  
  
<style>
@import '/assets/css/Main.css';
@import '/assets/css/Popup.css';
</style>
