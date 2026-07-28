using Rokna.Domain.Entities;
using Rokna.Domain.Interfaces;

namespace RoknaCafe;

public class OrderItem
{
    public int MenuItemId { get; set; }
    public string Name { get; set; } = "";
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}




public partial class Form1 : Form
{
    private System.Drawing.Printing.PrintDocument printDocument;
    private PrintPreviewDialog printPreviewDialog;
    private List<OrderItem> currentOrderItems = new List<OrderItem>();
    private decimal currentSubtotal;
    private decimal currentTax;
    private decimal currentTotal;
    private readonly ICategoryService _categoryService;
    private readonly IMenuItemService _menuItemService;
    private readonly IOrderService _orderService;

    public Form1(ICategoryService categoryService, IMenuItemService menuItemService, IOrderService orderService)
    {
        _categoryService = categoryService;
        _menuItemService = menuItemService;
        _orderService = orderService;

        InitializeComponent();

        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        using (var stream = assembly.GetManifestResourceStream("RoknaCafe.rukn-hady.ico"))
        {
            if (stream != null)
                this.Icon = new Icon(stream);
        }
        SetupPrintDocument();
        btnPrint.Click += BtnPrint_Click;
        btnPayNow.Click += BtnPayNow_Click;
        btnViewOrders.Click += BtnViewOrders_Click;
        btnNewOrder.Click += BtnNewOrder_Click;

        btnHotDrinks.Click += CategoryButton_Click;
        btnJuices.Click += CategoryButton_Click;
        btnSmoothies.Click += CategoryButton_Click;
        btnMilkshakes.Click += CategoryButton_Click;

        this.Load += Form1_Load;
        this.WindowState = FormWindowState.Maximized;
    }

    private void SetupPrintDocument()
    {
        printDocument = new System.Drawing.Printing.PrintDocument();
        printDocument.PrintPage += PrintDocument_PrintPage;
        printDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Receipt", 280, 800);

        printPreviewDialog = new PrintPreviewDialog();
        printPreviewDialog.Document = printDocument;
        printPreviewDialog.Width = 450;
        printPreviewDialog.Height = 700;
    }

    private async void Form1_Load(object sender, EventArgs e)
    {
        await InitializeAsync();
        await UpdateTodayTotalAsync();
    }

    private async Task UpdateTodayTotalAsync()
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        var todayOrders = (await _orderService.GetByDateRangeAsync(today, tomorrow))
            .Where(o => o.Status == OrderStatus.Completed)
            .ToList();

        var total = todayOrders.Sum(o => o.TotalAmount);

        lblTodayTotal.Text = total == 0
            ? "إجمالي اليوم: 0 ج.م"
            : $"إجمالي اليوم: {total:F0} ج.م";
    }

    public async Task InitializeAsync()
    {
        var cats = await _categoryService.GetAllAsync();
        if (!cats.Any()) return;

        var first = cats.First();
        await LoadMenuItems(first.Name);
        SetActiveCategoryButton(FindButtonByText(first.Name));
    }

    private async void CategoryButton_Click(object sender, EventArgs e)
    {
        if (sender is not Button btn) return;

        string categoryName = btn.Text;
        await LoadMenuItems(categoryName);
        SetActiveCategoryButton(btn);
    }

    private async Task LoadMenuItems(string categoryName)
    {
        menuItemsFlow.Controls.Clear();

        var categories = await _categoryService.GetAllAsync();
        var category = categories.FirstOrDefault(c => c.Name == categoryName);
        if (category is null)
        {
            MessageBox.Show($"Category '{categoryName}' was not found.");
            return;
        }

        var menuItems = await _menuItemService.GetByCategoryAsync(category.Id);

        foreach (var item in menuItems)
        {
            var card = CreateMenuItemCard(item);
            menuItemsFlow.Controls.Add(card);
        }
    }

    private Panel CreateMenuItemCard(Rokna.Domain.Entities.MenuItem item)
    {
        var card = new Panel
        {
            Size = new Size(160, 100),
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Color.FromArgb(250, 251, 252),
            Margin = new Padding(8)
        };

        var lblName = new Label
        {
            Text = item.Name,
            Font = new Font("Tahoma", 10, FontStyle.Bold),
            ForeColor = Color.FromArgb(50, 50, 50),
            Location = new Point(10, 15),
            AutoSize = true,
            MaximumSize = new Size(140, 0)
        };

        var lblPrice = new Label
        {
            Text = $"{item.Price:F0} ج.م",
            Font = new Font("Tahoma", 9),
            ForeColor = Color.FromArgb(107, 142, 35),
            Location = new Point(10, 65),
            AutoSize = true
        };

        var btnAdd = new Button
        {
            Size = new Size(50, 30),
            Location = new Point(100, 60),
            Text = "أضف",
            Font = new Font("Tahoma", 9, FontStyle.Bold),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatAppearance = { BorderSize = 0 }
        };

        btnAdd.Click += (s, e) => AddToOrder(item);

        card.Controls.Add(lblName);
        card.Controls.Add(lblPrice);
        card.Controls.Add(btnAdd);

        return card;
    }

    private void AddToOrder(Rokna.Domain.Entities.MenuItem item)
    {
        var existing = currentOrderItems.FirstOrDefault(o => o.Name == item.Name);

        if (existing != null)
            existing.Quantity++;
        else
            currentOrderItems.Add(new OrderItem
            {
                MenuItemId = item.Id,
                Name = item.Name,
                Price = item.Price,
                Quantity = 1
            });

        RenderOrderItems();
        CollectOrderData();
    }

    private void RenderOrderItems()
    {
        orderItemsPanel.Controls.Clear();

        foreach (var item in currentOrderItems)
        {
            var miniCard = new Panel
            {
                Size = new Size(280, 42),
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                Margin = new Padding(2)
            };

            var lbl = new Label
            {
                Text = $"{item.Name} × {item.Quantity}",
                Font = new Font("Tahoma", 9),
                ForeColor = Color.FromArgb(50, 50, 50),
                Location = new Point(6, 11),
                AutoSize = true
            };

            var lblPrice = new Label
            {
                Text = $"{item.Price * item.Quantity:F0}",
                Font = new Font("Tahoma", 9),
                ForeColor = Color.FromArgb(80, 80, 80),
                Location = new Point(210, 11),
                AutoSize = true
            };

            miniCard.Controls.Add(lbl);
            miniCard.Controls.Add(lblPrice);
            miniCard.Tag = item;

            orderItemsPanel.Controls.Add(miniCard);
        }
    }

    private void SetActiveCategoryButton(Button activeBtn)
    {
        var buttons = new[] { btnHotDrinks, btnJuices, btnSmoothies, btnMilkshakes };
        foreach (var btn in buttons)
        {
            if (btn == activeBtn)
            {
                btn.BackColor = Color.FromArgb(52, 152, 219);
                btn.ForeColor = Color.White;
                btn.Font = new Font("Tahoma", 11, FontStyle.Bold);
                btn.FlatAppearance.BorderSize = 0;
            }
            else
            {
                btn.BackColor = Color.White;
                btn.ForeColor = Color.FromArgb(52, 152, 219);
                btn.Font = new Font("Tahoma", 11, FontStyle.Regular);
                btn.FlatAppearance.BorderSize = 1;
            }
        }
    }

    private Button? FindButtonByText(string text)
    {
        return new[] { btnHotDrinks, btnJuices, btnSmoothies, btnMilkshakes }
               .FirstOrDefault(b => b.Text == text);
    }

    private void CollectOrderData()
    {
        currentOrderItems.Clear();
        currentSubtotal = 0;
        currentTax = 0;
        currentTotal = 0;

        foreach (Control ctrl in orderItemsPanel.Controls)
        {
            if (ctrl is Panel itemCard && itemCard.Tag is OrderItem item)
            {
                currentOrderItems.Add(item);
                currentSubtotal += item.Price * item.Quantity;
            }
        }

        currentTotal = currentSubtotal;

        lblSubtotal.Text = $"المجموع الفرعي: {currentSubtotal:F0} ج.م";
        lblTax.Text = $"{currentTax:F0} ج.م";
        lblTotalFooter.Text = $"الإجمالي: {currentTotal:F0} ج.م";
        lblTotalValue.Text = $"{currentTotal:F0} ج.م";
    }

    private async void BtnPayNow_Click(object sender, EventArgs e)
    {
        CollectOrderData();

        if (currentOrderItems.Count == 0)
        {
            MessageBox.Show("لا يوجد عنصر في الطلب", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var items = currentOrderItems
                .Select(o => new OrderItemRequest
                {
                    MenuItemId = o.MenuItemId,
                    Quantity = o.Quantity,
                    UnitPrice = o.Price
                })
                .ToList();

            var order = await _orderService.CreateOrderAsync(null, null, items);
            await _orderService.CloseOrderAsync(order.Id, isPaid: true);

            MessageBox.Show($"تم حفظ الطلب رقم {order.OrderNumber}", "تم الدفع", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearOrder();
            await UpdateTodayTotalAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"خطأ أثناء حفظ الطلب: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnNewOrder_Click(object sender, EventArgs e)
    {
        ClearOrder();
    }

    private async void BtnViewOrders_Click(object sender, EventArgs e)
    {
        using var ordersForm = new PaidOrdersForm(_orderService);
        ordersForm.ShowDialog(this);
        await UpdateTodayTotalAsync();
    }

    private void ClearOrder()
    {
        currentOrderItems.Clear();
        orderItemsPanel.Controls.Clear();
        currentSubtotal = 0;
        currentTax = 0;
        currentTotal = 0;

        lblSubtotal.Text = "المجموع الفرعي: 0 ج.م";
        lblTax.Text = $"{currentTax:F0} ج.م";
        lblTotalFooter.Text = "الإجمالي: 0 ج.م";
        lblTotalValue.Text = "0 ج.م";
    }

    private void BtnPrint_Click(object sender, EventArgs e)
    {
        CollectOrderData();

        if (currentOrderItems.Count == 0)
        {
            MessageBox.Show("لا يوجد طلب للطباعة", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var result = printPreviewDialog.ShowDialog(this);
        if (result == DialogResult.OK)
        {
            try
            {
                printDocument.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في الطباعة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
    {
        Graphics g = e.Graphics;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        int receiptWidth = 280;
        int y = 10;

        // Store name header
        using (Font headerFont = new Font("Tahoma", 14, FontStyle.Bold))
        using (Brush textBrush = new SolidBrush(Color.FromArgb(50, 50, 50)))
        {
            string header = "روكن هادي - الكاشير";
            SizeF headerSize = g.MeasureString(header, headerFont);
            float x = (receiptWidth - headerSize.Width) / 2;
            g.DrawString(header, headerFont, textBrush, x, y);
            y += (int)headerSize.Height + 8;
        }

        // Divider line
        using (Pen linePen = new Pen(Color.FromArgb(200, 200, 200), 1))
        {
            g.DrawLine(linePen, 10, y, receiptWidth - 10, y);
            y += 8;
        }

        // Date and time
        using (Font dateFont = new Font("Tahoma", 9))
        using (Brush textBrush = new SolidBrush(Color.FromArgb(100, 100, 100)))
        {
            string dateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            SizeF dtSize = g.MeasureString(dateTime, dateFont);
            float x = (receiptWidth - dtSize.Width) / 2;
            g.DrawString(dateTime, dateFont, textBrush, x, y);
            y += 18;
        }

        // Divider
        using (Pen linePen = new Pen(Color.FromArgb(200, 200, 200), 1))
        {
            g.DrawLine(linePen, 10, y, receiptWidth - 10, y);
            y += 10;
        }

        // Items header
        using (Font boldFont = new Font("Tahoma", 10, FontStyle.Bold))
        using (Brush textBrush = new SolidBrush(Color.FromArgb(50, 50, 50)))
        {
            g.DrawString("الصنف", boldFont, textBrush, 10, y);
            g.DrawString("الكمية", boldFont, textBrush, 130, y);
            g.DrawString("السعر", boldFont, textBrush, 200, y);
            y += 20;
        }

        // Divider
        using (Pen linePen = new Pen(Color.FromArgb(200, 200, 200), 1))
        {
            g.DrawLine(linePen, 10, y, receiptWidth - 10, y);
            y += 6;
        }

        // Order items
        using (Font itemFont = new Font("Tahoma", 10))
        using (Brush textBrush = new SolidBrush(Color.FromArgb(50, 50, 50)))
        {
            foreach (var item in currentOrderItems)
            {
                string itemText = item.Name;

                g.DrawString(itemText, itemFont, textBrush, 10, y);

                string qtyText = item.Quantity.ToString();
                SizeF qtySize = g.MeasureString(qtyText, itemFont);
                g.DrawString(qtyText, itemFont, textBrush, 130 + (50 - qtySize.Width) / 2, y);

                string priceText = (item.Price * item.Quantity).ToString("F0");
                SizeF priceSize = g.MeasureString(priceText, itemFont);
                g.DrawString(priceText, itemFont, textBrush, 200 + (60 - priceSize.Width) / 2, y);

                y += 18;
            }
        }

        y += 6;

        // Divider
        using (Pen linePen = new Pen(Color.FromArgb(200, 200, 200), 1))
        {
            g.DrawLine(linePen, 10, y, receiptWidth - 10, y);
            y += 10;
        }

        // Totals
        using (Font totalFont = new Font("Tahoma", 10))
        using (Brush textBrush = new SolidBrush(Color.FromArgb(80, 80, 80)))
        {
            g.DrawString($"المجموع الفرعي: {currentSubtotal:F0} ج.م", totalFont, textBrush, 10, y);
            y += 18;
            g.DrawString($"{currentTax:F0} ج.م", totalFont, textBrush, 10, y);
            y += 18;
        }

        using (Font boldTotalFont = new Font("Tahoma", 11, FontStyle.Bold))
        using (Brush totalBrush = new SolidBrush(Color.FromArgb(107, 142, 35)))
        {
            g.DrawString($"الإجمالي: {currentTotal:F0} ج.م", boldTotalFont, totalBrush, 10, y);
            y += 26;
        }

        // Divider
        using (Pen linePen = new Pen(Color.FromArgb(200, 200, 200), 1))
        {
            g.DrawLine(linePen, 10, y, receiptWidth - 10, y);
            y += 10;
        }

        // Footer message
        using (Font footerFont = new Font("Tahoma", 9))
        using (Brush textBrush = new SolidBrush(Color.FromArgb(100, 100, 100)))
        {
            string thanks = "شكراً لزيارتكم!";
            SizeF thanksSize = g.MeasureString(thanks, footerFont);
            float x = (receiptWidth - thanksSize.Width) / 2;
            g.DrawString(thanks, footerFont, textBrush, x, y);
            y += 20;
        }

        e.HasMorePages = false;
    }
}
