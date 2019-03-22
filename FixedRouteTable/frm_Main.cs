using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FixedRouteTable
{
    public partial class frm_Main : Form
    {
        #region Biến cục bộ
        /// <summary>
        /// Lưu trử mô hình liên kết
        /// </summary>
        private Topology topology;

        public Topology Topology
        {
            get => topology;
            set
            {
                if (value != null)
                {
                    if (!value.Equals(topology))
                    {
                        topology = value;
                        NeedToReDraw = true;
                        IsTopoChange = true;
                    }
                    else
                    {
                        NeedToReDraw = false;
                        IsTopoChange = false;
                    }
                }
                TopoSet?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Random Color
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// Ma trận routing toàn node trong mô hình
        /// </summary>
        private int[,] routeMetrix;

        /// <summary>
        /// Chứa thông tin đường đi cho chức năng tìm kiếm
        /// </summary>
        private RoutePath path;

        /// <summary>
        /// Xác định có nên vẽ lại mô hình hay không
        /// </summary>
        private bool NeedToReDraw = false;

        private bool IsTopoChange = false;
        #endregion

        #region Delegate

        /// <summary>
        /// Event handler
        /// </summary>
        private EventHandler TopoSet;

        #endregion

        public frm_Main()
        {
            InitializeComponent();

            TopoSet += new EventHandler((o, a) =>
            {
                if (InvokeRequired)
                {
                    EndInvoke(BeginInvoke(new MethodInvoker(
                        () =>
                        {
                            if (Topology == null)
                            {
                                gBFunction.Enabled = false;
                            }
                            else
                            {
                                gBFunction.Enabled = true;
                            }
                        })));
                }
                else
                {
                    if (Topology == null)
                    {
                        gBFunction.Enabled = false;
                    }
                    else
                    {
                        gBFunction.Enabled = true;
                    }
                }
            });
            Topology = null;
        }

        #region Function
        private void WriteTopoToFile(string path)
        {
            StreamWriter writer = new StreamWriter(path);
            try
            {
                writer.WriteLine(Topology.TopologySize);
                foreach (KeyValuePair<int, Router> lnode in Topology.AllNode)
                {
                    foreach (KeyValuePair<Router, int> rnode in lnode.Value.DirectedRoutersWithCost)
                    {
                        writer.WriteLine($"{lnode.Key} {rnode.Key.HostID} {rnode.Value}");
                    }
                }
                writer.WriteLine("0");
            }
            catch
            {
                MessageBox.Show("Không thể ghi file !");
                return;
            }
            finally
            {
                writer.Dispose();
            }
        }

        private void ReadInputFromFile(string path)
        {
            if (InvokeRequired)
            {
                EndInvoke(BeginInvoke(new MethodInvoker(
                    () =>
                    {
                        ClearUI();
                    })));
            }
            else
            {
                ClearUI();
            }
            StreamReader file = new StreamReader(path);
#if !DEBUG
            try
            {
#endif
            int topo_size = int.Parse(file.ReadLine());

            Topology = Topology.CreateTopology(topo_size);
            string line;
            while (!(line = file.ReadLine()).Equals("0"))
            {
                int[] args = line.Split(' ').Select(o => int.Parse(o)).ToArray();
                if (args[2] <= 0)
                    continue;
                Topology.AddRelative(from: args[0], to: args[1], cost: args[2]);
            }
            if (InvokeRequired)
            {
                EndInvoke(BeginInvoke(new MethodInvoker(
                    () =>
                    {
                        InitUI();
                    })));
            }
            else
            {
                InitUI();
            }
#if !DEBUG
            }
            catch
            {
                if (InvokeRequired)
                {
                    EndInvoke(BeginInvoke(new MethodInvoker(
                        () =>
                        {
                            MessageBox.Show("File not support !");
                        })));
                }
                else
                {
                    MessageBox.Show("File not support !");
                }
            }
            finally
            {
#endif
            file.Dispose();
#if !DEBUG
            }
#endif

        }

        private void ClearUI()
        {
            listBox1.Items.Clear();
            lstResult.Items.Clear();
            cbbFrom.Items.Clear();
            cbbTo.Items.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            radioButton1.Checked = radioButton2.Checked = false;
            checkBox1.Checked = true;
        }

        private void InitUI()
        {
            listBox1.Items.AddRange(Topology.AllNode.Values.ToArray());
            cbbFrom.Items.AddRange(Topology.AllNode.Select((o) => (object)o.Key).ToArray());
            cbbTo.Items.AddRange(Topology.AllNode.Select((o) => (object)o.Key).ToArray());
            cbbFrom.SelectedIndex = 0;
            cbbTo.SelectedIndex = 0;
            radioButton1.Checked = true;
        }

        private void DrawGrid()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            for (int x = 0; x <= routeMetrix.GetLength(0); x++)
            {
                dataGridView1
                        .Columns
                        .Add(x == 0 ? "" : $"node{x}", x == 0 ? "+" : $"{x}");
            }
            dataGridView1.AutoResizeColumns();
            DataGridViewCellStyle cStyle = new DataGridViewCellStyle(); ;
            cStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.AllowUserToOrderColumns = false;
            for (int x = 0; x < routeMetrix.GetLength(0); x++)
            {
                List<object> intl = new List<object>
                {
                    x + 1
                };
                for (int y = 0; y < routeMetrix.GetLength(1); y++)
                {
                    intl.Add(routeMetrix.GetValue(y, x));
                }
                dataGridView1.Rows.Add(intl.ToArray());
            }
            dataGridView1.DefaultCellStyle = cStyle;
            dataGridView1.ColumnHeadersDefaultCellStyle = cStyle;
            dataGridView1.RowHeadersDefaultCellStyle = cStyle;
            dataGridView1.ClearSelection();
        }

        private void GenerateMatrixTableAndDrawGird()
        {
            IsTopoChange = false;
            UseWaitCursor = true;
            if (checkBox1.Checked)
            {
                if (radioButton1.Checked)
                    routeMetrix = Topology.MetrixCaculateParallel(Topology.Mode.MinimumHop);
                else
                    routeMetrix = Topology.MetrixCaculateParallel(Topology.Mode.LeastCost);
            }
            else
            {
                if (radioButton1.Checked)
                    routeMetrix = Topology.MetrixCaculate(Topology.Mode.MinimumHop);
                else
                    routeMetrix = Topology.MetrixCaculate(Topology.Mode.LeastCost);
            }
            DrawGrid();
            btnFind_Click(null, null);
            UseWaitCursor = false;
        }

        /// <summary>
        /// Vẽ mô hình
        /// </summary>
        private void DrawTopology()
        {
            panelBackground.Controls.Clear();
            panelBackground.Refresh();
            int centerX = panelBackground.Location.X + panelBackground.Size.Width / 2;
            int centerY = panelBackground.Location.Y + panelBackground.Size.Height / 2;

            KeyValuePair<int, Router> centerRouter = Topology
                .AllNode
                .Where(
                o => o.Value.DirectedRoutersWithCost.Count
                ==
                Topology.AllNode.Max(mo => mo.Value.DirectedRoutersWithCost.Count))
                .First();
            int firstLeftX = panelBackground.Size.Width / 3;

            Router router = centerRouter.Value;
            if (!CheckRouterIsDraw(router))
            {
                Color backColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                RouterControlUI nodeUI = new RouterControlUI(router, backColor);

                nodeUI.Location = new Point(panelBackground
                    .Location.X + panelBackground.Size.Width / 3 + firstLeftX - nodeUI.Size.Width / 2
                    , centerY - panelBackground.Size.Height / 2 + nodeUI.Size.Height / 2);

                panelBackground.Controls.Add(nodeUI);
                DrawDrectedRouter(nodeUI.Location, centerRouter.Value);
            }

            DetectOverlapAndTransforms();
        }

        /// <summary>
        /// Phát hiện phần mô hình bị mất do lấn biên
        /// và chỉnh sửa lại
        /// </summary>
        void DetectOverlapAndTransforms()
        {
            IEnumerable<Control> controls = panelBackground
                .Controls
                .Cast<Control>();
            int overlapTop =
                controls.Min(mo => mo.Location.Y);
            int overlapLeft =
                controls.Min(mo => mo.Location.X);
            foreach (Control control in panelBackground.Controls)
            {
                control.Location = new Point(
                    control.Location.X + Math.Abs(overlapLeft < 0 ? overlapLeft : 0)
                    ,
                    control.Location.Y + Math.Abs(overlapTop < 0 ? overlapTop : 0)
                    );
            }
        }

        bool CheckRouterIsDraw(Router router)
        {
            int sum = panelBackground.Controls
                .Cast<RouterControlUI>()
                .Sum(o => o.Self.Equals(router) ? 1 : 0);
            return sum != 0;
        }

        Control GetRouterIsDrawed(Router router)
        {
            RouterControlUI rCUI = panelBackground.Controls
                .Cast<RouterControlUI>()
                .Where(o => o.Self.Equals(router)).FirstOrDefault();
            return rCUI;
        }

        /// <summary>
        /// Vẽ liên kết giữa các Router
        /// </summary>
        void DrawLink()
        {

            foreach (RouterControlUI item in panelBackground.Controls)
            {

                foreach (KeyValuePair<Router, int> directedRouter in item.Self.DirectedRoutersWithCost)
                {
                    RouterControlUI ctrUI = panelBackground
                        .Controls
                        .Cast<RouterControlUI>()
                        .Where(o => o.Self.Equals(directedRouter.Key))
                        .First();
                    DrawLink(item.Size, Color.Red, item.BackColor, directedRouter.Value.ToString()
                        , item.Location.X + item.Size.Width / 2, item.Location.Y + item.Size.Height / 2
                        , ctrUI.Location.X + ctrUI.Size.Width / 2, ctrUI.Location.Y + ctrUI.Size.Height / 2);
                }
            }
        }

        /// <summary>
        /// Vẽ các node liên kết trực tiếp xung quanh node hiện tại
        /// </summary>
        /// <param name="moc">Kích thước node được vẽ (chỉ dành cho việc vẽ)</param>
        /// <param name="current">Node hiện tại</param>
        void DrawDrectedRouter(Point moc, Router current)
        {
            int degreePernodeLink = 360 / current.DirectedRoutersWithCost.Count;
            int distantDefault = 200;
            Dictionary<Router, int> DiretedRouters = current.DirectedRoutersWithCost;
            for (int j = 0; j < DiretedRouters.Count; j++)
            {
                Router DirectedRouter = DiretedRouters.ElementAt(j).Key;
                if (CheckRouterIsDraw(DirectedRouter))
                    continue;
                Color backColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                RouterControlUI nodeUIAround = new RouterControlUI(DirectedRouter, backColor);
                int curentDegree = degreePernodeLink * j;
                double width = distantDefault * Math.Cos(curentDegree);
                double height = distantDefault * Math.Sin(curentDegree);
                Size Location;
                if (curentDegree <= 90)
                {
                    Location = new Size(
                        moc.X - (int)width
                        , moc.Y - (int)height
                        );
                }
                else if (curentDegree <= 180)
                {
                    Location = new Size(
                        moc.X + (int)width
                        , moc.Y - (int)height
                        );
                }
                else if (curentDegree <= 270)
                {
                    Location = new Size(
                        moc.X + (int)width
                        , moc.Y + (int)height
                        );
                }
                else
                {
                    Location = new Size(
                        moc.X - (int)width
                        , moc.Y + (int)height
                        );
                }
                nodeUIAround.Location = new Point(Location);
                panelBackground.Controls.Add(nodeUIAround);
                DrawDrectedRouter(nodeUIAround.Location, DirectedRouter);
            }
            //for (int j = 0; j < DiretedRouters.Count; j++)
            //{
            //    Router DirectedRouter = DiretedRouters.ElementAt(j).Key;
            //    if (!CheckRouterIsDraw(DirectedRouter))
            //        continue;
            //    var nodeuiaround = GetRouterIsDrawed(DirectedRouter);
            //    DrawDrectedRouter(nodeuiaround.Location, DirectedRouter);
            //}
        }

        /// <summary>
        /// Vẽ đường liên kết giữa các node
        /// </summary>
        /// <param name="ctrlSize">Kích thước Node được vẽ</param>
        /// <param name="colorPen">Màu đường liên kết</param>
        /// <param name="colorString">Màu chử</param>
        /// <param name="s">Message</param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        private void DrawLink(Size ctrlSize, Color colorPen, Color colorString, string s, int x1, int y1, int x2, int y2)
        {
            Pen pen = new Pen(colorPen, 1);

            Graphics g = panelBackground.CreateGraphics();

            g.DrawLine(pen, x1, y1, x2, y2);
            Font font = new Font(FontFamily.GenericSansSerif, 18);

            float rectheight = Math.Abs(y1 - y2);
            float rectwidth = Math.Abs(x1 - x2);

            double cosB = rectheight / Math.Sqrt(Math.Pow(rectwidth, 2) + Math.Sqrt(Math.Pow(rectheight, 2)));
            double B = Math.Tanh(cosB);
            double A = 90 - B;

            float halfctrlSize_W = ctrlSize.Width;
            float halfctrlSize_H = ctrlSize.Height;

            float px = 0;
            float py = 0;
            if (B >= 44.5 && B <= 45.5)
            {
                px = x1 >= x2 ? x1 - halfctrlSize_W : x1 + halfctrlSize_W;
                py = y1 >= y2 ? y1 - halfctrlSize_H : y1 + halfctrlSize_H;
            }
            else if (B > 44)
            {
                px = x1 >= x2 ? x1 - halfctrlSize_W * (float)Math.Tan(A) : x1 + halfctrlSize_W * (float)Math.Tan(A);
                py = y1 >= y2 ? y1 - halfctrlSize_H : y1 + halfctrlSize_H;
            }
            else
            {
                px = x1 >= x2 ? x1 - halfctrlSize_W : x1 + halfctrlSize_W;
                py = y1 >= y2 ? y1 - halfctrlSize_H * (float)Math.Tan(B) : y1 + halfctrlSize_H * (float)Math.Tan(B);
            }
            g.DrawString(s, font
                , new SolidBrush(colorString)
                , px, py);

            g.Dispose();
        }


        #endregion

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = Application.StartupPath;
            openFile.Filter = "Text File|*.txt";
            openFile.Multiselect = false;
            if (openFile.ShowDialog() != DialogResult.OK)
            {
                UseWaitCursor = false;
                return;
            }
            textBox1.Text = openFile.FileName;
            Task.Factory.StartNew(() =>
            {
                ReadInputFromFile(openFile.FileName);
                if (InvokeRequired)
                {
                    UseWaitCursor = false;
                }
                else
                {
                    UseWaitCursor = false;
                }
            }, TaskCreationOptions.LongRunning);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            ListBox lstB = sender as ListBox;
            int index = lstB.SelectedIndex;
            index = Topology.AllNode[index + 1].HostID;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[index].Selected = true;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                GenerateMatrixTableAndDrawGird();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                GenerateMatrixTableAndDrawGird();
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            lstResult.Items.Clear();
            int from = (int)cbbFrom.SelectedItem;
            int to = (int)cbbTo.SelectedItem;
            if (from == to)
            {
                lstResult.Items.Add("Nguồn và Đích phải khác nhau !");
            }
            else
            {
                path = Topology[from].GetRoutePathFromRoutingTable(Topology[to]);
                if (path.IsNotValid)
                {

                    lstResult
                        .Items
                        .Add($"Không có tuyến đường nào từ {from} =>  {to}.");
                    return;
                }
                for (int i = 1; i < path.Path.Count; i++)
                {
                    lstResult
                        .Items
                        .Add($"Nguồn: {path.Path[i - 1].HostID} " +
                        $"-> Đích: {path.Path[i].HostID} " +
                        $"| Cost: {path.Path[i - 1].DirectedRoutersWithCost[path.Path[i]]}");
                }
                string mode = radioButton1.Checked ? "Router đi qua" : "Phí";
                string discription = radioButton1.Checked ? " (Không tính nguồn)" : "";
                int result = radioButton1.Checked ? path.NumHop : path.Cost;
                lstResult
                        .Items
                        .Add($"Tổng {mode}:  {result}{discription}.");
            }
        }

        private void lstResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            ListBox list = sender as ListBox;
            int index = list.SelectedIndex;
            if (index >= 0 && index < path.NumHop)
            {
                int source = path.Path[index].HostID;
                int destination = path.Path.Last().HostID;
                dataGridView1.Rows[destination - 1].Cells[source].Selected = true;
            }
        }

        private void tabPanel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabPanel.SelectedTab.Equals(tablVisual))
            {
                if (Topology != null)
                {
                    if (NeedToReDraw)
                    {
                        DrawTopology();
                        NeedToReDraw = false;
                    }
                    DrawLink();
                }
            }
        }

        private void frm_Main_Load(object sender, EventArgs e)
        {
            panelBackground.MouseWheel += PanelBackground_MouseWheel;
        }

        private void PanelBackground_MouseWheel(object sender, MouseEventArgs e)
        {
            if (panelBackground.Controls.Count != 0)
            {
                DrawLink();
            }
        }

        private void GenerateDataTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frmTestGenerate = new frmTestGenerate();
            frmTestGenerate.Owner = this;
            frmTestGenerate.Show();
        }

        private void xuấtMôHìnhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Topology == null)
            {
                MessageBox.Show("Không có mô hình để xuất !", "Thông báo !");
                return;
            }
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.InitialDirectory = Application.StartupPath;
            saveFile.RestoreDirectory = true;
            saveFile.Filter = "Text File|*.txt";
            if (saveFile.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            WriteTopoToFile(saveFile.FileName);
            if (MessageBox.Show($"Đã xuất mô hình !\nMở ngay bây giờ ?", "Thông báo !", MessageBoxButtons.OKCancel)
                == DialogResult.OK)
            {
                try
                {
                    Process.Start(saveFile.FileName);
                }
                catch
                {

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form frmInput = new frmMutualInput();
            frmInput.Owner = this;
            frmInput.FormClosing += FrmInput_FormClosing;
            frmInput.Show();
        }

        private void FrmInput_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Topology != null && IsTopoChange)
            {
                ClearUI();
                InitUI();
            }
        }

        private void authorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAbout();
            frm.Owner = this;
            frm.Show();
        }
    }
}
