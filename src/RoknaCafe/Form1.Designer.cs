namespace RoknaCafe;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    private System.Windows.Forms.Panel orderPanel;
    private System.Windows.Forms.Panel totalBar;
    private System.Windows.Forms.Label lblTotalHeader;
    private System.Windows.Forms.Label lblTotalValue;
    private System.Windows.Forms.FlowLayoutPanel orderItemsPanel;
    private System.Windows.Forms.Label lblSubtotal;
    private System.Windows.Forms.Label lblTax;
    private System.Windows.Forms.Label lblTotalFooter;
    private System.Windows.Forms.Label lblTodayTotal;
    private System.Windows.Forms.Button btnPayNow;
    private System.Windows.Forms.Button btnPrint;
    private System.Windows.Forms.Button btnViewOrders;
    private System.Windows.Forms.Button btnNewOrder;

    private System.Windows.Forms.Panel menuPanel;
    private System.Windows.Forms.Panel topBar;
    private System.Windows.Forms.Label lblGreeting;
    private System.Windows.Forms.FlowLayoutPanel categoryTabsFlow;
    private System.Windows.Forms.Button btnHotDrinks;
    private System.Windows.Forms.Button btnJuices;
    private System.Windows.Forms.Button btnSmoothies;
    private System.Windows.Forms.Button btnMilkshakes;
    private System.Windows.Forms.FlowLayoutPanel menuItemsFlow;




    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(1200, 732);
        this.Text = "روكن هادي - الكاشير";
        this.Font = new Font("Tahoma", 10);
        this.BackColor = Color.White;
        

        // Icon is loaded from the output .ico file using the correct filename.
        this.Icon = new System.Drawing.Icon("Rukn-Hady.ico");

        // orderPanel
        this.orderPanel = new System.Windows.Forms.Panel();
        this.totalBar = new System.Windows.Forms.Panel();
        this.lblTotalHeader = new System.Windows.Forms.Label();
        this.lblTotalValue = new System.Windows.Forms.Label();
        this.orderItemsPanel = new System.Windows.Forms.FlowLayoutPanel();
        this.lblSubtotal = new System.Windows.Forms.Label();
        this.lblTax = new System.Windows.Forms.Label();
        this.lblTotalFooter = new System.Windows.Forms.Label();
        this.btnPayNow = new System.Windows.Forms.Button();
        this.btnPrint = new System.Windows.Forms.Button();
        this.btnNewOrder = new System.Windows.Forms.Button();
        this.orderPanel.SuspendLayout();
        this.totalBar.SuspendLayout();
        this.SuspendLayout();

        // orderPanel
        this.orderPanel.Location = new Point(0, 0);
        this.orderPanel.Name = "orderPanel";
        this.orderPanel.Size = new Size(300, 732);
        this.orderPanel.BorderStyle = BorderStyle.None;
        this.orderPanel.BackColor = Color.White;
        this.orderPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;

        // totalBar
        this.totalBar.Controls.Add(this.lblTotalValue);
        this.totalBar.Controls.Add(this.lblTotalHeader);
        this.totalBar.Dock = DockStyle.Top;
        this.totalBar.Location = new Point(0, 0);
        this.totalBar.Name = "totalBar";
        this.totalBar.Size = new Size(300, 60);
        this.totalBar.BackColor = Color.FromArgb(107, 142, 35);

        // lblTotalHeader
        this.lblTotalHeader.Dock = DockStyle.Top;
        this.lblTotalHeader.Font = new Font("Tahoma", 10, FontStyle.Bold);
        this.lblTotalHeader.ForeColor = Color.White;
        this.lblTotalHeader.Location = new Point(0, 0);
        this.lblTotalHeader.Size = new Size(300, 25);
        this.lblTotalHeader.Text = "الإجمالي";
        this.lblTotalHeader.TextAlign = ContentAlignment.MiddleCenter;

        // lblTotalValue
        this.lblTotalValue.Dock = DockStyle.Fill;
        this.lblTotalValue.Font = new Font("Tahoma", 16, FontStyle.Bold);
        this.lblTotalValue.ForeColor = Color.White;
        this.lblTotalValue.Location = new Point(0, 25);
        this.lblTotalValue.Size = new Size(300, 35);
        this.lblTotalValue.Text = "0 ج.م";
        this.lblTotalValue.TextAlign = ContentAlignment.MiddleCenter;

        // orderItemsPanel
        this.orderItemsPanel.Dock = DockStyle.Top;
        this.orderItemsPanel.Location = new Point(0, 60);
        this.orderItemsPanel.Name = "orderItemsPanel";
        this.orderItemsPanel.Size = new Size(300, 260);
        this.orderItemsPanel.AutoScroll = true;
        this.orderItemsPanel.BackColor = Color.White;
        this.orderItemsPanel.FlowDirection = FlowDirection.TopDown;
        this.orderItemsPanel.WrapContents = false;

        // lblSubtotal
        this.lblSubtotal.Dock = DockStyle.Top;
        this.lblSubtotal.Font = new Font("Tahoma", 10);
        this.lblSubtotal.ForeColor = Color.FromArgb(80, 80, 80);
        this.lblSubtotal.Location = new Point(0, 320);
        this.lblSubtotal.Size = new Size(300, 25);
        this.lblSubtotal.Text = "المجموع الفرعي: 0 ج.م";
        this.lblSubtotal.TextAlign = ContentAlignment.MiddleLeft;
        this.lblSubtotal.Padding = new Padding(10, 0, 10, 0);

        // lblTax
        this.lblTax.Dock = DockStyle.Top;
        this.lblTax.Font = new Font("Tahoma", 10);
        this.lblTax.ForeColor = Color.FromArgb(80, 80, 80);
        this.lblTax.Location = new Point(0, 345);
        this.lblTax.Size = new Size(300, 25);
        this.lblTax.Text = "الضريبة: 0 ج.م";
        this.lblTax.TextAlign = ContentAlignment.MiddleLeft;
        this.lblTax.Padding = new Padding(10, 0, 10, 0);

        // lblTotalFooter
        this.lblTotalFooter.Dock = DockStyle.Top;
        this.lblTotalFooter.Font = new Font("Tahoma", 12, FontStyle.Bold);
        this.lblTotalFooter.ForeColor = Color.FromArgb(107, 142, 35);
        this.lblTotalFooter.Location = new Point(0, 370);
        this.lblTotalFooter.Size = new Size(300, 30);
        this.lblTotalFooter.Text = "الإجمالي: 0 ج.م";
        this.lblTotalFooter.TextAlign = ContentAlignment.MiddleLeft;
        this.lblTotalFooter.Padding = new Padding(10, 0, 10, 0);

        // lblTodayTotal
        this.lblTodayTotal = new System.Windows.Forms.Label();
        this.lblTodayTotal.Dock = DockStyle.Top;
        this.lblTodayTotal.Font = new Font("Tahoma", 11, FontStyle.Bold);
        this.lblTodayTotal.ForeColor = Color.FromArgb(52, 152, 219);
        this.lblTodayTotal.Location = new Point(0, 400);
        this.lblTodayTotal.Size = new Size(300, 32);
        this.lblTodayTotal.Text = "إجمالي اليوم: جاري التحميل...";
        this.lblTodayTotal.TextAlign = ContentAlignment.MiddleLeft;
        this.lblTodayTotal.Padding = new Padding(10, 0, 10, 0);

        // btnPayNow
        this.btnPayNow = new System.Windows.Forms.Button();
        this.btnPayNow.Dock = DockStyle.Bottom;
        this.btnPayNow.FlatStyle = FlatStyle.Flat;
        this.btnPayNow.Font = new Font("Tahoma", 12, FontStyle.Bold);
        this.btnPayNow.ForeColor = Color.White;
        this.btnPayNow.Size = new Size(300, 45);
        this.btnPayNow.Text = "ادفع الان";
        this.btnPayNow.UseVisualStyleBackColor = false;
        this.btnPayNow.BackColor = Color.FromArgb(52, 152, 219);
        this.btnPayNow.FlatAppearance.BorderSize = 0;

        // btnPrint
        this.btnPrint = new System.Windows.Forms.Button();
        this.btnPrint.Dock = DockStyle.Bottom;
        this.btnPrint.FlatStyle = FlatStyle.Flat;
        this.btnPrint.Font = new Font("Tahoma", 11, FontStyle.Bold);
        this.btnPrint.ForeColor = Color.White;
        this.btnPrint.Size = new Size(300, 40);
        this.btnPrint.Text = "طباعة الفاتورة";
        this.btnPrint.UseVisualStyleBackColor = false;
        this.btnPrint.BackColor = Color.FromArgb(107, 142, 35);
        this.btnPrint.FlatAppearance.BorderSize = 0;

        // btnViewOrders
        this.btnViewOrders = new System.Windows.Forms.Button();
        this.btnViewOrders.Dock = DockStyle.Bottom;
        this.btnViewOrders.FlatStyle = FlatStyle.Flat;
        this.btnViewOrders.Font = new Font("Tahoma", 11, FontStyle.Bold);
        this.btnViewOrders.ForeColor = Color.FromArgb(80, 80, 80);
        this.btnViewOrders.Size = new Size(300, 45);
        this.btnViewOrders.Text = "طلبات اليوم";
        this.btnViewOrders.UseVisualStyleBackColor = false;
        this.btnViewOrders.BackColor = Color.FromArgb(248, 249, 250);
        this.btnViewOrders.FlatAppearance.BorderSize = 1;
        this.btnViewOrders.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);

        // btnNewOrder
        this.btnNewOrder = new System.Windows.Forms.Button();
        this.btnNewOrder.Dock = DockStyle.Bottom;
        this.btnNewOrder.FlatStyle = FlatStyle.Flat;
        this.btnNewOrder.Font = new Font("Tahoma", 11);
        this.btnNewOrder.ForeColor = Color.FromArgb(120, 120, 120);
        this.btnNewOrder.Size = new Size(300, 35);
        this.btnNewOrder.Text = "طلب جديد";
        this.btnNewOrder.UseVisualStyleBackColor = false;
        this.btnNewOrder.BackColor = Color.FromArgb(240, 240, 240);
        this.btnNewOrder.FlatAppearance.BorderSize = 1;
        this.btnNewOrder.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);

        this.orderPanel.Controls.Add(this.totalBar);
        this.orderPanel.Controls.Add(this.orderItemsPanel);
        this.orderPanel.Controls.Add(this.lblSubtotal);
        this.orderPanel.Controls.Add(this.lblTax);
        this.orderPanel.Controls.Add(this.lblTotalFooter);
        this.orderPanel.Controls.Add(this.lblTodayTotal);
        this.orderPanel.Controls.Add(this.btnPrint);
        this.orderPanel.Controls.Add(this.btnViewOrders);
        this.orderPanel.Controls.Add(this.btnPayNow);
        this.orderPanel.Controls.Add(this.btnNewOrder);

        // menuPanel
        this.menuPanel = new System.Windows.Forms.Panel();
        this.topBar = new System.Windows.Forms.Panel();
        this.lblGreeting = new System.Windows.Forms.Label();
        this.categoryTabsFlow = new System.Windows.Forms.FlowLayoutPanel();
        this.menuItemsFlow = new System.Windows.Forms.FlowLayoutPanel();
        this.menuPanel.SuspendLayout();
        this.topBar.SuspendLayout();

        // menuPanel
        this.menuPanel.Location = new Point(300, 0);
        this.menuPanel.Name = "menuPanel";
        this.menuPanel.Size = new Size(870, 680);
        this.menuPanel.BorderStyle = BorderStyle.None;
        this.menuPanel.BackColor = Color.White;
        this.menuPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        // topBar
        this.topBar.Controls.Add(this.lblGreeting);
        this.topBar.Dock = DockStyle.Top;
        this.topBar.Location = new Point(0, 0);
        this.topBar.Name = "topBar";
        this.topBar.Size = new Size(870, 45);
        this.topBar.BackColor = Color.White;

        // lblGreeting
        this.lblGreeting.Dock = DockStyle.Right;
        this.lblGreeting.Font = new Font("Tahoma", 11, FontStyle.Bold);
        this.lblGreeting.ForeColor = Color.FromArgb(52, 152, 219);
        this.lblGreeting.Location = new Point(690, 0);
        this.lblGreeting.Size = new Size(180, 45);
        this.lblGreeting.Text = "مرحباً، كاشير";
        this.lblGreeting.TextAlign = ContentAlignment.MiddleRight;

        // categoryTabsFlow
        this.categoryTabsFlow.Dock = DockStyle.Top;
        this.categoryTabsFlow.Location = new Point(0, 45);
        this.categoryTabsFlow.Name = "categoryTabsFlow";
        this.categoryTabsFlow.Size = new Size(870, 55);
        this.categoryTabsFlow.FlowDirection = FlowDirection.RightToLeft;
        this.categoryTabsFlow.Padding = new Padding(10, 5, 10, 5);

        // menuItemsFlow
        this.menuItemsFlow.Dock = DockStyle.Fill;
        this.menuItemsFlow.Location = new Point(0, 100);
        this.menuItemsFlow.Name = "menuItemsFlow";
        this.menuItemsFlow.Size = new Size(870, 580);
        this.menuItemsFlow.AutoScroll = true;
        this.menuItemsFlow.BackColor = Color.White;
        this.menuItemsFlow.FlowDirection = FlowDirection.RightToLeft;
        this.menuItemsFlow.Padding = new Padding(10, 10, 10, 10);
        this.menuItemsFlow.WrapContents = true;

        // Category tabs
        this.btnHotDrinks = new System.Windows.Forms.Button();
        this.btnJuices = new System.Windows.Forms.Button();
        this.btnSmoothies = new System.Windows.Forms.Button();
        this.btnMilkshakes = new System.Windows.Forms.Button();

        this.btnHotDrinks.FlatStyle = FlatStyle.Flat;
        this.btnHotDrinks.Font = new Font("Tahoma", 11, FontStyle.Bold);
        this.btnHotDrinks.Size = new Size(150, 40);
        this.btnHotDrinks.Text = "مشروبات ساخنة";
        this.btnHotDrinks.BackColor = Color.FromArgb(52, 152, 219);
        this.btnHotDrinks.ForeColor = Color.White;
        this.btnHotDrinks.UseVisualStyleBackColor = false;
        this.btnHotDrinks.FlatAppearance.BorderSize = 0;

        this.btnJuices.FlatStyle = FlatStyle.Flat;
        this.btnJuices.Font = new Font("Tahoma", 11);
        this.btnJuices.Size = new Size(120, 40);
        this.btnJuices.Text = "عصائر";
        this.btnJuices.BackColor = Color.White;
        this.btnJuices.ForeColor = Color.FromArgb(52, 152, 219);
        this.btnJuices.UseVisualStyleBackColor = false;
        this.btnJuices.FlatAppearance.BorderSize = 1;
        this.btnJuices.FlatAppearance.BorderColor = Color.FromArgb(52, 152, 219);

        this.btnSmoothies.FlatStyle = FlatStyle.Flat;
        this.btnSmoothies.Font = new Font("Tahoma", 11);
        this.btnSmoothies.Size = new Size(120, 40);
        this.btnSmoothies.Text = "اسموزي";
        this.btnSmoothies.BackColor = Color.White;
        this.btnSmoothies.ForeColor = Color.FromArgb(52, 152, 219);
        this.btnSmoothies.UseVisualStyleBackColor = false;
        this.btnSmoothies.FlatAppearance.BorderSize = 1;
        this.btnSmoothies.FlatAppearance.BorderColor = Color.FromArgb(52, 152, 219);

        this.btnMilkshakes.FlatStyle = FlatStyle.Flat;
        this.btnMilkshakes.Font = new Font("Tahoma", 11);
        this.btnMilkshakes.Size = new Size(120, 40);
        this.btnMilkshakes.Text = "ميلك شيك";
        this.btnMilkshakes.BackColor = Color.White;
        this.btnMilkshakes.ForeColor = Color.FromArgb(52, 152, 219);
        this.btnMilkshakes.UseVisualStyleBackColor = false;
        this.btnMilkshakes.FlatAppearance.BorderSize = 1;
        this.btnMilkshakes.FlatAppearance.BorderColor = Color.FromArgb(52, 152, 219);

        this.categoryTabsFlow.Controls.Add(this.btnMilkshakes);
        this.categoryTabsFlow.Controls.Add(this.btnSmoothies);
        this.categoryTabsFlow.Controls.Add(this.btnJuices);
        this.categoryTabsFlow.Controls.Add(this.btnHotDrinks);

        this.menuPanel.Controls.Add(this.menuItemsFlow);
        this.menuPanel.Controls.Add(this.categoryTabsFlow);
        this.menuPanel.Controls.Add(this.topBar);
        this.topBar.ResumeLayout(false);
        this.menuPanel.ResumeLayout(false);

        this.orderPanel.ResumeLayout(false);
        // Form
        this.Controls.Add(this.menuPanel);
        this.Controls.Add(this.orderPanel);
        this.ResumeLayout(false);
    }
}
