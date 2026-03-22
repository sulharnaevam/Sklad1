namespace Sklad1.Forms
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            dgvProducts = new DataGridView();
            btnCreate = new Button();
            CreateMenu = new ContextMenuStrip(components);
            menuProduct = new ToolStripMenuItem();
            menuCategory = new ToolStripMenuItem();
            menuShipment = new ToolStripMenuItem();
            btnEdit = new Button();
            EditMenu = new ContextMenuStrip(components);
            menuEditProduct = new ToolStripMenuItem();
            menuEditCategory = new ToolStripMenuItem();
            btnDelete = new Button();
            btnHistory = new Button();
            panel1 = new Panel();
            panel2 = new Panel();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            CreateMenu.SuspendLayout();
            EditMenu.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // dgvProducts
            // 
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.Location = new Point(0, 37);
            dgvProducts.MultiSelect = false;
            dgvProducts.Name = "dgvProducts";
            dgvProducts.ReadOnly = true;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.Size = new Size(900, 494);
            dgvProducts.TabIndex = 2;
            // 
            // btnCreate
            // 
            btnCreate.BackColor = Color.White;
            btnCreate.ContextMenuStrip = CreateMenu;
            btnCreate.Location = new Point(394, 12);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new Size(120, 40);
            btnCreate.TabIndex = 3;
            btnCreate.Text = "Создать ";
            btnCreate.UseVisualStyleBackColor = false;
            // 
            // CreateMenu
            // 
            CreateMenu.Items.AddRange(new ToolStripItem[] { menuProduct, menuCategory, menuShipment });
            CreateMenu.Name = "createMenu";
            CreateMenu.Size = new Size(131, 70);
            // 
            // menuProduct
            // 
            menuProduct.Name = "menuProduct";
            menuProduct.Size = new Size(130, 22);
            menuProduct.Text = "Товар";
            // 
            // menuCategory
            // 
            menuCategory.Name = "menuCategory";
            menuCategory.Size = new Size(130, 22);
            menuCategory.Text = "Категория";
            // 
            // menuShipment
            // 
            menuShipment.Name = "menuShipment";
            menuShipment.Size = new Size(130, 22);
            menuShipment.Text = "Отгрузка";
            // 
            // btnEdit
            // 
            btnEdit.BackColor = Color.White;
            btnEdit.ContextMenuStrip = EditMenu;
            btnEdit.Location = new Point(268, 11);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(120, 40);
            btnEdit.TabIndex = 4;
            btnEdit.Text = "Редактировать ";
            btnEdit.UseVisualStyleBackColor = false;
            // 
            // EditMenu
            // 
            EditMenu.Items.AddRange(new ToolStripItem[] { menuEditProduct, menuEditCategory });
            EditMenu.Name = "editMenu";
            EditMenu.Size = new Size(131, 48);
            // 
            // menuEditProduct
            // 
            menuEditProduct.Name = "menuEditProduct";
            menuEditProduct.Size = new Size(130, 22);
            menuEditProduct.Text = "Товар";
            // 
            // menuEditCategory
            // 
            menuEditCategory.Name = "menuEditCategory";
            menuEditCategory.Size = new Size(130, 22);
            menuEditCategory.Text = "Категория";
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.White;
            btnDelete.Location = new Point(16, 12);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(120, 40);
            btnDelete.TabIndex = 5;
            btnDelete.Text = "Удалить";
            btnDelete.UseVisualStyleBackColor = false;
            // 
            // btnHistory
            // 
            btnHistory.BackColor = Color.White;
            btnHistory.Location = new Point(142, 12);
            btnHistory.Name = "btnHistory";
            btnHistory.Size = new Size(120, 40);
            btnHistory.TabIndex = 6;
            btnHistory.Text = "История отгрузок";
            btnHistory.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.MidnightBlue;
            panel1.Controls.Add(btnDelete);
            panel1.Controls.Add(btnCreate);
            panel1.Controls.Add(btnEdit);
            panel1.Controls.Add(btnHistory);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 537);
            panel1.Name = "panel1";
            panel1.Size = new Size(900, 63);
            panel1.TabIndex = 7;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.ActiveCaption;
            panel2.Controls.Add(label1);
            panel2.Controls.Add(dgvProducts);
            panel2.Controls.Add(panel1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(900, 600);
            panel2.TabIndex = 8;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Black", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.Location = new Point(360, 9);
            label1.Name = "label1";
            label1.Size = new Size(193, 25);
            label1.TabIndex = 0;
            label1.Text = "СПИСОК ТОВАРОВ";
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 600);
            Controls.Add(panel2);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FormMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Список товаров";
            ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
            CreateMenu.ResumeLayout(false);
            EditMenu.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvProducts;
        private Button btnCreate;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnHistory;
        private ContextMenuStrip CreateMenu;
        private ContextMenuStrip EditMenu;
        private ToolStripMenuItem menuProduct;
        private ToolStripMenuItem menuCategory;
        private ToolStripMenuItem menuShipment;
        private ToolStripMenuItem menuEditProduct;
        private ToolStripMenuItem menuEditCategory;
        private Panel panel1;
        private Panel panel2;
        private Label label1;
    }
}