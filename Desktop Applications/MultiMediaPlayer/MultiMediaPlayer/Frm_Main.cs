namespace MultiMediaPlayer
{
    public partial class Frm_Main : Form
    {
        public Frm_Main()
        {
            InitializeComponent();
            customizeDesign();
        }

        private void customizeDesign()
        {
            panelMediaSubmenu.Visible = false;
            panelPlaylistSubMenu.Visible = false;
            panelToolsSubmenu.Visible = false;
        }
        private void hideSubMenu()
        {
            if (panelPlaylistSubMenu.Visible == true)
                panelPlaylistSubMenu.Visible = false;
            if (panelPlaylistSubMenu.Visible == true)
                panelPlaylistSubMenu.Visible = false;
            if (panelToolsSubmenu.Visible == true)
                panelToolsSubmenu.Visible = false;
        }

        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideSubMenu();
                subMenu.Visible = true;
            }
            else
            {
                subMenu.Visible = false;
            }
        }

        private void Frm_Main_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panelMediaSubmenu_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void panelMediaSubmenu_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            // 
            // your code
            hideSubMenu();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            showSubMenu(panelPlaylistSubMenu);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click_2(object sender, EventArgs e)
        {
            // 
            // your code
            hideSubMenu();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            // 
            // your code
            hideSubMenu();
        }

        private void btnMedia_Click(object sender, EventArgs e)
        {
            showSubMenu(panelMediaSubmenu);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openChildForm(new Form2());
            // 
            // your code
            hideSubMenu();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 
            // your code
            hideSubMenu();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // 
            // your code
            hideSubMenu();
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            // 
            // your code
            hideSubMenu();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // 
            // your code
            hideSubMenu();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // 
            // your code
            hideSubMenu();
        }

        private void btnEqualizer_Click(object sender, EventArgs e)
        {
            openChildForm(new Form3());
            // 
            // your code
            hideSubMenu();
        }

        #region ToolsSubMenu

        private void btnTools_Click(object sender, EventArgs e)
        {
            showSubMenu(panelToolsSubmenu);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            // 
            // your code
            hideSubMenu();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            // 
            // your code
            hideSubMenu();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            // 
            // your code
            hideSubMenu();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            // 
            // your code
            hideSubMenu();
        }
        #endregion

        private void panelChildForm_Paint(object sender, PaintEventArgs e)
        {

        }

        private Form activeForm = null;

        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelChildForm.Controls.Add(childForm);
            panelChildForm.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
