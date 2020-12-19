$(document).ready(function () { 
    // Load Messages From Bottom
    $("#messages").scrollTop($("#messages")[0].scrollHeight);

    // Toggle Create Conversation Box
    $("#createConversation").click(function () {
        $("#blurForCreate").fadeIn();
        $("#createConversationBox").fadeIn();
        $("#searchUser").hide();
    });
    $("#closeCreateConversationBox").click(function () {
        $("#blurForCreate").fadeOut();
        $("#createConversationBox").fadeOut();
    });
    // Show Input For Searching User
    $("#singleConversation").click(function () {
        $("#searchUser").fadeIn();
    });
    // Create A New Conversation
    $(".getUserId").click(function (event) {
        // Get UserId That Wanted To Create Conversation With
        event.preventDefault();
        var index = $(".getUserId").index($(this));
        var userId = $(".getUserId").eq(index).attr('href');
        // Create A New Conversation
        let data = {
            'userId': userId
        };
        $.ajax({
            data: data,
            dataType: 'json',
            method: "POST",
            url: "/Messenger/NewConversation"
        }).done(function (response) {
            console.log(response);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus + ': ' + errorThrown);
        });
    });
});

setInterval(function () {
    getConversations();
}, 1000);

// Render Conversations
function getConversations() {
    $(document).ready(function () {
        $.ajax({
            dataType: 'json',
            method: "GET",
            url: "/Messenger/GetConversations"
        }).done(function (response) {
            var conversations = "";
            response.forEach(function (value, index, array) {
                conversations += "<li class='conversation'>"
                    + "<img src='./images/" + value['userAvatar'] + "' alt='avatar' />"
                    + "<span class='username'>" + value['userFullName'] + "</span>"
                    + "<span class='time'>2:09 PM</span>"
                    + "<span class='preview'>I was wondering...</span>"
                            + "</li>";
            });
            $("#conversationList").html(conversations);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            alert(textStatus + ': ' + errorThrown);
        });
    });
}