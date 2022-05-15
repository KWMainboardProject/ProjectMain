<?php

class DB_Connect {
    private $conn;

    // DB 연결
    public function connect() {
        require_once 'config.php';

        // MySQL DB 연결
        $this->conn = new mysqli(DB_HOST, DB_USER, DB_PASSWORD, DB_DATABASE);

        // return database handler
        return $this->conn;
    }
}

?>
