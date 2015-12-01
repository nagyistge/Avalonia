﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace Perspex.Xaml.Interactivity
{
    using System;
    using System.Collections.Generic;
    using Perspex.Controls;
    using Perspex.Interactivity;

    /// <summary>
    /// Defines a <see cref="BehaviorCollection"/> attached property and provides a method for executing an <seealso cref="ActionCollection"/>.
    /// </summary>
    public sealed class Interaction
    {
        /// <remarks>
        /// CA1053: Static holder types should not have public constructors
        /// </remarks>
        private Interaction()
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="BehaviorCollection"/> associated with a specified object.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly PerspexProperty BehaviorsProperty = PerspexProperty.RegisterAttached<PerspexObject, BehaviorCollection>(
            "Behaviors",
            typeof(Interaction));
            //TODO: new PropertyMetadata(null, new PropertyChangedCallback(Interaction.OnBehaviorsChanged)));

        /// <summary>
        /// Gets the <see cref="BehaviorCollection"/> associated with a specified object.
        /// </summary>
        /// <param name="obj">The <see cref="PerspexObject"/> from which to retrieve the <see cref="BehaviorCollection"/>.</param>
        /// <returns>A <see cref="BehaviorCollection"/> containing the behaviors associated with the specified object.</returns>
        public static BehaviorCollection GetBehaviors(PerspexObject obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            BehaviorCollection behaviorCollection = (BehaviorCollection)obj.GetValue(Interaction.BehaviorsProperty);
            if (behaviorCollection == null)
            {
                behaviorCollection = new BehaviorCollection();
                obj.SetValue(Interaction.BehaviorsProperty, behaviorCollection);

                var frameworkElement = obj as Control;

                if (frameworkElement != null)
                {
                    frameworkElement.AttachedToVisualTree -= FrameworkElement_Loaded;
                    frameworkElement.AttachedToVisualTree += FrameworkElement_Loaded;
                    frameworkElement.DetachedFromVisualTree -= FrameworkElement_Unloaded;
                    frameworkElement.DetachedFromVisualTree += FrameworkElement_Unloaded;
                }
            }

            return behaviorCollection;
        }

        /// <summary>
        /// Sets the <see cref="BehaviorCollection"/> associated with a specified object.
        /// </summary>
        /// <param name="obj">The <see cref="PerspexObject"/> on which to set the <see cref="BehaviorCollection"/>.</param>
        /// <param name="value">The <see cref="BehaviorCollection"/> associated with the object.</param>
        public static void SetBehaviors(PerspexObject obj, BehaviorCollection value)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            obj.SetValue(Interaction.BehaviorsProperty, value);
        }

        /// <summary>
        /// Executes all actions in the <see cref="ActionCollection"/> and returns their results.
        /// </summary>
        /// <param name="sender">The <see cref="System.Object"/> which will be passed on to the action.</param>
        /// <param name="actions">The set of actions to execute.</param>
        /// <param name="parameter">The value of this parameter is determined by the calling behavior.</param>
        /// <returns>Returns the results of the actions.</returns>
        public static IEnumerable<object> ExecuteActions(object sender, ActionCollection actions, object parameter)
        {
            List<object> results = new List<object>();

            // TODO: Check for design mode
            if (actions == null /*|| Windows.ApplicationModel.DesignMode.DesignModeEnabled*/)
            {
                return results;
            }

            foreach (PerspexObject dependencyObject in actions)
            {
                IAction action = (IAction)dependencyObject;
                results.Add(action.Execute(sender, parameter));
            }

            return results;
        }

        /* TODO: 
        private static void OnBehaviorsChanged(PerspexObject sender, PerspexPropertyChangedEventArgs args)
        {
            BehaviorCollection oldCollection = (BehaviorCollection)args.OldValue;
            BehaviorCollection newCollection = (BehaviorCollection)args.NewValue;

            if (oldCollection == newCollection)
            {
                return;
            }

            if (oldCollection != null && oldCollection.AssociatedObject != null)
            {
                oldCollection.Detach();
            }

            if (newCollection != null && sender != null)
            {
                newCollection.Attach(sender);
            }
        }
        */

        private static void FrameworkElement_Loaded(object sender, VisualTreeAttachmentEventArgs e)
        {
            var d = sender as PerspexObject;

            if (d != null)
            {
                GetBehaviors(d).Attach(d);
            }
        }

        private static void FrameworkElement_Unloaded(object sender, VisualTreeAttachmentEventArgs e)
        {
            var d = sender as PerspexObject;

            if (d != null)
            {
                GetBehaviors(d).Detach();
            }
        }
    }
}