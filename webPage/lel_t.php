<?php
     sleep(1);

     if(file_exists("./json/Top.json")){
     $json=file_get_contents("./json/Top.json");
 
     echo($json);
     }
?>