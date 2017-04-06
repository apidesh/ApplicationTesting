<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DownloadWebPortal._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Download File Web Portal</h1>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Download File</h2>
            <p>
                Confirmation #1</p>
            <p>
                <a class="btn btn-default" href="http://localhost:18002/v1/downloadcontent?id=B5BC2687-BC79-4A38-803C-3FDDAD2C14A6">Download#1 &raquo;</a>
            </p>
             <p>
                Confirmation #2

            </p>
            <p>
                <a class="btn btn-default" href="http://localhost:18002/v1/downloadcontent?id=BD0B9C2B-2744-4413-BAC4-5176D3A07FA2">Download#2 &raquo;</a>
            </p>
           
        </div>
        <div class="col-md-4">
        </div>
        <div class="col-md-4">
        </div>
    </div>
    <div class="row">
        <div class="col-md-4">
            <h2>Views File</h2>
            <p>
                View image #1</p>
            <p>
                <img src="http://localhost:18002/v1/downloadstream?id=B5BC2687-BC79-4A38-803C-3FDDAD2C14A6" height="250" width="350" />
            </p>
             <p>
                View image #2

            </p>
            <p>
                <img src="http://localhost:18002/v1/downloadstream?id=3B042429-1A36-4184-9EA2-F7CF163B3393" height="250" width="350" />
            </p>
           
        </div>
        <div class="col-md-4">
        </div>
        <div class="col-md-4">
        </div>
    </div>
</asp:Content>
