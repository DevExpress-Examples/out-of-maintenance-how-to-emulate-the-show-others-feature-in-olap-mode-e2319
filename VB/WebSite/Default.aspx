<%@ Page Language="vb" AutoEventWireup="true"  CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register assembly="DevExpress.Web.ASPxPivotGrid.v13.1, Version=13.1.14.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPivotGrid" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
	<div>
	<dx:ASPxPivotGrid ID="ASPxPivotGrid_Source" runat="server" Visible="False" 
			OLAPConnectionString="provider=MSOLAP;data source=local;initial catalog=&quot;Adventure Works DW Standard Edition&quot;;cube name=&quot;Adventure Works&quot;">
			<Fields>
				<dx:PivotGridField ID="fieldSalesAmount_Source" Area="DataArea" AreaIndex="0" 
					Caption="Sales Amount" FieldName="[Measures].[Sales Amount]" CellFormat-FormatType="Numeric" CellFormat-FormatString="c">
				</dx:PivotGridField>
				<dx:PivotGridField ID="fieldProduct_Source" Area="RowArea" AreaIndex="0" 
					Caption="Product" FieldName="[Product].[Product].[Product]">
				</dx:PivotGridField>
				<dx:PivotGridField ID="fieldSalesTerritoryCountry_Source" Area="ColumnArea" 
					AreaIndex="1" Caption="Country" 
					FieldName="[Sales Territory].[Sales Territory Country].[Sales Territory Country]">
				</dx:PivotGridField>
				<dx:PivotGridField ID="fieldSalesTerritoryGroup_Source" Area="ColumnArea" 
					AreaIndex="0" Caption="Group" 
					FieldName="[Sales Territory].[Sales Territory Group].[Sales Territory Group]">
				</dx:PivotGridField>
			</Fields>
		</dx:ASPxPivotGrid>

		<dx:ASPxPivotGrid ID="ASPxPivotGrid1" runat="server" 

			OLAPConnectionString="provider=MSOLAP;data source=local;initial catalog=&quot;Adventure Works DW Standard Edition&quot;;cube name=&quot;Adventure Works&quot;" 
			EnableCallBacks="False" 
			OnAfterPerformCallback="ASPxPivotGrid1_AfterPerformCallback">
			<Fields>
				<dx:PivotGridField ID="fieldSalesAmount" Area="DataArea" AreaIndex="0" 
					Caption="Sales Amount" FieldName="[Measures].[Sales Amount]" CellFormat-FormatType="Numeric" CellFormat-FormatString="c">
				</dx:PivotGridField>
				<dx:PivotGridField ID="fieldProduct" Area="RowArea" AreaIndex="0" 
					Caption="Product" FieldName="[Product].[Product].[Product]" TopValueCount="5">
				</dx:PivotGridField>
				<dx:PivotGridField ID="fieldSalesTerritoryCountry" Area="ColumnArea" 
					AreaIndex="1" Caption="Country" 
					FieldName="[Sales Territory].[Sales Territory Country].[Sales Territory Country]">
				</dx:PivotGridField>
				<dx:PivotGridField ID="fieldSalesTerritoryGroup" Area="ColumnArea" 
					AreaIndex="0" Caption="Group" 
					FieldName="[Sales Territory].[Sales Territory Group].[Sales Territory Group]">
				</dx:PivotGridField>
			</Fields>
		</dx:ASPxPivotGrid>



	</div>
	</form>
</body>
</html>