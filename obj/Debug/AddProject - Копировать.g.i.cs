﻿#pragma checksum "..\..\AddProject - Копировать.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "1001B6F9D4C85F929978EB3E81259D84FBC981F206BC7624D2FD69E140B0CAD2"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using WpfTaskManager;


namespace WpfTaskManager {
    
    
    /// <summary>
    /// AddProject
    /// </summary>
    public partial class AddProject : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\AddProject - Копировать.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid Main_grid;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\AddProject - Копировать.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border AddProject_border;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\AddProject - Копировать.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Name_textbox;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\AddProject - Копировать.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Description_textbox;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\AddProject - Копировать.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker Deadline_datepicker;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\AddProject - Копировать.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Close_button;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\AddProject - Копировать.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddProject_button;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WpfTaskManager;component/addproject%20-%20%d0%9a%d0%be%d0%bf%d0%b8%d1%80%d0%be%d" +
                    "0%b2%d0%b0%d1%82%d1%8c.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\AddProject - Копировать.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Main_grid = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.AddProject_border = ((System.Windows.Controls.Border)(target));
            return;
            case 3:
            this.Name_textbox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.Description_textbox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.Deadline_datepicker = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 6:
            this.Close_button = ((System.Windows.Controls.Button)(target));
            
            #line 37 "..\..\AddProject - Копировать.xaml"
            this.Close_button.Click += new System.Windows.RoutedEventHandler(this.Close_button_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.AddProject_button = ((System.Windows.Controls.Button)(target));
            
            #line 38 "..\..\AddProject - Копировать.xaml"
            this.AddProject_button.Click += new System.Windows.RoutedEventHandler(this.AddProject_button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

