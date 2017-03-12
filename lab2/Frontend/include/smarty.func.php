<?php

function parseTemplate($templateName, $vars)
{
    $smarty = new Smarty();
    $smarty->setTemplateDir(TEMPLATE_PATH);
    $smarty->setCompileDir(TEMPLATE_COMPILE_PATH);
    $smarty->assign($vars);
    return $smarty->fetch($templateName);
}

function buildPoemFormLayout($poem, $message)
{
    $templatePath = TEMPLATE_PATH . 'poem_form.tpl';
    $vars = [
        'POEM'   => $poem,
        'MESSAGE'=> $message
    ];
    echo parseTemplate($templatePath, $vars);
}

function buildPoemResultLayout($corrId)
{
    $templatePath = TEMPLATE_PATH . 'poem_result.tpl';
    $vars = [
        'CORR_ID' => $corrId
    ];
    echo parseTemplate($templatePath, $vars);
}

function buildStatsLayout($goodLinesPercent, $message)
{
    $templatePath = TEMPLATE_PATH . 'stats.tpl';
    $vars = [
        'GOOD_LINES'   => $goodLinesPercent,
        'MESSAGE'=> $message
    ];
    echo parseTemplate($templatePath, $vars);
}
