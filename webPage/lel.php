<?php
    //$index = $_GET['index'];
    sleep(3);
    
    $desc = ['Main Category', 'Sub Category', 'Style', 'Pattern'];
    $name = ['Top', 'Pullover', 'Casual', 'Solid'];

    $json = json_encode(array('desc' => $desc, 'name' => $name));

    echo($json);
?>