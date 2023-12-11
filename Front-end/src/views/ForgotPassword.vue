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
                            <profileFill  style="color: #6b6b6b;"/>
                        </div>
                        <input v-if="isSend == false" id="input-email" v-model="this.user.email" type="text"
                            placeholder="Email" required />
                        <input v-else id="input-email" v-model="this.code" type="text" placeholder="Code" required />
                        <div class="space" id="eye-icon"></div>
                    </div>

                    <button v-if="isSend == false" @click="sendEmail()" class="login-button" id="verstuur-button"
                        type="button">Verstuur</button>
                    <button v-else @click="sendCode()" class="login-button" id="verstuur-button"
                        type="button"> Verander wachtwoord</button>
                    <div id="error-message">
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
                email: '',
            },
            code: "",
            isSend: false
        };
    },
    methods: {
        sendEmail() {
            axios.get("forgot-password",
                {
                    params: {
                        email: this.user.email,
                    },
                })
                .then((res) => {
                    console.log(res.data);
                    this.isSend = true;
                    this.errorMessage = "";
                }).catch((error) => {
                    this.errorMessage = error.response.data
                });
        },
        sendCode() {
            axios.get("forgot-password/check-code", {
                    params: {
                        email: this.user.email,
                        code: this.code,
                    },
                })
                .then((res) => {
                    console.log(res.data);
                    localStorage.setItem("userEmail", this.user.email)
                    this.$router.push("/login/wachtwoord-vergeten/nieuw-wachtwoord/" + this.code);
                }).catch((error) => {
                    this.errorMessage = error.response.data
                });
        },
    },
};
</script>
     
<style>
@import '../assets/css/Main.css';
@import '../assets/css/Login.css';
</style>
  