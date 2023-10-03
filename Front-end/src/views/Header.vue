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
#logoHeader {
    width: 130px;
    height: 80px;
    margin-left: 3px;
    margin-top: 28px;
    padding: 0.5% 0% 0% 1%;
}

a {
    font-size: 28px;
    color: white;
    margin-left: 30px;
}

#buttonsHeader {
    position: absolute;
    right: 0px;
    padding: 50px 40px 0px 0px;
}

.header {
    width: 103%;
    height: 120px;
    background-color: #153912;
    margin-left: -1.5%;
    margin-top: -1.5%;
    display: flex;
    flex-direction: row;
}
</style>
