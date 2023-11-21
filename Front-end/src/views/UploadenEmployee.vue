<template>
    <div>
        <Header></Header>
        <div class="upload-container">
            <div id="upload-employee">
                <h1 id="h1">Medewerker uploaden</h1>

                <ul>
                    <form class="gegevens">
                        <input v-model="this.Employee.Name" type="text" class="name" placeholder="Naam" name="Zoek" />
                        <input v-model="this.Employee.Email" type="text" class="email" placeholder="Email"
                            name="Email" />
                    </form>
                </ul>

                <button @click="this.PostEmployee()" class="verstuur-employee">
                    Verstuur document
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
            Employee: {
                employeeId: 0,
                Name: '',
                Email: '',
            },
        };
    },
    methods: {
        PostEmployee() {
            axios.post("Employee", this.Employee, {
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
  