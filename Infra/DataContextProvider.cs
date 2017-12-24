using System;
using System.Threading;

namespace MVVMApplication.Infra
{
    public class DataContextProvider
    {
        private Lazy<object> _dataGetter;

        public DataContextProvider(string resourceKey)
        {
            ResourceKey = resourceKey;

            _dataGetter = new Lazy<object>
            (
                () => JustSayNo.ViewModelHelper.GetNewViewModel(ResourceKey),
                LazyThreadSafetyMode.PublicationOnly
            );
        }

        public string ResourceKey { get; }
        public object Data => _dataGetter.Value;
        public object GetData() => Data;
    }
}
