<?php
require_once('db.php');

$username = $_POST['user_name'];
$sql = "select money from users where user_name = '".$username."'";
$result = mysql_query($sql);

if(mysql_num_rows($result) > 0) {
    while($row = mysql_fetch_assoc($result)) {
        echo $row['money'];
    }
}
else {
    echo "NOTHING FOUND";
}
?>