﻿<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DownloadWebPortal._Default" %>

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
                <a class="btn btn-default" href="http://localhost:18002/common/filedownload/downloadcontent?id=1e5e55be-8cae-41c8-be62-24330cae1b66">Download#1 &raquo;</a>
            </p>
             <p>
                Confirmation #2

            </p>
            <p>
                <a class="btn btn-default" href="http://localhost:18002/common/filedownload/downloadcontent?id=1e5e55be-8cae-41c8-be62-24330cae1b66">Download#2 &raquo;</a>
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
                <img src="http://localhost:18002/common/filedownload/downloadstream?id=1e5e55be-8cae-41c8-be62-24330cae1b66" height="250" width="350" />
            </p>
             <p>
                View image #2

            </p>
            <p>
                <img src="http://localhost:18002/common/filedownload/downloadstream?id=1e5e55be-8cae-41c8-be62-24330cae1b66" height="250" width="350" />
            </p>
           
        </div>
        <div class="col-md-4">
        </div>
        <div class="col-md-4">
        </div>
    </div>
</asp:Content>
