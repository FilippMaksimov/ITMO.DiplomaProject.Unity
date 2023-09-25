<?php
require_once('db.php');

$username = $_POST['user_name'];
$sql = "SELECT a.user_name, c.character_name, MAX(b.grade)
    FROM users_characters b
    LEFT JOIN users a ON b.user_id = a.user_id
    JOIN characters c ON b.character_id = c.character_id
    WHERE a.user_name = '".$username."'
    GROUP BY 1,2
    ORDER BY 2";
$result = mysql_query($sql);

if(mysql_num_rows($result) > 0) {
    while($row = mysql_fetch_assoc($result)) {
        echo $row['character_name'] . ';';
    }
}
else {
    echo "NOTHING FOUND";
}
?>