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
.popup {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
    background: rgba(0, 0, 0, 0.5);
}

.popup-content {
    background: white;
    padding: 20px;
    border-radius: 5px;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.2);
    display: flex;
    flex-direction: column;
    align-items: center;
}

#button-popup {
    width: fit-content;
    font-size: 18px;
    margin-top: 25px;
    color: white;
    padding: 0px;
    background-color: #22421f;
    padding: 5px 15px 5px 15px;
}

#button-popup:hover {
    background-color: #133610;
}

</style>
  