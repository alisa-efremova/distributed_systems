<!DOCTYPE html>
<html>
    <head>
        <meta charset="utf-8">
        <title>Poem result</title>
        <link rel="stylesheet" type="text/css" href="/css/style.css" media="all">
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js"></script>
        <script type="text/javascript" src="/script/load_poem.js"></script>
    </head>
    <body>
        <h4>Best lines of your poem</h4>
        <input type="hidden" id="corr_id" value="{$CORR_ID}">
        <img id="loading" src="/image/loading_spinner.gif" width="100px" height="100px" />
        <p id="poem"></p>
        <p id="poem_error" class="error"></p>
    </body>
</html>