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
    sleep(2);
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

    $contents = ftp_nlist($conn_id, "./json");

    var_dump($contents);
// FTP 서버와 연결 끊음
    ftp_close($conn_id);
?>