<template>
    <div>
        <Header></Header>
        <div class="upload-container-edit">
            <h1 id="h1">Edit {{ this.route }}</h1>

            <form>
                <div class="gegevens-edit">
                    <input class="Email" v-model="this.employee.name" @input="filterDocuments" />
                    <input class="Email" v-model="this.employee.email" @input="filterDocuments" />
                </div>
            </form>
            <button @click="editEmployee()" class="verstuur-edit">Aanpassen</button>
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
    props: {
        type: String,
        id: Number
    },
    data() {
        return {
            employee: {
                employeeId: 0,
                name: '',
                email: '',
            },
        };
    },
    mounted() {
        this.getEmployee();
    },
    methods: {
        getEmployee() {
            axios.get("employee/" + this.id, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                    console.log(res.data)
                    this.employee = res.data;
                }).catch((error) => {
                    this.$refs.PopUpMessage.popUpError(error.response.data);
                });
        },
        editEmployee() {
            console.log(this.employee)

            axios.put("employee", this.employee,{
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                },
            })
                .then((res) => {
                    localStorage.setItem('popUpSucces', 'true');
                    this.$router.push({ path: '/info/medewerker/' + this.type + "/" + this.id, query: { activePopup: true } });
                }).catch((error) => {
                    this.$refs.PopUpMessage.popUpError(error.response.data);
                });
        },
    },
    computed: {
        formattedDate: {
            get() {
                return this.document.Date.split('T')[0];
            },
            set(newDate) {
                this.document.Date = newDate;
            },
        }
    },
};
</script>
  
<style>
@import '../assets/css/Edit.css';
@import '../assets/css/Main.css';
</style>
  