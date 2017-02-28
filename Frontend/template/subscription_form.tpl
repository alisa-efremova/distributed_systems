<!DOCTYPE html>
<html>
    <head>
        <meta charset="utf-8">
        <title>Subscription</title>
        <link rel="stylesheet" type="text/css" href="/css/style.css" media="all">
    </head>
    <body>
    <h4>Subscription</h4>
    <p>Enter your name and email to subscribe to our newsletters</p>
    <form action="/subscription_form.php" method="post">
        <input name="name" placeholder="Name" value="{$NAME}">
        <input name="email" placeholder="Email" value="{$EMAIL}">
        <input type="submit" value="Submit">
    </form>
    <p class="error">{$MESSAGE}</p>
    </body>
</html>