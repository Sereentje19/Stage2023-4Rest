<template>
    <Header></Header>
    <div class="InfoLoanContainer">
        <h1>Info</h1>
        <div id="leftSide">
            <div id="LoanTitle">
                Item
            </div>
            <div id="LoanInfo">
                <div id="LoanInfoLeftSide">
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
            <button @click="toEdit('document')" id="EditButton">Edit</button>
        </div>


        <div id="box">
            <div id="Available">
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
                        <button id="buttonItem" @click="confirm">{{ this.product.type }} terugbrengen</button>
                        <div v-if="this.itemReturned == true" id="buttons2">
                            <b> Weet je het zeker? &nbsp; &nbsp; &nbsp; </b>
                            <button id="buttonItem2" @click="returnItem"><b>Ja</b></button>
                            <button id="buttonItem2" @click="confirm"><b>Nee</b></button>
                        </div>
                    </div>
                </div>
                <div v-else id="collegue">
                    <div id="secondLeftSide">
                        Deze {{ this.product.type.toLowerCase() }} is nog beschikbaar.
                    </div>
                </div>
            </div>
            <div id="customersList">

                <div v-if="this.loanHistory.returnDate != null" id="rightSide">
                    <input @input="filterCustomer" v-model="searchField" type="search" class="ZoekVeldLoan"
                        placeholder="Zoek klant" />

                    <table id="topTable">
                        <tr>
                            <td class="row1"><b>Name</b></td>
                            <td id="topTableRow"><b>Email</b></td>
                            <td id="emptyRow"></td>
                        </tr>
                    </table>
                    <div id="tableInfoLoan">
                        <table id="bottomTable">
                            <tr v-for="(customer, i) in this.filteredCustomers" @click="selectCustomer(customer)">
                                <td class="row1" id="bottomTableRow">{{ customer.name }}</td>
                                <td id="bottomTableRow">{{ customer.email }}</td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>

    </div>
    <br><br><br>








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
                    this.filteredCustomers = res.data.customers;
                }).catch((error) => {
                    this.$refs.Popup.popUpError(error.response.data);
                });
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
  