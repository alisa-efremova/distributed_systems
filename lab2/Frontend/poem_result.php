<?php
require_once 'include/common.inc.php';

$corrId = "";

if (isGet())
{
    $corrId = getGetParam(PARAMETER_CORRID);
}
buildPoemResultLayout($corrId);
