<template>
  <div>
    <div class="header">
      <img
        id="logoHeader"
        src="@/assets/Pictures/Logo-4-rest-IT.png"
        alt="does not work"
      />
      <div id="buttonsHeader">
        <a href="/">Document uploaden</a>
        <a href="/">Overzicht</a>
        <a href="/">Uitloggen</a>
      </div>
    </div>

    <div class="info">
      <ul>
        <h1>Contract</h1>
        <br /><br />
        <h3>
          23-11-20
          <br />
          Dit is over X dagen
        </h3>
        <br /><br />
        <h3>
          Serena Kenter
          <br />
          serena@gmail.com
        </h3>
        <br /><br />
        <h3>
          Opmerking
          <br />
          Tarief omhoog
        </h3>
      </ul>
    </div>

    <img src="../views/contract.jpg" class="foto" alt="Document" />
  </div>
</template>

<style scoped>
body {
  background-color: #d9d9d9;
  font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
}

#logoHeader {
  width: 130px;
  height: 80px;
  margin-left: 20px;
  margin-top: 20px;
  padding: 0.5% 0% 0% 1%;
}

.header {
  width: 100%;
  height: 120px;
  background-color: #153912;
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  padding: 0 30px;
}

#buttonsHeader a {
  font-size: 28px;
  color: white;
  text-decoration: none;
  margin-left: 30px;
}

.info ul {
  list-style: none;
  padding: 0;
  margin: 0;
  text-align: left;
}

h1 {
  font-size: 70px;
  margin-left: 20px;
}

h3 {
  font-size: 25px;
  margin-left: 20px;
}

.foto {
  width: 550px;
  height: 650px;
  margin-top: 50px;
  margin-right: 20px;
  float: right;
}
</style>
