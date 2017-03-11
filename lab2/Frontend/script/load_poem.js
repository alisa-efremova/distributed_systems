$(document).ready(function(){
    var corrId = $("#corr_id").val();
    getPoem(corrId);
});

function getPoem(corrId) {
    $.ajax({
        url: "get_poem.php",
        data: { corr_id: corrId },
        type: "POST",
        context: document.body
    }).done(function(responseJSON) {
        console.log(responseJSON);
        var response = jQuery.parseJSON(responseJSON);
        handleResponse(response.result, response.poem);
    }).fail(function() {
        handleResponse(false, "");
    });
}

function handleResponse(result, poem)
{
    if (result)
    {
        $("#loading").hide();
        $("#poem").html(poem == "" ? "Sorry, all lines are bad" : nl2br(poem));
    }
    else
    {
        $("#loading").hide();
        $("#poem_error").html("Timeout expired. Unable to get poem.");
    }
}

function nl2br(str)
{
    // Inserts HTML line breaks before all newlines in a string
    return str.replace(/([^>])\n/g, '$1<br/>');
}
