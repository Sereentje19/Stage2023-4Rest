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

                    <button @click="send()" class="login-button" id="verstuur-button" type="button">Verstuur</button>
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
                Email: '',
                Password: '',
            }
        };
    },
    methods: {
        send() {
            this.$router.push("/");
        },
    },
};
</script>
     
<style>
@import '../assets/css/Main.css';
@import '../assets/css/Login.css';
</style>
  