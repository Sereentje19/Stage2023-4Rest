
<template>
    <Header></Header>
    <div class="history-container">
        <div class="info">
            <h1>Leen geschiedenis</h1>
            <div id="item-data">
                <div>Naam: &nbsp; {{ this.loanHistory[0].name }}</div>
                <div>Email: &nbsp; {{ this.loanHistory[0].email }} </div>
            </div>
            <br><br><br>
            <div id="history-items" v-for="(loanHistory, i) in this.loanHistory">
                <div id="line"></div>

                <br>
                <div>{{ formatDate(loanHistory.loanDate) }} - {{ formatDate(loanHistory.returnDate) }}</div>
                <div>{{ loanHistory.type }} - {{ loanHistory.serialNumber }}</div>
                <div></div>
                <br>
            </div>
        </div>
        <PopUpMessage ref="Popup" />
    </div>
</template>

<script>
import axios from '../../axios-auth.js';
import moment from 'moment';
import PopUpMessage from '../views/PopUpMessage.vue';
import Header from '../views/Header.vue';

export default {
    name: "collegueHistory",
    props: {
        id: Number,
    },
    components: {
        PopUpMessage,
        Header
    },
    data() {
        return {
            loanHistory: [
                {
                    loanDate: "",
                    returnDate: "",
                    product: {
                        productId: 0,
                        type: "",
                        serialNumber: "",
                        expirationDate: "",
                        purchaseDate: "",
                    },
                    customer: {
                        name: "",
                        email: ""
                    }
                }
            ],
        }
    },
    mounted() {
        this.getLoanHistory();
    },
    methods: {
        getLoanHistory() {
            axios.get("LoanHistory/CustomerId/" + this.id, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                    console.log(res.data)
                    this.loanHistory = res.data;

                }).catch((error) => {
                    this.$refs.Popup.popUpError(error.response.data);
                });
        },
        formatDate(date) {
            if (date == null) {
                return "Heden";
            }
            return moment(date).format("DD MMM YYYY");
        },
    }
}

</script>

<style>
@import '../assets/Css/History.css';
@import '../assets/Css/Main.css';
</style>
  