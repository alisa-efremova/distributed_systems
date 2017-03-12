<!DOCTYPE html>
<html>
    <head>
        <meta charset="utf-8">
        <title>Stats</title>
        <link rel="stylesheet" type="text/css" href="/css/style.css" media="all">
    </head>
    <body>
    <h4>Stats</h4>
    {if {$MESSAGE} == ""}
        <p>Percent of good lines in your poems: {$GOOD_LINES}%.</p>
    {else}
        <p class="error">{$MESSAGE}</p>
    {/if}
    </body>
</html>