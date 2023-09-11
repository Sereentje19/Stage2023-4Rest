<template>
  <body>
    <div class="loginBody">
      <img
        id="logoLogin"
        src="../assets/Pictures/Logo-4-rest-IT.png"
        alt="does not work"
      />
      <div class="loginContainer">
        <div id="backgroundCircel">
          <div id="alignmentBody">
            <div class="cirkel">
              <div id="profileIcon">
                <profile />
              </div>
              <img src="../components/icons/iconLoginProfile.svg" alt="" />
            </div>
            <div class="inputContainer">
              <div id="profileFillIcon">
                <profileFill />
              </div>
              <input id="inputEmail" type="text" placeholder="Email" required />
              <div class="divSpace" id="eyeIcon"></div>
            </div>
            <div class="inputContainer">
              <div id="profileFillIcon">
                <lockClosed />
              </div>
              <input
                id="inputWachtwoord"
                type="text"
                placeholder="Wachtwoord"
                required
              />
              <div id="eyeIcon">
                <eyeOpen />
              </div>
            </div>
            <button @click="login()" class="loginButton" type="button">Login</button>
          </div>
        </div>
      </div>
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
    eyeClosed,
  },
  methods:{
    login(){
      this.$router.push("/overzicht");
    }
  }
};
</script>

<style>
#backgroundCircel {
  width: 100%;
  height: 90%;
  border-radius: 140% 0 15% 15%;
  background-color: rgb(150, 172, 144);
  margin-left: auto;
  margin-top: auto;
}

#alignmentBody {
  margin-top: -110px;
  margin-left: auto;
  margin-right: auto;
}

#logoLogin {
  width: 150px;
  padding: 1% 0% 0% 1.5%;
}

.divSpace {
  width: 30px;
}

.inputContainer {
  display: flex;
  margin-left: 10%;
  margin-right: 13%;
  margin-bottom: 7%;
}

#eyeIcon {
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
  background-color: #153912;
  border-radius: 100%;
  display: inline-block;
  margin-left: auto;
  margin-right: auto;
  margin-top: -60px;
  display: flex;
  margin-bottom: 70px;
}

.loginContainer {
  background-color: #aec5a6;
  border-radius: 10%;
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
  font-size: 25px;
  background-color: #153912;
  border: none;
  color: white;
  margin-left: 10%;
  cursor: pointer;
}

#inputWachtwoord,
#inputEmail {
  border-color: rgb(205, 205, 205);
  background-color: rgb(205, 205, 205);
  border: none;
  outline: none;
  width: 100%;
  height: 50px;
  font-size: 22px;
  padding-left: 10px;
}

#inputWachtwoord {
  margin-bottom: 80px;
}

body {
  margin: 0px;
}

.loginBody {
  background-image: url("../assets/Pictures/green-background-blur2.png");
  background-size: 100% 100%;
  position: absolute; 
  height: 100%; 
  width: 100%;
}
</style>
