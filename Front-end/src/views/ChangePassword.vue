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
                        <div id="icon" class="icon-lock">
                            <lockClosed />
                        </div>
                        <input id="input-wachtwoord" v-model="this.user.password1" :type="this.inputType"
                            placeholder="Wachtwoord" required />
                        <a v-if="this.eyeCon == true" @click="eyeconChange" id="eye-icon">
                            <eyeOpen />
                        </a>
                        <a v-else-if="this.eyeCon == false" class="eyeconClosed" @click="eyeconChange" id="eye-icon">
                            <eyeClosed />
                        </a>
                    </div>
                    <div class="input-container">
                        <div id="icon" class="icon-lock">
                            <lockClosed />
                        </div>
                        <input id="input-wachtwoord" class="password-margin" v-model="this.user.password2"
                            :type="this.inputType" placeholder="Wachtwoord" required />
                        <a v-if="this.eyeCon == true" @click="eyeconChange" id="eye-icon">
                            <eyeOpen />
                        </a>
                        <a v-else-if="this.eyeCon == false" class="eyeconClosed" @click="eyeconChange" id="eye-icon">
                            <eyeClosed />
                        </a>
                    </div>
                    <button @click="changePassword()" class="login-button" type="button">Opslaan</button>

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
    props: {
        code: Number,
    },
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
                email: localStorage.getItem("userEmail"),
                password1: '',
                password2: '',
                code: this.code
            }
        };
    },
    methods: {
        changePassword() {
            axios.post("forgot-password", this.user
                )
                .then((res) => {
                    console.log(res.data);
                    this.$router.push("/");
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
  