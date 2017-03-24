
<?php
require_once 'include/common.inc.php';

$poem    = "";
$message = "";
$selectedUser = "user1";
$users = [
    "user1" => "User 1",
    "user2" => "User 2",
    "user3" => "User 3",
    "user4" => "User 4"
];

if (isPost())
{
    $poem  = getPostParam(KEY_POEM_TEXT);
    $selectedUser = getPostParam(KEY_USER_ID);
    if (!empty($poem))
    {
        $corrId = uniqid();
        $isPoemSaved = savePoem($selectedUser, $corrId, $poem);

        if ($isPoemSaved)
        {
            header("Location: " . getRelativePath('poem_result.php', [PARAMETER_CORRID => $corrId]));
            exit;
        }
        else
        {
            $message = "Sorry, cannot process poem. No connection.";
        }
    }
    else
    {
        $message = "Please enter anything to the form.";
    }
}
buildPoemFormLayout($poem, $message, $users, $selectedUser);