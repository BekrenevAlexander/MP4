﻿<%@ Page Title="Contact" Language="C#"  MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Game1.aspx.cs" Inherits="MP4_Durak.Game1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class='message'>
        <span>Ожидайте подключение другого игрока...</span>
        <div class="loader"></div>
    </div>

    <div class="gameField">
        <div class="protivnic">
            <div class="cards">

            </div>
        </div>
        <div class="pole">
            <div class="warField">
                <div class="post">
                    <div class="attackCard">
                        <div class="cardContent" style='z-index:1; background:url("/Images/piki12.jpg");'></div>
                    </div>
                    <div class="defenceCard">
                        <div class="cardContent" style='z-index:1; background:url("/Images/piki14.jpg");'></div>
                    </div>
                </div>
            </div>
            <div class="calloda">
                
            </div>
        </div>
        <div class="myField">
            <div class="cards">
                

            </div>
        </div>
    </div>

    <script>
        var roomId = window.localStorage.getItem('roomId');
        var player = window.localStorage.getItem('player');
        var countCardsProtivnic;
        var cards = [];
        var cardsOnTable = [];
        var atacker;
        var trump;
        if (roomId) {
            checkStartGame();
        }


        function checkStartGame() {
            var interval = setInterval(function () {
                $.ajax({
                    type: "get", url: `/api/rooms/getGame?roomId=${roomId}`,
                    success: function (data, text) {
                        if (data) {
                            $('.message').remove();
                            clearTimeout(interval);
                            startGame();
                        }
                    },
                    error: function (request, status, error) {
                        console.warn(error);
                    }
                });
            }, 2000);
        }
        
        function startGame() {
            getCards();
            getAllCardsOnTable();
            getAttacker();
            getTrump();
            getEnemyCardsCount();

            if (player == atacker) {
                view.infoBlock('Вы атакуете')
            } else {
                view.infoBlock('Вы защищаетесь')
            }
        }

        // Получить карты 
        function getCards() {
            $.ajax({
                type: "get", url: `/api/game/getCards?gameId=${roomId}`,
                success: function (data, text) {
                    console.warn('getCards', data);
                    cards = data;
                    view.userCards();
                },
                error: function (request, status, error) {
                    console.warn(error);
                }
            });
        }

        // Получить карты на столе
        function getAllCardsOnTable() {
            $.ajax({
                type: "get", url: `/api/game/getAllCardsOnTable?gameId=${roomId}`,
                success: function (data, text) {
                    console.warn('getAllCardsOnTable', data);
                    cardsOnTable = data;
                },
                error: function (request, status, error) {
                    console.warn(error);
                }
            });
        }

        // Узнать кто атакует
        function getAttacker() {
            $.ajax({
                type: "get", url: `/api/game/getAttacker?gameId=${roomId}`,
                success: function (data, text) {
                    console.warn('getAttacker', data);
                    atacker = data;
                },
                error: function (request, status, error) {
                    console.warn(error);
                }
            });
        }
        
        // Получить козырь
        function getTrump() {
            $.ajax({
                type: "get", url: `/api/game/getTrump?gameId=${roomId}`,
                success: function (data, text) {
                    console.warn('getTrump', data);
                    trump = data;
                    view.calods();
                },
                error: function (request, status, error) {
                    console.warn(error);
                }
            });
        }

        // Получить кол-вв корт противнка
        function getEnemyCardsCount() {
            $.ajax({
                type: "get", url: `/api/game/getEnemyCardsCount?gameId=${roomId}`,
                success: function (data, text) {
                    console.warn('getEnemyCardsCount', data);
                    countCardsProtivnic = data;
                    view.protivnicCards();
                },
                error: function (request, status, error) {
                    console.warn(error);
                }
            });
        }

        // отобра же ни е
        var view = {
            userCards: function () {
                var parent = $('.myField .cards').empty();
                console.warn(cards);
                $.each(cards, function (key, card) {
                    $('<div/>', { class: "card" }).append(
                        $('<div/>', { class: "cardContent", 'data-val': card, style: `background: url("/Images/${cardConvector(card)}.jpg")`})
                    ).appendTo(parent);
                    console.warn(cardConvector(card));
                });
            },

            protivnicCards: function () {
                var parent = $('.protivnic .cards').empty();
                console.warn(cards);
                for (var i = 0; i < countCardsProtivnic; i++) {
                    $('<div/>', { class: "card" }).append(
                        $('<div/>', { class: "cardContent", style: `background: url("/Images/back.jpg")`})
                    ).appendTo(parent);
                };
            },

            table: function () {

            },

            calods: function () {
                var parent = $('.calloda').empty();
                $('<div/>', { class: 'cosir' }).append(
                    $('<div/>', { class: "cardContent", style: `background: url("/Images/${cardConvector(parseInt(`${trump}3`))}.jpg")` })
                ).appendTo(parent);
                $('<div/>', { class: 'other' }).append(
                    $('<div/>', { class: "cardContent", style: `background: url("/Images/back.jpg")` })
                ).appendTo(parent);

            },

            infoBlock: function (mes) {
                $('.pole').append($('<div/>', { class: 'infoMessage' }).append(mes));
            }
        }

        function cardConvector(cardId) {
            var mast = {
                0: 'bubi',
                1: 'chervi',
                2: 'kresti',
                3: 'piki'
            }
                
            return mast[Math.floor(cardId / 10)] + (cardId % 10 + 5);
        }
    </script>

    <style>
        .gameField {
            height: calc(100vh - 141px)
        }

        .protivnic,
        .myField {
            width: 100%;
            height: 250px;
        }

        .pole {
            width: 100%;
            height: 350px;
        }

        .poleField {
            margin-left: calc(100% - 900px);
            margin-top: 50px;
            width:1000px;
        }

        .warField {
            display: table;
            min-width: 850px;
            width: 85%;
        }

        .post {
            display: table-cell;
        }

        .attackCard {
            position:relative;
            top: 0;
        }

        .defenceCard {
            position:relative;
            top: 50px;
            left: 50px;
        }

        .calloda {
            float:right;
            display: table;
            min-width: 150px;
            width: 15%;
        }

        .cosir {
            position:relative;
            display: inline-block;
        }

        .other {
                margin-left: 50px;
            position:relative;
            display: inline-block;
        }

        .cards {
            margin-left: calc(50% - 350px);
            height: 200px;
            width: 700px;
            display:table;
        }

        .card {
            display: table-cell;
            width: auto;
            position: relative;
            height:200px;
        }

        .cardContent {
            position: absolute;
            height:200px;
            width: 125px;
            top: 0;
            left: calc(50% - 35px);
            border: 1px #0000008a solid;
            border-radius: 15px;
            background-repeat: round !important;
            cursor: pointer;
            box-shadow: 3px 3px 20px 0px rgba(0, 0, 0, 0.18);
        }

        .myField .cardContent:hover {
            top: -50px;
            transition: all 0.5s;
        }

        .infoMessage {
            position: absolute;
            left: calc(50% - 75px);
            z-index: 100;
            width: 150px;
            text-align: center;
            background: #ffffff00;
            border: 1px solid #00000021;
            border-radius: 5px;
            padding: 5px;
            top: 601px;
            box-shadow: 2px 2px 6px 0px #00000038;
        }

        .message {
            background: white;
            z-index: 100;
            position: absolute;
            top: calc(50% - 75px);
            left: calc(50% - 150px);
            border: 1px solid #80808038;
            border-radius: 3px;
            width: 300px;
            height: 150px;
            text-align: center;
            vertical-align: middle;
            box-shadow: 3px 3px 20px 0px rgba(0, 0, 0, 0.18);
        }

        .message span {
            margin-top: 20px;
            display:block;
            font-size: 16px;
        }

        .message .loader {
            margin-left: 120px;
            margin-top: 12px;
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