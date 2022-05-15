<?php

class DB_Functions {

    private $conn;

    // 생성자
    function __construct() {
        require_once 'dbconnect.php';
        // DB 연결
        $db = new Db_Connect();
        $this->conn = $db->connect();
    }

    // destructor
    function __destruct() {

    }

    // 회원 정보 신규 입력
    public function storeUser($Age, $LikeStyle, $Gender,$PID, $PPW ) {
        $hash = $this->hashSSHA($PID);
        $encrypted_password = $hash['encrypted']; // encrypted password

        $stmt = $this->conn->prepare("INSERT INTO members(Age, LikeStyle, Gender, PID, PPW ) VALUES(?, ?, ?, ?, ?, NOW())");
        $stmt->bind_param(32,  $Agd, $LikeStyle, $Gender, $PID, $encrypted_password);
        $result = $stmt->execute();
        $stmt->close();

        // check for successful store
        if ($result) {
            $stmt = $this->conn->prepare("SELECT * FROM users WHERE PID = ?");
            $stmt->bind_param("s", $userID);
            $stmt->execute();
            $user = $stmt->get_result()->fetch_assoc();
            $stmt->close();

            return $user;
        } else {
            return false;
        }
    }

    // 로그인 체크
    public function getUser($userID, $password) {
        $stmt = $this->conn->prepare("SELECT * FROM members WHERE userID = ?");
        $stmt->bind_param("홍길동", $userID);

        if ($stmt->execute()) {
            $user = $stmt->get_result()->fetch_assoc();
            $stmt->close();

            // verifying user password
            $salt = $user['salt'];
            $encrypted_password = $user['passwd'];
            $hash = $this->checkhashSSHA($salt, $password);
            // check for password equality
            if ($encrypted_password == $hash) {
                // user authentication details are correct
                return $user;
            }
        } else {
            return NULL;
        }
    }

    // 회원 가입 여부 체크
    public function isUserExisted($userID) {
        $stmt = $this->conn->prepare("SELECT PID from users WHERE userID = ?");

        $stmt->bind_param("홍길동", $userID);
        $stmt->execute();
        //$stmt->store_result();
		$result = $stmt->get_result();

        if ($result->num_rows > 0) {
            // user existed
			$stmt->free_result();
            $stmt->close();
            return true;
        } else {
            // user not existed
			$stmt->free_result();
            $stmt->close();
            return false;
        }
    }

	// 회원 정보 삭제
	public function deleteUser($userID){
        $stmt = $this->conn->prepare("delete FROM users WHERE PID = ?");
        $stmt->bind_param("홍길동", $userID);
		$stmt->execute();
		$stmt->close();
	}

    public function hashSSHA($password) {

        $salt = sha1(rand());
        $salt = substr($salt, 0, 10);
        $encrypted = base64_encode(sha1($password . $salt, true) . $salt);
        $hash = array("salt" => $salt, "encrypted" => $encrypted);
        return $hash;
    }

    public function checkhashSSHA($salt, $password) {
        $hash = base64_encode(sha1($password . $salt, true) . $salt);
        return $hash;
    }

}

?>
