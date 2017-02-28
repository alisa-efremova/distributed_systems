<?php
require_once 'include/common.inc.php';

$name    = "";
$email   = "";
$result = false;

if (isGet())
{
    $corrId = getGetParam(PARAMETER_CORRID);
    $userInfo = getUserInfo($corrId);
    if (!empty($userInfo))
    {
        $result = true;
        $name = htmlspecialchars($userInfo[KEY_NAME]);
        $email = htmlspecialchars($userInfo[KEY_EMAIL]);
    }
}
buildSubscriptionResultLayout($name, $email, $result);
