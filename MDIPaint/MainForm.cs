using LibraryForPaint;
using MDIPaint.AddonForms;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDIPaint
{
    public partial class MainForm : Form, IPublisherAddon
    {
        public static MainForm Instante { get; private set; }

        private Form currentActiveForm;

        private List<IAddonDraw> Brushes = new List<IAddonDraw>();

        public static IAddonDraw CurrentBrush { get; private set; }

        public static Color BasePaintColor { get; set; }
        public static int PaintWidth
        {
            get => paintWidth.Value;
            set => paintWidth.Value = value;
        }

        private static FixedTextToValue<int> paintWidth;

        public MainForm()
        {
            InitializeComponent();
            Instante = this;

            var addons = this.GetAlmonerAddon();

            AddonsDescription.AlmonerAddon = addons;

            foreach (var addon in addons.GetAddonsInfo())
            {
                Brushes.AddRange(addons.GetAddonsToGroup(addon).addons);
            }

            foreach(IAddonDraw draw in Brushes)
            {
                var atr = draw.GetType().GetCustomAttribute<PropertyAddonAttribute>();
                if(atr.TypeToolStip == TypeToolStip.Tool)
                {
                    var toolStrip = draw.PostAddonToToolStip();
                    toolStrip.Size = new Size(130, 20);

                    toolStrip.Click += (obj, e) =>
                    {
                        CurrentBrush = draw;

                        draw.PostAddonToWindowMDI(this);
                    };

                    toolStripDropDown.DropDownItems.Add(toolStrip);
                }

            }

            paintWidth = new FixedTextToValue<int>("Width", width_Tool, 3);

            paintWidth.Value = 3;

            paintWidth.ChangeValue += (value) =>
            {
                if(CurrentBrush != null)
                {
                    CurrentBrush.Width = value;
                }
            };

            CurrentBrush = new DefauldBrush()
            {
                Color = Color.Black,
                Width = PaintWidth
            };
        }

        private void Tool_Exit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Tool_Information(object sender, EventArgs e)
        {
            var frmAbout = new AboutForm();
            frmAbout.ShowDialog();
        }

        private void CreateNew_Paint(object sender, EventArgs e)
        {
            var canvasSize = new CanvasSizeForm();
            canvasSize.SetDefaultValue(400, 400);
            if (canvasSize.ShowDialog() == DialogResult.OK)
            {
                var form = ShowFormMDI<PaintForm>();
                form.SetSeze(canvasSize);
                form.Size = new Size(canvasSize.Width + 100, canvasSize.Height + 100);
            }
        }

        private T ShowFormMDI<T>() where T : Form, new()
        {
            var frm = new T();
            frm.MdiParent = this;
            frm.GotFocus += new EventHandler(SelfMDIActiveMDI);
            frm.LostFocus += new EventHandler(SelfMDIDectiveMDI);
            frm.Show();
            return frm;
        }

        private void SizeCanvas_Tool(object sender, EventArgs e)
        {
            var canvasSize = new CanvasSizeForm();
            if (currentActiveForm is IReceiverSize receiver)
            {
                receiver.SetDefaultSize(canvasSize);
                if (canvasSize.ShowDialog() == DialogResult.OK)
                {
                    receiver.SetSeze(canvasSize);
                }
            }
        }

        private void рисунокToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            размерХолстраToolStripMenuItem.Enabled = (this.currentActiveForm != null);
        }

        public static void SelfMDIActiveMDI(object form, EventArgs e)
        {
            if(form is Form ItsMeActive)
                if(ItsMeActive.MdiParent == Instante)
                    Instante.currentActiveForm = ItsMeActive;
        }
        public static void SelfMDIDectiveMDI(object form, EventArgs e)
        {
            if (form is Form ItsMeActive)
                if (ItsMeActive.MdiParent == Instante)
                    Instante.currentActiveForm = null;
        }

        

        private void SelectOtherColor_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            if(colorDialog.ShowDialog() == DialogResult.OK)
            {
                CurrentBrush = new DefauldBrush()
                {
                    Color = colorDialog.Color,
                    Width = PaintWidth
                };
            }    
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (currentActiveForm is ISavedImage saved)
            {
                SaveSystem.SaveImage(saved, isSaveAs: false);
            }
        }

        private void SaveAs_Click(object sender, EventArgs e)
        {
            if (currentActiveForm is ISavedImage saved)
            {
                SaveSystem.SaveImage(saved, isSaveAs: true);
            }
        }

        private void File_DropDownOpened(object sender, EventArgs e)
        {
            bool isSaved = currentActiveForm is ISavedImage;
            сохранитьToolStripMenuItem.Enabled = isSaved;
            SaveAs_Tool.Enabled = isSaved;
        }

        private void Open_Click(object sender, EventArgs e)
        {
            SaveSystem.OpenImage(ShowFormMDI<PaintForm>());
        }

        private void CascadeToolStrip_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileHorizontalToolStrip_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void TileVerticalToolStrip_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void ArrangeIconsToolStrip_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void EraserTool_Click(object sender, EventArgs e)
        {
            CurrentBrush = new EraserBrush()
            {
                Width = PaintWidth
            };
        }

        private void SelectBlackColor_Click(object sender, EventArgs e)
        {
            CurrentBrush = new DefauldBrush()
            {
                Color = Color.Black,
                Width = PaintWidth
            };
        }
        private void SelectWhiteColor_Click(object sender, EventArgs e)
        {
            CurrentBrush = new DefauldBrush()
            {
                Width = PaintWidth
            };
        }
        private void SelectRedColor_Click(object sender, EventArgs e)
        {
            CurrentBrush = new DefauldBrush()
            {
                Color = Color.Red,
                Width = PaintWidth
            };
        }

        private void SelectBlueColor_Click(object sender, EventArgs e)
        {
            CurrentBrush = new DefauldBrush()
            {
                Color = Color.Blue,
                Width = PaintWidth
            };
        }

        private void SelectGreenColor_Click(object sender, EventArgs e)
        {
            CurrentBrush = new DefauldBrush()
            {
                Color = Color.Green,
                Width = PaintWidth
            };
        }

        private void RoughBrush_Click(object sender, EventArgs e)
        {
            CurrentBrush.Color = Color.FromArgb(255, CurrentBrush.Color);
        }

        private void SoftBrush_Click(object sender, EventArgs e)
        {
            CurrentBrush.Color = Color.FromArgb(20, CurrentBrush.Color);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void OpenAddonsDescriptionList_Click(object sender, EventArgs e)
        {
            AddonsDescription addons = new AddonsDescription();
            addons.Show();
        }
    }
}
