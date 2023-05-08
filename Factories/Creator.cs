using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfTaskManager
{
    public abstract class Creator
    {
        public abstract Prototype Create(AddVM vm);
    }
}
