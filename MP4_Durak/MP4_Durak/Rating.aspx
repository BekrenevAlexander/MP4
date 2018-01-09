<%@ Page Title="Рейтинги" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Rating.aspx.cs" Inherits="MP4_Durak.Rating" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Рейтинг игроков:</h2>
    <div class='mainContent'>
    </div>

    <script>
        var players = [];
        var method = "getRatingSort";
        function getRating() {
            $.ajax({
                type: "get", url: "/api/rating/" + method,
                success: function (data, text) {
                    //console.log(data);
                    viewData(data);
                },
                error: function (request, status, error) {
                    console.warn(error);
                    viewData([]);
                }
            });
        }
        setInterval(getRating, 10000);

        function viewData(data) {
            if (data && data.length) {

                //if (!$('.contentBlock').length) {
                //    var content = $('<div/>', { class: 'contentBlock' });
                //    var list = $('<ul/>', { class: 'listRooms' }).appendTo(content);
                //    $('.tablebody').empty().append(content);
                //}

                updateTable([], data);
            } else {
                rooms = [];
                var content = $('<span/>', { class: 'message' }).append('Нет игроков');

                $('.mainContent').empty().append(content);
            }

            function updateTable(originData, newData) {
                $('.tablebody').empty();
                //console.warn(newData);
                newData.map(function (item) {
                    var res = originData.find(function (oItem) {
                        return oItem.Id == item.Id;
                    })
                    if (!res) {
                        originData.push(item);
                        var number = $('<th/>').append(originData.length);
                        var name = $('<th/>').append(item.UserName);
                        var countWin = $('<th/>').append(item.Wins);
                        var countLose = $('<th/>').append(item.Games - item.Wins);
                        var countTotal = $('<th/>').append(item.Games);
                        var percent = $('<th/>').append(parseInt(item.Wins / item.Games * 100));

                        var row = $('<tr/>').append(number).append(name).append(countWin).append(countLose).append(countTotal).append(percent);
                        $('.tablebody').append(
                            row
                        )
                    }
                });

                //for (var i = originData.length - 1; i >= 0; i--) {
                //    var res = newData.find(function (oItem) {
                //        return oItem.Id == originData[i].Id;
                //    })

                //    if (!res) {
                //        $(`[data-id="${originData[i].Id}"]`).remove();
                //        originData.split(i, 1);
                //    }
                //};
            }
        }
        $.ajax({
            type: "get", url: "/api/rating/" + method,
            success: function (data, text) {
                viewData(data);
            },
            error: function (request, status, error) {
                console.warn(error);
                viewData([]);
            }
        });
        $(document).ready(function () {
            $('.sortb').on('click', function () {
                method = $(this).attr('name');
                console.log(method);
                $('.sortb').removeClass('btn-primary btn-secondary');
                $('.sortb').addClass("sortb");  
                $('.sortb').addClass("btn");
                $('.sortb').addClass("btn-secondary");
                $(this).removeClass("btn-secondary");
                $(this).addClass("btn-primary");
                getRating();
            });
        });
    </script>
    <div class="row">
        <input type="button" id="button_wins" class="sortb btn btn-primary" name="getRatingSort" value ="По количеству выигрышей"></input>
        <input type="button" id="button_lose" class="sortb btn btn-secondary" name="getRatingSortByLose" value ="По количеству проигрышей"></input>
        <input type="button" id="button_perc" class="sortb btn btn-secondary" name="getRatingSortByPercent" value ="Процент выигрышей"></input>
    </div>
    <table class="table">
      <thead class="thead-dark">
            <tr>
              <th scope="col">#</th>
              <th scope="col">Имя</th>
              <th scope="col">Выигрыши</th>
              <th scope="col">Проигрыши</th>
              <th scope="col">Всего игр</th>
              <th scope="col">% Выигрышей</th>
            </tr>
          </thead>
          <tbody class="tablebody">
          </tbody>
        </table>
</asp:Content>
