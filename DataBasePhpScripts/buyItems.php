<?php
$username = $_POST['user_name'];
$itemId = $_POST['item_id'];
$money = $_POST['money'];
$userID;
require_once('db.php');

$sql = "select user_id from users where user_name = '".$username."'";
$result = mysql_query($sql);
if(mysql_num_rows($result) > 0) {
    while($row = mysql_fetch_assoc($result)) {
        $userID = $row['user_id'];
    }
}

$upd = mysql_query("UPDATE users SET money = ".$money." WHERE user_name = '".$username."'");
$ins = mysql_query("INSERT INTO users_items VALUES (".$userID.", ".$itemId.")");

if ($ins && $upd)
	die ("Succesfully purchased!");
else
	die ("Error: " . mysql_error());

if (!$ins)
    die ("Error: " . mysql_error());
?>