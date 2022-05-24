<?php
$JsonParser = file_get_contents("json/fashion.json");
$data = json_decode($JsonParser , true);
//print_r($data);
//echo $data;
//var_dump($JsonParser,true);
foreach ($data as $key=> $data1) {
    if(is_array($data1)==1){
        echo $key," >>>";
    foreach($data1 as $key=>$data2){
        echo $key," >>> ";
        if(is_array($data2)==1){
            echo $key," >>> ";
            foreach($data2 as $key=>$data3){
                if(is_array($data3)==1){
                    echo $key," >>> ";
                    foreach($data3 as $key=>$data4){
                        if(is_array($data4)==1){
                            echo $key," >>> ";
                            foreach($data4 as $key=>$data5){
                                if(is_array($data5)==1){
                                    echo $key," >>>";
                                    foreach($data5 as $key=>$data6){
                                        echo $key," : ";
                                        echo $data6, "<br>";
                                    }
                                }
                                echo $key, " : ";
                                echo $data5, "<br>";
                            }
                        }
                    echo $key, " : ";
                    echo $data4, "<br>";
                    }
                }    
                echo $key, " : ";
                echo $data3, "<br>";
            }
            }
        echo $key, " : ";
        echo $data2, "<br>";
    }
    }
    
    echo $key, " : ";
    echo $data1, "<br>";
}
?>