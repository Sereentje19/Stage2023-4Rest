<template>
    <div class="header">
        <a href="/overzicht"><img id="logoHeader" src="../assets/Pictures/Logo-4-rest-IT.png" alt="does not work" /></a>
        <div id="buttonsHeader">
            <div class="dropdown" @mouseenter="showDropdown = true" @mouseleave="showDropdown = false">
                <a class="dropdown-link" href="/overzicht" @click="changeOverviewType('overview')">Overzicht</a>
                <div v-if="showDropdown" class="dropdown-content">
                    <router-link to="/bruikleen">Bruikleen</router-link>
                    <router-link to="/medewerkers">Medewerkers</router-link>
                    <a href="/overzicht" @click="changeOverviewType('valid')">Lang geldig</a>
                    <a href="/overzicht" @click="changeOverviewType('archive')">Archief</a>
                </div>
            </div> &nbsp; &nbsp; &nbsp;
            <router-link to="/uploaden">Document uploaden</router-link>
            &nbsp; &nbsp; &nbsp;
            <router-link to="/" @click="logOut">Uitloggen</router-link>
        </div>
    </div>
</template>

<script>
export default {
    name: "Header",
    data() {
        return {
            showDropdown: false
        };
    },
    methods: {
        changeOverviewType(type) {
            localStorage.setItem("overviewType", type)
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
