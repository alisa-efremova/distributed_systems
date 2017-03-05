$(document).ready(function(){
    var corrId = $("#corr_id").val();
    getPoem(corrId);
});

var poem = "";
const maxRetryCount = 10;
const delayMs = 1000;
var currentRetryCount = 0;

function getPoem(corrId) {
    $.ajax({
        url: "get_poem.php",
        data: { corr_id: corrId },
        type: "POST",
        async: false,
        context: document.body
    }).done(function(responseJSON) {
        console.log(responseJSON);
        var response = jQuery.parseJSON(responseJSON);
        poem = response.poem;
        handleResponse(response.result, corrId);
    }).fail(function() {
        handleResponse(false, corrId);
    });
}

function handleResponse(result, corrId)
{
    if (result)
    {
        $("#loading").hide();
        $("#poem").html(poem == "" ? "Sorry, all lines are bad" : nl2br(poem));
    }
    else
    {
        currentRetryCount++;
        if (currentRetryCount < maxRetryCount)
        {
            setTimeout(function() {getPoem(corrId)}, delayMs);
        }
        else
        {
            $("#loading").hide();
            $("#poem_error").html("Timeout expired. Unable to get poem.");
        }
    }
}

function nl2br(str)
{
    // Inserts HTML line breaks before all newlines in a string
    return str.replace(/([^>])\n/g, '$1<br/>');
}
