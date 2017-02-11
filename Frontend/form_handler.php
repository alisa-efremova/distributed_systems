
<?php
require_once 'common.inc.php';

if (!$_POST)
{
  die('POST data is expected.');
}

$name = htmlspecialchars($_POST[FORM_FIELD_NAME]);
$email = htmlspecialchars($_POST[FORM_FIELD_EMAIL]);

if (is_null($name) || is_null($email))
{
  die('Form parameters are not set correctly.');
}

$corrId = 'unique_id';
$result = saveUserInfo($corrId, [
  'name' => $name,
  'email' => $email
]);

$host = $_SERVER['HTTP_HOST'];
$uri = rtrim(dirname($_SERVER['PHP_SELF']), '/\\');
header("Location: http://$host$uri/form_result.php?" . PARAMETER_RESULT . "=$result&" . PARAMETER_CORRID . "=$corrId");
exit;

/* Helpers */
function saveUserInfo($corrId, $userInfo)
{
  $curl = new Curl\Curl();
  $curl->post(BACKEND_PATH . '/api/values/' . $corrId, '=' . json_encode($userInfo));
  $curl->close();
  return !$curl->error;
}
