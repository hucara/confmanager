using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Configuration_Manager.Views;
using Configuration_Manager.CustomControls;

using Debug = System.Diagnostics.Debug;
using System.Xml.Linq;
using Configuration_Manager.RelationManagers;

namespace Configuration_Manager
{
    public partial class MainForm : Form
    {
        private Model model = Model.getInstance();
        private ObjectDefinitionManager odm = ObjectDefinitionManager.getInstance();
        private Util.FormImmobiliser fi;

        SectionTabsView sectionTabsView;
        SectionMenuView sectionMenuView;
        CustomHandler ch;
        SplashScreen sc;
        GlassForm gf;

        public MainForm()
        {
            InitializeComponent();
            this.Show();
            this.Visible = false;
            this.BringToFront();

            model.ReadConfigurationFile();
            sc = new SplashScreen();
            sc.SendToBack();
            System.Threading.Thread t = new System.Threading.Thread(ShowSplashScreen);
            t.Start();
            
            System.Threading.Thread.Sleep(1000);
            LoadingProcess();

            System.Threading.Thread.Sleep(100);
            t.Abort();

            this.Activate();
            this.TopMost = true;
            this.TopMost = false;
            this.Focus();

            this.notificationPopUp.Top = this.Size.Height;
            this.notificationPopUp.BringToFront();

            gf = new GlassForm();
            SetUpGlassForm();

            this.Width += 1;
            this.Width -= 1;

            model.uiChanged = false;
        }

        private void SetUpGlassForm()
        {
            model.glassScreen = gf;
            gf.Show();
            gf.Enabled = false;
            gf.Visible = false;
        }

        public void ShowSplashScreen()
        {
            sc.Show(model.headline, Application.ProductVersion);
            sc.SendToBack();
            sc.Dispose();
        }

        public void LoadingProcess()
        {
            CheckObjectDefinitionIntegrity();

            SetUpHeadLine();
            SetUpMainForm();
            PrintWellcomeLogMessage();
            InitCustomHandler();
            InitViews();
            InitHandlers();
        }

        // Checks if the structure of the XML file is correct. Deal with it.
        private void CheckObjectDefinitionIntegrity()
        {
            if (System.IO.File.Exists(model.objectDefinitionsPath) && !model.objectDefinitionExists)
            {
                // The structure is broken
                String caption = Model.GetTranslationFromID(37);
                String msg = Model.GetTranslationFromID(47) + " " + Model.GetTranslationFromID(52) + "\n" +Model.GetTranslationFromID(43);
                MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);

                model.logCreator.Append("Integrity problem in Object Definition File");
                model.logCreator.Append(msg);

                System.Environment.Exit(1);
            }
        }

        private void SetUpHeadLine()
        {
            this.Text = model.headline;
        }

        private void PrintWellcomeLogMessage()
        {
            model.logCreator.Append(" ");
            model.logCreator.AppendDividingLine();
            model.logCreator.AppendCenteredWithFrame("");
            model.logCreator.AppendCenteredWithFrame(" Configuration Manager ");
            model.logCreator.AppendCenteredWithFrame("");
            model.logCreator.AppendDividingLine();
            model.logCreator.Append(" ");
        }

        private void SetUpMainForm()
        {
            this.TopMost = model.stayOnTop;
            this.Top = model.top;
            this.Left = model.left;

            this.ClientSize = new Size(model.width, model.height);

            this.tabControl.Width = this.ClientSize.Width - (this.sectionBar.Width + 5);
            this.tabControl.Height = this.ClientSize.Height + 30;

            if (model.resizable)
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            else
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;

            if (!model.movable)
                fi = new Util.FormImmobiliser(this);

            this.BringToFront();
        }

        private void InitCustomHandler()
        {
            ch = new CustomHandler(contextMenu);
            ControlFactory.SetCustomHandler(ch);
        }

        private void InitHandlers()
        {
            this.labelToolStripMenuItem.Click += ch.labelToolStripMenuItem_Click;
            this.textBoxToolStripMenuItem.Click += ch.textBoxToolStripMenuItem_Click;
            this.checkBoxToolStripMenuItem.Click += ch.checkBoxToolStripMenuItem_Click;
            this.comboBoxToolStripMenuItem.Click += ch.comboBoxToolStripMenuItem_Click;
            this.buttonToolStripMenuItem.Click += ch.buttonToolStripMenuItem_Click;

            this.groupBoxToolStripMenuItem.Click += ch.groupBoxToolStripMenuItem_Click;
            this.shapeToolStripMenuItem.Click += ch.shapeToolStripMenuItem_Click;
            this.ImageToolStripMenuItem.Click += ch.bitmapToolSTripMenuItem_Click;

            this.tabControlMenuItem.Click += ch.tabControlToolStripMenuItem_Click;
            this.tabPageMenuItem.Click += ch.tabPageToolStripMenuItem_Click;

            this.editToolStripMenuItem.Click += ch.editToolStripMenuItem_Click;
            this.copyStripMenuItem.Click += ch.copyStripMenuItem_Click;
            this.cutToolStripMenuItem.Click += ch.cutToolStripMenuItem_Click;
            this.pasteToolStripMenuItem.Click += ch.pasteToolStripMenuItem_Click;
            this.deleteToolStripMenuItem.Click += ch.deleteToolStripMenuItem_Click;
        }

        private void InitViews()
        {
            sectionTabsView = new SectionTabsView(tabControl, ch);
            sectionMenuView = new SectionMenuView(sectionBar, tabControl, contextMenu);

            if (model.objectDefinitionExists)
            {
                odm.SetDocument(model.configObjects);
                odm.RestoreOldUI();
                sectionMenuView.readAndShow();
                sectionTabsView.readAndShow();
            }
            else
            {
                model.logCreator.AppendCenteredWithFrame(" ! Object Definition file not found ! ");
                model.logCreator.AppendCenteredWithFrame(" Creating new empty UI ");
                model.logCreator.Append(" ");
            }
        }

        private void SwitchProgMode()
        {
            model.SwitchProgrammingMode();
            if (model.progMode)
                sectionBar.BackColor = System.Drawing.Color.LightPink;
            else
                sectionBar.BackColor = System.Windows.Forms.ProfessionalColors.ToolStripBorder;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.Control && e.KeyCode == Keys.P)
            {
                SwitchProgMode();
                this.Refresh();
            }

            if (e.Alt && e.Control && e.KeyCode == Keys.G && !model.saving)
            {
                WriteConfigurationManager.SaveChanges();
                ShowNotificationPanel(Model.GetTranslationFromID(74));
            }
            if (model.progMode)
            {
                if (e.Alt && e.Control && e.KeyCode == Keys.S && !model.saving)
                {
                    odm.SerializeObjectDefinition();
                    ShowNotificationPanel(Model.GetTranslationFromID(72));
                }
                if (e.Alt && e.Control && e.KeyCode == Keys.D)
                {
                    System.Diagnostics.Debug.WriteLine("\n###################################");
                    System.Diagnostics.Debug.WriteLine("! Printing Sections:");

                    foreach (Section s in model.sections)
                        System.Diagnostics.Debug.WriteLine(
                            s.Name + " " + s.Tab.GetType().Name + ":" + s.Tab.Name + " " + s.Button.GetType().Name + ":" + s.Button.Text);

                    System.Diagnostics.Debug.WriteLine("-----------------------------------");
                    System.Diagnostics.Debug.WriteLine("! Printing List of Controls: ");
                    System.Diagnostics.Debug.WriteLine("\tControl        Parent");
                    System.Diagnostics.Debug.WriteLine("___________________________________");
                    foreach (ICustomControl c in model.allControls)
                    {
                        String line = "\t";
                        if (c.cd.Name != null) line += c.cd.Name + " ..... ";
                        if (c.cd.Parent != null) line += c.cd.Parent.Name;
                        System.Diagnostics.Debug.WriteLine(line);
                    }
                    System.Diagnostics.Debug.WriteLine("#####################################");
                }

                if (e.Alt && e.Control && e.KeyCode == Keys.R)
                {
                    System.Diagnostics.Debug.WriteLine("\n###################################");
                    Debug.WriteLine("Main Visibility Rights: " + model.mainDisplayRights.ToString());
                    Debug.WriteLine("Main Modificiation Rights: " + model.mainModificationRights.ToString());
                    Debug.WriteLine("---------------------------------------------------------");
                    foreach (ICustomControl c in model.allControls)
                    {
                        string line = "\t";
                        line += "- " + c.cd.Name + "\t\t Display: " + c.cd.operatorVisibility + "\t Modify: " + c.cd.operatorModification;
                        System.Diagnostics.Debug.WriteLine(line);
                    }
                }

                if (e.Alt && e.KeyCode == Keys.Down)
                    ch.MultiplyControl();

                if (e.Control && (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up))
                    ch.TransportControl(e);

                if (e.Control && e.KeyCode == Keys.E)
                    editToolStripMenuItem.PerformClick();
            }
        }

        private void newSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SectionEditor se = new SectionEditor();
            if (DialogResult.OK == se.ShowDialog())
            {
                sectionMenuView.AddNewSection(se.section);
                sectionTabsView.readAndShow();
            }
            else
            {
                model.sections.Remove(se.section);
                sectionTabsView.readAndShow();
            }

            this.Width++;
            this.Width--;
        }

        private void deleteSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String msg = Model.GetTranslationFromID(36);
            String caption = Model.GetTranslationFromID(32);
            DialogResult dr = MessageBox.Show(msg, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                sectionMenuView.RemoveSection();
                sectionTabsView.readAndShow();
            }
        }

        private void toolStrip_RightClick(object sender, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            Control c = sender as Control;

            if (model.progMode && me.Button == MouseButtons.Right)
            {
                if (sectionBar.GetItemAt(me.X, me.Y) is CToolStripButton)
                {
                    sectionBar.GetItemAt(me.X, me.Y).PerformClick();
                    deleteSectionToolStripMenuItem.Enabled = true;
                    editSectionNameToolStripMenuItem.Enabled = true;
                    sectionMenuView.SetSelectedButton(sectionBar.GetItemAt(me.X, me.Y) as CToolStripButton);
                }
                else
                {
                    deleteSectionToolStripMenuItem.Enabled = false;
                    editSectionNameToolStripMenuItem.Enabled = false;
                }

                if (model.sections.Count >= Model.getInstance().maxSections)
                    newSectionToolStripMenuItem.Enabled = false;
                else
                    newSectionToolStripMenuItem.Enabled = true;

                contextNavMenu.Show(c, me.X, me.Y);
            }
        }

        private void toolStrip_SizeChanged(object sender, EventArgs e)
        {
            model.sectionMenuWidth = (sender as ToolStrip).Size.Width;
        }

        private void ReplaceLabels()
        {
            XDocument xdoc;
            try
            {
                xdoc = XDocument.Load(Model.getInstance().textsFilePath);
                IEnumerable<XElement> texts = xdoc.Descendants("TextFile").Descendants("Texts").Descendants("Text");
                this.Text = texts.Single(x => (int?)x.Attribute("id") == 26).Value;
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("*** ERROR *** There was an error reading text for main form labels.");
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Model.getInstance().uiChanged)
            {
                String caption = Model.GetTranslationFromID(60);
                String msg = Model.GetTranslationFromID(62);
                DialogResult dr = MessageBox.Show(msg, caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    odm.SerializeObjectDefinition();
                    e.Cancel = false;
                }
                else if (dr == System.Windows.Forms.DialogResult.No)
                    e.Cancel = false;
                else if (dr == System.Windows.Forms.DialogResult.Cancel)
                    e.Cancel = true;
            }

            foreach (ICustomControl c in Model.getInstance().allControls)
            {
                if (c.cd.Changed && c.cd.MainDestination != "")
                {
                    Debug.WriteLine("Changed: " + c.cd.Name);
                    String caption = Model.GetTranslationFromID(60);
                    String msg = Model.GetTranslationFromID(61);
                    DialogResult dr = MessageBox.Show(msg, caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

                    if (dr == System.Windows.Forms.DialogResult.Yes)
                    {
                        WriteConfigurationManager.SaveChanges();
                        e.Cancel = false;
                    }
                    else if (dr == System.Windows.Forms.DialogResult.No)
                        e.Cancel = false;
                    else if (dr == System.Windows.Forms.DialogResult.Cancel)
                        e.Cancel = true;

                    break;
                }
            }

            SaveBoundaries();
        }

        private void SaveBoundaries()
        {
            XDocument xdoc;
            try
            {
                xdoc = XDocument.Load(model.configFilePath);
                XElement settings = xdoc.Element("ConfigurationManager").Element("Settings");

                settings.Element("Top").Value = this.Top.ToString();
                settings.Element("Left").Value = this.Left.ToString();
                settings.Element("Height").Value = this.ClientSize.Height.ToString();
                settings.Element("Width").Value = this.ClientSize.Width.ToString();

                xdoc.Save(model.configFilePath);
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("*** ERROR *** There was an error writing location in config file.");
            }
        }

        private void editSectionNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            CToolStripButton tb = null;

            foreach (Section s in model.sections)
                if (s.Selected) tb = s.Button;

            if (tb != null)
            {
                SectionEditor se = new SectionEditor(model.currentSection);
                se.ShowDialog();
                //sectionMenuView.readAndShow();
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            // Update the info in the model
            this.model.width = this.ClientSize.Width;
            this.model.height = this.ClientSize.Height;

            // Set the size of the tabs
            this.tabControl.Width = this.ClientSize.Width - (this.sectionBar.Width + 5);
            this.tabControl.Height = this.ClientSize.Height + 30;

            Debug.WriteLine(" ");
            Debug.WriteLine("Form: " + this.Size + " || Section bar: " +sectionBar.Width);
            Debug.WriteLine("Tab Control: " +tabControl.Size);

            if (gf != null) UpdateGlassForm();
        }

        private void MainForm_Move(object sender, EventArgs e)
        {
            if (gf != null) UpdateGlassForm();
        }

        private void UpdateGlassForm()
        {
            //gf.Activate();
            if (this.tabControl.TabCount > 0)
            {
                gf.Size = this.tabControl.SelectedTab.ClientSize;
                Size difference = this.tabControl.Size - this.tabControl.SelectedTab.ClientSize;
                gf.DesktopLocation = this.PointToScreen(this.tabControl.Location + difference);
            }
        }

        private void ShowNotificationPanel(String message)
        {
            //Show
            notificationPopUp.Text = " " + Model.GetTranslationFromID(70) + " ";
            notificationLabel.Text = message;
            notificationPopUp.Update();

            for (int i = 0; i < notificationPopUp.Height + 10; i++)
            {
                notificationPopUp.Top -= 1;
                notificationPopUp.Update();
                System.Threading.Thread.Sleep(1);
            }

            //rotate icon
            System.Threading.Thread.Sleep(1000);
            //RotateImage();
            
            for (int i = 0; i < notificationPopUp.Height + 10; i++)
            {
                notificationPopUp.Top += 1;
                sectionBar.Update();
                notificationPopUp.Update();
                System.Threading.Thread.Sleep(1);
            }

            notificationPopUp.Top = this.Height ;
        }

        private void RotateImage()
        {
            if (notificationPictureBox.Image == null) return;

            Image image = notificationPictureBox.Image;
            Bitmap bmp = new Bitmap(image.Width, image.Height);

            Graphics gfx = Graphics.FromImage(bmp);

            //now we set the rotation point to the center of our image
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image
            gfx.RotateTransform(85f);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            gfx.DrawImage(image, new Point(0, 0));
            //gfx.Dispose();
            notificationPictureBox.Image = image;
            notificationPictureBox.Invalidate();
            notificationPopUp.Invalidate();

            //dispose of our Graphics object

            System.Threading.Thread.Sleep(1000);

            //Bitmap rotated = new Bitmap(image.Width, image.Height);

            //Graphics g = Graphics.FromImage(image);

            //g.TranslateTransform(image.Width / 2, image.Height / 2);

            //for (int i = 0; i < 361; i++)
            //{
            //    g.RotateTransform(i);
            //    g.DrawImage(image, new Point(0, 0));
            //    System.Threading.Thread.Sleep(1);
            //    notificationPictureBox.Update();
            //}
        }
    }
}
