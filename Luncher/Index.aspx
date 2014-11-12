<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Luncher.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <h1>&nbsp;</h1>
    <div>
        Zobrazit jídla na:<br />
        <asp:RadioButton ID="rbDnes" runat="server" AutoPostBack="True" Checked="True" GroupName="den" Text="Dnes" />
        <br />
        <asp:RadioButton ID="rbZitra" runat="server" AutoPostBack="True" GroupName="den" Text="Zítra" />
        <br />
        <br />
        Výdejna:<br />
        <asp:RadioButton ID="rbHarcov" runat="server" AutoPostBack="True" Checked="True" GroupName="vydejna" Text="Harcov" />
        <br />
        <asp:RadioButton ID="rbHusova" runat="server" AutoPostBack="True" GroupName="vydejna" Text="Husova" />
        <br />
        <asp:RadioButton ID="rbVoronezska" runat="server" AutoPostBack="True" GroupName="vydejna" Text="Voroněžská" />
        <br />
        <br />
        <asp:CheckBox ID="cbUpozornitObed" runat="server" Text="Upozornit na volný oběd" AutoPostBack="True" />
        <br />
        <asp:CheckBox ID="cbUpozornitVeceri" runat="server" Text="Upozornit na volnou večeři" AutoPostBack="True" />
    </div>
    <div>
        <br />
        <br />
        <br />
        <asp:Label ID="lbStatus" runat="server"></asp:Label>
        <br />
        <asp:PlaceHolder ID="phZvuk" runat="server"></asp:PlaceHolder>
        <br />
        <asp:PlaceHolder ID="phMenu" runat="server"></asp:PlaceHolder>
        <br />
        <br />
    </div>
&nbsp;
</asp:Content>
