<?php
// 안드로이드에서 넘어온 데이터라고 가정하고 직접 DB에 데이터 저장 테스트
// 제대로 저장되는지 확인했으면 아래 4줄은 주석처리 또는 삭제해야 함
/*$_POST['userID'] = 'unhappy';
$_POST['Age'] = '24';
$_POST['Gender'] = 'Woman';
$_POST['userpw'] = 'sk1234!';
$_POST['LikeStyle'] = 'sk1234!';
*/


extract($_POST);

require_once 'config/db_functions.php';

$db = new DB_Functions();


// json response array
$response = array("error" => FALSE);

if (isset($_POST['userID']) && isset($_POST['Age']) && isset($_POST['Gender']) && isset($_POST['userpw']) && isset($_POST['LikeStyle'])) {

    // POST 배열로 받은 데이터
    $userID = $_POST['userID'];
    $Age = $_POST['Age'];
    $Gender = $_POST['Gender'];
    $password = $_POST['userpw'];
    $LikeStyle = $_POST['LikeStyle'];
    
    // 동일한 userID 등록되어 있는지 체크
    if ($db->isUserExisted($userID)) { // E-Mail 이 key value
        // user already existed
        $response["error"] = TRUE;
        $response["error_msg"] = "User already existed with " . $userID;
        echo json_encode($response);
    } else {
        // 사용자 등록
        
        $user = $db->storeUser($userID, $password, $LikeStyle, $Age, $Gender );
        
        if ($user) { // 사용자 등록 성공
            //storeUser($PID, $PPW, $LikeStyle, $Age, $Gender)
            $response['error'] = FALSE;
            $response['users']['PID'] = $user['PID'];
            $response['users']['PPW'] = $user['PPW'];
            $response['users']['LikeStyle'] = $user['LikeStyle'];
            $response['users']['Age'] = $user['Age'];
            $response['users']['Gender'] = $user['Gender'];

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

