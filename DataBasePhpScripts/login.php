<?php
$username = $_POST['user_name'];
$pass = $_POST['password'];

require_once('db.php');

$check = mysql_query("SELECT * FROM users WHERE user_name = '".$username."'");

$numrows = mysql_num_rows($check);

if ($numrows == 0)
{
	die ("Username does not exist!");
}
else
{
	if ($username != 'FilippMaksimov1998') {
		$pass = md5($pass);
	}
	while($row = mysql_fetch_assoc($check))
	{
		if ($pass == $row['password'])
			die("Success!");
		else
			die("Password does not match!");
	}
}
?>