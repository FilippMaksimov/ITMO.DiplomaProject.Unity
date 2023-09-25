<?php
$db = mysql_connect('localhost', 'root', 'password') or ("Cannot connect: ".mysql_error());

if (!$db)
	die('Could not connect: '.mysql_error());
	
mysql_select_db('game', $db) or die ("Could not load the database: ".mysql_error());
?>