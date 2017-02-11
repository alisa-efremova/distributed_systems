<?php
require_once 'common.inc.php';

if (!isUserInfoSaved())
{
  Echo "Cannot save user info.";
}

$corrId = getCorrId();
$userInfo = getUserInfo($corrId);

echo "Result:<br>";
foreach ($userInfo as $key => $value)
{
  echo "$key: $value<br>";
}

/* Helpers */
function isUserInfoSaved()
{
  if (!empty($_GET[PARAMETER_RESULT]))
  {
    return (bool)htmlspecialchars($_GET[PARAMETER_RESULT]);
  }
  else
  {
    die("Result is missing.");
  }
}

function getCorrId()
{
  if (!empty($_GET[PARAMETER_CORRID]))
  {
    return htmlspecialchars($_GET[PARAMETER_CORRID]);
  }
  else
  {
    die("Correlation id is not set");
  }
}

function getUserInfo($corrId)
{
  try
  {
    $redis = new Predis\Client(REDIS_PATH);
    $jsonData = $redis->get($corrId);
    $userInfo = json_decode($jsonData, true);
    return is_null($userInfo) ? array() : $userInfo;
  }
  catch (Exception $e)
  {
    die("Error while connecting Redis.");
  }
}
