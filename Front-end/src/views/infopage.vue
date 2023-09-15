<template>
  <div>
    <div class="header">
      <img
        id="logoHeader"
        src="@/assets/Pictures/Logo-4-rest-IT.png"
        alt="does not work"
      />
      <div id="buttonsHeader">
        <router-link to="/uploaden">Document uploaden</router-link>
        <router-link to="/overzicht">Overzicht</router-link>
        <a href="/">Uitloggen</a>
      </div>
    </div>
    
    <div class="info">
      <ul>
        <h1 class="title">Contract</h1>
        <br /><br />
        <h3 class="text">
          23-11-20
          <br />
          Dit is over X dagen
        </h3>
        <br /><br />
        <h3 class="text">
          Serena Kenter
          <br />
          serena@gmail.com
        </h3>
        <br /><br />
        <h3 class="text">
          Opmerking
          <br />
          Tarief omhoog
        </h3>
      </ul>
    </div>
</div>
    <img src="../assets/pictures/contract.jpg" class="foto" alt="Document" />
  
</template>

<style scoped>
.text{ 
  position: relative;
  left: 400px;
  top: 80px;
text-align: left;
font-size: 30px;
}

.title{
  position: relative;
  top: 80px;
  left: 400px;
text-align: left;
font-size: 50px;
  
}
.foto{
  position:fixed;
right: 300px;
top: 200px;
width: 600px;
height: 700px;
}
</style>