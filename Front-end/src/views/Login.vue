<template>
  <body>
    <img
      id="logo"
      src="../assets/Pictures/Logo-4-rest-IT.png"
      alt="does not work"
    />
    <div class="loginContainer">
      <div class="cirkel">
        <div id="profileIcon">
          <profile />
        </div>
        <img src="../components/icons/iconLoginProfile.svg" alt="" />
      </div>
      <div class="divInput">
        <div id="profileFillIcon">
          <profileFill />
        </div>
        <input
          id="inputEmail"
          class="inputFieldLogin"
          type="text"
          placeholder="Email"
          required
        />
        <div class="divie" id="eyeIcon">
        </div>
      </div>
      <div class="divInput">
        <div id="profileFillIcon">
          <lockClosed />
        </div>
        <input
          class="inputFieldLogin"
          id="inputWachtwoord"
          type="text"
          placeholder="Wachtwoord"
          required
        />
        <div id="eyeIcon">
          <eyeOpen />
        </div>
      </div>
      <button class="loginButton" type="button">Login</button>
    </div>
  </body>
</template>

<script>
import profile from "../components/icons/iconLoginProfile.vue";
import profileFill from "../components/icons/iconLoginProfileFill.vue";
import lockClosed from "../components/icons/iconLoginLockClosed.vue";
import eyeOpen from "../components/icons/iconLoginEyeOpen.vue";
import eyeClosed from "../components/icons/iconLoginEyeClosed.vue";

export default {
  name: "login",
  components: {
    profile,
    profileFill,
    lockClosed,
    eyeOpen,
    eyeClosed
  },
};
</script>

<style>
#logo{
  width: 150px;
}

.divie{
  width: 30px;
}

.divInput {
  display: flex;
  margin-left: 10%;
  margin-right: 13%;
  margin-bottom: 7%;
}

#eyeIcon{
  height: 40px;
  margin-left: -35px;
  margin-top: 13px;
}

#profileIcon {
  margin: auto;
}

#profileFillIcon {
  background-color: #ffffff;
  height: 47px;
  padding-top: 5px;
}

.cirkel {
  height: 120px;
  width: 120px;
  background-color: #1a4716;
  border-radius: 100%;
  display: inline-block;
  margin-left: auto;
  margin-right: auto;
  margin-top: -60px;
  display: flex;
  margin-bottom: 70px;
}

.loginContainer {
  background-color: #98b88d;
  border-radius: 40px;
  border-color: white;
  border-width: 2px;
  border-style: solid;
  height: 450px;
  width: 25%;
  display: flex;
  flex-direction: column;
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
}

.loginButton {
  width: 80%;
  height: 45px;
  margin-left: auto;
  margin-right: auto;
  font-size: 25px;
  background-color: #1a4716;
  border: none;
  color:white
}

.inputFieldLogin {
  height: 50px;
  font-size: 22px;
  padding-left: 10px;
}

#inputWachtwoord, 
#inputEmail {
  border-color: rgb(205, 205, 205);
  background-color: rgb(205, 205, 205);
  border: none;
  outline: none;
  width: 100%;
}

#inputWachtwoord{
  margin-bottom: 80px;
}

body {
  background-image: url("../assets/Pictures/green-background-blur.png");
  background-size: 100%;
}
</style>
