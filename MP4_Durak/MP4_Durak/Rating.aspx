<%@ Page Title="Рейтинги" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Rating.aspx.cs" Inherits="MP4_Durak.Rating" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Рейтинг игроков:</h2>
    <div class="row">
        <button type="submit" class="btn btn-primary">По количеству выигрешей</button>
        <button type="submit" class="btn btn-secondary">По количеству проигрешей</button>
        <button type="submit" class="btn btn-secondary">Процент выигрышей</button>
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
          <tbody>
            <tr>
              <th scope="row">1</th>
              <td>Mark</td>
              <td>4</td>
              <td>1</td>
              <td>5</td>
              <td>80</td>
            </tr>
          </tbody>
        </table>
</asp:Content>
