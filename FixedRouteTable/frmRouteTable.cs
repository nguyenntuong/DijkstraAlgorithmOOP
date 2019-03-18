using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FixedRouteTable
{
    public partial class frmRouteTable : Form
    {
        private Router Router;

        public frmRouteTable(Router router)
        {
            InitializeComponent();
            Router = router;
            Init();
        }

        private void Init()
        {
            Text = $">>{Router.HostID}<< - Routing Table";
        }

        private void frmRouteTable_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            foreach (var item in Router.RouteTable)
            {
                dataGridView1.Rows.Add(item.Key.HostID, item.Value.HostID);
            }
        }
    }
}
