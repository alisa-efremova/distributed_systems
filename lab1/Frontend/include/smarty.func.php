<?php

function parseTemplate($templateName, $vars)
{
    $smarty = new Smarty();
    $smarty->setTemplateDir(TEMPLATE_PATH);
    $smarty->setCompileDir(TEMPLATE_COMPILE_PATH);
    $smarty->assign($vars);
    return $smarty->fetch($templateName);
}

function buildSubscriptionFormLayout($name, $email, $message)
{
    $templatePath = TEMPLATE_PATH . 'subscription_form.tpl';
    $vars = [
        'NAME'   => $name,
        'EMAIL'  => $email,
        'MESSAGE'=> $message
    ];
    echo parseTemplate($templatePath, $vars);
}

function buildSubscriptionResultLayout($name, $email, $result)
{
    $templatePath = TEMPLATE_PATH . 'subscription_result.tpl';
    $vars = [
        'NAME'   => $name,
        'EMAIL'  => $email,
        'RESULT' => $result
    ];
    echo parseTemplate($templatePath, $vars);
}