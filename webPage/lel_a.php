<?php
    sleep(1);

    if(file_exists("./json/Overall.json")){
    $json=file_get_contents("./json/Overall.json");

    echo($json);
    }
?>