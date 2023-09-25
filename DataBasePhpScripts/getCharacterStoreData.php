<?php
$username = $_POST['user_name'];
$characterID = $_POST['character_id'];
require_once('db.php');

$check = mysql_query("SELECT b.user_name, a.character_id, a.grade 
    FROM users_characters a 
    LEFT JOIN users b ON a.user_id = b.user_id
    WHERE b.user_name = '".$username."' AND a.character_id = ".$characterID."");

$numrows = mysql_num_rows($check);

if ($numrows == 0)
{
	die("Buy");
}
else if ($numrows == 1)
{
    die("Update");
}
else if ($numrows == 2)
{
    die("Update");
}
else if ($numrows == 3)
{
    die("Max");
}
?>