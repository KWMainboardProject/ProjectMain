<?php
    session_start();

	//exit;
    $Random_str = uniqid();
    
	if($_FILES['upload_file']['size'] > 0) {

		$file_tmp_name = $_FILES['upload_file']['tmp_name'];
		$save_filename = $_SERVER['DOCUMENT_ROOT'] . "/imgwork/" . $Random_str .".". substr($_FILES['upload_file']['name'],-3,3);
        
		$file_upload	= move_uploaded_file($file_tmp_name, $save_filename);

	} else {
		echo "failed";
	}
    
    require_once 'config/ftp_connect.php';

    // FTP서버 접속
    $conn_id = ftp_connect($ftp_server);

    // FTP서버 접속 실패한 경우
    if($conn_id == false){
        print_r("[IP:".$ftp_server.":".$ftp_port ."] FTP 서버 접속 실패");
    }

    // FTP서버 로그인
    $login_result = ftp_login($conn_id, $ftp_user_name, $ftp_user_pass);

    // 로그인 실패한 경우
    if($login_result == false){
        print_r("[IP:".$ftp_server.":".$ftp_port ."], [USER:".$ftp_user_name."] 로그인 실패");
    }

    // 패시브 모드 설정
    ftp_pasv($conn_id, true);

    $filePathA = "./json/".$Random_str."_a.json";
    $filePathB = "./json/".$Random_str."_b.json";
    $filePathO = "./json/".$Random_str."_o.json";
    $filePathT = "./json/".$Random_str."_t.json";
    $a = 0;
    while(!file_exists($filePathA) && !file_exists($filePathB) && !file_exists($filePathO) && !file_exists($filePathT)){

        sleep(1);
        
        $a = $a +1;
        if($a==20){
            break;
        }        
    }
    sleep(1);
    $contents = [];
    if(file_exists($filePathA)){
    $JsonParser = file_get_contents("$filePathA");
    $data = json_decode($JsonParser);
    $overallarr=[];
    array_push($overallarr,'Overall');
    array_push( $overallarr,$data->Overall->subcategory->classfication);
    array_push( $overallarr,$data->Overall->maincolor->rgb);
    array_push( $overallarr,$data->Overall->subcolor->rgb);
    array_push( $overallarr,$data->Overall->pattern->classfication);
    array_push( $overallarr,$data->Overall->style->classfication);
    
    array_push($contents,'Maincategory','Subcategory','MainColor','SubColor','Pattern','Style');
        
        $json = json_encode(array('type' => $contents, 'prop'=> $overallarr));
        file_put_contents("./json/Overall.json", $json);

        unlink($filePathA);
    }
    $contents = [];
    if(file_exists($filePathB)){
        $JsonParser = file_get_contents("$filePathB");
        $data = json_decode($JsonParser);
        $bottomarr=[];
        array_push($bottomarr,'Bottom');
        array_push( $bottomarr,$data->Bottom->subcategory->classfication);
        array_push( $bottomarr,$data->Bottom->maincolor->rgb);
        array_push( $bottomarr,$data->Bottom->subcolor->rgb);
        array_push( $bottomarr,$data->Bottom->pattern->classfication);
        array_push( $bottomarr,$data->Bottom->style->classfication);
        array_push($contents,'Maincategory','Subcategory','MainColor','SubColor','Pattern','Style');
        
        $json = json_encode(array('type' => $contents, 'prop'=> $bottomarr));
        file_put_contents("./json/Bottom.json", $json);

        unlink($filePathB);
    }
    $contents = [];
    if(file_exists($filePathO)){
        $JsonParser = file_get_contents("$filePathO");
        $data = json_decode($JsonParser);
        $outerarr=[];
        array_push($outerarr,'Outer');
        array_push( $outerarr,$data ->Outer->subcategory->classfication);
        array_push( $outerarr,$data ->Outer->maincolor->rgb);
        array_push( $outerarr,$data ->Outer->subcolor->rgb);
        array_push( $outerarr,$data ->Outer->pattern->classfication);
        array_push( $outerarr,$data ->Outer->style->classfication);
        array_push($contents,'Maincategory','Subcategory','MainColor','SubColor','Pattern','Style');
        
        $json = json_encode(array('type' => $contents, 'prop'=> $outerarr));
        file_put_contents("./json/Outer.json", $json);

        unlink($filePathO);
                
    }
    $contents = [];
    if(file_exists($filePathT)){
        $JsonParser = file_get_contents("$filePathT");
        $data = json_decode($JsonParser);
        $toparr=[];
        array_push($toparr,'Top');
        array_push( $toparr,$data ->Top->subcategory->classfication);
        array_push( $toparr,$data ->Top->maincolor->rgb);
        array_push( $toparr,$data ->Top->subcolor->rgb);
        array_push( $toparr,$data ->Top->pattern->classfication);
        array_push( $toparr,$data ->Top->style->classfication);
        array_push($contents,'Maincategory','Subcategory','MainColor','SubColor','Pattern','Style');
        
        $json = json_encode(array('type' => $contents, 'prop'=> $toparr));
        file_put_contents("./json/Top.json", $json);

        unlink($filePathT);
            
    }
    //$json = json_encode($contents);
    
    //echo($json);
    
    //var_dump($contents);
// FTP 서버와 연결 끊음
    ftp_close($conn_id);

    





?>