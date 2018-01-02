namespace DicomEncapsulatedPdf
{
   partial class MainForm
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
         this.rasterImageViewer1 = new Leadtools.WinForms.RasterImageViewer();
         this.btnCreate = new System.Windows.Forms.Button();
         this.btnLoad = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // rasterImageViewer1
         // 
         this.rasterImageViewer1.Location = new System.Drawing.Point(12, 42);
         this.rasterImageViewer1.Name = "rasterImageViewer1";
         this.rasterImageViewer1.Size = new System.Drawing.Size(522, 342);
         this.rasterImageViewer1.TabIndex = 0;
         this.rasterImageViewer1.Text = "rasterImageViewer1";
         // 
         // btnCreate
         // 
         this.btnCreate.Location = new System.Drawing.Point(12, 12);
         this.btnCreate.Name = "btnCreate";
         this.btnCreate.Size = new System.Drawing.Size(189, 23);
         this.btnCreate.TabIndex = 1;
         this.btnCreate.Text = "Create Dicom Encapsulated PDF";
         this.btnCreate.UseVisualStyleBackColor = true;
         this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
         // 
         // btnLoad
         // 
         this.btnLoad.Location = new System.Drawing.Point(345, 12);
         this.btnLoad.Name = "btnLoad";
         this.btnLoad.Size = new System.Drawing.Size(189, 23);
         this.btnLoad.TabIndex = 2;
         this.btnLoad.Text = "Load Dicom Encapsulated PDF";
         this.btnLoad.UseVisualStyleBackColor = true;
         this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
         // 
         // MainForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(546, 396);
         this.Controls.Add(this.btnLoad);
         this.Controls.Add(this.btnCreate);
         this.Controls.Add(this.rasterImageViewer1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.MaximizeBox = false;
         this.Name = "MainForm";
         this.Text = "Dicom Encapsulated PDF";
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
         this.Load += new System.EventHandler(this.MainForm_Load);
         this.ResumeLayout(false);

      }

      #endregion

      private Leadtools.WinForms.RasterImageViewer rasterImageViewer1;
      private System.Windows.Forms.Button btnCreate;
      private System.Windows.Forms.Button btnLoad;
   }
}

