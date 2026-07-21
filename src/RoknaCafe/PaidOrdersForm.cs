using System;
using System.Drawing;
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
    private DateTime? _selectedDate;

    public PaidOrdersForm(IOrderService orderService)
    {
        _orderService = orderService;
        BuildUi();
        LoadOrdersForDate(DateTime.Now.Date);
    }

    private void BuildUi()
    {
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        Text = "طلبات اليوم";
        Font = new Font("Tahoma", 10);
        Size = new Size(850, 650);
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
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(0),
            Font = new Font("Tahoma", 8),
            ForeColor = Color.FromArgb(120, 120, 120),
            AutoSize = false
        };

        _lblTotal = new Label
        {
            Height = 28,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(0),
            Font = new Font("Tahoma", 11, FontStyle.Bold),
            ForeColor = Color.FromArgb(107, 142, 35),
            AutoSize = false
        };

        var topLeft = new Panel { Dock = DockStyle.Left, Width = 260 };
        var topRight = new Panel { Dock = DockStyle.Fill };

        var btnFlow = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.RightToLeft,
            Dock = DockStyle.Top,
            Height = 28,
            Padding = new Padding(0),
            Margin = new Padding(0)
        };
        btnFlow.Controls.Add(_btnToday);
        btnFlow.Controls.Add(_btnAll);
        topLeft.Controls.Add(btnFlow);
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
                var itemTotal = order.GrandTotal;
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
                if (o.GrandTotal != 0) return o.GrandTotal;
                if (o.OrderItems == null || !o.OrderItems.Any()) return 0;
                var subtotal = o.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity);
                return subtotal + subtotal * 0.15m;
            });

            _lblTotal.Text = date.HasValue
                ? $"إجمالي اليوم ({date.Value:yyyy/MM/dd}): {total:F0} ر.س"
                : $"إجمالي كل الطلبات: {total:F0} ر.س";

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
}
