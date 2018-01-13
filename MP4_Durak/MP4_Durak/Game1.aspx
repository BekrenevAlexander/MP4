<%@ Page Title="Contact" Language="C#"  MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Game1.aspx.cs" Inherits="MP4_Durak.Game1" %>

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

    <div id="chatAndMessage" style="display:none;">
        <div style="position:absolute;bottom:0;">
        <input id="username" value="<%=Context.User.Identity.GetUserName()%>" style="display:none;" />
        <textarea style="width:100%;" id="messengerText"></textarea>
        <input type="button" class="btn btn-default" id="sendMessage" value="Отправить">
        </div>
    </div>
    <script>
        var socket,
            txt =$('#messengerText'),
            user = $('#username'),
            messages = $('#chatAndMessage');

        if (typeof (WebSocket) !== 'undefined') {
            socket = new WebSocket("ws://localhost/MP4_Durak/ChatHandler.ashx");
        } else {
            socket = new MozWebSocket("ws://localhost/MP4_Durak/ChatHandler.ashx");
        }

        socket.onmessage = function (msg) {
            var mesmaindiv = $('<div/>', { class: 'messageInChat' });
            var mesinnerdiv = $('<div/>', { class: 'messageManager' }).append(msg.data.replace(/\0/g, ''));
            mesmaindiv.append(mesinnerdiv);
            messages.append(mesmaindiv);
        };

        socket.onclose = function (event) {
            alert('Потеряна связь. Пожалуйста, обновите страницу');
        };

        document.getElementById('sendMessage').onclick = function () {
            socket.send(user.val() + ': ' + txt.val());
            txt.val() = '';
            txt.empty();
        };
    </script>

    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
  <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
        <script src="https://unpkg.com/draggabilly@2/dist/draggabilly.pkgd.js"></script>
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
        var inArea = false;

        function checkStartGame() {
            var interval = setInterval(function () {
                $.ajax({
                    type: "get", url: `/api/rooms/getGame?roomId=${roomId}`,
                    success: function (data, text) {
                        if (data) {
                            $('.message').remove();
                            $('#chatAndMessage').show();
                            clearTimeout(interval);
                            startGame();
                        }
                    },
                    error: function (request, status, error) {
                    }
                });
            }, 2000);
        }
        
        var activeGame;
        function startGame() {
            getAllCardsOnTable();
            getAttacker();
            getTrump();
            
            activeGame = setInterval(function () {
                getAll();
            }, 2000);
        }

        function getAll() {
            getAttacker();
            getTrump();
            checkAttacks();
            checkWinner();
            getAllCardsOnTable();
            getCards();
            getEnemyCardsCount();
        }

        function checkAttacks() {
            if (player == atacker.toString()) {
                view.infoBlock('Вы атакуете', true)

                $('.myField .cardContent').off('click');
                $('.myField .cardContent').on('click', function () {
                    atack($(this).data('val'));
                });
            } else {
                view.infoBlock('Вы защищаетесь', false)

                $('.myField .cardContent').draggabilly()
                            .on('dragStart', function () {
                                $(this).addClass('move')
                            })
                            .on('dragEnd', function (event, pointer) {
                                var area = $('.without').offset();
                                if (area) {
                                    if (area.top < pointer.screenY && area.top + 300 > pointer.screenY && area.left < pointer.screenX && area.left + 225 > pointer.screenX) {
                                        defend($($('.without').parent().parent().find('.attackCard .cardContent')).data('val'), $(this).data('val'));
                                    } else {
                                        view.userCards();
                                    }
                                } else {
                                    view.userCards();
                                }
                                checkAttacks();
                            });
            }
        }


        function checkWinner() {
            $.ajax({
                type: "get", url: `/api/game/getWhoWin?gameId=${roomId}`,
                success: function (data, text) {
                    clearInterval(activeGame);
                    view.endGameMessage(data.toString() == player);
                },
                error: function (request, status, error) {
                    console.warn(error);
                }
            });
        }

        // Атака
        function atack(card) {
            $.ajax({
                type: "get", url: `/api/game/doSmth?gameId=${roomId}&attackCard=${card}&defendCard=0`,
                success: function (data, text) {
                    cards = data;
                    view.atackCard(card);
                    getAll();
                },
                error: function (request, status, error) {
                    console.warn(error);
                }
            });
        }

        // Защита
        function defend(cardA, cardD) {
            $.ajax({
                type: "get", url: `/api/game/doSmth?gameId=${roomId}&attackCard=${cardA}&defendCard=${cardD}`,
                success: function (data, text) {
                    cards = data;
                    getAll();
                },
                error: function (request, status, error) {
                    console.warn(error);
                }
            });
        }

        // Получить карты 
        function getCards() {
            $.ajax({
                type: "get", url: `/api/game/getCards?gameId=${roomId}`,
                success: function (data, text) {
                    if (JSON.stringify(cards) != JSON.stringify(data)) {
                        cards = data;
                        view.userCards();
                    }
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
                    if (JSON.stringify(cardsOnTable) != JSON.stringify(data)) {
                        cardsOnTable = data;
                        view.table();
                    }
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
                    if (atacker != data) {
                        atacker = data;
                        view.userCards();
                    }
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
                    if (trump != data) {
                        trump = data;
                        view.calods();
                    }
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
                    if (countCardsProtivnic != data) {
                        countCardsProtivnic = data;
                        view.protivnicCards();
                        view.userCards();
                    }
                },
                error: function (request, status, error) {
                    console.warn(error);
                }
            });
        }

        function beru() {
            $.ajax({
                type: "get", url: `/api/game/defenderGetCards?gameId=${roomId}`,
                success: function (data, text) {
                    getAll();
                },
                error: function (request, status, error) {
                    console.warn(error);
                }
            });
        }

        function bito() {
            $.ajax({
                type: "get", url: `/api/game/defenderWinRound?gameId=${roomId}`,
                success: function (data, text) {
                    getAll();
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

                $.each(cards, function (key, card) {
                    var card2 = $('<div/>', {
                        class: "cardContent",
                        'data-val': card,
                        style: `background: url("/Images/${cardConvector(card)}.jpg")`
                    })
                    var card = $('<div/>', { class: "card" }).append(
                        card2
                    ).appendTo(parent);
                    checkAttacks();
                    /*
                    if (player != atacker.toString() && cardsOnTable && cardsOnTable.length) {
                        card2.draggabilly()
                            .on('dragStart', function() {
                                card2.addClass('move')
                            })
                            .on('dragEnd', function (event, pointer) {
                                var area = $('.without').offset();
                                if (area) {
                                    if (area.top < pointer.screenY && area.top + 300 > pointer.screenY && area.left < pointer.screenX && area.left + 225 > pointer.screenX) {
                                        defend($($('.without').parent().parent().find('.attackCard .cardContent')).data('val'), card2.data('val'));
                                    } else {
                                        view.userCards();
                                    }
                                } else {
                                    view.userCards();
                                }
                        });
                    }*/
                });
            },

            protivnicCards: function () {
                var parent = $('.protivnic .cards').empty();
                for (var i = 0; i < countCardsProtivnic; i++) {
                    $('<div/>', { class: "card" }).append(
                        $('<div/>', { class: "cardContent", style: `background: url("/Images/back.jpg")`})
                    ).appendTo(parent);
                };
            },

            table: function () {
                var parent = $('.warField').empty();
                $.each(cardsOnTable, function (key, val) {
                    var post = $('<div/>', {class: 'post'}).appendTo(parent);
                    post.append(
                        $('<div/>', {class: 'attackCard'}).append(
                            $('<div/>', {class: 'cardContent', 'data-val':val.Attack, style: `background:url("/Images/${cardConvector(val.Attack)}.jpg")`})
                        )
                    )
                    var dk = $('<div/>', { class: 'defenceCard' }).appendTo(post);

                    if (val.Defend) {
                        $('<div/>', { class: 'cardContent', 'data-val': val.Defend, style: `background:url("/Images/${cardConvector(val.Defend)}.jpg")` }).appendTo(dk);
                    } else {
                        var area = $('<div/>', { class: 'cardContent without'}).appendTo(dk)
                        area.on('mouseenter', function () {
                            inArea = true;
                        }).on('mouseleave', function () {
                            inArea = false;
                        });
                    }
                })
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

            infoBlock: function (mes, is_at) {
                $('.infoBlock').remove()
                var info_block = $('<div/>', { class: 'infoBlock' }).appendTo($('.pole'));
                if (is_at) {
                    info_block.append($('<div/>', { class: 'infoBtn' }).append('Бито').on('click', function () { bito() }));
                } else {
                    info_block.append($('<div/>', { class: 'infoBtn' }).append('Беру').on('click', function () { beru() }));
                }
                info_block.append($('<div/>', { class: 'infoMessage' }).append(mes));
            },

            atackCard: function (card) {
                var oldParent = $(`[data-val="${card}"]`).parent();
                $('.pole .warField').append(
                    $('<div/>', { class: 'post' }).append(
                        $('<div/>', { class: 'attackCard' }).append(
                            $(`[data-val="${card}"]`)
                        )
                    )
                )
                oldParent.remove();
            },

            endGameMessage: function (res) {
                var parent = $('.gameField').empty();
                
                var mes = $('<div/>', {class: 'message'}).appendTo(parent);
                $('<span/>').append(
                    res ? 
                    'Поздравляем!! Вы победили.' :
                    'Поздравляем. Ты проиграл.'
                ).appendTo(mes);

                $('<div/>', { class: 'infoBtn', style: 'margin-top: 30px;' }).append('Продолжить').appendTo(mes).on('click', function () { location = '/ListRooms'; })
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
        * {
            -webkit-touch-callout: none;
            -webkit-user-select: none;
            -khtml-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
        }

        #chatAndMessage {
            max-width: 250px;
        }

        .body-content {
            min-width: 1000px;
        }

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

        .ui-draggable-dragging {
            
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

        .myField .card .cardContent:not(.move):hover {
           /* top: -50px;
            transition: all 0.2s;
            top: -50px; */
            box-shadow: 0px 0px 20px 3px rgba(89, 186, 255, 0.78);
        }

        .infoBlock {
            position: absolute;
            left: calc(50% - 125px);
            z-index: 100;
            width: 250px;
            top: 601px;
        }

        .infoBtn {
            z-index: 10;
            cursor:pointer;
            display:inline-block;
            width: 125px;
            text-align: center;
            background: #ffffff00;
            border: 1px solid #00000021;
            border-radius: 5px;
            padding: 5px;
            box-shadow: 2px 2px 6px 0px #00000038;
        }

        .infoBtn:hover {
            background: #e8e8e8f0;
        }

        .infoMessage {
            z-index: 10;
            display:inline-block;
            width: 125px;
            text-align: center;
            background: #ffffff00;
            border: 1px solid #00000021;
            border-radius: 5px;
            padding: 5px;
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
        #chatAndMessage {
            width: 20%;
            height: 450px;
            overflow-y: auto;
            border: 1px solid #dbdbdd;
            background: #ffffff;
            float: left;
            position: fixed;
            margin-top: -500px;
    
        }

        .messageInChat {
            overflow: hidden;
            margin-bottom: 10px;
            border-radius: 5px;
            padding: 5px;
        }

        

        .messageManager {
            max-width: 286px;
            background: #dedede;
            color: black;
            right: 10px;
            width: 80%;
            float: left;
            padding: 5px;
        }
    </style>
</asp:Content>