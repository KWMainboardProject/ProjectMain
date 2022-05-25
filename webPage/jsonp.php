<?php if(file_exists("./json/Bottom.json")){
        $JsonParser = file_get_contents("./json/Bottom.json");
        $obj = json_decode($JsonParser);
        
        
        //print_r($obj);
        echo $obj->prop[0];
        echo $obj->prop[1]; 
        echo $obj->prop[4];
            
    }else{echo "no";}
    /*$response = file_get_contents("./json/j.json");
    
    $obj = json_decode($response);
 
echo $obj->startDate . "<br>";
echo $obj->endDate . "<br>";
echo $obj->timeUnit . "<br>";
echo $obj->results[0]->title . "<br>";
echo $obj->results[0]->category[0] . "<br>";
echo ": " . $obj->results[0]->data[0]->period . "<br>";
echo ": " . $obj->results[0]->data[0]->ratio . "<br>";
echo ": " . $obj->results[0]->data[1]->period . "<br>";
echo ": " . $obj->results[0]->data[1]->ratio . "<br>";
 
echo "<br>";
 
echo $obj->results[1]->title . "<br>";
echo $obj->results[1]->category[0] . "<br>";
echo ": " . $obj->results[1]->data[0]->period . "<br>";
echo ": " . $obj->results[1]->data[0]->ratio . "<br>";
echo ": " . $obj->results[1]->data[1]->period . "<br>";
echo ": " . $obj->results[1]->data[1]->ratio . "<br>";
  */  ?>