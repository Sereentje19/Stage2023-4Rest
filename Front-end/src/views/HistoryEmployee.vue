
<template>
    <Header></Header>
    <div class="history-container">
        <div class="info">
            <h1>Leen geschiedenis</h1>
            <div id="item-data">
                <div v-if="loanHistory.length > 0">
                    <div>Naam: &nbsp; {{ loanHistory[0].name }}</div>
                    <div>Email: &nbsp; {{ loanHistory[0].email }} </div>
                </div>
                <div v-else>
                    Er is nog geen product uitgeleend aan deze medewerker.
                </div>
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
        <PopUpMessage ref="PopUpMessage" />
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
                        type: "",
                        serialNumber: "",
                    },
                    name: "",
                    email: ""
                }
            ],
        }
    },
    mounted() {
        this.getLoanHistory();
    },
    methods: {
        getLoanHistory() {
            axios.get("loan-history/employee/" + this.id, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                    console.log(res.data)
                    this.loanHistory = res.data;
                    console.log(this.loanHistory)

                }).catch((error) => {
                    this.$refs.PopUpMessage.popUpError(error.response.data);
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
  