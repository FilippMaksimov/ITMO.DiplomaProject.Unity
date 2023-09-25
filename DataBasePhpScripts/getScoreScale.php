<?php
require_once('db.php');

$levelId = $_POST['level_id'];
$sql = "SELECT * FROM levels WHERE level_id = ".$levelId."";
$result = mysql_query($sql);

if(mysql_num_rows($result) > 0) {
    while($row = mysql_fetch_assoc($result)) {
        echo $row['max_score'];
    }
}
else {
    echo "NOTHING FOUND";
}
?>