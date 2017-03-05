<?php
require_once 'include/common.inc.php';

if (isPost())
{
    $corrId = getPostParam(PARAMETER_CORRID);
    $poem = getPoem($corrId);
    $result = [];
    if (is_null($poem))
    {
        $result = [
            'result' => false,
            'poem'   => ""
        ];
    }
    else
    {
        $result = [
            'result' => true,
            'poem'   => $poem
        ];
    }
    echo json_encode($result);
}