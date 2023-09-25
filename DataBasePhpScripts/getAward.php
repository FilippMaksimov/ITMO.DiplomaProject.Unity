<?php
require_once('db.php');

$username = $_POST['user_name'];
$level = $_POST['level_id'];
$score = $_POST['score'];

$upd = mysql_query("UPDATE users SET level_id = ".$level.", score = ".$score.",
    money = money + 500 WHERE user_name = '".$username."'");
if ($upd)
	die ("Reward is granted");
else
	die ("Error: " . mysql_error());
?>