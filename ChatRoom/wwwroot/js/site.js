// SignalR part - Start
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.on("ChatroomDeleted", function (chatroomID) {
    $("#chatroom_" + chatroomID).remove();
    if (selectedChatroomId === chatroomID) {
        selectChatroom(0, false);
    }
});

connection.on("UserAdded", function (chatroomID, participantID, userName) {
    if ($("#chatroom_" + chatroomID).length && selectedChatroomId === chatroomID) {
        const participantsList = $("#participants");
        const crItem = $("<div id='participant_" + participantID + "' class='list-item'></div>")
            .append($("<label>" + userName + "</label>"));
        crItem.prependTo(participantsList);
    }
});

connection.on("UserLeft", function (chatroomID, participantID) {
    if ($("#chatroom_" + chatroomID).length
        && selectedChatroomId === chatroomID
        && $("#participant_" + participantID).length
    ) {
        $("#participant_" + participantID).remove();
    }
});

connection.start().then(function () {
    // Add some code here for initializing the client after start
}).catch(function (err) {
    return console.error(err.toString());
});
// SignalR part - End

$(document).ready(function () {
    getChatrooms();

    $("#cancelChatroom").click(function () {
        $("#addChatroom").modal('hide');
    });

    $("#cancelParticipant").click(function () {
        $("#addParticipant").modal('hide');
    });

    $("#addNewParticipant").hide();
});

var selectedChatroomId = 0;

function getChatrooms() {
    $.ajax({
        type: "GET",
        url: "api/Chatroom",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "Json",
        success: function (response) {
            const chatroomList = $("#chatrooms");
            $(chatroomList).empty();
            response.forEach(function (item) {
                var deleteOperation;
                if (item.isOwner === true) {
                    deleteOperation = "deleteChatroom";
                }
                else {
                    deleteOperation = "deleteMe";
                }
                const crItem = $("<div id='chatroom_" + item.id + "' class='list-item'></div>")
                    .append($("<a onclick='selectChatroom(" + item.id + ", " + item.isOwner + ")'>" + item.title + "</a>"))
                    .append($("<a onclick='" + deleteOperation + "(" + item.id + ")'>&#10006;</a>"));

                crItem.appendTo(chatroomList);
            });
        }
    });
}

function getParticipants() {
    const participantsList = $("#participants");
    $(participantsList).empty();

    var data = { chatroomID: selectedChatroomId };
    $.ajax({
        type: "GET",
        url: "api/Participant/GetChatroomParticipants?" + jQuery.param(data),
        accepts: "application/json",
        contentType: "application/json; charset=utf-8",
        dataType: "Json",
        success: function (response) {
            var deleteAllowed = false;
            response.forEach(function (item) {
                const crItem = $("<div id='participant_" + item.id + "' class='list-item'></div>")
                    .append($("<label>" + item.userName + "</label>"));

                if (item.deletable === true) {
                    deleteAllowed = true;
                    crItem.append($("<a onclick='deleteParticipant(" + item.id + "," + deleteAllowed + ")'>&#10006;</a>"));
                }

                crItem.appendTo(participantsList);
            });
        }
    });
}

function selectChatroom(chatroomID, isOwner) {
    if (selectedChatroomId !== 0) {
        $("#chatroom_" + selectedChatroomId).css("background-color", "white");
    }
    if (chatroomID !== 0) {
        $("#chatroom_" + chatroomID).css("background-color", "lightskyblue");
    }

    selectedChatroomId = chatroomID;

    if (isOwner) {
        $("#addNewParticipant").show();
    }
    else {
        $("#addNewParticipant").hide();
    }

    getParticipants();
}

function deleteChatroom(chatroomID) {
    if (confirm("Are you sure that you want to delete this Chatroom?")) {
        var data = { id: chatroomID };
        $.ajax({
            url: "api/Chatroom?" + jQuery.param(data),
            type: "DELETE",
            accepts: "application/json",
            contentType: "application/json",
            success: function (response) {
                $("#chatroom_" + chatroomID).remove();
                if (selectedChatroomId === chatroomID) {
                    selectChatroom(0, false);
                }
                connection.invoke("DeleteChatroom", chatroomID).catch(function (err) {
                    return console.error(err.toString());
                });
            }
        });
    }
}

function deleteMe(chatroomID) {
    if (confirm("Are you sure that you want to leave this Chatroom?")) {
        var data = { chatroomID: chatroomID };
        $.ajax({
            url: "api/Participant/LeaveChatroom?" + jQuery.param(data),
            type: "GET",
            accepts: "application/json",
            contentType: "application/json",
            success: function (response) {
                $("#chatroom_" + chatroomID).remove();
                selectChatroom(0, false);
                connection.invoke("LeaveChatroom", chatroomID, response.id).catch(function (err) {
                    return console.error(err.toString());
                });
            }
        });
    }
}

function deleteParticipant(participantID) {
    if (confirm("Are you sure that you want to delete this Participant?")) {
        var data = { id: participantID };
        $.ajax({
            url: "api/Participant?" + jQuery.param(data),
            type: "DELETE",
            accepts: "application/json",
            contentType: "application/json",
            success: function (response) {
                $("#participant_" + participantID).remove();
            }
        });
    }
}

function addChatroom() {
    $("#addChatroom").modal('show');
}

function addParticipant() {
    $("#users").empty();
    var data = { chatroomID: selectedChatroomId };
    $.ajax({
        type: "GET",
        url: "api/Participant/GetChatroomNonParticipants?" + jQuery.param(data),
        accepts: "application/json",
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            const userList = $("#users");
            response.forEach(function (item) {
                const uItem = $("<option value='" + item.userID + "'>" + item.userName + "</option>");
                uItem.appendTo(userList);
            });
        }
    });

    $("#addParticipant").modal('show');
}

function postChatroom() {
    var chatroomData = {
        title: $("#cahtroomTitle").val()
    };
    $.ajax({
        url: "api/Chatroom",
        type: "POST",
        accepts: "application/json",
        contentType: "application/json",
        data: JSON.stringify(chatroomData),
        dataType: "Json",
        success: function (response) {
            $("#addChatroom").modal('hide');
            var deleteOperation;
            if (response.isOwner === true) {
                deleteOperation = "deleteChatroom";
            }
            else {
                deleteOperation = "deleteMe";
            }

            const crItem = $("<div id='chatroom_" + response.id + "' class='list-item'></div>")
                .append($("<a onclick='selectChatroom(" + response.id + ", true)'>" + response.title + "</a>"))
                .append($("<a onclick='" + deleteOperation + "(" + response.id + ")'>&#10006;</a>"));

            const chatroomList = $("#chatrooms");
            crItem.prependTo(chatroomList);
        },
        error: function (response) {
        }
    });
}

function postParticipant(chatroomID) {
    const participantsList = $("#participants");
    var participants =
    {
        chatroomID: selectedChatroomId,
        users: []
    };

    $.each($("#users option:selected"), function () {
        participants.users.push({ userID: $(this).val(), userName: $(this).text() });
    });

    $.ajax({
        url: "api/Participant/CreateRange",
        type: "POST",
        accepts: "application/json",
        contentType: "application/json",
        data: JSON.stringify(participants),
        dataType: "Json",
        success: function (response) {
            $("#addParticipant").modal('hide');
            response.forEach(function (item) {
                const crItem = $("<div id='participant_" + item.id + "' class='list-item'></div>")
                    .append($("<label>" + item.userName + "</label>"));

                if (item.deletable === true) {
                    deleteAllowed = true;
                    crItem.append($("<a onclick='deleteParticipant(" + item.id + "," + deleteAllowed + ")'>&#10006;</a>"));
                }

                crItem.prependTo(participantsList);

                connection.invoke("AddedToChatroom", selectedChatroomId, item.id, item.userName).catch(function (err) {
                    return console.error(err.toString());
                });
            });
        },
        error: function (response) {
        }
    });
}
