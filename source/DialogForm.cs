using System;
using System.Text;
using System.Windows.Forms;  //DialogForm 类只加入这个就可以
using System.Drawing;
using System.Drawing.Drawing2D;

public class DialogForm : Form
    {
        private readonly bool _disableCloseButton;

        public DialogForm()
        {
            ApplyDialogStyle(this);
        }

        public DialogForm(bool disableCloseButton)
            :this()
        {
            _disableCloseButton = disableCloseButton;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                if (_disableCloseButton)
                {
                    var CS_NOCLOSE = 0x200;
                    var parameters = base.CreateParams;
                    parameters.ClassStyle |= CS_NOCLOSE;
                    return parameters;
                }

                return base.CreateParams;
            }
        }

        /// <summary>
        /// 使普通Form应用具有对话框窗口的样式
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static Form ApplyDialogStyle(Form form)
        {
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.ShowIcon = false;
            form.ShowInTaskbar = false;
            form.StartPosition = FormStartPosition.CenterParent;
            return form;
        }
    }


class MainWindow: Form
{
	Button btnClickMe;
	Button btnShowNoCloseDialog;

	void btnClickMe_Click(object sender, EventArgs e)
	{
		Form f=DialogForm.ApplyDialogStyle(new MainWindow{Text="modal对话框"});
		f.ShowDialog();
		f.Dispose();
	}
	
	MainWindow()
	{
		this.SuspendLayout();
		Text="DialogForm DEMO"+new string(' ',12)+"F1 - Blog";
		btnClickMe=new Button{Text="显示一个modal对话框",Location=new Point(15,15),Width=225};
		btnClickMe.Click+=new EventHandler(btnClickMe_Click);
		Controls.Add(btnClickMe);		
		
		btnShowNoCloseDialog=new Button{Text="显示禁用关闭按钮的modeless窗口",Location=new Point(15,45),Width=225};
		btnShowNoCloseDialog.Click+=(sender,args)=>new DialogForm(true){Owner=this,Text="禁用关闭按钮的modeless窗口"}.Show();
		Controls.Add(btnShowNoCloseDialog);
		
		this.ResumeLayout(false);
	}
	
	protected override bool ProcessCmdKey(ref Message msg,Keys keyData)
	{
		if(keyData==Keys.F1)System.Diagnostics.Process.Start("http://hi.baidu.com/wingingbob/blog/item/f55349306910ca92a9018ec8.html");
		return base.ProcessCmdKey(ref msg,keyData);
	}
		
	[STAThread]
	static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Application.Run(new MainWindow{Width=450});
	}
}