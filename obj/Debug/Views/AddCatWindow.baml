   M S B A M L   `   `   `  ����I  EWpfTaskManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:WpfTaskManagerWpfTaskManager  5   	   !    WpfTaskManager.AddCatWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   .localclr-namespace:WpfTaskManager     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   -   5          �Name$  addCatWindow��5   	    � Title$	 Add��5	   	   $	��180��5
   	   $	��400��5   	    � WindowStartupLocation$ CenterOwner=�5   	    � 
ResizeMode$ NoResize=�5   	    � AllowsTransparency .  � WindowStyle$
 None=�5   	    � ShowInTaskbar .  ��5   	   Z�  � DataContext 5         WpfTaskManager.AddCatVM  5   
    � 	Resources 5      .   (WpfTaskManager.BoolToVisibilityConverter   	BoolToVis	   	TrueValue
   
FalseValue��%=   &	          5   
   $	 Visible=�6<   $
 Hidden=�6P   5      .��5      � .+�5   
   ��  �� CornerRadius$ 5��6     MaterialDesignBackground#��[ 5      ._�5       �� .+�5!      ��   	name_caps#���  6   $��Center=�5"         MaterialDesign.Brush.Primary#��[ 5#      $
��Bold"�5$      $��14(�5%      �� 5'      
�������$��SemiBold"�5(      $��12(�5)      $��10, 0q�5*      ��5+      �� *Name+ �� UpdateSourceTrigger$ PropertyChanged=��� 5-        
color_caps#���  6   $��Center=�5.      #��[ 5/      $
��Bold"�50      $��14(�51      *  $MaterialDesignThemes.Wpf.ColorPicker  53         Color 6-   �� *Color+�� 55      $��1��6   $��Center=�56       �� Orientation$ 
Horizontal=�57      $��Center=�58      $��14q�59      .+�5;      ��  	 close#���  6   $��20, 0q�5<      $	��150��5=      ��5>      �� *CloseCommand+��5?      ��  �� ElementName$ addCatWindow���� 5A        add#���  6   $��20, 0q�5B      $	��150��5C      ��5D      �� *
AddCommand+��5E      �� $ addCatWindow��5F      5G      5H      5I      5J      