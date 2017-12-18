using DRM.PropBag;
using DRM.PropBag.AutoMapperSupport;
using DRM.PropBag.Caches;
using DRM.PropBag.ControlModel;
using DRM.PropBag.ControlsWPF;
using DRM.PropBagWPF;
using DRM.TypeSafePropertyBag;
using DRM.ViewModelTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
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

        public static string PackageConfigName { get; set; }

        static JustSayNo() 
        {
            PropStoreAccessServiceProvider =  GetNewSimplePropStoreAccessServiceProvider();

            //IProvideDelegateCaches delegateCacheProvider = new SimpleDelegateCacheProvider();

            ThePropFactory = new WPFPropFactory
                (
                    propStoreAccessServiceProvider: PropStoreAccessServiceProvider,
                    //delegateCacheProvider: delegateCacheProvider,
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

        //if (typeName.ToLower().EndsWith("listcollectionview"))
        //{
        //    System.Windows.Data.ListCollectionView listCollectionView = new System.Windows.Data.ListCollectionView(new List<string>());
        //    result = listCollectionView.GetType();
        //}
        //else
        //{
        //    throw new InvalidOperationException($"Cannot create a Type instance from the string: {typeName}.");
        //}

        public static Type GetTypeFromName(string typeName)
        {
            if (!TryFindType(typeName, out Type result))
            {
                throw new InvalidOperationException($"Cannot create a Type instance from the string: {typeName}.");
            }
            return result;
        }

        static Dictionary<string, Type> typeCache = new Dictionary<string, Type>();

        public static bool TryFindType(string typeName, out Type t)
        {
            lock (typeCache)
            {
                if (!typeCache.TryGetValue(typeName, out t))
                {
                    try
                    {
                        t = Type.GetType(typeName);
                    }
                    catch (Exception e)
                    {
                        throw new InvalidOperationException($"Cannot create a Type instance from the string: {typeName}.", e);
                    }

                    if (t == null)
                    {
                        foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            try
                            {
                                t = a.GetType(typeName);
                                if (t != null)
                                    break;
                            }
                            catch (Exception e)
                            {
                                throw new InvalidOperationException($"Cannot create a Type instance from the string: {typeName}.", e);
                            }
                        }
                    }
                    typeCache[typeName] = t; // perhaps null
                }
            }
            return t != null;
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
