<?php
// 안드로이드에서 넘어온 데이터라고 가정하고 직접 DB에 데이터 저장 테스트
// 제대로 저장되는지 확인했으면 아래 4줄은 주석처리 또는 삭제해야 함
$_POST['UserID'] = 'jsk005@gmail.com';
$_POST['name'] = '홍길동';
$_POST['email'] = 'jsk005@gmail.com';
$_POST['password'] = 'sk1234!';

extract($_POST);

require_once 'config/db_functions.php';
$db = new DB_Functions();

// json response array
$response = array("error" => FALSE);

if (isset($_POST['userID']) && isset($_POST['name']) && isset($_POST['email']) && isset($_POST['password'])) {

    // POST 배열로 받은 데이터
    $userID = $_POST['userID'];
    $name = $_POST['name'];
    $email = $_POST['email'];
    $password = $_POST['password'];
	$telNO = '02-1234-4567';
	$mobileNO = '010-1234-2580';

    // 동일한 userID 등록되어 있는지 체크
    if ($db->isUserExisted($userID)) { // E-Mail 이 key value
        // user already existed
        $response["error"] = TRUE;
        $response["error_msg"] = "User already existed with " . $userID;
        echo json_encode($response);
    } else {
        // 사용자 등록
        $user = $db->storeUser($userID, $name, $email, $password, $telNO, $mobileNO);
        if ($user) { // 사용자 등록 성공
            $response['error'] = FALSE;
            $response['user']['userID'] = $user['userID'];
            $response['user']['name'] = $user['userNM'];
            $response['user']['email'] = $user['email'];
            $response['user']['created_at'] = $user['created_at'];
            $response['user']['updated_at'] = $user['regdate'];
            echo json_encode($response);
        } else {
            // 사용자 등록 실패
            $response['error'] = TRUE;
            $response['error_msg'] = "Unknown error occurred in registration!";
            echo json_encode($response);
        }
    }
} else { // 입력받은 데이터에 문제가 있을 경우
    $response['error'] = TRUE;
    $response['error_msg'] = "Required parameters (name, email or password) is missing!";
    echo json_encode($response);
}
?>

