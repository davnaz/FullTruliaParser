﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TruliaParser {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FT.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на https://www.FT.com.
        /// </summary>
        internal static string BaseLink {
            get {
                return ResourceManager.GetString("BaseLink", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SP_ClearRegions.
        /// </summary>
        internal static string ClearRegionsSPName {
            get {
                return ResourceManager.GetString("ClearRegionsSPName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Data Source=DESKTOP-9QE6CM7;Initial Catalog=TruliaProperty;Integrated Security=True.
        /// </summary>
        internal static string DbConnectionStringAzure {
            get {
                return ResourceManager.GetString("DbConnectionStringAzure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на AddOrUpdateOffer.
        /// </summary>
        internal static string InsertOrUpdateOfferStoredProcedure {
            get {
                return ResourceManager.GetString("InsertOrUpdateOfferStoredProcedure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SP_Insertregion.
        /// </summary>
        internal static string InsertRegionSP {
            get {
                return ResourceManager.GetString("InsertRegionSP", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на 5.
        /// </summary>
        internal static string MaxDegreeOfParallelism {
            get {
                return ResourceManager.GetString("MaxDegreeOfParallelism", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на 400.
        /// </summary>
        internal static string MaxProxyPing {
            get {
                return ResourceManager.GetString("MaxProxyPing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на 3.
        /// </summary>
        internal static string NumberOfLoadRetrying {
            get {
                return ResourceManager.GetString("NumberOfLoadRetrying", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на proxylist.txt.
        /// </summary>
        internal static string ProxyList {
            get {
                return ResourceManager.GetString("ProxyList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SELECT * FROM Cities WHERE Parsed = 0.
        /// </summary>
        internal static string QuerySelectCitiesUndone {
            get {
                return ResourceManager.GetString("QuerySelectCitiesUndone", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SELECT * FROM Regions WHERE Done = &apos;0&apos;.
        /// </summary>
        internal static string QuerySelectRegionsUndone {
            get {
                return ResourceManager.GetString("QuerySelectRegionsUndone", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SELECT * FROM States WHERE Parsed = 0.
        /// </summary>
        internal static string QuerySelectStatesUndone {
            get {
                return ResourceManager.GetString("QuerySelectStatesUndone", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SP_AddNewCity.
        /// </summary>
        internal static string SP_AddNewCity {
            get {
                return ResourceManager.GetString("SP_AddNewCity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SP_AddNewState.
        /// </summary>
        internal static string SP_AddNewState {
            get {
                return ResourceManager.GetString("SP_AddNewState", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SP_AddNewStreet.
        /// </summary>
        internal static string SP_AddNewStreet {
            get {
                return ResourceManager.GetString("SP_AddNewStreet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SP_FinalizeCity.
        /// </summary>
        internal static string SP_FinalizeCity {
            get {
                return ResourceManager.GetString("SP_FinalizeCity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SP_FinalizeRegion.
        /// </summary>
        internal static string SP_FinalizeRegion {
            get {
                return ResourceManager.GetString("SP_FinalizeRegion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SP_AddOffer.
        /// </summary>
        internal static string SP_InsertOffer {
            get {
                return ResourceManager.GetString("SP_InsertOffer", resourceCulture);
            }
        }
    }
}
