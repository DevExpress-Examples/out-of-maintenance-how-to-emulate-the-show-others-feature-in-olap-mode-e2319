<!-- default file list -->
*Files to look at*:

* [Default.aspx](./CS/WebSite/Default.aspx) (VB: [Default.aspx.vb](./VB/WebSite/Default.aspx.vb))
* [Default.aspx.cs](./CS/WebSite/Default.aspx.cs) (VB: [Default.aspx.vb](./VB/WebSite/Default.aspx.vb))
<!-- default file list end -->
# How to emulate the Show Others feature in OLAP mode


<p>This example demonstrates how to synchronize two ASPxPivotGrid controls to emulate the Show Others feature. To accomplish this task, it is necessary to place two equal pivot grids onto the same page. One of them will be the main pivot, which end-users will use. In this pivot, the <a href="http://documentation.devexpress.com/#WindowsForms/DevExpressXtraPivotGridPivotGridFieldBase_TopValueCounttopic">PivotGridFieldBase.TopValueCount</a> property of the "Product" field should be set. The second ASPxPivotGrid control (<i>"*_Source"</i>) will be hidden (Visible = "False"). We will use this pivot to get values regardless of the <a href="http://documentation.devexpress.com/#WindowsForms/DevExpressXtraPivotGridPivotGridFieldBase_TopValueCounttopic">PivotGridFieldBase.TopValueCount</a> property. The <strong>Other</strong> value will be displayed by using the <a href="http://documentation.devexpress.com/#AspNet/DevExpressWebASPxPivotGridASPxPivotGrid_CellTemplatetopic">ASPxPivotGrid.CellTemplate</a> property. </p><p>Here are short comments on the main function that is used on the web page:</p><p>1. void SyncPivots(ASPxPivotGrid source, ASPxPivotGrid visiblePivot) - Synchronizes ASPxPivotGrid controls state.<br />
2. bool ShouldDisplaySecondValue(PivotGridCellTemplateItem cell) - Checks whether the <strong>Others</strong> value should be shown for a current cell.<br />
3. private object GetTotalValue(PivotGridCellTemplateItem c, ASPxPivotGrid sourcePivotGrid) - Gets a value from the source pivot grid.<br />
4. private Table CreateTable(string topText, string totalText) - Creates a table that is used for displaying two values in one cell.<br />
 <br />
Please note that the attached project works with the Adventure Works Cube provided along with the MS Analysis Services installation. However, it can be used with any other OLAP cube.</p>

<br/>


