﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PrepTime.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
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
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PrepTime.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
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
        ///   Looks up a localized string similar to PrepTime.
        /// </summary>
        internal static string ApplicationName {
            get {
                return ResourceManager.GetString("ApplicationName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This dish contains tasks are you sure you want to delete this dish and all of its associated tasks?.
        /// </summary>
        internal static string ConfirmDeleteDish {
            get {
                return ResourceManager.GetString("ConfirmDeleteDish", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Are you sure you want to delete this task?.
        /// </summary>
        internal static string ConfirmDeleteTask {
            get {
                return ResourceManager.GetString("ConfirmDeleteTask", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Would you like to save the meal before exiting?.
        /// </summary>
        internal static string ConfirmExitDataDirty {
            get {
                return ResourceManager.GetString("ConfirmExitDataDirty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Would you like to save the meal before creating a new one?.
        /// </summary>
        internal static string ConfirmNewDataDirty {
            get {
                return ResourceManager.GetString("ConfirmNewDataDirty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Would you like to save the meal before opening a new one?.
        /// </summary>
        internal static string ConfirmOpenDataDirty {
            get {
                return ResourceManager.GetString("ConfirmOpenDataDirty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dish{0}.
        /// </summary>
        internal static string DefaultDishName {
            get {
                return ResourceManager.GetString("DefaultDishName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Description.
        /// </summary>
        internal static string DescriptionColumn {
            get {
                return ResourceManager.GetString("DescriptionColumn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Add.
        /// </summary>
        internal static string DishActionAdd {
            get {
                return ResourceManager.GetString("DishActionAdd", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Delete.
        /// </summary>
        internal static string DishActionDelete {
            get {
                return ResourceManager.GetString("DishActionDelete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Edit.
        /// </summary>
        internal static string DishActionEdit {
            get {
                return ResourceManager.GetString("DishActionEdit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dish.
        /// </summary>
        internal static string DishColumn {
            get {
                return ResourceManager.GetString("DishColumn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This dependency cannot be added because it would create a circular dependency..
        /// </summary>
        internal static string ErrorCircularDependency {
            get {
                return ResourceManager.GetString("ErrorCircularDependency", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The dependent {0} does not exist..
        /// </summary>
        internal static string ErrorDependencyDoesNotExist {
            get {
                return ResourceManager.GetString("ErrorDependencyDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was no dish found that matched the ID given for the dependent dish..
        /// </summary>
        internal static string ErrorDependentDishIDNotFound {
            get {
                return ResourceManager.GetString("ErrorDependentDishIDNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The dish &quot;{0}&quot; cannot depend on the dish &quot;{1}&quot; because there would be a circular dependency..
        /// </summary>
        internal static string ErrorDishCircularDependencyFound {
            get {
                return ResourceManager.GetString("ErrorDishCircularDependencyFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A dish with that name exists already..
        /// </summary>
        internal static string ErrorDishNameExists {
            get {
                return ResourceManager.GetString("ErrorDishNameExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The dish name cannot be empty..
        /// </summary>
        internal static string ErrorEmptyDishName {
            get {
                return ResourceManager.GetString("ErrorEmptyDishName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to order the tasks for export..
        /// </summary>
        internal static string ErrorFailedToOrderTasks {
            get {
                return ResourceManager.GetString("ErrorFailedToOrderTasks", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} Error.
        /// </summary>
        internal static string ErrorFormTitle {
            get {
                return ResourceManager.GetString("ErrorFormTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was an error generating the HTML to export..
        /// </summary>
        internal static string ErrorHtmlGeneration {
            get {
                return ResourceManager.GetString("ErrorHtmlGeneration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was no dish found that matched the ID given for the independent dish..
        /// </summary>
        internal static string ErrorIndependentDishIDNotFound {
            get {
                return ResourceManager.GetString("ErrorIndependentDishIDNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to More than one colon found in interval string..
        /// </summary>
        internal static string ErrorIntervalStringMoreThanOneColon {
            get {
                return ResourceManager.GetString("ErrorIntervalStringMoreThanOneColon", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The interval string must contain only digits and, optionally, a single colon..
        /// </summary>
        internal static string ErrorIntervalStringUnknownCharacter {
            get {
                return ResourceManager.GetString("ErrorIntervalStringUnknownCharacter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The meal that was loaded had no dishes..
        /// </summary>
        internal static string ErrorLoadMealWithNoDishes {
            get {
                return ResourceManager.GetString("ErrorLoadMealWithNoDishes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There must be at least one dish..
        /// </summary>
        internal static string ErrorMustBeOneDish {
            get {
                return ResourceManager.GetString("ErrorMustBeOneDish", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was no dish found that matched the given ID..
        /// </summary>
        internal static string ErrorNoMatchingDishID {
            get {
                return ResourceManager.GetString("ErrorNoMatchingDishID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was no entity found that matched the given ID..
        /// </summary>
        internal static string ErrorNoMatchingEntityID {
            get {
                return ResourceManager.GetString("ErrorNoMatchingEntityID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was no task found that matched the given ID..
        /// </summary>
        internal static string ErrorNoMatchingTaskID {
            get {
                return ResourceManager.GetString("ErrorNoMatchingTaskID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was an error saving the meal to the file..
        /// </summary>
        internal static string ErrorSavingMealToFile {
            get {
                return ResourceManager.GetString("ErrorSavingMealToFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The task description cannot be empty..
        /// </summary>
        internal static string ErrorTaskDescriptionEmpty {
            get {
                return ResourceManager.GetString("ErrorTaskDescriptionEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The task interval cannot be empty..
        /// </summary>
        internal static string ErrorTaskIntervalEmpty {
            get {
                return ResourceManager.GetString("ErrorTaskIntervalEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The task interval is invalid..
        /// </summary>
        internal static string ErrorTaskIntervalInvalid {
            get {
                return ResourceManager.GetString("ErrorTaskIntervalInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The tasks be from the same dish..
        /// </summary>
        internal static string ErrorTasksNotFromSameDish {
            get {
                return ResourceManager.GetString("ErrorTasksNotFromSameDish", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was an error writing the exported HTML file..
        /// </summary>
        internal static string ErrorWritingHtmlFile {
            get {
                return ResourceManager.GetString("ErrorWritingHtmlFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HTML Files (*.html)|*.html.
        /// </summary>
        internal static string FilterHtmlFile {
            get {
                return ResourceManager.GetString("FilterHtmlFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Meal Files (*.meal)|*.meal.
        /// </summary>
        internal static string FilterMealFile {
            get {
                return ResourceManager.GetString("FilterMealFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;!DOCTYPE html&gt;
        ///&lt;html&gt;
        ///&lt;head&gt;
        ///&lt;title&gt;PrepTime Help&lt;/title&gt;
        ///&lt;style&gt;
        ////* Reset default browser styles */
        ///html, body, header, nav, footer, article, p, h1, h2, div, span
        ///{
        ///   margin: 0px;
        ///   padding: 0px;
        ///   border: 0px;
        ///   font-size: 1em;
        ///   font-weight: normal;
        ///   vertical-align: baseline;
        ///   line-height: 1.2;
        ///}
        ///ol, ul
        ///{
        ///   margin: 0px;
        ///   border: 0px;
        ///   font-size: 1em;
        ///   vertical-align: baseline;
        ///   line-height: 1.2;
        ///}
        ////* End reset default browser styles */
        ///html[data-useragent*=&apos;MSI [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string help {
            get {
                return ResourceManager.GetString("help", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Interval.
        /// </summary>
        internal static string IntervalColumn {
            get {
                return ResourceManager.GetString("IntervalColumn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;New Meal&gt;.
        /// </summary>
        internal static string NewMealTitleText {
            get {
                return ResourceManager.GetString("NewMealTitleText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Options.
        /// </summary>
        internal static string OptionsColumn {
            get {
                return ResourceManager.GetString("OptionsColumn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select Action.
        /// </summary>
        internal static string SelectDishAction {
            get {
                return ResourceManager.GetString("SelectDishAction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Delete.
        /// </summary>
        internal static string TaskActionDelete {
            get {
                return ResourceManager.GetString("TaskActionDelete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dependencies.
        /// </summary>
        internal static string TaskActionDependencies {
            get {
                return ResourceManager.GetString("TaskActionDependencies", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Edit.
        /// </summary>
        internal static string TaskActionEdit {
            get {
                return ResourceManager.GetString("TaskActionEdit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Move Down.
        /// </summary>
        internal static string TaskActionMoveDown {
            get {
                return ResourceManager.GetString("TaskActionMoveDown", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Move Up.
        /// </summary>
        internal static string TaskActionMoveUp {
            get {
                return ResourceManager.GetString("TaskActionMoveUp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Time.
        /// </summary>
        internal static string TimeColumn {
            get {
                return ResourceManager.GetString("TimeColumn", resourceCulture);
            }
        }
    }
}
