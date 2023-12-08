<template>
    <div>
        <Header></Header>
        <div class="upload-container-edit">
            <h1 id="h1">Edit Document</h1>

            <form>
                <div class="gegevens-edit">
                    <select v-model="this.document.Type.name" class="Type">
                        <option value="0">Selecteer type...</option>
                        <option v-for="(type, index) in documentTypes" :key="index" :value="type.name">
                            {{ type.name }}
                        </option>
                    </select>
                    <input v-model="formattedDate" type="date" class="Date" />

                </div>
            </form>
            <button @click="editDocument()" class="verstuur-edit">Aanpassen</button>
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
        id: Number,
        type: String
    },
    data() {
        return {
            document: {
                DocumentId: 0,
                Type: {
                    id: 0,
                    name: "0"
                },
                Date: "",
            },
            documentTypes: [],
            employeeDocument: {
                employeeId: 0,
                Name: '',
                Email: '',
                DocumentId: 0,
            },
            filteredEmployees: []
        };
    },
    mounted() {
        this.getDocument();
        this.getDocumentTypes();
    },
    methods: {
        getDocument() {
            axios.get("document/" + this.id, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                    this.document = res.data.document;
                    this.document.Date = res.data.document.date;
                    this.document.Type = res.data.document.type;
                }).catch((error) => {
                    this.$refs.PopUpMessage.popUpError(error.response.data);
                });
        },
        getDocumentTypes() {
            axios
                .get("document/types", {
                    headers: {
                        Authorization: "Bearer " + localStorage.getItem("jwt"),
                    }
                })
                .then((res) => {
                    console.log(res.data)
                    this.documentTypes = res.data;
                })
                .catch((error) => {
                    this.$refs.PopUpMessage.popUpError(error.response.data);
                });
        },
        editDocument() {
            this.document.Type = parseInt(this.document.Type, 10);
            this.document.DocumentId = this.id;

            axios.put("document", this.document, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                    localStorage.setItem('popUpSucces', 'true');
                    this.$router.push({ path: '/info/document/' + this.type + "/" + this.id, query: { activePopup: true } });
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
  