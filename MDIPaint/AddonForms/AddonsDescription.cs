using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryForPaint;

namespace MDIPaint.AddonForms
{
    public partial class AddonsDescription : Form
    {
        public AddonsForm AddonsForm { get; private set; }

        public AddonsList AddonsList { get; private set; }

        public static IAlmonerAddon AlmonerAddon { get; set; }

        private List<InfoAddon> infoAddons = new List<InfoAddon>();

        public AddonsDescription()
        {
            InitializeComponent();
            AddonsForm = new AddonsForm();
            AddonsList = new AddonsList();

            infoAddons = AlmonerAddon.GetAddonsInfo().ToList();

            var dllGroup = infoAddons.GroupBy((info) => info.DllName).ToDictionary(
                (group) => group.Key, (group) => group.OrderBy(e => e.DllName).ToList());

            List<Control> controls = new List<Control>();

            var arrayKey = dllGroup.Keys.ToArray();
            var arrayValue = dllGroup.Values.ToArray();

            Point currentPosition = new Point(10, 10);

            for (int i = 0; i < arrayKey.Length; i++)
            {
                Label label = new Label();
                label.Text = arrayKey[i].Remove(arrayKey[i].LastIndexOf(','));
                label.Location = currentPosition;
                label.Size = new Size(400, 20);
                currentPosition = new Point(currentPosition.X, currentPosition.Y + 20);
                controls.Add(label); 
                for (int y = 0; y < arrayKey.Length; y++)
                {
                    CheckBox checkBox = new CheckBox();
                    InfoAddon addon = arrayValue[i][y];
                    checkBox.Text = addon.Name;
                    checkBox.Location = currentPosition;
                    checkBox.Size = new Size(400, 20);

                    checkBox.Checked = AlmonerAddon.IsAutoLoad(addon);
                    checkBox.CheckedChanged += (obj, e) => 
                    {
                        AlmonerAddon.SetAutoLoad(addon, checkBox.Checked);
                    };
                    currentPosition = new Point(currentPosition.X, currentPosition.Y + 20);
                    controls.Add(checkBox);
                }
            }
            AddonsList.Controls.AddRange(controls.ToArray());
        }

        private void AddonsDescription_Load(object sender, EventArgs e)
        {
            ShowFormChild(AddonsList);
            ShowFormChild(AddonsForm);
        }

        private void ShowFormChild(Form child)
        {
            child.MdiParent = this;
            child.AutoScroll = true;
            child.Show();
        }

        private void AddonsDescription_Shown(object sender, EventArgs e)
        {
            AddonsDescription_Resize(sender,e);
        }

        private void AddonsDescription_Resize(object sender, EventArgs e)
        {
            Form[] forms = this.MdiChildren;

            if (forms.Length == 0) return;

            int heightPerForm = this.ClientSize.Width / forms.Length;

            for (int i = 0; i < forms.Length; i++)
            {
                forms[i].SuspendLayout();
                forms[i].Location = new Point(i * heightPerForm, 0);
                forms[i].Size = new Size(heightPerForm - 5, this.ClientSize.Height - 5);
                forms[i].ResumeLayout();
            }
        }
    }
}
