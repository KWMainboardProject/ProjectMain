<?php
    sleep(1);

    if(file_exists("./json/Outer.json")){
    $json=file_get_contents("./json/Outer.json");

    echo($json);
    }
?>