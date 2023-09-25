<?php
require_once('db.php');

$username = $_POST['user_name'];
$characterName = $_POST['character_name'];
$grade;

$sql = "SELECT a.user_name, c.character_name, MAX(b.grade) AS max_grade
    FROM users_characters b
    LEFT JOIN users a ON b.user_id = a.user_id
    JOIN characters c ON b.character_id = c.character_id
    WHERE a.user_name = '".$username."' AND c.character_name = '".$characterName."' 
    GROUP BY 1,2
    ORDER BY 2";
$result = mysql_query($sql);
if(mysql_num_rows($result) > 0) {
    while($row = mysql_fetch_assoc($result)) {
        $grade = $row['max_grade'];
    }
}
else {
    echo "NOTHING FOUND;";
}

$sql1 = "SELECT a.user_name, c.character_name, b.grade, b.power, b.health, b.speed
    FROM users_characters b
    LEFT JOIN users a ON b.user_id = a.user_id
    JOIN characters c ON b.character_id = c.character_id
    WHERE a.user_name = '".$username."' AND c.character_name = '".$characterName."' AND b.grade = ".$grade."";
$result1 = mysql_query($sql1);
if(mysql_num_rows($result1) > 0) {
    while($row = mysql_fetch_assoc($result1)) {
        echo $row['health'] .';'. $row['power']. ';' . $row['speed']. ';';
    }
}
else {
    echo "NOTHING FOUND;";
}

$sql2 = "SELECT * FROM users WHERE user_name = '".$username."'";
$result2 = mysql_query($sql2);
if(mysql_num_rows($result2) > 0) {
    while($row = mysql_fetch_assoc($result2)) {
        echo $row['level_id'] .';'. $row['score'];
    }
}
else {
    echo "NOTHING FOUND;";
}
?>