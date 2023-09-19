<template>
  <div class="header">
    <a href="/overzicht"><img id="logoHeader" src="@/assets/Pictures/Logo-4-rest-IT.png" alt="does not work" /></a>
    <div id="buttonsHeader">
      <router-link to="/overzicht">Overzicht</router-link>
      <router-link to="/uploaden">Document uploaden</router-link>
      <router-link to="/">Uitloggen</router-link>
    </div>
  </div>
  <div class="overviewContainer">
    <div class="info">
      <ul>
        <h1 class="title">Contract</h1>
        <br /><br />
        <h2 class="text">
          23-11-20
          <br />
          Dit is over X dagen
        </h2>
        <br /><br />
        <h2 class="text">
          Serena Kenter
          <br />
          serena@gmail.com
        </h2>
      </ul>
    </div>
    <div class="foto">
      <img src="../assets/pictures/contract.jpg" class="foto" alt="Document" />
    </div>
  </div>
</template>

<style scoped>
.overviewContainer {
  width: 85%;
  margin: auto;
  margin-top: 80px;
  display: flex;
}

.foto {
  max-height: 500px;
  max-width: 500px;
  margin-left: auto;
}

/* .text {
  position: relative;
  left: 400px;
  top: 80px;
  text-align: left;
  font-size: 30px;
}

.title {
  position: relative;
  top: 80px;
  left: 400px;
  text-align: left;
  font-size: 50px;

}

.foto {
  position: fixed;
  right: 300px;
  top: 200px;
  width: 600px;
  height: 700px;
} */
</style>