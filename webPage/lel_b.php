<?php   
    sleep(1);

    if(file_exists("./json/Bottom.json")){
    $json=file_get_contents("./json/Bottom.json");

    echo($json);
    }
?>