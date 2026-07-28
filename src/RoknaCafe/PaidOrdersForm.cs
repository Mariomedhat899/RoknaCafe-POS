using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using Rokna.Domain.Entities;
using Rokna.Domain.Interfaces;

namespace RoknaCafe;

public class PaidOrdersForm : Form
{
    private readonly IOrderService _orderService;
    private MonthCalendar _calendar;
    private ListView _lvOrders;
    private Label _lblTotal;
    private Label _lblStatus;
    private ListView _lvItems;
    private Button _btnToday;
    private Button _btnAll;
    private Button _btnPrint;
    private DateTime? _selectedDate;

    public PaidOrdersForm(IOrderService orderService)
    {
        _orderService = orderService;
        BuildUi();
        LoadOrdersForDate(DateTime.Now.Date);
    }
    private void BuildUi()
    {
        Size = new Size(1100, 720);
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        Text = "طلبات اليوم";
        Font = new Font("Tahoma", 10);
        StartPosition = FormStartPosition.CenterParent;

        var table = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            ColumnCount = 1,
            Padding = new Padding(10),
            AutoSize = false
        };
        table.RowStyles.Add(new RowStyle(SizeType.Percent, 38));
        table.RowStyles.Add(new RowStyle(SizeType.Percent, 35));
        table.RowStyles.Add(new RowStyle(SizeType.Percent, 27));

        var topPanel = new Panel { Dock = DockStyle.Fill };
        var listPanel = new Panel { Dock = DockStyle.Fill };
        var itemsPanel = new Panel { Dock = DockStyle.Fill };

        table.Controls.Add(topPanel, 0, 0);
        table.Controls.Add(listPanel, 0, 1);
        table.Controls.Add(itemsPanel, 0, 2);
        Controls.Add(table);

        _btnToday = new Button
        {
            Text = "اليوم",
            Size = new Size(90, 26),
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Tahoma", 9, FontStyle.Bold)
        };
        _btnToday.FlatAppearance.BorderSize = 0;
        _btnToday.Click += (s, e) =>
        {
            _selectedDate = DateTime.Now.Date;
            _calendar.SetDate(DateTime.Now.Date);
            LoadOrdersForDate(_selectedDate.Value);
        };

        _btnAll = new Button
        {
            Text = "كل الطلبات",
            Size = new Size(90, 26),
            BackColor = Color.FromArgb(248, 249, 250),
            ForeColor = Color.FromArgb(80, 80, 80),
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Tahoma", 9, FontStyle.Bold)
        };
        _btnAll.FlatAppearance.BorderSize = 1;
        _btnAll.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
        _btnAll.Click += (s, e) =>
        {
            _selectedDate = null;
            LoadOrdersForDate(null);
        };

        _btnPrint = new Button
        {
            Text = "طباعة الطلب",
            Size = new Size(110, 26),
            BackColor = Color.FromArgb(107, 142, 35),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Tahoma", 9, FontStyle.Bold)
        };
        _btnPrint.FlatAppearance.BorderSize = 0;
        _btnPrint.Click += (s, e) => PrintSelectedOrder();

        _calendar = new MonthCalendar
        {
            MaxSelectionCount = 1,
            TodayDate = DateTime.Now,
            ShowToday = true,
            ShowTodayCircle = true
        };
        _calendar.DateSelected += (s, e) =>
        {
            _selectedDate = _calendar.SelectionStart.Date;
            LoadOrdersForDate(_selectedDate.Value);
        };

        _lblStatus = new Label
        {
            Height = 22,
            Width = 600,
            Location = new Point(0, 0),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(0),
            Font = new Font("Tahoma", 8),
            ForeColor = Color.FromArgb(120, 120, 120),
            AutoSize = false
        };

        _lblTotal = new Label
        {
            Height = 28,
            Width = 600,
            Location = new Point(0, 0),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(0),
            Font = new Font("Tahoma", 11, FontStyle.Bold),
            ForeColor = Color.FromArgb(107, 142, 35),
            AutoSize = false
        };

        var topLeft = new Panel { Dock = DockStyle.Left, Width = 320 };
        var topRight = new Panel { Dock = DockStyle.Fill };

        var btnPanel = new Panel { Dock = DockStyle.Top, Height = 32 };
        _btnPrint.Location = new Point(0, 3);
        _btnToday.Location = new Point(120, 3);
        _btnAll.Location = new Point(240, 3);
        btnPanel.Controls.Add(_btnPrint);
        btnPanel.Controls.Add(_btnToday);
        btnPanel.Controls.Add(_btnAll);

        topLeft.Controls.Add(btnPanel);
        topLeft.Controls.Add(_calendar);

        var lblTotalWrapper = new Panel { Dock = DockStyle.Bottom, Height = 28 };
        lblTotalWrapper.Controls.Add(_lblTotal);
        topRight.Controls.Add(lblTotalWrapper);
        topRight.Controls.Add(_lblStatus);

        topPanel.Controls.Add(topRight);
        topPanel.Controls.Add(topLeft);

        _lvOrders = new ListView
        {
            Dock = DockStyle.Fill,
            View = View.Details,
            FullRowSelect = true,
            GridLines = true,
            Font = new Font("Tahoma", 10)
        };
        _lvOrders.Columns.Add("رقم الطلب", 110);
        _lvOrders.Columns.Add("التاريخ", 90);
        _lvOrders.Columns.Add("الوقت", 70);
        _lvOrders.Columns.Add("القسم", 150);
        _lvOrders.Columns.Add("الإجمالي", 90);
        _lvOrders.SelectedIndexChanged += (s, e) =>
        {
            if (_lvOrders.SelectedItems.Count == 0) return;
            if (_lvOrders.SelectedItems[0].Tag is Order order)
                LoadOrderItems(order);
        };

        _lvItems = new ListView
        {
            Dock = DockStyle.Fill,
            View = View.Details,
            FullRowSelect = true,
            GridLines = true,
            Font = new Font("Tahoma", 10)
        };
        _lvItems.Columns.Add("الصنف", 220);
        _lvItems.Columns.Add("الكمية", 70);
        _lvItems.Columns.Add("سعر الوحدة", 90);
        _lvItems.Columns.Add("الإجمالي", 90);

        var lblOrders = new Label
        {
            Text = "الطلبات المدفوعة:",
            Dock = DockStyle.Top,
            Height = 26,
            Font = new Font("Tahoma", 10, FontStyle.Bold),
            ForeColor = Color.FromArgb(80, 80, 80),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(0)
        };

        var lblItems = new Label
        {
            Text = "عناصر الطلب المحدد:",
            Dock = DockStyle.Top,
            Height = 26,
            Font = new Font("Tahoma", 10, FontStyle.Bold),
            ForeColor = Color.FromArgb(80, 80, 80),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(0)
        };

        var lblEmpty = new Label
        {
            Text = "لا توجد طلبات مدفوعة في هذا اليوم",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.FromArgb(150, 150, 150),
            Font = new Font("Tahoma", 11, FontStyle.Italic),
            Visible = false
        };
        lblEmpty.Name = "lblEmpty";

        listPanel.Controls.Add(_lvOrders);
        listPanel.Controls.Add(lblOrders);

        itemsPanel.Controls.Add(_lvItems);
        itemsPanel.Controls.Add(lblEmpty);
        itemsPanel.Controls.Add(lblItems);
    }

    private async void LoadOrdersForDate(DateTime? date)
    {
        var emptyLabel = Controls.Find("lblEmpty", true).FirstOrDefault() as Label;
        _lvOrders.Items.Clear();
        _lvItems.Items.Clear();

        try
        {
            IEnumerable<Order> orders;
            string rangeLabel;

            if (date.HasValue)
            {
                var start = date.Value.Date;
                var end = start.AddDays(1).AddTicks(-1);
                orders = await _orderService.GetByDateRangeAsync(start, end);
                rangeLabel = $"الفترة: {start:yyyy/MM/dd} 00:00 → {end:yyyy/MM/dd} 23:59";
                _lblStatus.Text = $"تاريخ اليوم: {date.Value:yyyy/MM/dd}";
            }
            else
            {
                orders = await _orderService.GetAllAsync();
                rangeLabel = "كل الطلبات";
                _lblStatus.Text = "كل الطلبات";
            }

            var allList = orders.ToList();
            var paidOrders = allList
                .Where(o => o.Status == OrderStatus.Completed)
                .OrderByDescending(o => o.DateTime)
                .ToList();

            _lblStatus.Text = $"{_lblStatus.Text} | {rangeLabel} | إجمالي السجلات: {allList.Count} | مكتملة: {paidOrders.Count}";
            _lvOrders.Items.Clear();
            foreach (var order in paidOrders)
            {
                var itemTotal = order.TotalAmount;
                if (itemTotal == 0 && order.OrderItems != null && order.OrderItems.Any())
                {
                    var subtotal = order.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity);
                    var tax = subtotal * 0.15m;
                    itemTotal = subtotal + tax;
                }

                var row = new ListViewItem(order.OrderNumber ?? order.Id.ToString());
                row.SubItems.Add(order.DateTime.ToString("yyyy/MM/dd"));
                row.SubItems.Add(order.DateTime.ToString("HH:mm"));
                row.SubItems.Add(order.CafeName ?? string.Empty);
                row.SubItems.Add(itemTotal.ToString("F0"));
                row.Tag = order;
                _lvOrders.Items.Add(row);
            }

            var total = paidOrders.Sum(o =>
            {
                if (o.TotalAmount != 0) return o.TotalAmount;
                if (o.OrderItems == null || !o.OrderItems.Any()) return 0;
                var subtotal = o.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity);
                return subtotal + subtotal * 0.15m;
            });

            _lblTotal.Text = date.HasValue
                ? $"إجمالي اليوم ({date.Value:yyyy/MM/dd}): {total:F0} ج.م"
                : $"إجمالي كل الطلبات: {total:F0} ج.م";

            if (emptyLabel != null)
                emptyLabel.Visible = paidOrders.Count == 0;

            if (paidOrders.Count == 0)
            {
                _lvItems.Items.Clear();
            }
            else if (_lvOrders.Items.Count > 0)
            {
                _lvOrders.Items[0].Selected = true;
                _lvOrders.Items[0].Focused = true;
                _lvOrders.EnsureVisible(0);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"خطأ أثناء تحميل الطلبات:\n{ex.Message}\n\nتأكد من أن قاعدة البيانات تحتوي على طلبات.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LoadOrderItems(Order order)
    {
        _lvItems.Items.Clear();
        foreach (var oi in order.OrderItems)
        {
            var item = new ListViewItem(oi.MenuItem?.Name ?? $"#{oi.MenuItemId}");
            item.SubItems.Add(oi.Quantity.ToString());
            item.SubItems.Add(oi.UnitPrice.ToString("F0"));
            item.SubItems.Add((oi.UnitPrice * oi.Quantity).ToString("F0"));
            _lvItems.Items.Add(item);
        }
    }

    private void PrintSelectedOrder()
    {
        if (_lvOrders.SelectedItems.Count == 0)
        {
            MessageBox.Show("اختر طلب من القائمة أولاً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_lvOrders.SelectedItems[0].Tag is not Order order)
        {
            MessageBox.Show("تعذر قراءة بيانات الطلب المحدد", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            using var bmp = new Bitmap(340, 520);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                g.Clear(Color.White);

                int receiptWidth = 320;
                int pad = 18;
                int y = 14;

                using (Pen headerLine = new Pen(Color.FromArgb(32, 42, 28), 2))
                {
                    g.DrawLine(headerLine, pad, y, receiptWidth - pad, y);
                    y += 14;
                }

                using (Font headerFont = new Font("Tahoma", 15, FontStyle.Bold))
                using (Brush headerBrush = new SolidBrush(Color.FromArgb(32, 42, 28)))
                {
                    string header = "روكن هادي - الكاشير";
                    SizeF headerSize = g.MeasureString(header, headerFont);
                    float x = pad + (receiptWidth - 2 * pad - headerSize.Width) / 2;
                    g.DrawString(header, headerFont, headerBrush, x, y);
                    y += (int)headerSize.Height + 10;
                }

                using (Pen linePen = new Pen(Color.FromArgb(220, 222, 224), 1))
                {
                    g.DrawLine(linePen, pad, y, receiptWidth - pad, y);
                    y += 10;
                }

                using (Font dateFont = new Font("Tahoma", 9))
                using (Brush mutedBrush = new SolidBrush(Color.FromArgb(90, 100, 110)))
                {
                    string dateTime = order.DateTime.ToString("yyyy/MM/dd  HH:mm");
                    SizeF dtSize = g.MeasureString(dateTime, dateFont);
                    float x = pad + (receiptWidth - 2 * pad - dtSize.Width) / 2;
                    g.DrawString(dateTime, dateFont, mutedBrush, x, y);
                    y += 18;
                }

                using (Font orderFont = new Font("Tahoma", 9))
                using (Brush mutedBrush = new SolidBrush(Color.FromArgb(90, 100, 110)))
                {
                    string orderText = $"طلب #{order.OrderNumber ?? order.Id.ToString()}";
                    SizeF orderSize = g.MeasureString(orderText, orderFont);
                    float x = pad + (receiptWidth - 2 * pad - orderSize.Width) / 2;
                    g.DrawString(orderText, orderFont, mutedBrush, x, y);
                    y += 18;
                }

                using (Pen linePen2 = new Pen(Color.FromArgb(220, 222, 224), 1))
                {
                    g.DrawLine(linePen2, pad, y, receiptWidth - pad, y);
                    y += 10;
                }

                using (Font colHeaderFont = new Font("Tahoma", 9, FontStyle.Bold))
                using (Brush textBrush = new SolidBrush(Color.FromArgb(45, 55, 45)))
                {
                    g.DrawString("الصنف", colHeaderFont, textBrush, pad, y);
                    g.DrawString("الكمية", colHeaderFont, textBrush, pad + 110, y);
                    g.DrawString("السعر", colHeaderFont, textBrush, pad + 190, y);
                    y += 20;
                }

                using (Pen accentLine = new Pen(Color.FromArgb(230, 232, 235), 1))
                {
                    g.DrawLine(accentLine, pad, y, receiptWidth - pad, y);
                    y += 8;
                }

                using (Font itemFont = new Font("Tahoma", 10))
                using (Brush textBrush = new SolidBrush(Color.FromArgb(45, 55, 45)))
                {
                    foreach (var oi in order.OrderItems)
                    {
                        string itemText = oi.MenuItem?.Name ?? $"#{oi.MenuItemId}";
                        g.DrawString(itemText, itemFont, textBrush, pad, y);

                        string qtyText = oi.Quantity.ToString();
                        SizeF qtySize = g.MeasureString(qtyText, itemFont);
                        g.DrawString(qtyText, itemFont, textBrush, pad + 110 + (50 - qtySize.Width) / 2, y);

                        string priceText = (oi.UnitPrice * oi.Quantity).ToString("F0");
                        SizeF priceSize = g.MeasureString(priceText, itemFont);
                        g.DrawString(priceText, itemFont, textBrush, pad + 190 + (70 - priceSize.Width) / 2, y);

                        y += 20;
                    }
                }

                y += 8;

                using (Pen linePen3 = new Pen(Color.FromArgb(220, 222, 224), 1))
                {
                    g.DrawLine(linePen3, pad, y, receiptWidth - pad, y);
                    y += 12;
                }

                using (Font totalFont = new Font("Tahoma", 12, FontStyle.Bold))
                using (Brush totalBrush = new SolidBrush(Color.FromArgb(64, 102, 36)))
                {
                    string totalText = $"الإجمالي: {order.TotalAmount:F0} ج.م";
                    SizeF totalSize = g.MeasureString(totalText, totalFont);
                    float x = pad + (receiptWidth - 2 * pad - totalSize.Width) / 2;
                    g.DrawString(totalText, totalFont, totalBrush, x, y);
                    y += 30;
                }

                using (Pen footerLine = new Pen(Color.FromArgb(220, 222, 224), 1))
                {
                    g.DrawLine(footerLine, pad, y, receiptWidth - pad, y);
                    y += 10;
                }

                using (Font footerFont = new Font("Tahoma", 9, FontStyle.Italic))
                using (Brush textBrush = new SolidBrush(Color.FromArgb(110, 115, 120)))
                {
                    string thanks = "شكراً لزيارتكم!";
                    SizeF thanksSize = g.MeasureString(thanks, footerFont);
                    float x = pad + (receiptWidth - 2 * pad - thanksSize.Width) / 2;
                    g.DrawString(thanks, footerFont, textBrush, x, y);
                    y += 22;
                }
            }

            using var preview = new Form
            {
                Text = $"معاينة الطلب - #{order.OrderNumber ?? order.Id.ToString()}",
                Size = new Size(400, 620),
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.White,
                MinimizeBox = false,
                MaximizeBox = false,
                ShowInTaskbar = false
            };

            var picture = new PictureBox
            {
                Image = bmp,
                SizeMode = PictureBoxSizeMode.CenterImage,
                Dock = DockStyle.Fill
            };

            var btnPanel = new Panel { Dock = DockStyle.Bottom, Height = 72 };
            var closeBtn = new Button
            {
                Text = "اغلاق",
                Dock = DockStyle.Bottom,
                Height = 32,
                FlatStyle = FlatStyle.Flat
            };
            closeBtn.FlatAppearance.BorderSize = 0;
            closeBtn.BackColor = Color.FromArgb(52, 110, 94);
            closeBtn.ForeColor = Color.White;
            closeBtn.Font = new Font("Tahoma", 10, FontStyle.Bold);
            closeBtn.Click += (s, e) => preview.Close();

            var printBtn = new Button
            {
                Text = "اطبع",
                Dock = DockStyle.Top,
                Height = 32,
                FlatStyle = FlatStyle.Flat
            };
            printBtn.FlatAppearance.BorderSize = 0;
            printBtn.BackColor = Color.FromArgb(52, 152, 219);
            printBtn.ForeColor = Color.White;
            printBtn.Font = new Font("Tahoma", 10, FontStyle.Bold);
            printBtn.Click += (s, e) =>
            {
                try
                {
                    using var tmp = new Bitmap(340, 520);
                    using (var g = Graphics.FromImage(tmp))
                    {
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                        g.Clear(Color.White);
                        g.DrawImage(bmp, 0, 0);
                    }

                    using var pd = new PrintDocument();
                    pd.DefaultPageSettings = new PageSettings(pd.PrinterSettings)
                    {
                        Margins = new Margins(60, 60, 60, 60)
                    };
                    pd.PrintPage += (ps, pe) =>
                    {
                        pe.Graphics.DrawImage(tmp, pe.PageBounds);
                        pe.HasMorePages = false;
                    };

                    pd.Print();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"خطأ في الطباعة:\n{ex.GetType().FullName}\n{ex.Message}\n{ex.StackTrace}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnPanel.Controls.Add(closeBtn);
            btnPanel.Controls.Add(printBtn);
            preview.Controls.Add(btnPanel);
            preview.Controls.Add(picture);
            preview.ShowDialog(this);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"خطأ في معاينة الطلب:\n{ex.GetType().FullName}\n{ex.Message}\n{ex.StackTrace}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
