<?php
// 안드로이드에서 넘어온 데이터라고 가정하고 직접 DB에 데이터 저장 테스트
// 제대로 저장되는지 확인했으면 아래 4줄은 주석처리 또는 삭제해야 함

$_POST['userid'];
$_POST['userpw'];


extract($_POST);

require_once 'config/db_functions.php';

$db = new DB_Functions();


// json response array
$response = array("error" => FALSE);

if (isset($_POST['userid']) && isset($_POST['userpw'])) {

    // POST 배열로 받은 데이터
    $userID = $_POST['userid'];
    $password = $_POST['userpw'];
      
    $user = $db->getUser($userID, $password);
        
    if ($user) { // 사용자 등록 성공
        //storeUser($PID, $PPW, $LikeStyle, $Age, $Gender)
        $response['error'] = FALSE;
        $response['users']['PID'] = $user['PID'];
        $response['users']['PPW'] = $user['PPW'];

        echo $_POST["userid"] ."님으로 로그인하였습니다.";

    } else {
        // 로그인 실패
        $response['error'] = TRUE;
        $response['error_msg'] = "비밀번호가 틀렸습니다.";
        echo "비밀번호가 틀렸습니다.";
    }

} else { // 입력받은 데이터에 문제가 있을 경우
    $response['error'] = TRUE;
    $response['error_msg'] = "fail to registration because input data be incorrected";
    echo json_encode($response);
}
?>

