   M S B A M L   `   `   `  ���� I  EWpfTaskManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:WpfTaskManagerWpfTaskManager      WpfTaskManager.AddWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     .localclr-namespace:WpfTaskManager     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  -     �Name$  	addWindow�� � Title$	 Add��$	��400��$	��400�� � WindowStartupLocation$ CenterOwner=� � 
ResizeMode$ NoResize=� � AllowsTransparency .  � WindowStyle$
 None=� � ShowInTaskbar .  ��Z�  � DataContext    WpfTaskManager.AddVM   � 	Resources .   (WpfTaskManager.BoolToVisibilityConverter   	BoolToVis	   	TrueValue
   
FalseValue�� %*   &	          $	 Visible=�$
 Hidden=�.��� .+���  �� CornerRadius$ 5��  MaterialDesignBackground#��[ ._��� .+���   	name_caps#���  $��Center=�  PrimaryHueMidBrush#��[ $
��Bold"�$��14(��� 
�������$��SemiBold"�$��12(�$��10, 0q����� *Name+ �� UpdateSourceTrigger$ PropertyChanged=���   	desc_caps#���  $��Center=�#��[ $
��Bold"�$��14(��� $��10, 0q�$	��235��.���� $	��Top=�
�������$��SemiBold"�$��12(� �� AcceptsReturn . ���� *Description+$ PropertyChanged=���  �� Orientation$ 
Horizontal=�$��Center=�$��3q�.+���   deadline_caps#���  #��[ $
��Bold"�$��14(�(  "System.Windows.Controls.DatePicker  $��SemiBold"�$��12(�   FirstDayOfWeek$ Monday=�   SelectedDate �� *
Deadline+   DisplayDateStart �� *	StartDate+   DisplayDateEnd �� *	EndDate+)  #MaterialDesignThemes.Wpf.TimePicker  $��1��$��SemiBold"�$��12(�   	Is24Hours .    SelectedTime �� *Time+�� $��1��$��Center=�$ 
Horizontal=�$��Center=�$��14q�.+���  	 close#���  $��20, 0q�$	��150������ *CloseCommand+����  �� ElementName$ 	addWindow����   add#���  $��20, 0q�$	��150������ *
AddCommand+���� $ 	addWindow��