<template>
    <div>
        <Header></Header>
        <div class="upload-container">
            <div id="upload-employee">
                <h1 id="h1">Medewerker uploaden</h1>

                <ul>
                    <form class="gegevens">
                       Naam <input v-model="this.employee.name" type="text" class="name" name="Zoek" />
                       Email <input v-model="this.employee.email" type="text" class="email"
                            name="Email" />
                    </form>
                </ul>

                <button @click="this.PostEmployee()" class="verstuur-employee">
                    Verstuur medewerker
                </button>
            </div>
        </div>

        <PopUpMessage ref="PopUpMessage" />

    </div>
</template>
  
<script>
import axios from '../../axios-auth.js'
import PopUpMessage from '../components/notifications/PopUpMessage.vue';
import Header from '../components/layout/Header.vue';

export default {
    components: {
        PopUpMessage,
        Header
    },
    data() {
        return {
            employee: {
                employeeId: 0,
                Name: '',
                email: '',
            },
        };
    },
    methods: {
        PostEmployee() {
            axios.post("Employee", this.employee, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                    localStorage.setItem('popUpSucces', 'true');
                    this.$router.push({ path: '/Overzicht/medewerkers', query: { activePopup: true } });
                }).catch((error) => {
                    this.$refs.PopUpMessage.popUpError(error.response.data);
                });
        },
    },
};
</script>
  
<style>
@import '../assets/css/Uploaden.css';
@import '../assets/css/Main.css';
</style>
  