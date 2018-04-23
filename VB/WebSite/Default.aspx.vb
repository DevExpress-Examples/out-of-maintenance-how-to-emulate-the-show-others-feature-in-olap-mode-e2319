Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DevExpress.Web.ASPxPivotGrid
Imports DevExpress.XtraPivotGrid.Data
Imports System.Drawing
Imports DevExpress.Utils

Partial Public Class _Default
	Inherits System.Web.UI.Page
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		If (Not IsPostBack) Then
			SyncPivots(ASPxPivotGrid_Source, ASPxPivotGrid1)
		End If
		ASPxPivotGrid1.CellTemplate = New CellTemplate(ASPxPivotGrid_Source, ASPxPivotGrid1)
	End Sub

	Protected Sub ASPxPivotGrid1_AfterPerformCallback(ByVal sender As Object, ByVal e As EventArgs)
		SyncPivots(ASPxPivotGrid_Source, ASPxPivotGrid1)
	End Sub

	Private Sub SyncPivots(ByVal source As ASPxPivotGrid, ByVal visiblePivot As ASPxPivotGrid)
		source.BeginUpdate()
		For Each visibleField As PivotGridField In visiblePivot.Fields
			Dim sourceField As PivotGridField = CType(source.Fields.GetFieldByName(visibleField.ID & "_Source"), PivotGridField)
			SyncFields(sourceField, visibleField)
		Next visibleField
		source.EndUpdate()
		source.ExpandAll()
	End Sub

	Private Sub SyncFields(ByVal sourceField As PivotGridField, ByVal visibleField As PivotGridField)
		sourceField.Area = visibleField.Area
		sourceField.Visible = visibleField.Visible
		sourceField.AreaIndex = visibleField.AreaIndex
		sourceField.FilterValues.FilterType = visibleField.FilterValues.FilterType
		sourceField.FilterValues.Values = visibleField.FilterValues.Values
	End Sub
End Class


Friend Class CellTemplate
	Implements ITemplate
	Private pivotGrid, sourcePivotGrid As ASPxPivotGrid
	Public Sub New(ByVal sourcePivotGrid As ASPxPivotGrid, ByVal pivotGrid As ASPxPivotGrid)
		Me.sourcePivotGrid = sourcePivotGrid
		Me.pivotGrid = pivotGrid
	End Sub

	Public Sub InstantiateIn(ByVal container As Control) Implements ITemplate.InstantiateIn
		Dim c As PivotGridCellTemplateContainer = TryCast(container, PivotGridCellTemplateContainer)


		If (Not ShouldDisplaySecondValue(c.Item)) Then
			Dim templateLable As New Label()
			templateLable.Text = c.Text
			c.Controls.Add(templateLable)
		Else
			Dim val As Object = GetTotalValue(c.Item, sourcePivotGrid)

			Dim t As Table = CreateTable(c.Text, GetCellDisplayText(val, c.Item))
			c.Controls.Add(t)
		End If
	End Sub
	Private Function GetCellDisplayText(ByVal val As Object, ByVal cell As PivotGridCellTemplateItem) As String
		If val Is Nothing Then
			Return "No Data"
		End If
		Dim cellFormat As FormatInfo = Nothing

		If cell.DataField.GrandTotalCellFormat.FormatType <> DevExpress.Utils.FormatType.None Then
			cellFormat = cell.DataField.GrandTotalCellFormat
		ElseIf cell.DataField.CellFormat.FormatType <> DevExpress.Utils.FormatType.None Then
			cellFormat = cell.DataField.CellFormat
		End If
		If cellFormat IsNot Nothing Then
			If cellFormat.FormatType = FormatType.DateTime Then
				Return Convert.ToDateTime(val).ToString(cellFormat.FormatString)
			Else
				Return Convert.ToDecimal(val).ToString(cellFormat.FormatString)
			End If
		End If
		Return val.ToString()
	End Function

	Private Function ShouldDisplaySecondValue(ByVal cell As PivotGridCellTemplateItem) As Boolean
		If cell.RowValueType = DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal Then
			Dim rowField As PivotGridField = pivotGrid.GetFieldByArea(DevExpress.XtraPivotGrid.PivotArea.RowArea, 0)
			If rowField IsNot Nothing AndAlso rowField.TopValueCount > 0 Then
				Return True
			End If
		End If
		If cell.ColumnValueType = DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal Then
			Dim columnField As PivotGridField = pivotGrid.GetFieldByArea(DevExpress.XtraPivotGrid.PivotArea.ColumnArea, 0)
			If columnField IsNot Nothing AndAlso columnField.TopValueCount > 0 Then
				Return True
			End If
		End If
		Return False
	End Function

	Private Function CreateTable(ByVal topText As String, ByVal totalText As String) As Table
		Dim table As New Table()
		Dim row1 As New TableRow()
		table.Controls.Add(row1)
		Dim cell1 As New TableCell()
		cell1.Style.Add(HtmlTextWriterStyle.Padding, "0px")
		cell1.Text = topText
		row1.Controls.Add(cell1)

		Dim row2 As New TableRow()
		table.Controls.Add(row2)
		Dim cell2 As New TableCell()
		cell2.Style.Add(HtmlTextWriterStyle.Padding, "0px")
		cell2.Text = totalText
		cell2.Font.Bold = True
		row2.Controls.Add(cell2)
		Return table
	End Function

	Private Function GetTotalValue(ByVal c As PivotGridCellTemplateItem, ByVal sourcePivotGrid As ASPxPivotGrid) As Object
		Dim columnFields As List(Of PivotGridField) = pivotGrid.GetFieldsByArea(DevExpress.XtraPivotGrid.PivotArea.ColumnArea)
		Dim rowFields As List(Of PivotGridField) = pivotGrid.GetFieldsByArea(DevExpress.XtraPivotGrid.PivotArea.RowArea)
		Dim rowFieldValues As New List(Of Object)()
		Dim columnFieldValues As New List(Of Object)()

		If c.ColumnField IsNot Nothing Then
			For Each field As PivotGridField In columnFields
				If field.AreaIndex > c.ColumnField.AreaIndex Then
					Continue For
				End If
				Dim currentValue As Object = c.GetFieldValue(field)
				If currentValue IsNot Nothing Then
					columnFieldValues.Add(currentValue)
				End If
			Next field
		End If
		If c.RowField IsNot Nothing Then
			For Each field As PivotGridField In rowFields
				If field.AreaIndex > c.RowField.AreaIndex Then
					Continue For
				End If
				Dim currentValue As Object = c.GetFieldValue(field)
				If currentValue IsNot Nothing Then
					rowFieldValues.Add(currentValue)
				End If
			Next field
		End If
		sourcePivotGrid.EnsureRefreshData()
		Dim res As Object = sourcePivotGrid.Data.GetCellValue(columnFieldValues.ToArray(), rowFieldValues.ToArray(), c.DataField)
		Return res
	End Function
End Class
