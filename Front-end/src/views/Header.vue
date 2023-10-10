<template>
    <div class="header">
        <a href="/overzicht"><img id="logoHeader" src="../assets/Pictures/Logo-4-rest-IT.png" alt="does not work" /></a>
        <div id="buttonsHeader">
            <div class="dropdown" @mouseenter="showDropdown = true" @mouseleave="showDropdown = false">
                <router-link class="dropdown-link" to="/overzicht">Overzicht</router-link>
                <div v-if="showDropdown" class="dropdown-content">
                    <router-link to="/bruikleen">Bruikleen</router-link>
                    <router-link to="/medewerkers">Medewerkers</router-link>
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
            showDropdown: false,
        };
    },
    methods: {
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
