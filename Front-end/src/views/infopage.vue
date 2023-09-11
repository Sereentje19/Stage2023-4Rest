<template>


<body>
        
        <div class="header">
      <img
        id="logoHeader"
        src="../assets/Pictures/Logo-4-rest-IT.png"
        alt="does not work"
      />
      <div id="buttonsHeader">
        <a href="/">Document uploaden</a>
        <a href="/">overzicht</a>
        <a href="/">Uitloggen</a>
      </div>
    </div>

    <div class="Info">
        <ul>
    

    
            <h1>
                 contract
            </h1>
<br>
<br>    
            <h3>
                 23-11-20  
<br>      
                 dit is over X dagen
            </h3>
<br>
<br>
            <h3>
                Serena Kenter
<br>
                serena@gmail.com
            </h3>
<br>
<br>
            <h3>
                opmerking
<br>
                tarief omhoog
            </h3>
        </ul>
</div>

        	
    <img src="../assets/pictures/contract.jpg" class="Foto"  alt="Document">


</body>
</template>
  
<style>
body {
  background-color: #d9d9d9;
  font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
}


#logoHeader{
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

.Overzicht{ 
position: fixed;
right: 545px;
top: 13px ;
font-size: 18px;
}

.Document{ 
    position: fixed;
    right: 255px;
    top: 13px ;
    font-size: 18px;


}




h1{
    position:fixed;
    left: 440px;
    top: 145px;
    font-size: 70px;
}
h3{
    position:relative;
    left: 400px;
    top: 180px;
    font-size: 25px;
}

.Foto{
    
    position:fixed;
    right: 20%;
    top: 200px;
    width:550px;
    height: 650px;
}
  

</style>