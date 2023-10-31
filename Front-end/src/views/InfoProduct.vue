<template>
    <Header></Header>
    <div class="info-container">
        <h1>Info</h1>

        <PopupChoice ref="PopupChoice" @delete="deleteProducts" @return="returnItem" />

        <div id="leftside">
            <div id="loan-title">
                Item
            </div>
            <div id="loan-info">
                <div id="loan-info-leftside">
                    Type: <br>
                    Gekocht op: <br>
                    Geldig tot: <br>
                    Serie nummer:
                </div>
                <div>
                    {{ this.product.type }} <br>
                    {{ formatDate(this.product.purchaseDate) }} <br>
                    {{ formatDate(this.product.expirationDate) }} <br>
                    {{ this.product.serialNumber }}
                </div>
            </div>
            <div id="box">
                <button @click="toEdit()" id="edit-button">Edit</button>
                <button @click="deleteProduct()" id="delete-button">Delete</button>
            </div>
        </div>


        <div id="box">
            <div id="product-availability">
                <div v-if="this.loanHistory.returnDate == null && this.loanHistory.loanDate != null" id="employee">
                    <div>
                        <div id="loan-title">
                            Uitgeleend aan medewerker
                        </div>
                        <div id="loan-info">
                            Naam: &nbsp; {{ this.loanHistory.customer.name }}
                            <br>
                            Email: &nbsp;&nbsp; {{ this.loanHistory.customer.email }}
                        </div>
                    </div>
                    <div>
                        <button id="button-item" @click="returnProduct">{{ this.product.type }} terugbrengen</button>
                    </div>
                </div>
                <div v-else id="employee">
                    <div id="second-leftside">
                        Deze {{ this.product.type.toLowerCase() }} is nog beschikbaar.
                    </div>
                </div>
            </div>
            <div id="customers-list">

                <div v-if="this.loanHistory.returnDate != null" id="rightside">
                    <input @input="filterCustomer" v-model="searchField" type="search" class="searchfield-loanhistory"
                        placeholder="Zoek klant" />

                    <table id="top-table">
                        <tr>
                            <td class="first-row"><b>Name</b></td>
                            <td id="second-row"><b>Email</b></td>
                            <td id="empty-row"></td>
                        </tr>
                    </table>
                    <div id="table-info-loan">
                        <table id="bottom-table">
                            <tr v-for="(customer, i) in this.filteredCustomers" @click="selectCustomer(customer)">
                                <td class="first-row">{{ customer.name }}</td>
                                <td>{{ customer.email }}</td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>

    </div>
    <br><br><br>

    <PopUpMessage ref="Popup" />
</template>
  
<script>
import axios from '../../axios-auth.js';
import moment from 'moment';
import PopUpMessage from '../views/PopUpMessage.vue';
import Header from '../views/Header.vue';
import PopupChoice from '../views/PopUpChoice.vue';

export default {
    name: "InfoLoan",
    props: {
        id: Number,
    },
    components: {
        PopUpMessage,
        Header,
        PopupChoice
    },
    data() {
        return {
            product:
            {
                expirationDate: "",
                purchaseDate: "",
                type: "",
                serialNumber: "",
                productId: 0,
            },
            loanHistory:
            {
                loanDate: "",
                returnDate: "",
                customer: {
                    customerId: 0,
                    name: "",
                    email: "",
                },
                loanHistoryId: 0,
                product: {
                    productId: 0,
                }
            },
            searchField: "",
            filteredCustomers: [],
            isPopUpReturn: false,
            isPopUpDelete: false
        }
    },
    mounted() {
        this.getLoanhistory();
        this.getProducts();
        this.getAllFilteredCustomers();

        if (this.$route.query.activePopup && localStorage.getItem('popUpSucces') === 'true') {
            this.$refs.Popup.popUpError("Data is bijgewerkt.");
        }
    },
    methods: {
        selectCustomer(cus) {
            this.loanHistory.customer = cus;
            this.loanHistory.product.productId = this.product.productId;
            this.loanHistory.loanDate = new Date();
            this.loanHistory.returnDate = null;
            console.log(this.loanHistory)

            axios.post("LoanHistory", this.loanHistory, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                },
            })
                .then((res) => {
                    this.getLoanhistory();
                })
                .catch((error) => {
                    this.$refs.Popup.popUpError(error.response.data);
                });

        },
        getAllFilteredCustomers() {
            axios.get("Customer/Filter", {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                },
                params: {
                    searchField: this.searchField,
                    page: 1,
                    pageSize: 5
                }
            })
                .then((res) => {
                    this.filteredCustomers = res.data;
                    console.log(this.filteredCustomers)
                }).catch((error) => {
                    this.$refs.Popup.popUpError(error.response.data);
                });
        },
        returnItem() {
            console.log(this.loanHistory)
            axios.put("LoanHistory", this.loanHistory, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                    this.getLoanhistory();
                }).catch((error) => {
                    this.$refs.Popup.popUpError(error.response.data);
                });
        },
        getLoanhistory() {
            axios.get("LoanHistory/Recent/" + this.id, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                }
            })
                .then((res) => {
                    console.log(res.data)
                    this.loanHistory = res.data;
                    this.loanHistory.product.productId = res.data.productId
                }).catch((error) => {
                    if (this.loanHistory.loanDate != "") {
                        this.$refs.Popup.popUpError(error.response.data);
                    }
                });
        },
        getProducts() {
            axios.get("Product/" + this.id, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                },
            })
                .then((res) => {
                    console.log(res.data)
                    this.product = res.data;
                    this.product.type = this.product.productType

                }).catch((error) => {
                    this.$refs.Popup.popUpError(error.response.data);
                });
        },
        deleteProducts() {
            axios.delete("Product/" + this.id, {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                },
            })
                .then((res) => {
                    this.$router.push({ path: '/overzicht/bruikleen', query: { activePopup: true } });
                }).catch((error) => {
                    this.$refs.Popup.popUpError(error.response.data);
                });
        },
        // confirm() {
        //     if (this.isPopUpDelete) {
        //         this.isPopUpDelete = false;
        //         this.deleteProducts();
        //         this.$router.push("/overzicht/bruikleen");
        //     }
        //     else if (this.isPopUpReturn) {
        //         this.isPopUpReturn = false;
        //         this.returnItem();
        //     }
        // },
        toEdit(route) {
            this.$router.push("/edit/product/" + this.id);
        },
        deleteProduct() {
            this.emitter.emit('isPopUpTrue', {'eventContent': true});
            this.emitter.emit('text', {'eventContent': `Weet je zeker dat je ${this.product.type.toLowerCase()} ${this.product.serialNumber} wilt verwijderen?`});
        },
        returnProduct() {
            this.emitter.emit('isPopUpTrue', {'eventContent': true});
            this.emitter.emit('toReturn', {'eventContent': true});
            this.emitter.emit('text', {'eventContent': `Weet je zeker dat je ${this.product.type.toLowerCase()} ${this.product.serialNumber} wilt terugbrengen?`});
        },
        formatDate(date) {
            return moment(date).format("DD-MM-YYYY");
        },
    }
}

</script>
  
<style>
@import '../assets/Css/Info.css';
@import '../assets/Css/Main.css';
</style>
  