<?php
$username = $_POST['user_name'];
$characterID = $_POST['character_id'];
$money = $_POST['money'];
$userID;
require_once('db.php');

$power;
$health;
$speed;
if ($characterID == "1")
{
    $power = "25";
    $health = "100";
    $speed = "5.5";
}
else if ($characterID == "2")
{
    $power = "30";
    $health = "200";
    $speed = "6.0";
}


$sql = "select user_id from users where user_name = '".$username."'";
$result = mysql_query($sql);
if(mysql_num_rows($result) > 0) {
    while($row = mysql_fetch_assoc($result)) {
        $userID = $row['user_id'];
    }
}

$check = mysql_query("SELECT b.user_name, a.character_id, a.grade 
    FROM users_characters a 
    LEFT JOIN users b ON a.user_id = b.user_id
    WHERE b.user_name = '".$username."' AND a.character_id = ".$characterID."");

$numrows = mysql_num_rows($check);

if ($numrows == 0)
{
	$upd = mysql_query("UPDATE users SET money = ".$money." WHERE user_name = '".$username."'");
    $ins = mysql_query("INSERT INTO users_characters (`user_id`, `character_id`, `grade`, `power`, `health`, `speed`)
        VALUES (".$userID.", ".$characterID.", 1, ".$power.", ".$health.", ".$speed.")");

	if ($ins && $upd)
		die ("Succesfully purchased!");
	else
		die ("Error: " . mysql_error());

    if (!$ins)
        die ("Error: " . mysql_error());
}
else if ($numrows == 1)
{
    $check1 = mysql_query("SELECT user_name, level_id FROM users 
        WHERE user_name = '".$username."' AND level_id = 2");
    $numrows1 = mysql_num_rows($check1);
    if ($numrows1 == 0)
    {
	    die("You have not reach level 2 for for upgrading");
    }
    else 
    {
        $upd = mysql_query("UPDATE users SET money = ".$money." WHERE user_name = '".$username."'");
        $ins = mysql_query("INSERT INTO users_characters (`user_id`, `character_id`, `grade`, `power`, `health`, `speed`)
        VALUES (".$userID.", ".$characterID.", 2, ".$power." + 5, ".$health." * 1.25, ".$speed." + 0.2)");
	    if ($ins && $upd)
		    die ("Succesfully purchased!");
	    else
		    die ("Error: " . mysql_error());
    }
}
else if ($numrows == 2)
{
    $check2 = mysql_query("SELECT user_name, level_id FROM users 
        WHERE user_name = '".$username."' AND level_id = 3");
    $numrows2 = mysql_num_rows($check2);
    if ($numrows2 == 0)
    {
	    die("You have not reach level 3 for for upgrading");
    }
    else 
    {
        $upd = mysql_query("UPDATE users SET money = ".$money." WHERE user_name = '".$username."'");
        $ins = mysql_query("INSERT INTO users_characters (`user_id`, `character_id`, `grade`, `power`, `health`, `speed`)
        VALUES (".$userID.", ".$characterID.", 3, ".$power." + 10, ".$health." * 1.5, ".$speed." + 0.5)");
	    if ($ins && $upd)
		    die ("Succesfully purchased!");
	    else
		    die ("Error: " . mysql_error());
    }
}
?>