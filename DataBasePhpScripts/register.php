<?php
$new_username = $_POST['name'];
$pass = $_POST['password'];

require_once('db.php');

$check = mysql_query("SELECT * FROM users WHERE `user_name`='".$new_username."'");

$numrows = mysql_num_rows($check);

if ($numrows == 0)
{
	$pass = md5($pass);
	$ins = mysql_query("INSERT INTO  users(`user_name`, `password`, `admin`, `level_id`, `score`, `money`) 
        VALUES ( '".$new_username."' ,  '".$pass."' , false, 1, 0, 1000);");
	
	if ($ins)
		die ("Succesfully Created User!");
	else
		die ("Error: " . mysql_error());
}
else
{
	die("User allready exists!");
}
?>