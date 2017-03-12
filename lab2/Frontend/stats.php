<?php
require_once 'include/common.inc.php';

$goodLinesPercent = "";
$message = "";

$stats = getStats();
if (is_null($stats))
{
    $message = "No connection.";
}
else
{
    $goodLinesPercent = round($stats * 100);
}

buildStatsLayout($goodLinesPercent, $message);
