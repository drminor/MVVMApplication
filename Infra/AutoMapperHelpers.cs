using DRM.PropBag;
using DRM.PropBag.AutoMapperSupport;
using DRM.PropBag.ControlModel;
using DRM.PropBag.ControlsWPF;
using DRM.TypeSafePropertyBag;
using System;
using System.ComponentModel;
using System.Windows;

namespace MVVMApplication.Infra
{
    public class AutoMapperHelpers
    {
        public AutoMapperProvider InitializeAutoMappers(IPropModelProvider propModelProvider)
        {
            IPropBagMapperBuilderProvider propBagMapperBuilderProvider
                = new SimplePropBagMapperBuilderProvider(wrapperTypeCreator: null, viewModelActivator: null);

            IMapTypeDefinitionProvider mapTypeDefinitionProvider = new SimpleMapTypeDefinitionProvider();

            ICachePropBagMappers mappersCachingService = new SimplePropBagMapperCache();

            AutoMapperProvider autoMapperProvider = new AutoMapperProvider
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
    public static class JustSayNo // To using Static-based config providers.
    {
        public static PropModelProvider PropModelProvider { get; }
        public static ViewModelHelper ViewModelHelper { get; }
        public static AutoMapperProvider AutoMapperProvider { get; }
        public static IPropFactory ThePropFactory { get; }

        static JustSayNo() 
        {
            ThePropFactory = new PropFactory
                (
                    //returnDefaultForUndefined: false,
                    typeResolver: GetTypeFromName,
                    valueConverter: null
                );

            IPropBagTemplateProvider propBagTemplateProvider
                = new PropBagTemplateProvider(Application.Current.Resources);

            PropModelProvider = new PropModelProvider(propBagTemplateProvider, ThePropFactory);

            ViewModelHelper = new ViewModelHelper(PropModelProvider);

            AutoMapperProvider = new AutoMapperHelpers().InitializeAutoMappers(PropModelProvider);
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
