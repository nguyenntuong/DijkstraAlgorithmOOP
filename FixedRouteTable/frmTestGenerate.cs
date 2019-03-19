using System;
using System.IO;
using System.Windows.Forms;

namespace FixedRouteTable
{
    public partial class frmTestGenerate : Form
    {
        public frmTestGenerate()
        {
            InitializeComponent();
        }

        private void frmTestGenerate_Load(object sender, EventArgs e)
        {
            Owner.Enabled = false;
        }

        private void frmTestGenerate_FormClosed(object sender, FormClosedEventArgs e)
        {
            Owner.Enabled = true;
        }

        private int topo_size = 0;
        private int max_cost = 0;
        private Random r = new Random();
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                topo_size = int.Parse(textBox1.Text);
                if (topo_size > 17)
                {
                    if(MessageBox.Show("Tối đa 17 node, hơn 17 node có thể stress CPU của bạn, khá là nặng !"
                        ,"Cảnh báo !",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning,MessageBoxDefaultButton.Button2)==DialogResult.Cancel)
                    return;
                    MessageBox.Show("Nói rồi đứng máy đấy, chỉnh lại đi !"
                        , "Cảnh báo lần 2 !", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                    return;
                }
                if(topo_size<2)
                {
                    MessageBox.Show("Ít nhất 2 node");
                    return;
                }
                max_cost = int.Parse(textBox3.Text);
            }
            catch (Exception)
            {

                MessageBox.Show("Dữ liệu nhập sai");
                return;
            }
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Text file|*.txt";
            saveFile.DefaultExt = "txt";
            saveFile.AddExtension = true;
            if (saveFile.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            StreamWriter writer = new StreamWriter(saveFile.FileName);
            await writer.WriteLineAsync(topo_size.ToString());
            for (int f = 1; f <= topo_size; f++)
            {
                for (int t = 1; t <= topo_size; t++)
                {
                    if (f == t)
                    {
                        await writer.WriteLineAsync($"{f} {t} 0");
                    }
                    else
                    {
                        int cost = r.Next(-max_cost*2, max_cost);
                        if (cost < 1)
                        {
                            cost = -1;
                            await writer.WriteLineAsync($"{f} {t} {cost}");
                            await writer.WriteLineAsync($"{t} {f} {cost}");
                        }
                        else
                        {
                            await writer.WriteLineAsync($"{f} {t} {cost}");
                            await writer.WriteLineAsync($"{t} {f} {r.Next(1, max_cost)}");
                        }
                    }
                    if (InvokeRequired)
                    {
                        EndInvoke(BeginInvoke(new MethodInvoker(
                        () =>
                        {
                            int percent = ((t) * (f)) / (topo_size * topo_size);
                            progressBar1.Value = percent*100;
                        })));
                    }
                    else
                    {
                        int percent = ((t) * (f)) / (topo_size * topo_size);
                        progressBar1.Value = percent * 100;
                    }
                }
            }
            await writer.WriteLineAsync("0");
            await writer.FlushAsync();
            writer.Close();
            if (InvokeRequired)
            {
                EndInvoke(BeginInvoke(new MethodInvoker(
                           () =>
                           {
                               textBox4.Text = saveFile.FileName;
                               MessageBox.Show("Tạo dữ liệu Test thành công!");
                           })));
            }
            else
            {
                textBox4.Text = saveFile.FileName;
                MessageBox.Show("Tạo dữ liệu Test thành công!");
            }
        }
    }
}
