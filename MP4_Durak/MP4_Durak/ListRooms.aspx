<%@ Page Title="Комнаты" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListRooms.aspx.cs" Inherits="MP4_Durak.Rooms" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Доступные комнаты:</h2>

    <div class='mainContent'>
        <div class="loader"></div>
    </div>

    <script>
        var rooms = [];

        setInterval(function () {
            $.ajax({
                type: "get", url: "/api/rooms/get",
                success: function (data, text) {
                    viewData(data);
                },
                error: function (request, status, error) {
                    console.warn(error);
                    viewData([]);
                }
            });
        }, 3000);

        function viewData(data) {
            if (data && data.length) {

                if (!$('.contentBlock').length) {
                    var content = $('<div/>', { class: 'contentBlock' });
                    var list = $('<ul/>', { class: 'listRooms' }).appendTo(content);
                    $('.mainContent').empty().append(content);
                }

                updateTAble(rooms, data);
            } else {
                rooms = [];
                var content = $('<span/>', { class: 'message' }).append('Созданных нет комнат :(');

                $('.mainContent').empty().append(content);
            }

            function updateTAble(originData, newData) {
                newData.map(function(item) {
                    var res = originData.find(function (oItem) {
                        return oItem.Id == item.Id;
                    })

                    if (!res) {
                        originData.push(item);
                        var room = $('<span/>', { class: 'room' }).append(`Комната ${originData.length}`);
                        var player = $('<span/>', { class: 'player' }).append(`Игрок: ${item.Name}`);
                        $('.listRooms').append(
                            $('<li/>', { class: 'listRoomsItem', 'data-id': item.Id }).append(room).append(player).on('click', selectRoom)
                        )
                    }

                    function selectRoom(e) {
                        var roomId = e.target.dataset.id;
                        if (roomId) {
                            $.ajax({
                                type: "post", url: `/api/rooms/connect?roomId=${roomId}`,
                                success: function (data, text) {
                                    window.localStorage.setItem('roomId', roomId);
                                    window.localStorage.setItem('player', false);
                                    location = '/Game1';
                                },
                                error: function (request, status, error) {
                                    console.warn(error);
                                }
                            })
                        }
                    }
                });

                for (var i = originData.length - 1; i >= 0; i--) {
                    var res = newData.find(function (oItem) {
                        return oItem.Id == originData[i].Id;
                    })

                    if (!res) {
                        $(`[data-id="${originData[i].Id}"]`).remove();
                        originData.split(i, 1);
                    }
                };
            }
        }
    </script>

    <style>
        .message {
            margin: 30px;
            font-size: 20px;
        }

        .mainContent {
            height: calc(100vh - 205px);
        }

        .contentBlock {
            overflow:hidden;
        }

        .listRooms {
            list-style: none;
            padding: 0;
        }

        .listRoomsItem {
            cursor: pointer;
            padding: 10px;
            border-top: 1px solid #00000030;
            border-bottom: 1px solid #00000030;
        }

        .listRoomsItem:hover {
            background: #f0f8ff73;
        }

        .listRoomsItem span {
            display: inline-block;
            font-size: 16px;
            min-width: 200px;
        }

        .loader {
            margin-left: calc(50% - 25px);
            margin-top: 50px;
            display: block;
            width: 50px;
            height: 50px;
            border: 2px transparent solid;
            border-bottom-color: blue;
            border-top-color: black;
            border-radius: 50%;

            animation-name: spin;
            animation-duration: 1000ms;
            animation-iteration-count: infinite;
            animation-timing-function: linear;
        }
        @keyframes spin {
            from {
                transform:rotate(0deg);
            }
            to {
                transform:rotate(360deg);
            }
        }
    </style>
</asp:Content>
