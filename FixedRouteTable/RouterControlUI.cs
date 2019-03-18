using System.Drawing;
using System.Windows.Forms;

namespace FixedRouteTable
{
    public partial class RouterControlUI : UserControl
    {
        private Router self;
        public Router Self { get => self; set => self = value; }

        private Color color;
        private ToolTip toolTip;
        public RouterControlUI(Router router,Color color)
        {
            InitializeComponent();
            Self = router;
            this.color = color;
            InitUI();
        }


        private void InitUI()
        {
            BackColor = color;
            lblRID.Text = Self.HostID.ToString();

            toolTip = new ToolTip();
            toolTip.InitialDelay = 0;
            toolTip.ToolTipIcon = ToolTipIcon.Info;
            toolTip.UseAnimation = true;
            toolTip.UseFading = true;
        }

        private void RouterControlUI_MouseHover(object sender, System.EventArgs e)
        {
            toolTip.Show(Self.ToString(), this);
        }

        private void lblRID_Click(object sender, System.EventArgs e)
        {
            Form frm = new frmRouteTable(Self);
            frm.Show();
        }
    }
}
