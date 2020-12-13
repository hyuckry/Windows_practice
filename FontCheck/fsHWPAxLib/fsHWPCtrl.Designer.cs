namespace fsHWPAxLib
{
    partial class fsHWPCtrl
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fsHWPCtrl));
            this.axHwpCtl = new AxHWPCONTROLLib.AxHwpCtrl();
            ((System.ComponentModel.ISupportInitialize)(this.axHwpCtl)).BeginInit();
            this.SuspendLayout();
            // 
            // axHwpCtl
            // 
            this.axHwpCtl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axHwpCtl.Enabled = true;
            this.axHwpCtl.Location = new System.Drawing.Point(0, 0);
            this.axHwpCtl.Name = "axHwpCtl";
            this.axHwpCtl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axHwpCtl.OcxState")));
            this.axHwpCtl.Size = new System.Drawing.Size(515, 339);
            this.axHwpCtl.TabIndex = 0;
            // 
            // fsHWPCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.axHwpCtl);
            this.Name = "fsHWPCtrl";
            this.Size = new System.Drawing.Size(515, 339);
            ((System.ComponentModel.ISupportInitialize)(this.axHwpCtl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxHWPCONTROLLib.AxHwpCtrl axHwpCtl;
    }
}
