using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Botany_Buddy.Helpers
{
    using Square.Picasso;
    using System;

    public class CustomCallback : Java.Lang.Object, ICallback
    {
        private readonly Action _onSuccessAction;
        private readonly Action _onErrorAction;

        public CustomCallback(Action onSuccessAction, Action onErrorAction = null)
        {
            _onSuccessAction = onSuccessAction;
            _onErrorAction = onErrorAction;
        }

        public void OnSuccess()
        {
            _onSuccessAction?.Invoke();
        }

        public void OnError()
        {
            _onErrorAction?.Invoke();
        }

        public void OnError(Java.Lang.Exception p0)
        {
            _onErrorAction?.Invoke();
        }
    }

}