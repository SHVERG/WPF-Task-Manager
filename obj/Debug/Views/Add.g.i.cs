﻿#pragma checksum "..\..\..\Views\Add.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "3D27847E14BA41A03886276473CD03D4A4AA856FA161E89B87F94A50AA742661"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
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
    /// Add
    /// </summary>
    public partial class Add : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\Views\Add.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal WpfTaskManager.Add addWindow;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\Views\Add.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid Main_grid;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\Views\Add.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border Add_border;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\Views\Add.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Name_textbox;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\Views\Add.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Description_textbox;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\Views\Add.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker Deadline_datepicker;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\Views\Add.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.TimePicker Deadline_timepicker;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\Views\Add.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Close_button;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\..\Views\Add.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Add_button;
        
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
            System.Uri resourceLocater = new System.Uri("/WpfTaskManager;component/views/add.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\Add.xaml"
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
            this.addWindow = ((WpfTaskManager.Add)(target));
            return;
            case 2:
            this.Main_grid = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.Add_border = ((System.Windows.Controls.Border)(target));
            return;
            case 4:
            this.Name_textbox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.Description_textbox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.Deadline_datepicker = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 7:
            this.Deadline_timepicker = ((MaterialDesignThemes.Wpf.TimePicker)(target));
            return;
            case 8:
            this.Close_button = ((System.Windows.Controls.Button)(target));
            return;
            case 9:
            this.Add_button = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
