namespace Sklad1
{
    partial class FormEditProduct
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
            panel1 = new Panel();
            lblTitle = new Label();
            lblArticle = new Label();
            txtArticle = new TextBox();
            lblName = new Label();
            txtName = new TextBox();
            lblCategory = new Label();
            lblPurchasePrice = new Label();
            txtPurchasePrice = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            cmbCategory = new ComboBox();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.MidnightBlue;
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4, 5, 4, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(181, 852);
            panel1.TabIndex = 1;
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 204);
            lblTitle.Location = new Point(214, 40);
            lblTitle.Margin = new Padding(4, 0, 4, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(627, 65);
            lblTitle.TabIndex = 2;
            lblTitle.Text = "Редактирование товара";
            // 
            // lblArticle
            // 
            lblArticle.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblArticle.AutoSize = true;
            lblArticle.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblArticle.Location = new Point(313, 177);
            lblArticle.Margin = new Padding(4, 0, 4, 0);
            lblArticle.Name = "lblArticle";
            lblArticle.Size = new Size(92, 28);
            lblArticle.TabIndex = 3;
            lblArticle.Text = "Артикул:";
            // 
            // txtArticle
            // 
            txtArticle.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtArticle.Location = new Point(313, 210);
            txtArticle.Margin = new Padding(4, 5, 4, 5);
            txtArticle.Name = "txtArticle";
            txtArticle.Size = new Size(404, 31);
            txtArticle.TabIndex = 7;
            // 
            // lblName
            // 
            lblName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblName.AutoSize = true;
            lblName.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblName.Location = new Point(313, 297);
            lblName.Margin = new Padding(4, 0, 4, 0);
            lblName.Name = "lblName";
            lblName.Size = new Size(104, 28);
            lblName.TabIndex = 8;
            lblName.Text = "Название:";
            // 
            // txtName
            // 
            txtName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtName.Location = new Point(313, 330);
            txtName.Margin = new Padding(4, 5, 4, 5);
            txtName.Name = "txtName";
            txtName.Size = new Size(404, 31);
            txtName.TabIndex = 9;
            // 
            // lblCategory
            // 
            lblCategory.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblCategory.AutoSize = true;
            lblCategory.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblCategory.Location = new Point(313, 422);
            lblCategory.Margin = new Padding(4, 0, 4, 0);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(110, 28);
            lblCategory.TabIndex = 10;
            lblCategory.Text = "Категория:";
            // 
            // lblPurchasePrice
            // 
            lblPurchasePrice.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblPurchasePrice.AutoSize = true;
            lblPurchasePrice.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblPurchasePrice.Location = new Point(313, 532);
            lblPurchasePrice.Margin = new Padding(4, 0, 4, 0);
            lblPurchasePrice.Name = "lblPurchasePrice";
            lblPurchasePrice.Size = new Size(141, 28);
            lblPurchasePrice.TabIndex = 12;
            lblPurchasePrice.Text = "Цена закупки:";
            // 
            // txtPurchasePrice
            // 
            txtPurchasePrice.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtPurchasePrice.Location = new Point(313, 565);
            txtPurchasePrice.Margin = new Padding(4, 5, 4, 5);
            txtPurchasePrice.Name = "txtPurchasePrice";
            txtPurchasePrice.Size = new Size(404, 31);
            txtPurchasePrice.TabIndex = 13;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom;
            btnSave.BackColor = SystemColors.ActiveCaption;
            btnSave.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnSave.Location = new Point(329, 667);
            btnSave.Margin = new Padding(4, 5, 4, 5);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(167, 70);
            btnSave.TabIndex = 14;
            btnSave.Text = "Сохранить";
            btnSave.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom;
            btnCancel.BackColor = SystemColors.ActiveCaption;
            btnCancel.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnCancel.Location = new Point(529, 667);
            btnCancel.Margin = new Padding(4, 5, 4, 5);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(167, 70);
            btnCancel.TabIndex = 15;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // cmbCategory
            // 
            cmbCategory.FormattingEnabled = true;
            cmbCategory.Location = new Point(313, 475);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(404, 33);
            cmbCategory.TabIndex = 16;
            // 
            // FormEditProduct
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(856, 852);
            Controls.Add(cmbCategory);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(txtPurchasePrice);
            Controls.Add(lblPurchasePrice);
            Controls.Add(lblCategory);
            Controls.Add(txtName);
            Controls.Add(lblName);
            Controls.Add(txtArticle);
            Controls.Add(lblArticle);
            Controls.Add(lblTitle);
            Controls.Add(panel1);
            Margin = new Padding(4, 5, 4, 5);
            Name = "FormEditProduct";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormEditProduct";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Label lblTitle;
        private Label lblArticle;
        private TextBox txtArticle;
        private Label lblName;
        private TextBox txtName;
        private Label lblCategory;
        private Label lblPurchasePrice;
        private TextBox txtPurchasePrice;
        private Button btnSave;
        private Button btnCancel;
        private ComboBox cmbCategory;
    }
}