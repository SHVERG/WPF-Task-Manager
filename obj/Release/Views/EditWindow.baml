   M S B A M L   `   `   `  ���� I  EWpfTaskManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:WpfTaskManagerWpfTaskManager      WpfTaskManager.EditWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     .localclr-namespace:WpfTaskManager     -     �Name$  
editWindow�� � Title$
 Edit��$	��400��$	��400�� � WindowStartupLocation$ CenterOwner=� � 
ResizeMode$ NoResize=� � AllowsTransparency .  � WindowStyle$
 None=� � ShowInTaskbar .  ��Z�  � DataContext    WpfTaskManager.EditVM  .��� .+���  �� CornerRadius$ 5��   MaterialDesignBackground#��[  ._��� .+���   	name_caps#���  $��Center=�  PrimaryHueMidBrush#��[ $
��Bold"�$��14(��� 
�������$��SemiBold"�$��12(�$��10, 0q����� *Name+	 �� UpdateSourceTrigger$	 PropertyChanged=���   	desc_caps#���  $��Center=�#��[ $
��Bold"�$��14(��� $	��Top=�
�������$��SemiBold"�$��12(�
 �� AcceptsReturn
 .  �� 
AcceptsTab . $��10, 0q�$	��265������ *Description+$	 PropertyChanged=��� $��1��$��Center=� �� Orientation$ 
Horizontal=�$��Center=�$��14q�.+���  	 close#���  $��20, 0q�$	��150������ *CloseCommand+����  �� ElementName$ 
editWindow����   edit#���  $��20, 0q�$	��150������ *EditCommand+���� $ 
editWindow��