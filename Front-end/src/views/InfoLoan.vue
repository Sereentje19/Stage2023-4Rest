<template>
    <Header></Header>
    <div class="InfoLoanContainer">
        <div class="info">
            <ul>
                <h1>Info</h1>
                <br>
                <div id="LoanTitle">
                    Item
                </div>
                <div id="LoanInfo">
                    Type: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; {{ this.product.type }}
                    <br>
                    Gekocht op: &nbsp;&nbsp;&nbsp;&nbsp; {{ formatDate(this.product.purchaseDate) }}
                    <br>
                    Geldig tot: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{{ formatDate(this.product.expirationDate)
                    }}
                    <br>
                    Serie nummer: &nbsp;&nbsp;{{ this.product.serialNumber }}
                </div>
                <button @click="toEdit('document')" id="EditButton">Edit</button>
                <br><br><br>
                <div v-if="this.loanHistory.returnDate == null && this.loanHistory.loanDate != null" id="collegue">
                    <div>
                        <div id="LoanTitle">
                            Uitgeleend aan medewerker
                        </div>
                        <div id="LoanInfo">
                            Naam: &nbsp; {{ this.loanHistory.customer.name }}
                            <br>
                            Email: &nbsp;&nbsp; {{ this.loanHistory.customer.email }}
                        </div>
                    </div>
                    <div>
                        <div>
                            <button id="buttonItem" @click="confirm">{{ this.product.type }} terugbrengen</button>
                            <div v-if="this.itemReturned == true" id="buttons2">
                                <b> Weet je het zeker? &nbsp; &nbsp; &nbsp; </b>
                                <button id="buttonItem2" @click="returnItem"><b>Ja</b></button>
                                <button id="buttonItem2" @click="confirm"><b>Nee</b></button>
                            </div>
                        </div>
                    </div>
                </div>
                <div v-else id="collegue">
                    <div>
                        <div id="LoanTitle">
                            Deze {{ this.product.type.toLowerCase() }} is nog beschikbaar.
                        </div>
                    </div>
                    <div>
                        <div v-if="this.itemLent == false">
                            <button id="buttonItem" @click="loan">{{ this.product.type }} uitlenen</button>
                        </div>
                        <div v-else id="customersList">

                            <input @input="filterCustomer" v-model="searchField" type="search" class="ZoekVeldLoan"
                                placeholder="Zoek klant" />

                            <div id="tableInfoLoan">
                                <table>
                                    <tr>
                                        <td><b>Name</b></td>
                                        <td><b>Email</b></td>
                                    </tr>
                                    <tr v-for="(customer, i) in this.filteredCustomers" @click="selectCustomer(customer)">
                                        <td>{{ customer.name }}</td>
                                        <td>{{ customer.email }}</td>
                                    </tr>
                                </table>
                            </div>
                        </div>

                    </div>
                </div>
            </ul>
        </div>
    </div>

    <Popup ref="Popup" />
</template>
  
<script>
import axios from '../../axios-auth.js';
import moment from 'moment';
import Popup from '../views/popUp.vue';
import Header from '../views/Header.vue';

export default {
    name: "InfoLoan",
    props: {
        id: Number,
    },
    components: {
        Popup,
        Header
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
            itemReturned: false,
            itemLent: false,
            filteredCustomers: []
        }
    },
    mounted() {
        this.getLoanhistory();
        this.getProducts();
        this.getAllCustomers();

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

            axios.post("LoanHistory", this.loanHistory,  {
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
        confirm() {
            this.itemReturned = !this.itemReturned;
        },
        loan() {
            this.itemLent = !this.itemLent;
        },
        getAllCustomers() {
            if (this.searchField == "") {
                this.getCustomers();
            }
            else {
                this.filterCustomer();
            }
        },
        getCustomers() {
            axios.get("Customer", {
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("jwt")
                },
            })
                .then((res) => {
                    this.filteredCustomers = res.data;
                    console.log(this.filteredCustomers)
                }).catch((error) => { });
        },
        filterCustomer() {
            if (this.searchField != "") {
                axios.get("Customer/Filter", {
                    headers: {
                        Authorization: "Bearer " + localStorage.getItem("jwt")
                    },
                    params: {
                        searchField: this.searchField
                    }
                })
                    .then((res) => {
                        this.filteredCustomers = res.data;
                        console.log(this.filteredCustomers)
                    }).catch((error) => { 
                        this.$refs.Popup.popUpError(error.response.data);
                    });
            }
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
                }).catch((error) => {
                    this.$refs.Popup.popUpError(error.response.data);
                });
        },
        toEdit(route) {
            this.$router.push("/edit/" + route + "/" + this.id);
        },
        formatDate(date) {
            return moment(date).format("DD-MM-YYYY");
        },
    }
}

</script>
  
<style>
@import '../assets/Css/InfoLoan.css';
@import '../assets/Css/Main.css';
</style>
  