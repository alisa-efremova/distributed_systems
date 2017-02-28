<?php

function getRelativePath($url, $params)
{
  return "http://" . $_SERVER['HTTP_HOST']  . "/" . $url . "?" . http_build_query($params);
}

function isGet()
{
  return $_SERVER['REQUEST_METHOD'] == 'GET';
}

function isPost()
{
  return $_SERVER['REQUEST_METHOD'] == 'POST';
}

function getGetParam($name)
{
  return isset($_GET[$name]) ? trim($_GET[$name]) : "";
}

function getPostParam($name)
{
  return isset($_POST[$name]) ? trim($_POST[$name]) : "";
}
