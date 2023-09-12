<template>
  <div>
    <div class="header">
      <img
        id="logoHeader"
        src="@/assets/Pictures/Logo-4-rest-IT.png"
        alt="does not work"
      />
      <div id="buttonsHeader">
        <router-link to="/overzicht">Overzicht</router-link>
        <a href="/">Uitloggen</a>
      </div>
    </div>

    <div class="input">
      <h1 id="h1">Document uploaden</h1>

      <div class="drop">
        <p id="p">drag & drop</p>
      </div>

      <label class="overlay">
        Selecteer document
        <input type="file" class="file" />
      </label>

      <div class="Klant">
        <ul>
          <form class="gegevens" action="/action_page.php">
            <input
              type="text"
              class="Zoek"
              placeholder="       Zoek klant"
              name="Naam"
            /><br /><br />
            <input
              type="text"
              class="Naam"
              placeholder="Naam klant"
              name="Zoek"
            /><br /><br />
            <input
              type="text"
              class="Email"
              placeholder="Email klant"
              name="Email"
            /><br /><br />
            <input
              type="text"
              class="Type"
              placeholder="Type bestand"
              name="Type"
            /><br /><br />
            <button @click="Overzicht()" class="verstuur" type="button">
              Verstuur document
            </button>
          </form>
        </ul>
      </div>
    </div>
  </div>
</template>

<script>

export default {
  
  name: "Uploaden",
  methods: {
    Overzicht() {
      const fileInput = document.querySelector(".file");
      if (fileInput.files.length === 0) {
        alert("Selecteer graag een bestand.");
      } else {
        alert("file in gevulld")
      }
    },
  },
};

</script>

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
  width: 103%;
  height: 120px;
  background-color: #153912;
  margin-left: -1.5%;
  margin-top: -1.5%;
  display: flex;
  flex-direction: row;
}

#buttonsHeader {
  position: absolute;
  right: 0px;
  padding: 50px 40px 0px 0px;
}

a {
  font-size: 28px;
  color: white;
  margin-left: 30px;
  text-decoration: none;
}

.input {
  position: fixed;
  top: 300px;
  left: 200px;
  font-size: 20px;
}

#h1 {
  position: fixed;
  top: 200px;
  left: 350px;
  font-size: 40px;
}

#p {
  position: relative;
  top: 45%;
  font-size: 40px;
}

.drop {
  position: relative;
  left: 150px;
  background-color: white;
  color: #717171;
  height: 400px;
  width: 400px;
  text-align: center;
}

.overlay {
  position: relative;
  display: block;
  top: 30px;
  left: 150px;
  background: #22421f;
  background-image: url(../components/icons/folder.png);
  background-repeat: no-repeat;
  background-position: 330px;
  background-size: 60px 50px;
  color: white;
  padding: 12px;
  font-size: 25px;
  width: 378px;
}

.file {
  display: none;
}

.Zoek {
  position: fixed;
  top: 226px;
  right: 350px;
  background-image: url(../components/icons/lens.png);
  background-position: 375px 10px;
  background-size: 45px 45px;
  background-color: #f4f4f4;
  background-repeat: no-repeat;
  border-color: black;
  padding: 12px;
  font-size: 23px;
  width: 350px;
}

.Naam {
  position: fixed;
  top: 362px;
  right: 350px;
  background-color: #f4f4f4;
  border-color: black;
  font-size: 23px;
  padding: 12px;
  width: 450px;
}

.Email {
  position: fixed;
  top: 440px;
  right: 350px;
  background-color: #f4f4f4;
  border-color: black;
  font-size: 23px;
  padding: 12px;
  width: 450px;
}

.Type {
  position: fixed;
  top: 515px;
  right: 350px;
  background-color: #f4f4f4;
  border-color: black;
  font-size: 23px;
  padding: 12px;
  width: 450px;
}

.verstuur {
  position: fixed;
  top: 745px;
  right: 350px;
  font-size: 20px;
  background-color: #22421f;
  color: white;
  padding: 12px;
}
</style>