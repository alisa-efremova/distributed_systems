<!DOCTYPE html>
<html>
    <head>
        <meta charset="utf-8">
        <title>Subscription result</title>
        <link rel="stylesheet" type="text/css" href="/css/style.css" media="all">
    </head>
    <body>
    <form action="/subscription_form.php" method="post">
        {if $RESULT}
            <h4>You successfully subscribed</h4>
            <p>
                Name: <b>{$NAME}</b><br>
                Email: <b>{$EMAIL}</b>
            </p>
        {else}
            <p class="error">Error occurred.</p>
        {/if}
    </form>
    </body>
</html>