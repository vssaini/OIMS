using System.Globalization;
using DevExpress.XtraReports.UI;
using OIMS.Repository.Supervisor;

namespace OIMS.Reports
{
    /// <summary>
    /// Summary description for XtraReport
    /// </summary>
    public class ShelterDetail : XtraReport
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
        private XRTableCell _xrChSlNo;
        private XRTableCell _xrChItemName;
        private XRTableCell _xrChSize;

        // Detail band and content
        private DetailBand _detail;
        private XRTable _xrTable2;
        private XRTableRow _xrRow2;
        private XRTableCell _xrCvSlNo;
        private XRTableCell _xrCvItemName;
        private XRTableCell _xrCvSize;
        private XRPageInfo _pageInfoNumber;
        private XRLabel _xrLblShelter;
        private XRLabel _xrLblStockCount;
        private XRLabel _lblShelterName;
        private XRLabel _lblShelterCount;
        private XRLabel _lblShelterDesc;
        private XRControlStyle _xrControlStyle1;
        private XRControlStyle _xrControlStyle2;
        private XRTableCell _xrChMarking;
        private XRTableCell _xrChQtyPerShelter;
        private XRTableCell _xrChInStock;
        private XRTableCell _xrChLastUpdated;
        private XRTableCell _xrChVendor;
        private XRTableCell _xrCvMarking;
        private XRTableCell _xrCvQtyPerShelter;
        private XRTableCell _xrCvInStock;
        private XRTableCell _xrCvLastUpdated;
        private XRTableCell _xrCvVendor;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private readonly System.ComponentModel.IContainer components = null;

        public ShelterDetail()
        {
            InitializeComponent();

            // 1. Get single shelter based on ShelterId
            var shelter = ShelterRepository.GetShelter();
            if (shelter == null) return;

            // 2. Set label's text from obtained details of Shelter
            _lblShelterName.Text = shelter.ShelterName;
            _lblShelterCount.Text = ShelterRepository.PossibleShelterStock(shelter.ShelterId).ToString(CultureInfo.InvariantCulture);
            _lblShelterDesc.Text = string.Format("List of items that make '{0}' -", shelter.ShelterName);

            // 3. Get shelter's items
            var shelterItems = ShelterRepository.GetShelterItemsForReport(shelter.ShelterId);

            // 4. Set datasource of report
            DataSource = shelterItems;

            // 5. Bind to cells in table
            _xrCvSlNo.DataBindings.AddRange(new[] { new XRBinding("Text", null, "ShelterDescKey") });
            _xrCvItemName.DataBindings.AddRange(new[] { new XRBinding("Text", null, "ItemName") });
            _xrCvSize.DataBindings.AddRange(new[] { new XRBinding("Text", null, "Size") });
            _xrCvMarking.DataBindings.AddRange(new[] { new XRBinding("Text", null, "Marking") });
            _xrCvQtyPerShelter.DataBindings.AddRange(new[] { new XRBinding("Text", null, "ItemQuantity") });
            _xrCvInStock.DataBindings.AddRange(new[] { new XRBinding("Text", null, "InStock") });
            _xrCvLastUpdated.DataBindings.AddRange(new[] { new XRBinding("Text", null, "UpdatedOn", "{0:dd-MM-yyyy}") });
            _xrCvVendor.DataBindings.AddRange(new[] { new XRBinding("Text", null, "Vendor") });
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
            this._xrCvSlNo = new DevExpress.XtraReports.UI.XRTableCell();
            this._xrCvItemName = new DevExpress.XtraReports.UI.XRTableCell();
            this._xrCvSize = new DevExpress.XtraReports.UI.XRTableCell();
            this._xrCvMarking = new DevExpress.XtraReports.UI.XRTableCell();
            this._xrCvQtyPerShelter = new DevExpress.XtraReports.UI.XRTableCell();
            this._xrCvInStock = new DevExpress.XtraReports.UI.XRTableCell();
            this._xrCvLastUpdated = new DevExpress.XtraReports.UI.XRTableCell();
            this._xrCvVendor = new DevExpress.XtraReports.UI.XRTableCell();
            this._topMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this._bottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this._pageInfoNumber = new DevExpress.XtraReports.UI.XRPageInfo();
            this._lbTitle = new DevExpress.XtraReports.UI.XRLabel();
            this._reportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this._lblShelterDesc = new DevExpress.XtraReports.UI.XRLabel();
            this._lblShelterCount = new DevExpress.XtraReports.UI.XRLabel();
            this._lblShelterName = new DevExpress.XtraReports.UI.XRLabel();
            this._xrLblStockCount = new DevExpress.XtraReports.UI.XRLabel();
            this._xrLblShelter = new DevExpress.XtraReports.UI.XRLabel();
            this._xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this._pageInfoDate = new DevExpress.XtraReports.UI.XRPageInfo();
            this._groupHeader = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this._xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this._xrRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this._xrChSlNo = new DevExpress.XtraReports.UI.XRTableCell();
            this._xrChItemName = new DevExpress.XtraReports.UI.XRTableCell();
            this._xrChSize = new DevExpress.XtraReports.UI.XRTableCell();
            this._xrChMarking = new DevExpress.XtraReports.UI.XRTableCell();
            this._xrChQtyPerShelter = new DevExpress.XtraReports.UI.XRTableCell();
            this._xrChInStock = new DevExpress.XtraReports.UI.XRTableCell();
            this._xrChLastUpdated = new DevExpress.XtraReports.UI.XRTableCell();
            this._xrChVendor = new DevExpress.XtraReports.UI.XRTableCell();
            this._xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this._xrControlStyle2 = new DevExpress.XtraReports.UI.XRControlStyle();
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
            this._xrTable2.EvenStyleName = "xrControlStyle1";
            this._xrTable2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._xrTable2.ForeColor = System.Drawing.Color.Black;
            this._xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this._xrTable2.Name = "_xrTable2";
            this._xrTable2.OddStyleName = "xrControlStyle2";
            this._xrTable2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this._xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this._xrRow2});
            this._xrTable2.SizeF = new System.Drawing.SizeF(819.9999F, 32.08329F);
            this._xrTable2.StylePriority.UseBorderColor = false;
            this._xrTable2.StylePriority.UseBorders = false;
            this._xrTable2.StylePriority.UseBorderWidth = false;
            this._xrTable2.StylePriority.UseFont = false;
            // 
            // _xrRow2
            // 
            this._xrRow2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this._xrRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this._xrCvSlNo,
            this._xrCvItemName,
            this._xrCvSize,
            this._xrCvMarking,
            this._xrCvQtyPerShelter,
            this._xrCvInStock,
            this._xrCvLastUpdated,
            this._xrCvVendor});
            this._xrRow2.Name = "_xrRow2";
            this._xrRow2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this._xrRow2.Weight = 1D;
            // 
            // _xrCvSlNo
            // 
            this._xrCvSlNo.BackColor = System.Drawing.Color.Empty;
            this._xrCvSlNo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(209)))), ((int)(((byte)(228)))));
            this._xrCvSlNo.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this._xrCvSlNo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._xrCvSlNo.Name = "_xrCvSlNo";
            this._xrCvSlNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this._xrCvSlNo.StylePriority.UseBackColor = false;
            this._xrCvSlNo.StylePriority.UseBorderColor = false;
            this._xrCvSlNo.StylePriority.UseBorders = false;
            this._xrCvSlNo.StylePriority.UseFont = false;
            this._xrCvSlNo.StylePriority.UseTextAlignment = false;
            this._xrCvSlNo.Text = "Id";
            this._xrCvSlNo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this._xrCvSlNo.Weight = 0.20083526305993307D;
            // 
            // _xrCvItemName
            // 
            this._xrCvItemName.BackColor = System.Drawing.Color.Empty;
            this._xrCvItemName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(209)))), ((int)(((byte)(228)))));
            this._xrCvItemName.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this._xrCvItemName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._xrCvItemName.Name = "_xrCvItemName";
            this._xrCvItemName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this._xrCvItemName.StylePriority.UseBackColor = false;
            this._xrCvItemName.StylePriority.UseBorderColor = false;
            this._xrCvItemName.StylePriority.UseBorders = false;
            this._xrCvItemName.StylePriority.UseFont = false;
            this._xrCvItemName.StylePriority.UseTextAlignment = false;
            this._xrCvItemName.Text = "Name of Item";
            this._xrCvItemName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this._xrCvItemName.Weight = 0.92399248220670493D;
            // 
            // _xrCvSize
            // 
            this._xrCvSize.BackColor = System.Drawing.Color.Empty;
            this._xrCvSize.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(209)))), ((int)(((byte)(228)))));
            this._xrCvSize.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this._xrCvSize.Font = new System.Drawing.Font("Verdana", 8.25F);
            this._xrCvSize.Name = "_xrCvSize";
            this._xrCvSize.StylePriority.UseBackColor = false;
            this._xrCvSize.StylePriority.UseBorderColor = false;
            this._xrCvSize.StylePriority.UseBorders = false;
            this._xrCvSize.StylePriority.UseFont = false;
            this._xrCvSize.StylePriority.UseTextAlignment = false;
            this._xrCvSize.Text = "Size";
            this._xrCvSize.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this._xrCvSize.Weight = 0.27942404743317539D;
            // 
            // _xrCvMarking
            // 
            this._xrCvMarking.BackColor = System.Drawing.Color.Empty;
            this._xrCvMarking.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this._xrCvMarking.Name = "_xrCvMarking";
            this._xrCvMarking.StylePriority.UseBackColor = false;
            this._xrCvMarking.StylePriority.UseBorderColor = false;
            this._xrCvMarking.StylePriority.UseBorders = false;
            this._xrCvMarking.StylePriority.UseTextAlignment = false;
            this._xrCvMarking.Text = "Marking";
            this._xrCvMarking.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this._xrCvMarking.Weight = 0.30194237605229013D;
            // 
            // _xrCvQtyPerShelter
            // 
            this._xrCvQtyPerShelter.BackColor = System.Drawing.Color.Empty;
            this._xrCvQtyPerShelter.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this._xrCvQtyPerShelter.Name = "_xrCvQtyPerShelter";
            this._xrCvQtyPerShelter.StylePriority.UseBackColor = false;
            this._xrCvQtyPerShelter.StylePriority.UseBorderColor = false;
            this._xrCvQtyPerShelter.StylePriority.UseBorders = false;
            this._xrCvQtyPerShelter.StylePriority.UseTextAlignment = false;
            this._xrCvQtyPerShelter.Text = "Qty/Sh.";
            this._xrCvQtyPerShelter.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this._xrCvQtyPerShelter.Weight = 0.26930325305658054D;
            // 
            // _xrCvInStock
            // 
            this._xrCvInStock.BackColor = System.Drawing.Color.Empty;
            this._xrCvInStock.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this._xrCvInStock.Name = "_xrCvInStock";
            this._xrCvInStock.StylePriority.UseBackColor = false;
            this._xrCvInStock.StylePriority.UseBorderColor = false;
            this._xrCvInStock.StylePriority.UseBorders = false;
            this._xrCvInStock.StylePriority.UseTextAlignment = false;
            this._xrCvInStock.Text = "InStock";
            this._xrCvInStock.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this._xrCvInStock.Weight = 0.29623750002572535D;
            // 
            // _xrCvLastUpdated
            // 
            this._xrCvLastUpdated.BackColor = System.Drawing.Color.Empty;
            this._xrCvLastUpdated.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this._xrCvLastUpdated.Name = "_xrCvLastUpdated";
            this._xrCvLastUpdated.StylePriority.UseBackColor = false;
            this._xrCvLastUpdated.StylePriority.UseBorderColor = false;
            this._xrCvLastUpdated.StylePriority.UseBorders = false;
            this._xrCvLastUpdated.StylePriority.UseTextAlignment = false;
            this._xrCvLastUpdated.Text = "Last Updated";
            this._xrCvLastUpdated.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this._xrCvLastUpdated.Weight = 0.4094528226758104D;
            // 
            // _xrCvVendor
            // 
            this._xrCvVendor.BackColor = System.Drawing.Color.Empty;
            this._xrCvVendor.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this._xrCvVendor.Name = "_xrCvVendor";
            this._xrCvVendor.StylePriority.UseBackColor = false;
            this._xrCvVendor.StylePriority.UseBorderColor = false;
            this._xrCvVendor.StylePriority.UseBorders = false;
            this._xrCvVendor.StylePriority.UseTextAlignment = false;
            this._xrCvVendor.Text = "Vendor";
            this._xrCvVendor.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this._xrCvVendor.Weight = 0.79207230490929825D;
            // 
            // TopMargin
            // 
            this._topMargin.HeightF = 22.91667F;
            this._topMargin.Name = "TopMargin";
            this._topMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this._topMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this._bottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this._pageInfoNumber});
            this._bottomMargin.HeightF = 90.00001F;
            this._bottomMargin.Name = "BottomMargin";
            this._bottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this._bottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // _pageInfoNumber
            // 
            this._pageInfoNumber.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this._pageInfoNumber.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._pageInfoNumber.Format = "Page: {0}/{1}";
            this._pageInfoNumber.LocationFloat = new DevExpress.Utils.PointFloat(719.9999F, 57.00003F);
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
            this._lbTitle.SizeF = new System.Drawing.SizeF(173.4167F, 38F);
            this._lbTitle.StylePriority.UseFont = false;
            this._lbTitle.StylePriority.UseForeColor = false;
            this._lbTitle.StylePriority.UseTextAlignment = false;
            this._lbTitle.Text = "Shelter Type";
            this._lbTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // ReportHeader
            // 
            this._reportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this._lblShelterDesc,
            this._lblShelterCount,
            this._lblShelterName,
            this._xrLblStockCount,
            this._xrLblShelter,
            this._xrLine1,
            this._lbTitle,
            this._pageInfoDate});
            this._reportHeader.HeightF = 177.1667F;
            this._reportHeader.Name = "ReportHeader";
            // 
            // lblShelterDesc
            // 
            this._lblShelterDesc.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this._lblShelterDesc.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblShelterDesc.ForeColor = System.Drawing.Color.DarkGreen;
            this._lblShelterDesc.LocationFloat = new DevExpress.Utils.PointFloat(0F, 154.1667F);
            this._lblShelterDesc.Name = "lblShelterDesc";
            this._lblShelterDesc.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this._lblShelterDesc.SizeF = new System.Drawing.SizeF(819.9998F, 23F);
            this._lblShelterDesc.StylePriority.UseBorders = false;
            this._lblShelterDesc.StylePriority.UseFont = false;
            this._lblShelterDesc.StylePriority.UseForeColor = false;
            this._lblShelterDesc.Text = "Desc";
            // 
            // lblShelterCount
            // 
            this._lblShelterCount.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this._lblShelterCount.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblShelterCount.LocationFloat = new DevExpress.Utils.PointFloat(146F, 121.9166F);
            this._lblShelterCount.Name = "lblShelterCount";
            this._lblShelterCount.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this._lblShelterCount.SizeF = new System.Drawing.SizeF(100F, 23F);
            this._lblShelterCount.StylePriority.UseBorders = false;
            this._lblShelterCount.StylePriority.UseFont = false;
            // 
            // lblShelterName
            // 
            this._lblShelterName.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this._lblShelterName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblShelterName.LocationFloat = new DevExpress.Utils.PointFloat(113.7084F, 87.45833F);
            this._lblShelterName.Name = "lblShelterName";
            this._lblShelterName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this._lblShelterName.SizeF = new System.Drawing.SizeF(706.2914F, 23F);
            this._lblShelterName.StylePriority.UseBorders = false;
            this._lblShelterName.StylePriority.UseFont = false;
            // 
            // xrLblStockCount
            // 
            this._xrLblStockCount.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this._xrLblStockCount.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._xrLblStockCount.ForeColor = System.Drawing.Color.Teal;
            this._xrLblStockCount.LocationFloat = new DevExpress.Utils.PointFloat(0F, 121.9166F);
            this._xrLblStockCount.Name = "xrLblStockCount";
            this._xrLblStockCount.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this._xrLblStockCount.SizeF = new System.Drawing.SizeF(146F, 23F);
            this._xrLblStockCount.StylePriority.UseBorders = false;
            this._xrLblStockCount.StylePriority.UseFont = false;
            this._xrLblStockCount.StylePriority.UseForeColor = false;
            this._xrLblStockCount.Text = "Possible Stock Count:";
            // 
            // xrLblShelter
            // 
            this._xrLblShelter.BackColor = System.Drawing.Color.Transparent;
            this._xrLblShelter.BorderColor = System.Drawing.Color.SpringGreen;
            this._xrLblShelter.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this._xrLblShelter.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._xrLblShelter.ForeColor = System.Drawing.Color.Teal;
            this._xrLblShelter.LocationFloat = new DevExpress.Utils.PointFloat(0F, 87.45833F);
            this._xrLblShelter.Name = "xrLblShelter";
            this._xrLblShelter.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this._xrLblShelter.SizeF = new System.Drawing.SizeF(113.7084F, 23F);
            this._xrLblShelter.StylePriority.UseBackColor = false;
            this._xrLblShelter.StylePriority.UseBorderColor = false;
            this._xrLblShelter.StylePriority.UseBorders = false;
            this._xrLblShelter.StylePriority.UseFont = false;
            this._xrLblShelter.StylePriority.UseForeColor = false;
            this._xrLblShelter.Text = "Name of Shelter:";
            // 
            // _xrLine1
            // 
            this._xrLine1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this._xrLine1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(167)))), ((int)(((byte)(73)))));
            this._xrLine1.LineWidth = 2;
            this._xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 59.12501F);
            this._xrLine1.Name = "_xrLine1";
            this._xrLine1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this._xrLine1.SizeF = new System.Drawing.SizeF(819.9999F, 9.000004F);
            this._xrLine1.StylePriority.UseBorders = false;
            this._xrLine1.StylePriority.UseForeColor = false;
            // 
            // _pageInfoDate
            // 
            this._pageInfoDate.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this._pageInfoDate.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this._pageInfoDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(132)))), ((int)(((byte)(213)))));
            this._pageInfoDate.Format = "{0: dddd, dd MMMM yyyy}";
            this._pageInfoDate.LocationFloat = new DevExpress.Utils.PointFloat(629.0417F, 36.12502F);
            this._pageInfoDate.Name = "_pageInfoDate";
            this._pageInfoDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this._pageInfoDate.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this._pageInfoDate.SizeF = new System.Drawing.SizeF(190.9583F, 23F);
            this._pageInfoDate.StylePriority.UseBorders = false;
            this._pageInfoDate.StylePriority.UseFont = false;
            this._pageInfoDate.StylePriority.UseForeColor = false;
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
            this._xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 7.999992F);
            this._xrTable1.Name = "_xrTable1";
            this._xrTable1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this._xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this._xrRow1});
            this._xrTable1.SizeF = new System.Drawing.SizeF(820F, 43.45833F);
            this._xrTable1.StylePriority.UseBackColor = false;
            this._xrTable1.StylePriority.UseBorderColor = false;
            this._xrTable1.StylePriority.UseBorders = false;
            this._xrTable1.StylePriority.UseFont = false;
            this._xrTable1.StylePriority.UseForeColor = false;
            // 
            // _xrRow1
            // 
            this._xrRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this._xrChSlNo,
            this._xrChItemName,
            this._xrChSize,
            this._xrChMarking,
            this._xrChQtyPerShelter,
            this._xrChInStock,
            this._xrChLastUpdated,
            this._xrChVendor});
            this._xrRow1.Name = "_xrRow1";
            this._xrRow1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this._xrRow1.Weight = 1D;
            // 
            // _xrChSlNo
            // 
            this._xrChSlNo.Name = "_xrChSlNo";
            this._xrChSlNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this._xrChSlNo.StylePriority.UseTextAlignment = false;
            this._xrChSlNo.Text = "#";
            this._xrChSlNo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this._xrChSlNo.Weight = 0.20594556464635061D;
            // 
            // _xrChItemName
            // 
            this._xrChItemName.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
            | DevExpress.XtraPrinting.BorderSide.Right)
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this._xrChItemName.Name = "_xrChItemName";
            this._xrChItemName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this._xrChItemName.StylePriority.UseBackColor = false;
            this._xrChItemName.StylePriority.UseBorders = false;
            this._xrChItemName.StylePriority.UseTextAlignment = false;
            this._xrChItemName.Text = "Name of Item";
            this._xrChItemName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this._xrChItemName.Weight = 0.94750356215784992D;
            // 
            // _xrChSize
            // 
            this._xrChSize.Name = "_xrChSize";
            this._xrChSize.StylePriority.UseTextAlignment = false;
            this._xrChSize.Text = "Size";
            this._xrChSize.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this._xrChSize.Weight = 0.28653416425893291D;
            // 
            // _xrChMarking
            // 
            this._xrChMarking.Name = "_xrChMarking";
            this._xrChMarking.StylePriority.UseTextAlignment = false;
            this._xrChMarking.Text = "Marking";
            this._xrChMarking.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this._xrChMarking.Weight = 0.30962536634258941D;
            // 
            // _xrChQtyPerShelter
            // 
            this._xrChQtyPerShelter.Name = "_xrChQtyPerShelter";
            this._xrChQtyPerShelter.StylePriority.UseTextAlignment = false;
            this._xrChQtyPerShelter.Text = "Qty/Sh.";
            this._xrChQtyPerShelter.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this._xrChQtyPerShelter.Weight = 0.27615575239297679D;
            // 
            // _xrChInStock
            // 
            this._xrChInStock.Name = "_xrChInStock";
            this._xrChInStock.StylePriority.UseTextAlignment = false;
            this._xrChInStock.Text = "In stock";
            this._xrChInStock.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this._xrChInStock.Weight = 0.30377533896269671D;
            // 
            // _xrChLastUpdated
            // 
            this._xrChLastUpdated.Name = "_xrChLastUpdated";
            this._xrChLastUpdated.StylePriority.UseTextAlignment = false;
            this._xrChLastUpdated.Text = "Last Updated";
            this._xrChLastUpdated.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this._xrChLastUpdated.Weight = 0.41987117121952783D;
            // 
            // _xrChVendor
            // 
            this._xrChVendor.Name = "_xrChVendor";
            this._xrChVendor.StylePriority.UseTextAlignment = false;
            this._xrChVendor.Text = "Vendor";
            this._xrChVendor.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this._xrChVendor.Weight = 0.81222726981267024D;
            // 
            // xrControlStyle1
            // 
            this._xrControlStyle1.BackColor = System.Drawing.Color.LightGreen;
            this._xrControlStyle1.Name = "xrControlStyle1";
            this._xrControlStyle1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // xrControlStyle2
            // 
            this._xrControlStyle2.Name = "xrControlStyle2";
            this._xrControlStyle2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // SheltersStock
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
            this.Margins = new System.Drawing.Printing.Margins(10, 10, 23, 90);
            this.ReportPrintOptions.DetailCountAtDesignTime = 15;
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this._xrControlStyle1,
            this._xrControlStyle2});
            this.Version = "13.1";
            ((System.ComponentModel.ISupportInitialize)(this._xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion
    }
}
