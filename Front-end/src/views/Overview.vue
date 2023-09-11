<template>
  <body class="overviewBody">
    <div class="header">
      <img
        id="logoHeader"
        src="../assets/Pictures/Logo-4-rest-IT.png"
        alt="does not work"
      />
      <div id="buttonsHeader">
        <a>Document uploaden</a>
        <a href="/">Uitloggen</a>
      </div>
    </div>
    <div class="overviewContainer">
      <h1>Overzicht</h1>

      <div id="titlesOverview">
        <h3 id="urgentie">Urgentie</h3>
        <h3 id="klantnaam">Klantnaam</h3>
        <h3 id="geldigVan">Geldig van</h3>
        <h3 id="geldigTot">Geldig tot</h3>
        <h3 id="Type">Type document</h3>
      </div>

      <div class="overview" v-for="i in 5">
        <a id="item" href="/">
          <img v-if="i == 1" id="urgentieSymbool" src="../assets/Pictures/hogeUrgentie.png" alt="does not work" />
          <img v-if="i >= 2 && i <= 4" id="urgentieSymbool" src="../assets/Pictures/middelUrgentie.png" alt="does not work" />
          <img v-if="i == 5" id="urgentieSymbool" src="../assets/Pictures/lageUrgentie.png" alt="does not work" />
          <div id="klantnaamTekst">Serena Kenter</div>
          <div id="geldigVanTekst">12-04-2019</div>
          <div id="geldigTotTekst">13-04-2024</div>
          <div id="typeTekst">Contract</div>
        </a>
      </div>

      <div id="pageNavigator">
        Pagina
        <ArrowLeft />
        <b>1</b> 2 3 ... 7
        <ArrowRight />
      </div>
    </div>
  </body>
</template>

<script>
import ArrowLeft from "../components/icons/iconOverviewArrowLeft.vue";
import ArrowRight from "../components/icons/iconOverviewArrowRight.vue";

export default {
  name: "overview",
  components: {
    ArrowLeft,
    ArrowRight,
  },
};
</script>

<style>
#urgentieSymbool{
  width: 50px;
  margin-top: 8px;
  margin-left: 8px;
}

#klantnaamTekst,
#geldigVanTekst,
#geldigTotTekst,
#typeTekst{
  margin-top: auto;
  margin-bottom: auto;
  font-size: large;
}

#titlesOverview {
  display: grid;
  grid-template-columns: 10% 30% 15% 15% 30%;
}

#pageNavigator {
  margin-top: 20px;
}

#item {
  width: 100%;
  height: 55px;
  background-color: white;
  margin-bottom: 6px;
  border-color: #868686;
  border-width: 1px;
  border-style: solid;
  display: grid;
  grid-template-columns: 10% 30% 15% 15% 30%;
  color: black;
  margin-left: auto;
}

a{
  text-decoration: none;
}

.overviewContainer {
  width: 85%;
  margin: auto;
  margin-top: 80px;
}

h1 {
  font-size: 50px;
}

#logoHeader {
  width: 130px;
  height: 80px;
  margin-left: 20px;
  margin-top: 20px;
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

body {
  background-color: #d9d9d9;
  font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
}
</style>
