   M S B A M L   `   `   `  ����I  EWpfTaskManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:WpfTaskManagerWpfTaskManager  5   	       WpfTaskManager.LogWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:WpfTaskManager     5   	   -   5          �Name$  	logWindow��5   	    � Title$	 Log��5   	   $	��600��5	   	   $	��600��5
   	    � WindowStartupLocation$ CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	    � AllowsTransparency .  � Icon$ /Images/icon.ico��5   	    � WindowStyle$
 None=�5   	    � ShowInTaskbar . ��5   	   Z�  � DataContext 5         WpfTaskManager.LogVM  5   
   .��5      � .+�5   
   �� 	 �� CornerRadius$	 5��6      MaterialDesignBackground#��[  5      ._�5      �� .+�5      �� $
��LOGS��6   $��Center=�5        PrimaryHueMidBrush#��[ 5      $
��Bold"�5      $��16(�5       �� 5"      
 �� Orientation$
 
Horizontal=�6   $��Center=�5#      .+�5$      �� $
��Logs��6   #��[ 5%      $��SemiBold"�5&      $��14(�5'      �� 5)      $��SemiBold"�5*      $��12(�5+       �� SelectedIndex 5)      �� *ChoiceIndex+.@�5-      �� $	��All��6'   �� 5.      $��Projects��6'   �� 5/      $��Tasks��6'   50      �� 52      $��
Start Date��6   #��[ 53      $��SemiBold"�54      $��14(�55      (  "System.Windows.Controls.DatePicker  56      -   5          Name$ 	StartDate��56   !      SelectedDate 64   �� *	StartDate+�� 58      $��End Date��6   #��[ 59      $��SemiBold"�5:      $��14(�5;        5<      -   5        $ EndDate��5<   !    62   �� *	EndDate+5=      �� 5?      $��10q�6   $	��463��5@      .��5A      &   System.Windows.Controls.DataGrid     
IsReadOnly .    AutoGenerateColumns .  ��5C      �� *
DGSource+   Columns 5D      0  *System.Windows.Controls.DataGridTextColumn  5E      ,  &System.Windows.Controls.DataGridColumn   Header$ Message��61   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6B   �� *	Message+  5F      $
 Date��61    5G   1   �� *Date+5I      5J      �� 5L      $��1��6   $��Center=�5M      $
 
Horizontal=�5N      $��Center=�5O      $��14q�5P      .+�5R      �� $��CLOSE��6   $��20, 0q�5S      $	��150��5T      ��5U      �� *CloseCommand+��5V      ��  �� ElementName$ 	logWindow���� 5X      $
��SHOW��6   $��20, 0q�5Y      $	��150��5Z      ��5[      �� *ShowCommand+�� 5]      $��
SAVE AS...��6   $��20, 0q�5^      $	��150��5_      ��5`      �� *SaveCommand+5a      5b      5c      5d      5e      