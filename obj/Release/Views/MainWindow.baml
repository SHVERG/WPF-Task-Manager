   M S B A M L   `   `   `  ���� I  EWpfTaskManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:WpfTaskManagerWpfTaskManager      WpfTaskManager.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     .localclr-namespace:WpfTaskManager     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -     �Name$  
mainWindow�� � Title$ Project Manager��$
��1280��$	��720��$
��1280��$	��720�� � WindowStartupLocation$ CenterScreen=� � WindowStyle$
 None=� � 
ResizeMode$ 	CanResize=�    MaterialDesignDarkBackground#��[   � Icon$ /Images/icon.ico�� J� Opacity �� *	Opacity+ � 	Resources /clr-namespace:WpfTaskManagerWpfTaskManager  +   %WpfTaskManager.IntToTimespanConverter  IntToTimespan.   (WpfTaskManager.DateTimeToStringConverter  DatetimeToString(   "WpfTaskManager.CompletionConverter  
Completion!   WpfTaskManager.BindingProxy  
OwnerProxy   Data	 �� ElementName
 �� 
TargetType  WindowButtonStyle �� Property �� Value �� 
TargetType ��Name J� SnapsToDevicePixels �� ContentStringFormat �� ContentStringFormat �� Triggers P� Property P� Value�� D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     &localclr-namespace:WpfTaskManager  %�  &	       &	      &	 
     &	      &	 .              �� $	 
mainWindow���� "
 ��.���  � �� �� " ��.R��� $ Chrome��#��z��$��0q�#��z�� . ._��� #��z��#��z��# z #��z��#��z����. # z #��z����  � ��
 ���� ��  � ��$ Webdings)���  � ��$ 13.333(���  � ��
 �������  � ��$ 0,2,3,0q� P�  �@J�IsMouseOver . . ���  � ��
 �   � � DataContext    WpfTaskManager.MainVM  .��� .+�� #��[   � RowDefinitions �� $��50 ��� .+��� -     MaterialDesignBackground#��[ $��
3, 3, 3, 0q� �� CornerRadius$ 5��._�� .+���  �� Orientation$ 
Horizontal=�.+��� $��/Images/icon.png��$��5, 5, -5, 5q�o� $��40��$	��Top=�{��� *
IsLogged+.@�m�   main_menu_file#���  $��SemiBold"�.@�m�   main_menu_add_project#���   m� Command �� *AddProjCommand+ m� CommandParameter �� $	 
mainWindow��m�  	 main_menu_add_task#��� 	  �� *AddTaskCommand+ �� $	 
mainWindow���� m�  
 main_menu_export#��� 
 .@�m�   main_menu_export_sel_proj#���   �� *ExportProjCommand+m�   main_menu_export_all_proj#���   �� *ExportAllProjsCommand+m�   main_menu_import#���   �� *ImportProjCommand+�� m�   main_menu_refresh#���   �� *RefreshCommand+ �� $	 Projs_datagrid���� m� -     main_menu_exit#���  m�   main_menu_edit#���  $��SemiBold"�.@�m�   main_menu_edit_sub#���  .@�m�   project#���   �� *EditProjCommand+ �� $	 
mainWindow��m�   task#���   �� *EditTaskCommand+ �� $	 
mainWindow��m�   main_menu_delete#���  .@�m� #���   �� *DeleteProjCommand+m� #���   �� *DeleteTaskCommand+m�   main_menu_report#���  $��SemiBold"�.@�m�   projects#���  $
 True�� �� *ReportCommand+m�  	 tasks#���  $ False�� �� *ReportCommand+m�   main_menu_log#���  .@�m�   main_menu_log_show#���   �� *ShowLogCommand+m�   main_menu_log_clear#���   �� *ClearLogCommand+m�   main_menu_settings#���  $��SemiBold"�.@�m�   main_menu_lang#���  .@�m�   main_menu_lang_ru#���   m� IsCheckable .  m� 	IsChecked �� *IsLangRussian+ �� *ChangeLanguageCommand+ �� *Languages[1]+m�   main_menu_lang_en#���   .  �� *IsLangRussian+  InvertBooleanConverter �� 	Converter# [  �� *ChangeLanguageCommand+ �� *Languages[0]+m�    main_menu_logout#���    �� *LogOutCommand+m�  ! main_menu_about#��� ! $��SemiBold"��� $��Project Manager��$��Center=�$��Center=�$��16(�
��������� $��Right=�$ 
Horizontal=�.+��� -   $��0��#��[ $��16(�$��50��$��3q��� -    ��Name$ Restore_button��$��1��#��[ $��16(�$��50��$��3q��� -   $��r��#��[ $��16(�$��50��$��3q�� $��1��{��� *
IsLogged+ � ColumnDefinitions �� $��3* ��� $��2* ��� $��2* �.+��  �� �� $��40 �.+��� $��0��$ 5��#��[ $��3q�._�� .+���  " projects_caps#��� " $��Center=� # PrimaryHueMidBrush#��[# $
��Bold"�$��14(��� $��5 25 5 5q�
�������.��&   System.Windows.Controls.DataGrid      
IsReadOnly  . !   SelectionMode$! Single=�"   SelectionUnit$" FullRow=�#   AutoGenerateColumns# .  ���� *
Projects+$ �� SelectedItem$ �� *SelectedProj+%   Columns% 0  *System.Windows.Controls.DataGridTextColumn   $ name,  &System.Windows.Controls.DataGridColumn&   Header#& � $ '   Width5	  /System.Windows.Controls.DataGridLengthConverter$	' 120	 1
  +System.Windows.Controls.DataGridBoundColumn( 
  Binding( �� *Name+   % deadline#& � % ( �� *
Deadline+) �� StringFormat$) {0:dd.MM.yyyy}��   	& state#& � & ( �� *	Completed+# [ * � ContextMenu* �� .@�m� #���   �� *EditProjCommand+ �� *Data++ �� Source#+ [ �� m� #��� 
  �� *ExportProjCommand+m� #���   �� *ImportProjCommand+�� m� #���   �� *DeleteProjCommand+�� $��1�� ' main_add_proj#��� ' $	��Top=�$��3q����� *AddProjCommand+���� $	 
mainWindow��� $��1�� �� �� $��40 �.+��� $��0��$ 5��#��[ $��3q�{��� *SelectedProj+, �� TargetNullValue$, False��._��  �� $��3* ��� $��4* �.+���  ( project_info#��� ( $��Center=�#��[# $
��Bold"�$��14(��� $
��Auto=�$��3, 25, 3, 3q�.���� .+��� $ 
Horizontal=�.+��� $
��Bold"�.���� �� #�� $  �� $�:���� $��Center=����� *SelectedProj.Name+�� $
��Bold"�.���� ��  ) desc#�� )  �� $�:���� $
��Left=�$	��Top=�- �� 
IsReadOnly- . ���� *SelectedProj.Description+�� $ 
Horizontal=�.+��� $
��Bold"�.���� �� #�� %  �� $�:���� $��Center=����� *SelectedProj.Deadline+# [ �� $ 
Horizontal=�.+��� $
��Bold"�.���� ��  * comp#�� *  �� $�:���� $��Center=����� *SelectedProj.Completed+# [ ��  + 
tasks_caps#��� + $��1��$��Center=�#��[# $
��Bold"�$��14(��� $��1��$��5,25,5,5q�
�������.��    . $! Single=�$" FullRow=�# .  ���� *	ProjTasks+$ �� *SelectedTask+%   #& � $ $	' 100	 ( �� *Name+  #& � % ( �� *
Deadline+$) {0:dd.MM.yyyy}��  #& � & ( �� *	Completed+# [ * �� .@�m�  , main_menu_start#��� ,  �� *StartTaskCommand+m� #���   �� *EditTaskCommand+ �� *Data+#+ [ m�  - main_menu_complete#��� -  �� *CompleteTaskCommand+�� m� #���   �� *DeleteTaskCommand+�� $��1�� . main_add_task#��� . $	��Top=�$��3q����� *AddTaskCommand+���� $	 
mainWindow��� $��2�� �� �� $��40 �.+��� $��0��$ 5��#��[ $��3q�{��� *SelectedTask+$, False��._�� .+���  / 	task_info#��� / $��Center=�#��[# $
��Bold"�$��14(��� $
��Auto=�$��3, 25, 3, 3q�.���� .+��� $ 
Horizontal=�.+��� $
��Bold"�.���� �� #�� $  �� $�:���� $��Center=����� *SelectedTask.Name+�� $
��Bold"�.���� �� #�� )  �� $�:���� $
��Left=�$	��Top=�- . ���� *SelectedTask.Description+�� $ 
Horizontal=�.+��� $
��Bold"�.���� �� #�� %  �� $�:���� $��Center=����� *SelectedTask.Deadline+# [ �� $ 
Horizontal=�.+��� $
��Bold"�.���� �� #�� *  �� $�:���� $��Center=����� *SelectedTask.Completed+# [ �� $ 
Horizontal=�.+��� $
��Bold"�.���� ��  0 
time_spent#�� 0  �� $�:���� $��Center=����� *SelectedTask.Timespent+# [ � $��1�� �� $��5* ��� $��4* �.+���  1 main_comp_task#��� 1 $	��Top=�$��0��$��3q����� *CompleteTaskCommand+��  2 main_start_task#��� 2 $	��Top=�$��1��$��3q����� *StartTaskCommand+�� $��30, 30, 200, 30q����� *SelectedViewModel+