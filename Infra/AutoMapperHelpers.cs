using DRM.PropBag;
using DRM.PropBag.AutoMapperSupport;
using DRM.PropBag.ControlModel;
using DRM.PropBag.ControlsWPF;
using DRM.TypeSafePropertyBag;
using DRM.ViewModelTools;
using System;
using System.ComponentModel;
using System.Windows;

namespace MVVMApplication.Infra
{
    using PropIdType = UInt32;
    using PropNameType = String;
    using PSAccessServiceProviderType = IProvidePropStoreAccessService<UInt32, String>;

    public class AutoMapperHelpers
    {
        public SimpleAutoMapperProvider InitializeAutoMappers(IPropModelProvider propModelProvider)
        {
            IPropBagMapperBuilderProvider propBagMapperBuilderProvider
                = new SimplePropBagMapperBuilderProvider(wrapperTypeCreator: null, viewModelActivator: null);

            IMapTypeDefinitionProvider mapTypeDefinitionProvider = new SimpleMapTypeDefinitionProvider();

            ICachePropBagMappers mappersCachingService = new SimplePropBagMapperCache();

            SimpleAutoMapperProvider autoMapperProvider = new SimpleAutoMapperProvider
                (
                mapTypeDefinitionProvider: mapTypeDefinitionProvider,
                mappersCachingService: mappersCachingService,
                mapperBuilderProvider: propBagMapperBuilderProvider,
                propModelProvider: propModelProvider
                );

            return autoMapperProvider;
        }
    }

    // TODO: Fix this!!
    public static class JustSayNo // JustSayNo to using Static-based config providers.
    {
        public static IPropModelProvider PropModelProvider { get; }
        public static ViewModelHelper ViewModelHelper { get; }
        public static SimpleAutoMapperProvider AutoMapperProvider { get; }
        public static IPropFactory ThePropFactory { get; }

        public static PSAccessServiceProviderType PropStoreAccessServiceProvider { get; }

        static JustSayNo() 
        {
            PropStoreAccessServiceProvider =  GetNewSimplePropStoreAccessServiceProvider();

            ThePropFactory = new PropFactory
                (
                    propStoreAccessServiceProvider: PropStoreAccessServiceProvider,
                    typeResolver: GetTypeFromName,
                    valueConverter: null
                );

            //ThePropFactory = GetPropFactory<PropBagT, PropDataT>(PropStoreAccessServiceProvider);

            IPropBagTemplateProvider propBagTemplateProvider = new PropBagTemplateProvider(Application.Current.Resources);

            IViewModelActivator vmActivator = new SimpleViewModelActivator();
            PropModelProvider = new PropModelProvider(propBagTemplateProvider, ThePropFactory, vmActivator);

            ViewModelHelper = new ViewModelHelper(PropModelProvider, vmActivator);

            AutoMapperProvider = new AutoMapperHelpers().InitializeAutoMappers(PropModelProvider);
        }

        //public static IPropFactory GetPropFactory(IProvidePropStoreAccessService<PropBagT, PropDataT> propStoreAccessServiceProvider)

        //{
        //    IPropFactory result = new PropFactory(propStoreAccessServiceProvider: propStoreAccessServiceProvider,
        //            /*returnDefaultForUndefined: false,*/ typeResolver: GetTypeFromName, valueConverter: null);

        //    return result;
        //}

        public static IProvidePropStoreAccessService<PropIdType, PropNameType> GetNewSimplePropStoreAccessServiceProvider()
        {
            int MAX_NUMBER_OF_PROPERTIES = 65536;

            IProvideHandlerDispatchDelegateCaches handlerDispatchDelegateCacheProvider = new SimpleHandlerDispatchDelegateCacheProvider();

            IProvidePropStoreAccessService<PropIdType, PropNameType> result = 
                new SimplePropStoreAccessServiceProvider(MAX_NUMBER_OF_PROPERTIES, handlerDispatchDelegateCacheProvider);

            return result;
        }

        public static Type GetTypeFromName(string typeName)
        {
            Type result;
            try
            {
                result = Type.GetType(typeName);
            }
            catch (System.Exception e)
            {
                throw new InvalidOperationException($"Cannot create a Type instance from the string: {typeName}.", e);
            }

            if (result == null)
            {
                throw new InvalidOperationException($"Cannot create a Type instance from the string: {typeName}.");
            }

            return result;
        }

        private static void XX()
        {

        }

        #region InDesign Support
        public static bool InDesignMode() => _isInDesignMode.HasValue && _isInDesignMode == true;

        public static bool? _isInDesignMode;

        public static bool IsInDesignModeStatic
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                {
                    var prop = DesignerProperties.IsInDesignModeProperty;
                    _isInDesignMode
                        = (bool)DependencyPropertyDescriptor
                                        .FromProperty(prop, typeof(FrameworkElement))
                                        .Metadata.DefaultValue;
                }

                return _isInDesignMode.Value;
            }
        }
        #endregion
    }
}
