<template>
    <div class="header">
        <a href="/overzicht"><img id="logoHeader" src="../assets/Pictures/Logo-4-rest-IT.png" alt="does not work" /></a>
        <div id="buttonsHeader">
            <router-link to="/overzicht">Overzicht</router-link>
            <router-link to="/uploaden">Document uploaden</router-link>
            <router-link to="/">Uitloggen</router-link>
        </div>
    </div>
</template>

<script>
export default {
    name: "Header",
};
</script>

<style>
@import '../assets/Css/Main.css';
@import '../assets/Css/Header.css';
</style>
