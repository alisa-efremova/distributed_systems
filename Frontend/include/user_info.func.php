<?php

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
        return array();
    }
}

function saveUserInfo($corrId, $userInfo)
{
    $curl = new Curl\Curl();
    $curl->post(BACKEND_PATH . '/api/values/' . $corrId, '=' . json_encode($userInfo));
    $curl->close();
    return !$curl->error;
}