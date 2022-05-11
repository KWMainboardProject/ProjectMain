<?php
    $id = $_POST['id'];
    $pass = $_POST['pass'];

    $con = mysqli_connect("xxxx");
    $sql = "SELECT * FROM user WHERE PID='$id'";

    $result = mysqli_query($con,$sql);

    $num_match = mysqli_num_rows($result);

    if(!$num_match){
        echo("
            <script>
                window.alert('등록되지 않은 아이디입니다!')
                history.go(-1)
            </script>
            ");
    } else{
        $row = mysqli_fetch_array($result);
        $db_pass = $row['PPW'];

        // 수정 부분 : $db -> $con
        mysql_close($con);
        /*로그인 화면에서 전송된 패스와 DB 패스 해쉬 비교
        */
        if(!password_verify($pass,$db_pass)){
            echo("
                <script>
                 window.alert('비밀번호가 틀립니다!')
                 history.go(-1)
                 </script>
            ");
            exit;
        } else {
            session_start();
            $_SESSION["userid"] = $row["PID"];
            $_SESSION["username"] = $row["PPW"];
            echo("
                <script>
                    location.href = 'list.php';
                </script>"
            );
        }
    }
    ?>