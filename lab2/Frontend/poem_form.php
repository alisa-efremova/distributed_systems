
<?php
require_once 'include/common.inc.php';

$poem    = "";
$message = "";

if (isPost())
{
    $poem  = getPostParam(KEY_POEM_TEXT);

    if (!empty($poem))
    {
        $corrId = uniqid();
        $isPoemSaved = savePoem($corrId, $poem);

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
buildPoemFormLayout($poem, $message);