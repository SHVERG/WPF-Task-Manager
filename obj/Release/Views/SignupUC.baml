   M S B A M L   `   `   `  ���� I  EWpfTaskManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:WpfTaskManagerWpfTaskManager      WpfTaskManager.SignupUC   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     .localclr-namespace:WpfTaskManager     _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  .���   J� InputBindings  �� 
 �� Key$ Return�� �� Command �� *SignUpCommand+.+���  �� CornerRadius$ 10��   PrimaryHueDarkBrush#��[  $��1��._�� $��5q� � RowDefinitions �� �� $	��12* �.+���   register_title#���  
�������$
��Bold"�$��28(�$��Center=��� $��1��
�������$ 10��$��5q�._��� $��20q�$��Center=�$��Center=�.+���   login_username#���  $��24(�$
��Bold"��� $��24(� # register_username_password_hint)  #MaterialDesignThemes.Wpf.HintAssist   Hint# �  ���� *
Username+ �� UpdateSourceTrigger$ PropertyChanged=���   register_name#���  $��24(�$
��Bold"��� $��24(�  register_name_hint# �  ���� *Name+$ PropertyChanged=��� $��E-Mail��$��24(�$
��Bold"�$��0, 20, 0, 0q��� $��24(�  register_email_hint# �  ���� *Email+$ PropertyChanged=���   login_password#���  $��24(�$
��Bold"�$��0, 20, 0, 0q�G� -   $��PassBox��$��24(�# �  ��  �� Orientation$ 
Horizontal=�$��0, 100, 0, 0q�.+���   login_register_btn#���  $	��300��$��50��$��24(����� *SignUpCommand+����  �� ElementName$ PassBox����  	 register_alr_reg#��� 	 $��50, 0, 10 ,0q�$��Center=�$��20(�
���������  
 login_login_btn#��� 
 $	��150��$��50��$��24(����� *LogInNavCommand+