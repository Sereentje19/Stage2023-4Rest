<template>
    <div id="loginBody">
      <img id="logoLogin" src="../assets/Pictures/Logo-4-rest-IT.png" alt="does not work" />
      <div id="loginContainer">
        <div id="backgroundCircel">
          <div id="alignmentBody">
            <div class="cirkel">
              <div id="profileIcon">
                <profile />
              </div>
            </div>
            <div class="inputContainer">
              <div id="icon" class="iconProfileFill">
                <profileFill />
              </div>
              <input id="inputEmail" v-model="this.user.Email" type="text" placeholder="Email" required />
              <div class="divSpace" id="eyeIcon"></div>
            </div>
            <div class="inputContainer">
              <div id="icon" class="iconLock">
                <lockClosed />
              </div>
              <input id="inputWachtwoord" v-model="this.user.Password" :type="this.inputType" placeholder="Wachtwoord" required />
              <a v-if="this.eyeCon == true" @click="eyeconChange" id="eyeIcon">
                <eyeOpen />
              </a>
              <a v-else-if="this.eyeCon == false" class="eyeconClosed" @click="eyeconChange" id="eyeIcon">
                <eyeClosed />
              </a>
            </div>
            <button @click="login()" class="loginButton" type="button">Login</button>
            <div id="errorMessage"> 
              {{ this.errorMessage }}
            </div>
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
      axios.post("Login", this.user)
        .then((res) => {
          axios.defaults.headers.common['Authorization'] = "Bearer " + res.data;
          localStorage.setItem("jwt", res.data)
          this.$router.push("/overzicht");
          console.log(res.data);
        }).catch((error) => {
          this.errorMessage = error.response.data
        });
    },
    eyeconChange(){
      if(this.eyeCon)
      {
        this.eyeCon = false;
        this.inputType = "password"
      }
      else{
        this.eyeCon = true;
        this.inputType = "text";
      }
    },
  },
};
</script>
   
<style>
@import '../assets/Css/Main.css';
@import '../assets/Css/Login.css';
</style>
