<?php
require_once('db.php');

$username = $_POST['user_name'];
$explspent = $_POST['expl_spent'];
$healspent = $_POST['heal_spent'];
$buffspent = $_POST['buff_spent'];
$userID;

$sql = "select user_id from users where user_name = '".$username."'";
$result = mysql_query($sql);
if(mysql_num_rows($result) > 0) {
    while($row = mysql_fetch_assoc($result)) {
        $userID = $row['user_id'];
    }
}

if ($explspent != "0")
{
    $del1 = mysql_query("DELETE FROM users_items WHERE user_id = ".$userID." AND item_id = 1 LIMIT ".$explspent."");
    if ($del1)
		die ("Explossive Potion has been succesfully removed from DB!");
	else
		die ("Error: " . mysql_error());
}
if ($healspent != "0")
{
    $del2 = mysql_query("DELETE FROM users_items WHERE user_id = ".$userID." AND item_id = 2 LIMIT ".$healspent."");
    if ($del2)
		die ("Healing potion has been succesfully removed from DB!");
	else
		die ("Error: " . mysql_error());
}
if ($buffspent != "0")
{
    $del3 = mysql_query("DELETE FROM users_items WHERE user_id = ".$userID." AND item_id = 3 LIMIT ".$buffspent."");
    if ($del2)
		die ("Buffing potion has been succesfully removed from DB!");
	else
		die ("Error: " . mysql_error());
}
?>