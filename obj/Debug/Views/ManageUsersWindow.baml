   M S B A M L   `   `   `  ����I  EWpfTaskManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:WpfTaskManagerWpfTaskManager  5   	   &     WpfTaskManager.ManageUsersWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:WpfTaskManager     5   	   -   5          �Name$  manageUsersWindow��5   	    � Title$ Users Control��5   	   $	��600��5	   	   $
��1000��5
   	    � WindowStartupLocation$ CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	    � AllowsTransparency .  � WindowStyle$
 None=�5   	    � ShowInTaskbar .  ��5   	   Z�  � 	Resources 5      *   $WpfTaskManager.RoleIdToNameConverter   RoleIdToName.   (WpfTaskManager.BoolToVisibilityConverter  	BoolToVis   	TrueValue	   
FalseValue!   WpfTaskManager.BindingProxy  VisProxy
   Data �� Path��%�   &	        &	      &	 Q       5   
   6&     5   
   $ 	Collapsed=�5   *   $	 Visible=�5   *     5   
   
 6.   �� $ ShowOnlyRequests�5       � DataContext 5      "   WpfTaskManager.ManageUsersVM  5   
   .��5      � .+�5    
   ��  �� CornerRadius$ 5��6     MaterialDesignBackground#��[ 5!      ._�5"      � $��10q�6    � RowDefinitions 5#      �� 5$      $��3* �6$   �� 5%      $
��Auto �6$   �� 5&      $
��Auto �6$   .+�5)      &   System.Windows.Controls.DataGrid  $��0��6      AutoGenerateColumns .     
IsReadOnly .    SelectionMode$ Single=�5.      ��5*      �� *Users+ �� SelectedItem 5+      �� *SelectedUser+   Columns 50      0  *System.Windows.Controls.DataGridTextColumn  51        login_username,  &System.Windows.Controls.DataGridColumn   Header# �  6-      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 53   -   1	  +System.Windows.Controls.DataGridBoundColumn 	  Binding 52   -   �� *
Username+53   -     55        register_name# �  6-   $ * 57   -    56   -   �� *Name+57   -     59      $ EMail��6-   $ * 5;   -    5:   -   �� *Email+5;   -     5=        register_role# �  6-   $ * 5>   -      
Visibility 5?   -   �� *Data+ �� Source# [  �� 	Converter# [  5@   -   �� *IdRole+# [  5B      �� 5D      $��1��6    �� Orientation$ 
Horizontal=�5E      $��Center=�5F      $��	0,10,0,10q�5G      .+�5I      ��   register_new_role#���  6    $��Center=�5J       $��0,0,5,0q�5K       �� 5M       �� DisplayMemberPath$
 Name��5O      $	��200��5P      ��5M      �� *AvailableRoles+ 5N      �� *SelectedRole+5P      5Q      �� 5S      $��2��6   $��Center=�5T      $ 
Horizontal=�5U      $��Center=�5V      .+�5X      ��  	 close#���  6   $��20, 0q�5Y      $	��150��5Z      ��5[      �� *CloseCommand+��5\      ��  �� ElementName$ manageUsersWindow���� 5^       	 set_role#��� 	 6   $	��150��5_      $��20, 0q�5`      ��5a      �� *ApplyRoleCommand+�� 5c       

 delete#��� 
 6   $	��150��5d      $��20, 0q�5e      ��5f      �� *DeleteUserCommand+5g      5h      5i      5j      5k      