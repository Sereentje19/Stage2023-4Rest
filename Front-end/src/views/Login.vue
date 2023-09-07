<template>
  <body>
    <div class="loginContainer">
      <input class="inputFieldLogin" type="text" placeholder="Email" required />
      <input
        class="inputFieldLogin"
        type="text"
        placeholder="Wachtwoord"
        required
      />
    </div>
  </body>
</template>

<style>
.loginContainer {
  background-color: aqua;
  height: 500px;
  width: 30%;
  display: flex;
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%,-50%);
}

.inputFieldLogin {
  height: 30px;
  width: 250px;
}

body {
  background-color: bisque;
  position: relative;
}
</style>
