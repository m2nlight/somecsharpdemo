using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace WpfApplication2
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			this.InitializeComponent();

			// 在此点下面插入创建对象所需的代码。
		}

		private void color_SelectedColorChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<System.Windows.Media.Color> e)
		{
			// 在此处添加事件处理程序实现。
			foreach(var button in LayoutRoot.Children)
			{
				if(button is ScrollBar)
				{
					((ScrollBar)button).Foreground=new SolidColorBrush(color1.SelectedColor);
					((ScrollBar)button).Background=new SolidColorBrush(color2.SelectedColor);
				}
				else if(button is WrapPanel)
				{
					foreach(var c2 in ((WrapPanel)button).Children)
					{
						if(c2 is ButtonBase)
						{
							((ButtonBase)c2).Foreground=new SolidColorBrush(color1.SelectedColor);
							((ButtonBase)c2).Background=new SolidColorBrush(color2.SelectedColor);
						}
					}
				}
			}
		}

		private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			// 在此处添加事件处理程序实现。
			var color=color1.SelectedColor;
			color1.SelectedColor=color2.SelectedColor;
			color2.SelectedColor=color;
		}

		private void color1_SelectedColorChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<System.Windows.Media.Color> e)
		{
			// 在此处添加事件处理程序实现。
			foreach(var button in monitorpoint.Children)
			{
				var btn=button as ButtonBase;
				if(btn==null)
					continue;
				
				btn.Foreground=new SolidColorBrush(color1_Copy.SelectedColor);
				btn.Background=new SolidColorBrush(color1_Copy1.SelectedColor);
			}
		}

		private void Button1_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			// 在此处添加事件处理程序实现。
			var color=color1_Copy.SelectedColor;
			color1_Copy.SelectedColor=color1_Copy1.SelectedColor;
			color1_Copy1.SelectedColor=color;
		}
	}
}