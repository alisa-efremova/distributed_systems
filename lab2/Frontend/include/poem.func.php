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

function savePoem($corrId, $poem)
{
    $curl = new Curl\Curl();
    $curl->post(POEM_BEAUTIFIER_SERVICE_PATH . '/api/poem/' . $corrId, '=' . $poem);
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