<template>
    <body>
<<<<<<< Updated upstream
        
     <div class="header">
         
        <div class="row">
            <ul> 
             <img  src="../assets/pictures/Logo.png" class="Logo" alt="4-Rest">   
             <il><a class="Overzicht" href="Overzicht,vue"><h2>Overzicht</h2></a></il>
             <il><a class="Uitloggen" href="login.vue"><h2>Uitloggen</h2></a> </il>
            </ul>
            </div>
    </div>
<br>
=======
>>>>>>> Stashed changes

        <div class="header">

            <div class="row">
                <ul>
                    <img src="../views/Logo.png" class="Logo" alt="4-Rest">
                    <il><a class="Overzicht" href="Overzicht,vue">
                            <h2>Overzicht</h2>
                        </a></il>
                    <il><a class="Uitloggen" href="login.vue">
                            <h2>Uitloggen</h2>
                        </a> </il>
                </ul>
            </div>
        </div>
        <br>

        <div class="input">
            <h1>Document uploaden</h1>

            <div class="drop">

                <p>drag & drop</p>

            </div>






            <input type="file" style="display: none;" id="bestand" name="document">
            <label class="overlay" for="bestand">Selecteer een bestand</label>
        </div>



        <div class="Klant">
            <ul>

                <form class="gegevens" action="/action_page.php">


                    <input type="text" class="Zoek" placeholder="        Zoek klant" name="Naam"><br><br>
                    <input type="text" class="Naam" placeholder="Naam klant" name="Zoek"><br><br>
                    <input type="text" class="Email" placeholder="Email klant" name="Email"><br><br>
                    <input type="text" class="Type" placeholder="Type bestand" name="Type"><br><br>

                    <input type="submit" class="verstuur" value="Verstuur document">
                </form>
            </ul>
        </div>



    </body>
</template>

<style>
<<<<<<< Updated upstream
.Logo{
position:fixed;
left: 50px;
top: 2px;
height: 95px;
width: 150px;
}

.row {
position: fixed;
height: 100px;
width: 98.5%;
=======
.Logo {
    position: fixed;
    left: 50px;
    top: 11px;
    height: 95px;
    width: 150px;
}

.row {
    position: fixed;
    background-color: #22421F;
    height: 100px;
    width: 98.5%;
>>>>>>> Stashed changes
}

a {
    color: white;
    text-decoration: none;

}

<<<<<<< Updated upstream
.Overzicht{ 
position: fixed;
right: 250px;
top: 13px ;
font-size: 18px;
}


.Uitloggen{ 
position: fixed;
right: 100px;
top: 13px ;
font-size: 18px;
=======
.Overzicht {
    position: fixed;
    right: 250px;
    top: 20px;
    font-size: 18px;
}


.Uitloggen {
    position: fixed;
    right: 100px;
    top: 20px;
    font-size: 18px;
>>>>>>> Stashed changes

}

body {
    background-color: #D9D9D9;
    width: 100%;
}

.input {
    position: fixed;
    top: 200px;
    left: 200px;
    font-size: 20px;
    left: 200px;

}

h1 {
    position: relative;
    left: 150px;
}

p {
    position: relative;
    top: 45%;
    font-size: 40px;
}

.drop {

    position: relative;
    left: 150px;
    background-color: white;
    color: #717171;
    height: 400px;
    width: 400px;
    text-align: center;
}



.overlay {
    position: relative;
    top: 30px;
    left: 150px;
    background: #22421F;
    background-image: url(../components/icons/folder.png);
    background-repeat: no-repeat;
    background-position: 330px;
    background-size: 60px 50px;
    color: white;
    padding: 12px;
    font-size: 25px;
    width: 378px;
}

label {
    display: block;

}



.Zoek {

    position: fixed;
    top: 226px;
    right: 350px;
    background-image: url(../components/icons/lens.png);
    background-position: 10px;
    background-size: 45px 45px;
    background-color: #F4F4F4;
    background-repeat: no-repeat;
    border-color: black;
    padding: 12px;
    font-size: 23px;
    width: 350px;

}

.Naam {
    position: fixed;
    top: 362px;
    right: 350px;
    background-color: #F4F4F4;
    border-color: black;
    font-size: 23px;
    padding: 12px;
    width: 450px;
}

.Email {

    position: fixed;
    top: 440px;
    right: 350px;
    background-color: #F4F4F4;
    border-color: black;
    font-size: 23px;
    padding: 12px;
    width: 450px;
}

.Type {
    position: fixed;
    top: 515px;
    right: 350px;
    background-color: #F4F4F4;
    border-color: black;
    font-size: 23px;
    padding: 12px;
    width: 450px;
}




.verstuur {
    position: fixed;
    top: 745px;
    right: 350px;
    font-size: 20px;
    background-color: #22421F;
    background-image: url('searchicon.png');
    color: white;
    padding: 12px;

}
</style>
<script setup>

function dragstart_handler(ev) {
    // Add the target element's id to the data transfer object
    ev.dataTransfer.setData("text/plain", ev.target.id);
}

window.addEventListener("DOMContentLoaded", () => {
    // Get the element by id
    const element = document.getElementById("p1");
    // Add the ondragstart event listener
    element.addEventListener("dragstart", dragstart_handler);
});

</script>