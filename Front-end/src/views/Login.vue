<template>
  <div id="login-body">
    <img id="logo-login" src="../assets/pictures/Logo-4-rest-IT.png" alt="does not work" />
    <div id="login-container">
      <div id="background-circel">
        <div id="alignment-body">
          <div class="circle">
            <div id="profile-icon">
              <profile />
            </div>
          </div>
          <div class="input-container">
            <div id="icon" class="icon-profile-fill">
              <profileFill />
            </div>
            <input id="input-email" v-model="this.user.Email" type="text" placeholder="Email" required />
            <div class="space" id="eye-icon"></div>
          </div>
          <div class="input-container">
            <div id="icon" class="icon-lock">
              <lockClosed />
            </div>
            <input id="input-wachtwoord" v-model="this.user.Password" :type="this.inputType" placeholder="Wachtwoord"
              required />
            <a v-if="this.eyeCon == true" @click="eyeconChange" id="eye-icon">
              <eyeOpen />
            </a>
            <a v-else-if="this.eyeCon == false" class="eyeconClosed" @click="eyeconChange" id="eye-icon">
              <eyeClosed />
            </a>
          </div>
          <button @click="login()" class="login-button" type="button">Login</button>

          <div id="error-message">
            {{ this.errorMessage }}
          </div>
         <a href="/login/wachtwoord-vergeten" id="forgot-password"> Wachtwoord vergeten? </a>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from '../../axios-auth.js';
import profile from "../components/icons/iconloginprofile.vue";
import profileFill from "../components/icons/iconLoginProfileFill.vue";
import lockClosed from "../components/icons/iconLoginLockClosed.vue";
import eyeOpen from "../components/icons/iconLoginEyeOpen.vue";
import eyeClosed from "../components/icons/iconLoginEyeClosed.vue";

export default {
  name: "Login",
  components: {
    profile,
    profileFill,
    lockClosed,
    eyeOpen,
    eyeClosed,
  },
  data() {
    return {
      eyeCon: false,
      inputType: "password",
      errorMessage: "",
      user: {
        Email: '',
        Password: '',
      }
    };
  },
  methods: {
    login() {
      axios.post("login", this.user)
        .then((res) => {
          axios.defaults.headers.common['Authorization'] = "Bearer " + res.data;
          localStorage.setItem("jwt", res.data)
          localStorage.setItem("overviewType", "Overzicht")
          this.$router.push("/overzicht/documenten");
          console.log(res.data);
        }).catch((error) => {
          this.errorMessage = error.response.data
        });
    },
    eyeconChange() {
      if (this.eyeCon) {
        this.eyeCon = false;
        this.inputType = "password"
      }
      else {
        this.eyeCon = true;
        this.inputType = "text";
      }
    },
  },
};
</script>
   
<style>
@import '../assets/css/Main.css';
@import '../assets/css/Login.css';
</style>
