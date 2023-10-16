<template>
    <div class="header">
        <a href="/overzicht"><img id="logoHeader" src="../assets/Pictures/Logo-4-rest-IT.png" alt="does not work" /></a>
        <div id="buttonsHeader">
            <div class="dropdown" @mouseenter="showDropdown = true" @mouseleave="showDropdown = false">
                <a id="headerItems" class="dropdown-link" href="/overzicht" @click="changeOverviewType('Overzicht')"
                    @mouseover="changeColor('#bebebe')" @mouseout="changeColor('white')">
                    Overzicht 
                    <ArrowDown :color="svgColor" />
                </a>
                <div v-if="showDropdown" class="dropdown-content">
                    <router-link id="dropDownLinks" to="/bruikleen">Bruikleen</router-link>
                    <router-link id="dropDownLinks" to="/medewerkers">Medewerkers</router-link>
                    <a id="dropDownLinks" href="/overzicht" @click="changeOverviewType('Lang geldig')">Lang geldig</a>
                    <a id="dropDownLinks" href="/overzicht" @click="changeOverviewType('Archief')">Archief</a>
                </div>
            </div> &nbsp; &nbsp; &nbsp;
            <a id="headerItems" href="/uploaden">Document uploaden</a>
            &nbsp; &nbsp; &nbsp;
            <router-link id="headerItems" to="/" @click="logOut">Uitloggen</router-link>
        </div>
    </div>
</template>

<script>
import ArrowDown from '../components/icons/IconArrowdown.vue';

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
        }
    }
};


</script>

<style>
@import '../assets/Css/Main.css';
@import '../assets/Css/Header.css';
</style>
