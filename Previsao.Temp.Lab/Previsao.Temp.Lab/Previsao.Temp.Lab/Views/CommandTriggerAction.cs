using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Previsao.Temp.Lab.Views
{
    public class CommandTriggerAction : TriggerAction<View> 
    {
        public string Command { get; set; }
        protected override void Invoke(View view)
        {
            var context = view.BindingContext;
            if (context == null)
                return;

            var prop = context.GetType()?.GetRuntimeProperty(Command);

            if (prop == null)
                return;

            var command = prop.GetValue(context) as ICommand;
            command?.Execute(null);
        }
    }
}
