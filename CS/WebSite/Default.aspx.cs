using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxPivotGrid;
using DevExpress.XtraPivotGrid.Data;
using System.Drawing;
using DevExpress.Utils;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            SyncPivots(ASPxPivotGrid_Source, ASPxPivotGrid1);
        ASPxPivotGrid1.CellTemplate = new CellTemplate(ASPxPivotGrid_Source, ASPxPivotGrid1);
    }

    protected void ASPxPivotGrid1_AfterPerformCallback(object sender, EventArgs e) {
        SyncPivots(ASPxPivotGrid_Source, ASPxPivotGrid1);
    }

    void SyncPivots(ASPxPivotGrid source, ASPxPivotGrid visiblePivot) {
        source.BeginUpdate();
        foreach(PivotGridField visibleField in visiblePivot.Fields) {
            PivotGridField sourceField = (PivotGridField)source.Fields.GetFieldByName(visibleField.ID + "_Source");
            SyncFields(sourceField, visibleField);
        }
        source.EndUpdate();
        source.ExpandAll();
    }

    void SyncFields(PivotGridField sourceField, PivotGridField visibleField) {
        sourceField.Area = visibleField.Area;
        sourceField.Visible = visibleField.Visible;
        sourceField.AreaIndex = visibleField.AreaIndex;
        sourceField.FilterValues.FilterType = visibleField.FilterValues.FilterType;
        sourceField.FilterValues.Values = visibleField.FilterValues.Values;
    }
}


class CellTemplate : ITemplate
{
    ASPxPivotGrid pivotGrid, sourcePivotGrid;
    public CellTemplate(ASPxPivotGrid sourcePivotGrid, ASPxPivotGrid pivotGrid)
    {
        this.sourcePivotGrid = sourcePivotGrid;
        this.pivotGrid = pivotGrid;
    }

    public void InstantiateIn(Control container)
    {
        PivotGridCellTemplateContainer c = container as PivotGridCellTemplateContainer;


        if (!ShouldDisplaySecondValue(c.Item))
        {
            Label templateLable = new Label();
            templateLable.Text = c.Text;
            c.Controls.Add(templateLable);
        }
        else
        {
            object val = GetTotalValue(c.Item , sourcePivotGrid);

            Table t = CreateTable(c.Text, GetCellDisplayText(val, c.Item));
            c.Controls.Add(t);
        }
    }
    private string GetCellDisplayText(object val, PivotGridCellTemplateItem cell)
    {
        if (val == null) return "No Data";
        FormatInfo cellFormat = null;
 
        if (cell.DataField.GrandTotalCellFormat.FormatType != DevExpress.Utils.FormatType.None)
            cellFormat = cell.DataField.GrandTotalCellFormat;
        else if (cell.DataField.CellFormat.FormatType != DevExpress.Utils.FormatType.None)
            cellFormat = cell.DataField.CellFormat;
        if (cellFormat != null)
            if (cellFormat.FormatType == FormatType.DateTime)
                return Convert.ToDateTime(val).ToString(cellFormat.FormatString );
            else
                return Convert.ToDecimal(val).ToString(cellFormat.FormatString );
        return val.ToString();
    }

    private bool ShouldDisplaySecondValue(PivotGridCellTemplateItem cell)
    {
        if (cell.RowValueType == DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal)
        {
            PivotGridField rowField = pivotGrid.GetFieldByArea(DevExpress.XtraPivotGrid.PivotArea.RowArea, 0);
            if (rowField != null && rowField.TopValueCount > 0) return true;            
        }
        if (cell.ColumnValueType == DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal)
        {
            PivotGridField columnField = pivotGrid.GetFieldByArea(DevExpress.XtraPivotGrid.PivotArea.ColumnArea, 0);
            if ( columnField != null && columnField.TopValueCount > 0) return true;    
        }
        return false;
    }

    private Table CreateTable(string topText, string totalText)
    {
        Table table = new Table();
        TableRow row1 = new TableRow();
        table.Controls.Add(row1);
        TableCell cell1 = new TableCell();
        cell1.Style.Add(HtmlTextWriterStyle.Padding, "0px");
        cell1.Text = topText;
        row1.Controls.Add(cell1);

        TableRow row2 = new TableRow();
        table.Controls.Add(row2);
        TableCell cell2 = new TableCell();
        cell2.Style.Add(HtmlTextWriterStyle.Padding, "0px");
        cell2.Text = totalText;
        cell2.Font.Bold = true;
        row2.Controls.Add(cell2);
        return table;
    }

    private object GetTotalValue(PivotGridCellTemplateItem c, ASPxPivotGrid sourcePivotGrid)
    {
        List<PivotGridField> columnFields = pivotGrid.GetFieldsByArea(DevExpress.XtraPivotGrid.PivotArea.ColumnArea);
        List<PivotGridField> rowFields = pivotGrid.GetFieldsByArea(DevExpress.XtraPivotGrid.PivotArea.RowArea);
        List<object> rowFieldValues = new List<object>();
        List<object> columnFieldValues = new List<object>();

        if (c.ColumnField != null)
            foreach (PivotGridField field in columnFields)
            {
                if (field.AreaIndex > c.ColumnField.AreaIndex) continue;
                object currentValue = c.GetFieldValue(field);
                if (currentValue != null)
                    columnFieldValues.Add(currentValue);
            }
        if (c.RowField != null)
            foreach (PivotGridField field in rowFields)
            {
                if (field.AreaIndex > c.RowField.AreaIndex) continue;
                object currentValue = c.GetFieldValue(field);
                if (currentValue != null)
                    rowFieldValues.Add(currentValue);
            }
        sourcePivotGrid.EnsureRefreshData();
        object res = sourcePivotGrid.Data.GetCellValue(columnFieldValues.ToArray(), rowFieldValues.ToArray(), c.DataField);
        return res;
    }
}
