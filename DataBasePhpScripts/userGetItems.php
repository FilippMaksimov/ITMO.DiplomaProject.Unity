<?php
require_once('db.php');

$username = $_POST['user_name'];

$sql1 = "SELECT a.user_name, b.item_id 
    FROM game.users_items b
    LEFT JOIN game.users a ON a.user_id = b.user_id
    WHERE a.user_name = '".$username."' AND b.item_id = 1";
$result1 = mysql_query($sql1);

$sql2 = "SELECT a.user_name, b.item_id 
    FROM game.users_items b
    LEFT JOIN game.users a ON a.user_id = b.user_id
    WHERE a.user_name = '".$username."' AND b.item_id = 2";
$result2 = mysql_query($sql2);

$sql3 = "SELECT a.user_name, b.item_id 
    FROM game.users_items b
    LEFT JOIN game.users a ON a.user_id = b.user_id
    WHERE a.user_name = '".$username."' AND b.item_id = 3";
$result3 = mysql_query($sql3);

$answer = (string)mysql_num_rows($result1).';'.(string)mysql_num_rows($result2).';'.(string)mysql_num_rows($result3);

echo $answer;
?>