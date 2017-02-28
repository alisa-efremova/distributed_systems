
<?php
require_once 'include/common.inc.php';

$name    = "";
$email   = "";
$message = "";

if (isPost())
{
    $name  = getPostParam(KEY_NAME);
    $email = getPostParam(KEY_EMAIL);

    if (!empty($name) && !empty($email))
    {
        $corrId = 'unique_id';
        $isUserInfoSaved = saveUserInfo($corrId, [
            KEY_NAME  => $name,
            KEY_EMAIL => $email
        ]);

        if ($isUserInfoSaved)
        {
            header("Location: " . getRelativePath('subscription_result.php', [PARAMETER_CORRID => $corrId]));
            exit;
        }
        else
        {
            $message = "Sorry, cannot save subscription. No connection.";
        }
    }
    else
    {
        $message = "Please fill all fields.";
    }
}
buildSubscriptionFormLayout($name, $email, $message);