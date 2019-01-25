<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CS.aspx.cs" Inherits="CS" %>

﻿<!DOCTYPE html>
<html lang="pt-br">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Exames</title>
    <link href="css/bootstrap.min.css" rel="stylesheet">
    <link href="css/stylo.css" rel="stylesheet">
    <script type="text/JavaScript" src="js/JavaScript.js"></script>
</head>
<body>

    <nav class="navbar navbar-inverse navbar-fixed-top">
        <a class="navbar-brand" href="#">EBSERH - Hospital Universitário de Lagarto - HUL</a>
    </nav>

    <div id="main" class="container-fluid" style="margin-top: 50px">
    </div>


    <nav class="navbar navbar-light bg-light">
        <form class="form-inline" id="form1" runat="server">


            <div id="top" class="row">
                <div class="col-sm-3">
                    <h2>Listas de Exames</h2>
                </div>


                <div class="col-sm-6">

                    <asp:TextBox class="form-control mr-sm-2" type="search" ID="searchBox" runat="server" placeholder="Pesquisar Exames " Width="400" Height="50"></asp:TextBox>
                    <asp:Button type="button" class="btn btn-primary" ID="searchButton" runat="server" Text="Buscar" OnClick="searchButton_Click" Width="120" Height="50" />
                    <asp:Button type="button" class="btn btn-primary" ID="reset" runat="server" Text="Voltar" OnClick="resetSearchButton_Click" Width="120" Height="50" />

                </div>


            </div>
            <asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="false" Width="1700px" AllowPaging="true" PageSize="12" ShowFooter="true" OnPageIndexChanging="gridPageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderText="Id">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server"
                                Text='<%# (gvFiles.PageSize * gvFiles.PageIndex) + Container.DisplayIndex + 1 %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Arquivo" HeaderText="Pontuario do paciente" ItemStyle-Width="650" />
                    <asp:BoundField DataField="Date" HeaderText="Data do Exame"   ItemStyle-Width="100" />
                    <asp:TemplateField HeaderText="Ações" ItemStyle-Width="650">
                        <ItemTemplate>

                            <asp:LinkButton ID="lnkDownload" runat="server" class="btn btn-success" Text="Visualizar" OnClientClick="window.document.forms[0].target = '_blank'" OnClick="DownloadFile"
                                CommandArgument='<%# Eval("Name") %>'></asp:LinkButton>
                          <asp:LinkButton ID="LinkButton1" runat="server" class="btn btn-primary" Text="Imprimir" OnClientClick="print()" OnClick="DownloadFile"
                                CommandArgument='<%# Eval("Name") %>'></asp:LinkButton>

                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

        </form>
    </nav>

</body>
</html>
