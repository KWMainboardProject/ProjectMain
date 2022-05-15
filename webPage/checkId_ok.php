<?php
    include 'config/db_connection.php';

    $userid = $_GET['userid'];
    $sql = "select PID from users where PID='$userid'";
    $result = mysqli_query($conn, $sql);
    $data = mysqli_fetch_array($result);

    echo isset($data['PID']) ? "X" : "O";
?>