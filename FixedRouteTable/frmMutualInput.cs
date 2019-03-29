using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace FixedRouteTable
{
    public partial class frmMutualInput : Form
    {
        public frmMutualInput()
        {
            InitializeComponent();
        }

        public frmMutualInput(Topology hostTopology) : this()
        {
            topology = hostTopology;
        }

        #region Var
        private int TopoSize = 0;

        private Topology topology;
        #endregion
        private void frmMutualInput_Load(object sender, EventArgs e)
        {
            Owner.Enabled = false;
            ClearUI();
            InitUI();
        }

        private void frmMutualInput_FormClosing(object sender, FormClosingEventArgs e)
        {
            Owner.Enabled = true;
        }

        private void btnBegin_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtTopoSize.Text, out TopoSize) || TopoSize < 2)
            {
                MessageBox.Show("Số lượng Node phải là một số nguyên, và phải lớn hơn 2 !", "Nhập liệu sai !");
                return;
            }
            topology = Topology.CreateTopology(TopoSize);
            ClearUI();
            InitUI();

        }

        private void ClearUI()
        {
            cbbLeftNodes.Items.Clear();
            cbbRightNodes.Items.Clear();
            lstTopoNodes.Items.Clear();
        }
        private void InitUI()
        {
            if (topology == null)
            {
                groupBox2.Enabled = true;
                grbRelativeInput.Enabled = false;
            }
            else
            {
                groupBox2.Enabled = false;
                grbRelativeInput.Enabled = true;
                if (TopoSize != topology.TopologySize)
                    TopoSize = topology.TopologySize;
                txtTopoSize.Text = TopoSize.ToString();
                cbbLeftNodes.Items.AddRange(topology.Routers.Select((o) => (object)o.Key).ToArray());
                cbbRightNodes.Items.AddRange(topology.Routers.Select((o) => (object)o.Key).ToArray());
                cbbLeftNodes.SelectedIndex = 0;
                cbbRightNodes.SelectedIndex = 0;
                ReDrawListNodeDetails();
            }
        }

        private void btnAddRelative_Click(object sender, EventArgs e)
        {
            if (cbbLeftNodes.SelectedItem == null || cbbRightNodes.SelectedItem == null)
            {
                MessageBox.Show($"Router không tồn tại !", "Cảnh báo !");
                return;
            }
            CheckSameNode();
            if (!int.TryParse(txtCost_left.Text, out int leftCost))
            {
                MessageBox.Show("Cost phải là một số");
                return;
            }
            if (!int.TryParse(txtCost_right.Text, out int rightCost))
            {
                MessageBox.Show("Cost phải là một số");
                return;
            }

            if (leftCost < 0 && rightCost >= 0 || leftCost >= 0 && rightCost < 0)
            {
                MessageBox.Show("Kết nối phải là 2 chiều !");
                return;
            }

            if (leftCost < 0 && rightCost < 0)
            {
                if (topology.HasLinkConnect((int)cbbLeftNodes.SelectedItem, (int)cbbRightNodes.SelectedItem))
                {
                    if (MessageBox.Show($"Node {(int)cbbLeftNodes.SelectedItem} " +
                        $" đã có kết nối với Node {(int)cbbRightNodes.SelectedItem}." +
                        $"\nCó muốn gở kết nối !", "Đã có liên kết") != DialogResult.OK)
                    {
                        return;
                    }
                    topology.RemoveRelative((int)cbbLeftNodes.SelectedItem,
                        (int)cbbRightNodes.SelectedItem);
                    topology.RemoveRelative((int)cbbRightNodes.SelectedItem,
                        (int)cbbLeftNodes.SelectedItem);
                }
            }

            if (leftCost > 0 && rightCost > 0)
            {

                if (topology.HasLinkConnect((int)cbbLeftNodes.SelectedItem, (int)cbbRightNodes.SelectedItem))
                {
                    if (MessageBox.Show($"Node {(int)cbbLeftNodes.SelectedItem} " +
                        $" đã có kết nối với Node {(int)cbbRightNodes.SelectedItem}." +
                        $"\nCó muốn cập nhật Cost !", "Đã có liên kết") != DialogResult.OK)
                    {
                        return;
                    }

                    topology.UpdateRelative((int)cbbLeftNodes.SelectedItem
                        , (int)cbbRightNodes.SelectedItem
                        , leftCost);
                    topology.UpdateRelative((int)cbbRightNodes.SelectedItem
                        , (int)cbbLeftNodes.SelectedItem
                        , rightCost);

                }
                else
                {
                    topology.AddRelative((int)cbbLeftNodes.SelectedItem
                        , (int)cbbRightNodes.SelectedItem
                        , leftCost);
                    topology.AddRelative((int)cbbRightNodes.SelectedItem
                        , (int)cbbLeftNodes.SelectedItem
                        , rightCost);
                }
            }
            ReDrawListNodeDetails();
        }

        private void ReDrawListNodeDetails()
        {
            lstTopoNodes.Items.Clear();
            lstTopoNodes.Items.AddRange(topology.Routers.Values.ToArray());
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xác nhận đã nhập xong và quay về trang chủ !", "Xác nhận !") != DialogResult.OK)
            {
                return;
            }
            ((frm_Main)Owner).Topology = topology;
            Close();
        }

        private void cbbCurrentNodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckNodeLinked();
        }

        private void cbbDirectedNodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckNodeLinked();
        }

        private void CheckNodeLinked()
        {
            if (cbbLeftNodes.SelectedItem != null && cbbRightNodes.SelectedItem != null)
            {
                int ln = (int)cbbLeftNodes.SelectedItem;
                int rn = (int)cbbRightNodes.SelectedItem;
                if( ln==rn )
                    txtCost_left.Text = txtCost_right.Text = 0.ToString();
                else if (topology[ln].HasLinkConnect(topology[rn]))
                {
                    txtCost_left.Text=topology[ln].DirectedRoutersWithCost[topology[rn]].ToString();
                    txtCost_right.Text = topology[rn].DirectedRoutersWithCost[topology[ln]].ToString();
                }
                else
                {
                    txtCost_left.Text = txtCost_right.Text = "";
                }
            }            
        }
        private void CheckSameNode()
        {
            if (cbbLeftNodes.SelectedItem != null && cbbRightNodes.SelectedItem != null)
            {
                int ln = (int)cbbLeftNodes.SelectedItem;
                int rn = (int)cbbRightNodes.SelectedItem;
                if( ln==rn )
                    txtCost_left.Text = txtCost_right.Text = 0.ToString();                
            }            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xác nhận huỷ tác tụ và quay về trang chủ !", "Xác nhận !") != DialogResult.OK)
            {
                return;
            }
            Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xác nhận xoá mô hình đang nhập và nhập lại !", "Xác nhận !") != DialogResult.OK)
            {
                return;
            }
            topology = null;
            ClearUI();
            InitUI();
        }
    }
}
