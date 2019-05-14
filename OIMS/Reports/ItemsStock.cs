using DevExpress.XtraReports.UI;
using OIMS.Repository.Supervisor;

namespace OIMS.Reports
{
    /// <summary>
    /// Summary description for XtraReport
    /// </summary>
    public class ItemsStock : XtraReport
    {
        private TopMarginBand _topMargin;
        private BottomMarginBand _bottomMargin;

        // Header band and content
        private ReportHeaderBand _reportHeader;
        private XRLabel _lbTitle;
        private XRLine _xrLine1;
        private XRPageInfo _pageInfoDate;

        // Group band and content
        private GroupHeaderBand _groupHeader;
        private XRTable _xrTable1;
        private XRTableRow _xrRow1;
        private XRTableCell _xrCell1;
        private XRTableCell _xrCell2;
        private XRTableCell _xrCell3;

        // Detail band and content
        private DetailBand _detail;
        private XRTable _xrTable2;
        private XRTableRow _xrRow2;
        private XRTableCell _xrCell4;
        private XRTableCell _xrCell5;
        private XRTableCell _xrCell6;
        private XRPageInfo _pageInfoNumber;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private readonly System.ComponentModel.IContainer components = null;

        public ItemsStock()
        {
            InitializeComponent();

            DataSource = ItemRepository.GetItems();

            _xrCell4.DataBindings.AddRange(new[] { new XRBinding("Text", null, "ItemId") });
            _xrCell5.DataBindings.AddRange(new[] { new XRBinding("Text", null, "ItemName") });
            _xrCell6.DataBindings.AddRange(new[] { new XRBinding("Text", null, "ItemQuantity") });
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._detail = new DevExpress.XtraReports.UI.DetailBand();
            this._xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this._xrRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this._xrCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this._xrCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this._xrCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this._topMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this._bottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this._pageInfoNumber = new DevExpress.XtraReports.UI.XRPageInfo();
            this._lbTitle = new DevExpress.XtraReports.UI.XRLabel();
            this._reportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this._xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this._pageInfoDate = new DevExpress.XtraReports.UI.XRPageInfo();
            this._groupHeader = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this._xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this._xrRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this._xrCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this._xrCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this._xrCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            ((System.ComponentModel.ISupportInitialize)(this._xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this._detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this._xrTable2});
            this._detail.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._detail.HeightF = 32.08329F;
            this._detail.Name = "Detail";
            this._detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this._detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("ItemId", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this._detail.StylePriority.UseFont = false;
            this._detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // _xrTable2
            // 
            this._xrTable2.BackColor = System.Drawing.Color.Beige;
            this._xrTable2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(209)))), ((int)(((byte)(228)))));
            this._xrTable2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this._xrTable2.BorderWidth = 1;
            this._xrTable2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._xrTable2.ForeColor = System.Drawing.Color.Black;
            this._xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(17.00001F, 0F);
            this._xrTable2.Name = "_xrTable2";
            this._xrTable2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this._xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this._xrRow2});
            this._xrTable2.SizeF = new System.Drawing.SizeF(616F, 32.08329F);
            this._xrTable2.StylePriority.UseBackColor = false;
            this._xrTable2.StylePriority.UseBorderColor = false;
            this._xrTable2.StylePriority.UseBorders = false;
            this._xrTable2.StylePriority.UseBorderWidth = false;
            this._xrTable2.StylePriority.UseFont = false;
            // 
            // _xrRow2
            // 
            this._xrRow2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this._xrRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this._xrCell4,
            this._xrCell5,
            this._xrCell6});
            this._xrRow2.Name = "_xrRow2";
            this._xrRow2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this._xrRow2.Weight = 1D;
            // 
            // _xrCell4
            // 
            this._xrCell4.BackColor = System.Drawing.Color.Empty;
            this._xrCell4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(209)))), ((int)(((byte)(228)))));
            this._xrCell4.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this._xrCell4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._xrCell4.Name = "_xrCell4";
            this._xrCell4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this._xrCell4.StylePriority.UseBackColor = false;
            this._xrCell4.StylePriority.UseBorderColor = false;
            this._xrCell4.StylePriority.UseBorders = false;
            this._xrCell4.StylePriority.UseFont = false;
            this._xrCell4.StylePriority.UseTextAlignment = false;
            this._xrCell4.Text = "Sl.No.";
            this._xrCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this._xrCell4.Weight = 0.28469967532467533D;
            // 
            // _xrCell5
            // 
            this._xrCell5.BackColor = System.Drawing.Color.Empty;
            this._xrCell5.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(209)))), ((int)(((byte)(228)))));
            this._xrCell5.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this._xrCell5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._xrCell5.Name = "_xrCell5";
            this._xrCell5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this._xrCell5.StylePriority.UseBackColor = false;
            this._xrCell5.StylePriority.UseBorderColor = false;
            this._xrCell5.StylePriority.UseBorders = false;
            this._xrCell5.StylePriority.UseFont = false;
            this._xrCell5.StylePriority.UseTextAlignment = false;
            this._xrCell5.Text = "Item Name";
            this._xrCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this._xrCell5.Weight = 2.1972398634080763D;
            // 
            // _xrCell6
            // 
            this._xrCell6.BackColor = System.Drawing.Color.Empty;
            this._xrCell6.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(209)))), ((int)(((byte)(228)))));
            this._xrCell6.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this._xrCell6.Font = new System.Drawing.Font("Verdana", 8.25F);
            this._xrCell6.Name = "_xrCell6";
            this._xrCell6.StylePriority.UseBackColor = false;
            this._xrCell6.StylePriority.UseBorderColor = false;
            this._xrCell6.StylePriority.UseBorders = false;
            this._xrCell6.StylePriority.UseFont = false;
            this._xrCell6.StylePriority.UseTextAlignment = false;
            this._xrCell6.Text = "Units In Stock";
            this._xrCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this._xrCell6.Weight = 0.51806046126724836D;
            // 
            // TopMargin
            // 
            this._topMargin.HeightF = 20.83333F;
            this._topMargin.Name = "TopMargin";
            this._topMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this._topMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this._bottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this._pageInfoNumber});
            this._bottomMargin.Name = "BottomMargin";
            this._bottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this._bottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // _pageInfoNumber
            // 
            this._pageInfoNumber.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this._pageInfoNumber.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._pageInfoNumber.Format = "Page: {0}/{1}";
            this._pageInfoNumber.LocationFloat = new DevExpress.Utils.PointFloat(550F, 67.00001F);
            this._pageInfoNumber.Name = "_pageInfoNumber";
            this._pageInfoNumber.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this._pageInfoNumber.SizeF = new System.Drawing.SizeF(100F, 23F);
            this._pageInfoNumber.StylePriority.UseBorders = false;
            this._pageInfoNumber.StylePriority.UseFont = false;
            this._pageInfoNumber.StylePriority.UseTextAlignment = false;
            this._pageInfoNumber.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // _lbTitle
            // 
            this._lbTitle.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this._lbTitle.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this._lbTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(132)))), ((int)(((byte)(213)))));
            this._lbTitle.LocationFloat = new DevExpress.Utils.PointFloat(0F, 20.12501F);
            this._lbTitle.Name = "_lbTitle";
            this._lbTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this._lbTitle.SizeF = new System.Drawing.SizeF(147.375F, 38F);
            this._lbTitle.StylePriority.UseFont = false;
            this._lbTitle.StylePriority.UseTextAlignment = false;
            this._lbTitle.Text = "Items in Stock";
            this._lbTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // ReportHeader
            // 
            this._reportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this._xrLine1,
            this._lbTitle,
            this._pageInfoDate});
            this._reportHeader.HeightF = 91.75F;
            this._reportHeader.Name = "ReportHeader";
            // 
            // _xrLine1
            // 
            this._xrLine1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this._xrLine1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(167)))), ((int)(((byte)(73)))));
            this._xrLine1.LineWidth = 2;
            this._xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 59.12501F);
            this._xrLine1.Name = "_xrLine1";
            this._xrLine1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this._xrLine1.SizeF = new System.Drawing.SizeF(650F, 9F);
            this._xrLine1.StylePriority.UseBorders = false;
            // 
            // _pageInfoDate
            // 
            this._pageInfoDate.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this._pageInfoDate.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this._pageInfoDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(132)))), ((int)(((byte)(213)))));
            this._pageInfoDate.Format = "{0: dddd, dd MMMM yyyy}";
            this._pageInfoDate.LocationFloat = new DevExpress.Utils.PointFloat(459.0417F, 36.12502F);
            this._pageInfoDate.Name = "_pageInfoDate";
            this._pageInfoDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this._pageInfoDate.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this._pageInfoDate.SizeF = new System.Drawing.SizeF(190.9583F, 23F);
            this._pageInfoDate.StylePriority.UseBorders = false;
            this._pageInfoDate.StylePriority.UseFont = false;
            this._pageInfoDate.StylePriority.UseTextAlignment = false;
            this._pageInfoDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
            // 
            // GroupHeader
            // 
            this._groupHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this._xrTable1});
            this._groupHeader.HeightF = 51.45833F;
            this._groupHeader.Name = "GroupHeader";
            this._groupHeader.RepeatEveryPage = true;
            // 
            // _xrTable1
            // 
            this._xrTable1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(181)))), ((int)(((byte)(252)))));
            this._xrTable1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(209)))), ((int)(((byte)(228)))));
            this._xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right)));
            this._xrTable1.BorderWidth = 1;
            this._xrTable1.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._xrTable1.ForeColor = System.Drawing.Color.White;
            this._xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(17.00001F, 7.999992F);
            this._xrTable1.Name = "_xrTable1";
            this._xrTable1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this._xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this._xrRow1});
            this._xrTable1.SizeF = new System.Drawing.SizeF(616F, 43.45833F);
            this._xrTable1.StylePriority.UseBackColor = false;
            this._xrTable1.StylePriority.UseBorderColor = false;
            this._xrTable1.StylePriority.UseBorders = false;
            this._xrTable1.StylePriority.UseFont = false;
            this._xrTable1.StylePriority.UseForeColor = false;
            // 
            // _xrRow1
            // 
            this._xrRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this._xrCell1,
            this._xrCell2,
            this._xrCell3});
            this._xrRow1.Name = "_xrRow1";
            this._xrRow1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this._xrRow1.Weight = 1D;
            // 
            // _xrCell1
            // 
            this._xrCell1.Name = "_xrCell1";
            this._xrCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this._xrCell1.StylePriority.UseTextAlignment = false;
            this._xrCell1.Text = "Sl.No.";
            this._xrCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this._xrCell1.Weight = 0.28469967532467533D;
            // 
            // _xrCell2
            // 
            this._xrCell2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this._xrCell2.Name = "_xrCell2";
            this._xrCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this._xrCell2.StylePriority.UseBackColor = false;
            this._xrCell2.StylePriority.UseBorders = false;
            this._xrCell2.StylePriority.UseTextAlignment = false;
            this._xrCell2.Text = "Item Name";
            this._xrCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this._xrCell2.Weight = 2.1972401606572136D;
            // 
            // _xrCell3
            // 
            this._xrCell3.Name = "_xrCell3";
            this._xrCell3.StylePriority.UseTextAlignment = false;
            this._xrCell3.Text = "Units In Stock";
            this._xrCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this._xrCell3.Weight = 0.51806016401811084D;
            // 
            // ItemsStock
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this._detail,
            this._topMargin,
            this._bottomMargin,
            this._reportHeader,
            this._groupHeader});
            this.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.Margins = new System.Drawing.Printing.Margins(100, 100, 21, 100);
            this.ReportPrintOptions.DetailCountAtDesignTime = 15;
            this.Version = "13.1";
            ((System.ComponentModel.ISupportInitialize)(this._xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion
    }
}
