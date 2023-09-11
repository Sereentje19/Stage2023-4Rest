<template>


    <body>
        
     <div class="header">
         
        <div class="row">
            <ul> 
             <img  src="../assets/pictures/Logo.png" class="Logo" alt="4-Rest">   
             <il><a class="Overzicht" href="Overzicht,vue"><h2>Overzicht</h2></a></il>
             <il><a class="Document" href="Document.vue"><h2>Document Uploaden</h2></a> </il>
             <il><a class="Uitloggen" href="login.vue"><h2>Uitloggen</h2></a> </il>
            </ul>
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
.Logo{

    position:fixed;
    left: 50px;
    top: 3px;
    height: 95px;
    width: 150px;
}
.row {
    position: fixed;
    height: 100px;
    width: 98.5%;
}

a{
    color: white;
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

.Uitloggen{ 
position: fixed;
right: 100px;
top: 13px ;
font-size: 18px;


} 

body{
    background-color: #D9D9D9;
    width: 100%;
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
    top: 270px;
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
  