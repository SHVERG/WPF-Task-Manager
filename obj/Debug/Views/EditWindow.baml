   M S B A M L   `   `   `  ����I  EWpfTaskManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:WpfTaskManagerWpfTaskManager  5   	       WpfTaskManager.EditWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:WpfTaskManager     5   	   -   5          �Name$  
editWindow��5   	    � Title$
 Edit��5   	   $	��400��5	   	   $	��400��5
   	    � WindowStartupLocation$ CenterOwner=�5   	    � 
ResizeMode$ NoResize=�5   	    � AllowsTransparency .  � WindowStyle$
 None=�5   	    � ShowInTaskbar .  ��5   	   Z�  � DataContext 5         WpfTaskManager.EditVM  5   
   .��5      � .+�5   
   ��  �� CornerRadius$ 5��6      MaterialDesignBackground#��[  5      ._�5      �� .+�5      ��   	name_caps#���  6   $��Center=�5         MaterialDesign.Brush.Primary#��[ 5      $
��Bold"�5      $��14(�5      �� 5!      
�������$��SemiBold"�5"      $��12(�5#      $��10, 0q�5$      ��5%      �� *Name+	 �� UpdateSourceTrigger$	 PropertyChanged=��� 5'        	desc_caps#���  6   $��Center=�5(      #��[ 5)      $
��Bold"�5*      $��14(�5+      �� 5-      $	��Top=�6   
�������$��SemiBold"�5/      $��12(�50      
 �� AcceptsReturn
 .  �� 
AcceptsTab . $��10, 0q�53      $	��265��54      ��55      �� *Description+$	 PropertyChanged=��� 57      $��1��6   $��Center=�58       �� Orientation$ 
Horizontal=�59      $��Center=�5:      $��14q�5;      .+�5=      ��  	 close#���  6   $��20, 0q�5>      $	��150��5?      ��5@      �� *CloseCommand+��5A      ��  �� ElementName$ 
editWindow���� 5C        edit#���  6   $��20, 0q�5D      $	��150��5E      ��5F      �� *EditCommand+��5G      �� $ 
editWindow��5H      5I      5J      5K      5L      