   M S B A M L   `   `   `  ����I  EWpfTaskManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:WpfTaskManagerWpfTaskManager  5   	       WpfTaskManager.AddWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   .localclr-namespace:WpfTaskManager     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   -   5          �Name$  	addWindow��5   	    � Title$	 Add��5	   	   $	��435��5
   	   $	��400��5   	    � WindowStartupLocation$ CenterOwner=�5   	    � 
ResizeMode$ NoResize=�5   	    � AllowsTransparency .  � WindowStyle$
 None=�5   	    � ShowInTaskbar .  ��5   	   Z�  � DataContext 5         WpfTaskManager.AddVM  5   
    � 	Resources 5      ,   &WpfTaskManager.DateTimeToBoolConverter   
DateToBool.   (WpfTaskManager.NullToVisibilityConverter  	NullToVis��%:   &	        &	        5   
   6(     5   
   6*   5      .��5      � .+�5   
   �� 	 �� CornerRadius$	 5��6     MaterialDesignBackground#��[ 5      ._�5!      �� .+�5"      ��   	name_caps#���  6   $��Center=�5#         MaterialDesign.Brush.Primary#��[ 5$      $
��Bold"�5%      $��14(�5&      �� 5(      
�������$��SemiBold"�5)      $��12(�5*      $��10, 0q�5+      ��5,      �� *Name+
 �� UpdateSourceTrigger$
 PropertyChanged=��� 5.        	desc_caps#���  6   $��Center=�5/      #��[ 50      $
��Bold"�51      $��14(�52      �� 54      $��10, 0q�6   $	��235��55      .��57      �� $	��Top=�6   
�������$��SemiBold"�59      $��12(�5:       �� AcceptsReturn . ��5<      �� *Description+$
 PropertyChanged=�5=      �� 5?       �� Orientation$ 
Horizontal=�6   $��Center=�66   $��3q�6S   .+�5A      ��   start_date_caps#���  6   #��[ 5B      $
��Bold"�5C      $��14(�5D      (  "System.Windows.Controls.DatePicker  5F      $��SemiBold"�6!   $��12(�5G   !      FirstDayOfWeek$ Monday=�5H   !      SelectedDate 5I   !   �� *	StartDate+   DisplayDateStart 5J   !   �� *StartDateLimitStart+   DisplayDateEnd 5K   !   �� *StartDateLimitEnd+5L      �� 5N        deadline_caps#���  6   #��[ 5O      $
��Bold"�5P      $��14(�5Q        5S      $��SemiBold"�6!   $��12(�5T   !   $ Monday=�5U   !    5V   !   �� *
Deadline+ 5W   !   �� *DeadlineLimitStart+ 5X   !   �� *DeadlineLimitEnd+{�5Y   !   �� *	StartDate+ �� 	Converter# [  5Z      )  #MaterialDesignThemes.Wpf.TimePicker  5\      $��1��60   $��SemiBold"�5]   0   $��12(�5^   0      	Is24Hours .    SelectedTime 5`   0   �� *Time+{�5a   0   �� *	StartDate+# [  5b      �� 5d      $ 
Horizontal=�6   $��Center=�5e      $��3q�5f      y�5g      �� *	Project+# [ .+�5i      ��   reliable_caps#���  6   #��[ 5j      $
��Bold"�5k      $��14(�5l      �� 5n      ��6   �� *Users+ �� SelectedItem 5o      �� *SelectedUser+��5p      �� 5q      .R�5r   "   �� ��6,   �� *Name+5s      5u      5v      �� 5x      $��1��6   $��Center=�5y      $ 
Horizontal=�5z      $��Center=�5{      $��14q�5|      .+�5~      ��  		 close#��� 	 6   $��20, 0q�5      $	��150��5�      ��5�      �� *CloseCommand+��5�      ��  �� ElementName$ 	addWindow���� 5�       
 add#��� 
 6   $��20, 0q�5�      $	��150��5�      ��5�      �� *
AddCommand+��5�      �� $ 	addWindow��5�      5�      5�      5�      5�      