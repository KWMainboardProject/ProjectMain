<?php
    include 'config/db_connection.php';

    $userid = $_GET['PID'];
    $sql = "select useridx from tb_user where userid='$userid'";
    $result = mysqli_query($conn, $sql);
    $data = mysqli_fetch_array($result);

    echo isset($data['useridx']) ? "X" : "O";
?>