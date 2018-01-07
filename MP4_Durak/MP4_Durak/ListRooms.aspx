<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListRooms.aspx.cs" Inherits="MP4_Durak.Rooms" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Доступные комнаты:</h2>

    <div class='mainContent'>
        <div class="loader"></div>
    </div>

    <script>
        setInterval(function () {
            $.get('/api/rooms', viewData)
        }, 2000);

        function viewData(data) {
            console.warn(data);
            data = JSON.parse(data);
            console.warn(data);
            if (data) {
                var content = $('<div/>');
                var list = $('<ul/>');
                data.map(function (data) {
                    list.append($('<li/>'));
                });

                $('.mainContent').empty().append(content);
            } else {
                var content = $('<span/>', { class: 'message' }).append('Созданных нет комнат :(');

                $('.mainContent').empty().append(content);
            }
        }
    </script>
</asp:Content>
