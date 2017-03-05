<?php

function getPoem($corrId)
{
    try
    {
        $redis = new Predis\Client(REDIS_PATH);
        return $redis->get($corrId);
    }
    catch (Exception $e)
    {
        return NULL;
    }
}

function savePoem($corrId, $poem)
{
    $curl = new Curl\Curl();
    $curl->post(BACKEND_PATH . '/api/poem/' . $corrId, '=' . $poem);
    $curl->close();
    return !$curl->error;
}