<?php

function getPoemInfo($corrId)
{
    $curl = new Curl\Curl();
    $curl->get(RESULT_POEM_SERVICE_PATH . '/api/goodPoem/' . $corrId);
    $curl->close();
    if ($curl->error)
    {
        return ["Error" => true];
    }
    else
    {
        return json_decode(json_decode($curl->response, true), true);
    }
}

function savePoem($userId, $corrId, $poem)
{
    $curl = new Curl\Curl();
    $params = json_encode([
        'CorrId' => $corrId,
        'UserId' => $userId,
        'Poem' => $poem
    ]);
    $ch = curl_init(POEM_BEAUTIFIER_SERVICE_PATH . '/api/poem');
    curl_setopt($ch, CURLOPT_CUSTOMREQUEST, "POST");
    curl_setopt($ch, CURLOPT_POSTFIELDS, $params);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_HTTPHEADER, array(
            'Content-Type: application/json',
            'Content-Length: ' . strlen($params))
    );
    curl_exec($ch);
    $curl->close();
    return !$curl->error;
}

function getStats()
{
    $curl = new Curl\Curl();
    $curl->get(STATS_SERVICE_PATH . '/api/stats');
    $curl->close();
    if ($curl->error)
    {
        return NULL;
    }
    else
    {
        return json_decode(json_decode($curl->response, true), true);
    }
}