<template>
  <div id="login-body">
    <img id="logo-login" :src="require('@/assets/pictures/Logo-4-rest-IT.png')" alt="does not work" />
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
              <profileFill style="color: #6b6b6b;" />
            </div>
            <input id="input-email" v-model="this.user.Email" type="text" placeholder="Email" required />
            <div class="space" id="eye-icon"></div>
          </div>
          <div class="input-container">
            <div id="icon" class="icon-lock">
              <lockClosed style="color: #6b6b6b;" />
            </div>
            <input id="input-wachtwoord" class="password-margin" v-model="this.user.Password" :type="this.inputType"
              placeholder="Wachtwoord" required />
            <a v-if="this.eyeCon == true" @click="eyeconChange" id="eye-icon">
              <eyeOpen />
            </a>
            <a v-else-if="this.eyeCon == false" @click="eyeconChange" id="eye-icon">
              <eyeClosed />
            </a>
          </div>
          <button @click="login()" class="login-button" type="button">Login</button>

          <div id="error-message">
            {{ this.errorMessage }}
          </div>
          <a href="/login/wachtwoord-vergeten" id="forgot-password">Wachtwoord vergeten? </a>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from '../../axios-auth.js';
import VueJwtDecode from 'vue-jwt-decode';
import profile from "../components/icons/IconLoginProfile.vue";
import profileFill from "../components/icons/IconLoginProfileFill.vue";
import lockClosed from "../components/icons/iconLoginLockClosed.vue";
import eyeOpen from "../components/icons/IconLoginEyeOpen.vue";
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
      },
      currentUser: {
        email: "",
        name: "",
        userId: 0
      }
    };
  },
  methods: {
    login() {
      axios.post("user/login", this.user)
        .then((res) => {
          axios.defaults.headers.common['Authorization'] = "Bearer " + res.data;
          localStorage.setItem("jwt", res.data)
          localStorage.setItem("overviewType", "Overzicht")

          const token = res.data;
          const decodedToken = VueJwtDecode.decode(token);

            this.currentUser.email = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];
            this.currentUser.name = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
            this.currentUser.userId = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];

          localStorage.setItem("currentUser", JSON.stringify(this.currentUser));

          this.$router.push("/overzicht/documenten");
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
