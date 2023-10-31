<template>
    <div class="header" @click="test">
        <a id="logo-header-link" href="/overzicht/document"><img id="logo-header" src="../assets/Pictures/Logo-4-rest-IT.png"
                alt="does not work" /></a>
        <div id="header-buttons">
            <div class="dropdown" @mouseenter="showDropdown = true" @mouseleave="showDropdown = false">
                <a id="header-items" class="dropdown-link" href="/overzicht/document"
                    @click="changeOverviewType('Overzicht')" @mouseover="changeColor('#bebebe')"
                    @mouseout="changeColor('white')">
                    Overzicht
                    <ArrowDown :color="svgColor" />
                </a>
                <div v-if="showDropdown" class="dropdown-content">
                    <a id="dropdown-links" href="/overzicht/bruikleen">Bruikleen</a>
                    <a id="dropdown-links" href="/overzicht/medewerkers">Medewerkers</a>
                    <a id="dropdown-links" href="/overzicht/document" @click="changeOverviewType('Lang geldig')">Lang
                        geldig</a>
                    <a id="dropdown-links" href="/overzicht/document" @click="changeOverviewType('Archief')">Archief</a>
                </div>
            </div> &nbsp; &nbsp; &nbsp;


            <div class="dropdown" @mouseenter="showDropdown = true" @mouseleave="showDropdown = false">
                <a id="headerItems" class="dropdown-link" href="/uploaden/document" @click="changeOverviewType('Overzicht')"
                    @mouseover="changeColor('#bebebe')" @mouseout="changeColor('white')">
                    Uploaden
                    <ArrowDown :color="svgColor" />
                </a>
                <div v-if="showDropdown" class="dropdown-content">
                    <a id="dropdown-links" href="/uploaden/document">Document</a>
                    <a id="dropdown-links" href="/uploaden/medewerker">Medewerker</a>
                    <a id="dropdown-links" href="/uploaden/product">Product</a>
                </div>
            </div> &nbsp; &nbsp; &nbsp;
            <router-link id="headerItems" to="/" @click="logOut">Uitloggen</router-link>
        </div>
    </div>
</template>

<script>
import ArrowDown from '../components/icons/IconArrowdown.vue';
import axios from '../../axios-auth.js';

export default {
    name: "Header",
    components: {
        ArrowDown
    },
    data() {
        return {
            svgColor: "white",
            showDropdown: false
        };
    },
    methods: {
        changeOverviewType(type) {
            localStorage.setItem("overviewType", type)
        },
        changeColor(color) {
            this.svgColor = color;
        },
        logOut() {
            localStorage.setItem("jwt", "");
        },

        test() {
            axios
                .get("Document", {
                    headers: {
                        Authorization: "Bearer " + localStorage.getItem("jwt"),
                    },
                })
                .then((res) => {
                })
                .catch((error) => {
                    this.$refs.Popup.popUpError(error.response.data);
                });
        },
    }
};


</script>

<style>
@import '../assets/Css/Main.css';
@import '../assets/Css/Header.css';
</style>
