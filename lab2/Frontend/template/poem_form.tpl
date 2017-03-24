<!DOCTYPE html>
<html>
    <head>
        <meta charset="utf-8">
        <title>Poem beautifier</title>
        <link rel="stylesheet" type="text/css" href="/css/style.css" media="all">
    </head>
    <body>
    <h4>Poem beautifier</h4>
    <p>Let's leave only best lines of your poem</p>
    <form action="/poem_form.php" method="post">
        <textarea class="text" cols="60" rows ="15" name="poemText">{$POEM}</textarea><br>
        {html_options name=userId options=$USERS selected=$USER_ID}
        <input type="submit" class="button" value="Delete bad lines">
    </form>
    <p class="error">{$MESSAGE}</p>
    </body>
</html>