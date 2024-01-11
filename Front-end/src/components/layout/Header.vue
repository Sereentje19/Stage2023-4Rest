<template>
    <div class="header">
        <a id="logo-header-link" href="/overzicht/documenten"><img id="logo-header"
                src="/pictures/logo.png" alt="does not work" /></a>
        <div id="header-buttons">
            <div class="dropdown" @mouseenter="showDropdown = true" @mouseleave="showDropdown = false">
                <a id="header-items" class="dropdown-link" href="/overzicht/documenten"
                    @click="changeOverviewType('Overzicht')" @mouseover="changeColor('#bebebe')"
                    @mouseout="changeColor('white')">
                    Overzicht
                    <ArrowDown :color="svgColor" />
                </a>
                <div v-if="showDropdown" class="dropdown-content">
                    <div id="box-shadow">
                        <a id="dropdown-links" href="/overzicht/documenten">Documenten</a>
                        <a id="dropdown-links" href="/overzicht/medewerkers">Medewerkers</a>
                        <a id="dropdown-links" href="/overzicht/bruikleen">Bruikleen</a>
                    </div>
                </div>
            </div> &nbsp; &nbsp; &nbsp;


            <div class="dropdown" @mouseenter="showDropdown = true" @mouseleave="showDropdown = false">
                <a id="header-items" class="dropdown-link" href="/uploaden/document"
                    @click="changeOverviewType('Overzicht')" @mouseover="changeColor('#bebebe')"
                    @mouseout="changeColor('white')">
                    Uploaden
                    <ArrowDown :color="svgColor" />
                </a>
                <div v-if="showDropdown" class="dropdown-content">
                    <div id="box-shadow">
                        <a id="dropdown-links" href="/uploaden/document">Document</a>
                        <a id="dropdown-links" href="/uploaden/medewerker">Medewerker</a>
                        <a id="dropdown-links" href="/uploaden/product">Product</a>
                    </div>
                </div>
            </div> &nbsp; &nbsp; &nbsp;
            <router-link id="header-items" to="/profiel/persoonsgegevens" >Profiel</router-link>

            &nbsp; &nbsp; &nbsp;
            <router-link id="header-items" to="/" @click="logOut">Uitloggen</router-link>
        </div>
    </div>
</template>

<script>
import ArrowDown from '../icons/IconArrowdown.vue';

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
        changeColor(color) {
            this.svgColor = color;
        },
        logOut() {
            localStorage.setItem("jwt", "");
        },
    }
};


</script>

<style>
@import '../../assets/css/Main.css';
@import '../../assets/css/Header.css';
</style>
