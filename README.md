# ChatRoom
This is not complete implementation of a Chatroom.
It developed with usage of ASP.NET Core 2.2 framework.
Back-End developed as a bundle of REST APIs.
Front-End is based on AJAX technology. 
SignalR Core 1.4 library was used for instant notification exchange between different clients.
Application compiled with Swagger, so for testing and review APIs just run application and follow the following link:
https://localhost:port_number/swagger/index.html

At the beginning a few users should be registered, after that they can Log-in to system, create Chatrooms and add other users to Chatrooms.
User who created the Chatroom is Owner of this Chatroom. Only an Owner can delete Chatroom and add participants.
Any Participant can Leave any Chatroom.
At the left side user see Chatrooms where s/he is Owner or Participant. Owned Chatrooms appears at the top of the list.
Every Chatroom has delete action (X sign after name). For Owned Chatrooms it is Deleting of entire Chatroom with the list of Participants.
For Chatrooms where user is just Participant it is Leaving functionality (Alert messages mention about it).
When user selects a Chatroom, on the Right side appears the list of Participants. Only Owner of the Chatroom can Add and Remove other participants. And Owner can't remove itself.
When owner deletes some Chatroom, other users get notified about it through SignalR infrastructure, and the Chatroom automatically removes from their views as well.
When some Participant leaves a Chatroom, s/he removes from the lists of other users as well (again SignalR).

Project implemented with usage of Repository pattern. Authorization/Authentication implemented with standard Identity Server (which comes out of the box when you create ASP.NET Core MVC Application).
