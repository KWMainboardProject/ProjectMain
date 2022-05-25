<?php
    //$index = $_GET['index'];
    sleep(1);
    
    $json=file_get_contents("./json/Overall.json");

    echo($json);
?>