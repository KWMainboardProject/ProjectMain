<?php 

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

// FTP 서버에 파일 전송
if (ftp_put($conn_id, $ftp_remote_file, $ftp_send_file, FTP_BINARY)) {
    print_r("파일 전송 (ftp) -> UPLOAD 성공");
} else {
    print_r("파일 전송 (ftp:".$conn_id.",".$ftp_remote_file.",".$ftp_send_file.") -> UPLOAD 실패");
}

// FTP 서버와 연결 끊음
ftp_close($conn_id);

?>