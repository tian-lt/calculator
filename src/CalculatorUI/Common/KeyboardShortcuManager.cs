// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;

using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.System;
using CalculatorApp;
using CalculatorApp.Common;
using CalculatorApp.ViewModel;
using MUXC = Microsoft.UI.Xaml.Controls;
using System.Globalization;
using System.Collections;

namespace CalculatorApp
{
    namespace Common
    {

        static partial class KeyboardShortcutManagerLocals
        {
            // Lights up all of the buttons in the given range
            // The range is defined by a pair of iterators
            static public void LightUpButtons(IEnumerable<WeakReference> buttons)
            {
                foreach(var button in buttons)
                {
                    var btn = button.Target as ButtonBase;
                    if(btn != null && btn.IsEnabled)
                    {
                        LightUpButton(btn);
                    }
                }
            }

            static public void LightUpButton(ButtonBase button)
            {
                // If the button is a toggle button then we don't need
                // to change the UI of the button
                if (button is ToggleButton)
                {
                    return;
                }

                // The button will go into the visual Pressed state with this call
                VisualStateManager.GoToState(button, "Pressed", true);

                // This timer will fire after lightUpTime and make the button
                // go back to the normal state.
                // This timer will only fire once after which it will be destroyed
                var timer = new DispatcherTimer();
                TimeSpan lightUpTime = TimeSpan.FromMilliseconds(500); // half second
                timer.Interval = lightUpTime;

                var timerWeakReference = new WeakReference(timer);
                var buttonWeakReference = new WeakReference(button);
                timer.Tick += (sender, args) =>
                {
                    var btn = buttonWeakReference.Target as ButtonBase;
                    if(btn != null)
                    {
                        VisualStateManager.GoToState(button, "Normal", true);
                    }

                    var tmr = timerWeakReference.Target as DispatcherTimer;
                    if(tmr != null)
                    {
                        tmr.Stop();
                    }
                };
                timer.Start();
            }

            // Looks for the first button reference that it can resolve
            // and execute its command.
            // NOTE: It is assumed that all buttons associated with a particular
            // key have the same command
            static public void RunFirstEnabledButtonCommand(IEnumerable<WeakReference> buttons)
            {
                foreach(var button in buttons)
                {
                    var btn = button.Target as ButtonBase;
                    if (btn != null && btn.IsEnabled)
                    {
                        RunButtonCommand(btn);
                        break;
                    }
                }
            }


            static public void RunButtonCommand(ButtonBase button)
            {
                if (button.IsEnabled)
                {
                    var command = button.Command;
                    var parameter = button.CommandParameter;
                    if (command != null && command.CanExecute(parameter))
                    {
                        command.Execute(parameter);
                    }

                    var radio = (button as RadioButton);
                    if (radio != null)
                    {
                        radio.IsChecked = true;
                        return;
                    }

                    var toggle = (button as ToggleButton);
                    if (toggle != null)
                    {
                        toggle.IsChecked = !(toggle.IsChecked != null && toggle.IsChecked.Value);
                        return;
                    }
                }
            }
        }

        public sealed class KeyboardShortcutManager : DependencyObject
        {

            public KeyboardShortcutManager()
            {

            }


            public string Character
            {
                get { return (string)GetValue(CharacterProperty); }
                set { SetValue(CharacterProperty, value); }
            }

            // Using a DependencyProperty as the backing store for string.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty CharacterProperty =
            DependencyProperty.RegisterAttached("Character", typeof(string), typeof(KeyboardShortcutManager), new PropertyMetadata(default(string), new PropertyChangedCallback((sender, args)=>
            {
                OnCharacterPropertyChanged(sender, args.OldValue as string, args.NewValue as string);
            })));


            public MyVirtualKey VirtualKey
            {
                get { return (MyVirtualKey)GetValue(VirtualKeyProperty); }
                set { SetValue(VirtualKeyProperty, value); }
            }

            // Using a DependencyProperty as the backing store for VirtualKey.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty VirtualKeyProperty =
            DependencyProperty.RegisterAttached("VirtualKey", typeof(MyVirtualKey), typeof(KeyboardShortcutManager), new PropertyMetadata(default(MyVirtualKey), new PropertyChangedCallback((sender, args)=>
            {
                OnVirtualKeyPropertyChanged(sender, (MyVirtualKey)args.OldValue, (MyVirtualKey)args.NewValue);
            })));


            public MyVirtualKey VirtualKeyControlChord
            {
                get { return (MyVirtualKey)GetValue(VirtualKeyControlChordProperty); }
                set { SetValue(VirtualKeyControlChordProperty, value); }
            }

            // Using a DependencyProperty as the backing store for VirtualKeyControlChord.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty VirtualKeyControlChordProperty =
            DependencyProperty.RegisterAttached("VirtualKeyControlChord", typeof(MyVirtualKey), typeof(KeyboardShortcutManager), new PropertyMetadata(default(MyVirtualKey), new PropertyChangedCallback((sender, args) =>
            {
                OnVirtualKeyControlChordPropertyChanged(sender, (MyVirtualKey)args.OldValue, (MyVirtualKey)args.NewValue);
            })));


            public MyVirtualKey VirtualKeyShiftChord
            {
                get { return (MyVirtualKey)GetValue(VirtualKeyShiftChordProperty); }
                set { SetValue(VirtualKeyShiftChordProperty, value); }
            }

            // Using a DependencyProperty as the backing store for VirtualKeyControlChord.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty VirtualKeyShiftChordProperty =
            DependencyProperty.RegisterAttached("VirtualKeyControlChord", typeof(MyVirtualKey), typeof(KeyboardShortcutManager), new PropertyMetadata(default(MyVirtualKey), new PropertyChangedCallback((sender, args)=>
            {
                OnVirtualKeyShiftChordPropertyChanged(sender, (MyVirtualKey)args.OldValue, (MyVirtualKey)args.NewValue);
            })));


            public MyVirtualKey VirtualKeyAltChord
            {
                get { return (MyVirtualKey)GetValue(VirtualKeyAltChordProperty); }
                set { SetValue(VirtualKeyAltChordProperty, value); }
            }

            // Using a DependencyProperty as the backing store for VirtualKeyAltChord.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty VirtualKeyAltChordProperty =
            DependencyProperty.RegisterAttached("VirtualKeyAltChord", typeof(MyVirtualKey), typeof(KeyboardShortcutManager), new PropertyMetadata(default(MyVirtualKey), new PropertyChangedCallback((sender, args)=>
            {
                OnVirtualKeyAltChordPropertyChanged(sender, (MyVirtualKey)args.OldValue, (MyVirtualKey)args.NewValue);
            })));


            public MyVirtualKey VirtualKeyControlShiftChord
            {
                get { return (MyVirtualKey)GetValue(VirtualKeyControlShiftChordProperty); }
                set { SetValue(VirtualKeyControlShiftChordProperty, value); }
            }

            // Using a DependencyProperty as the backing store for VirtualKeyControlShiftChord.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty VirtualKeyControlShiftChordProperty =
            DependencyProperty.RegisterAttached("VirtualKeyControlShiftChord", typeof(MyVirtualKey), typeof(KeyboardShortcutManager), new PropertyMetadata(default(MyVirtualKey), new PropertyChangedCallback((sender, args)=>
            {
                OnVirtualKeyControlShiftChordPropertyChanged(sender, (MyVirtualKey)args.OldValue, (MyVirtualKey)args.NewValue);
            })));


            internal static void Initialize()
            {
                var coreWindow = Window.Current.CoreWindow;
                coreWindow.CharacterReceived += OnCharacterReceivedHandler;
                coreWindow.KeyDown += OnKeyDownHandler;
                coreWindow.Dispatcher.AcceleratorKeyActivated += OnAcceleratorKeyActivated;
                KeyboardShortcutManager.RegisterNewAppViewId();
            }

            // Sometimes, like with popups, escape is treated as special and even
            // though it is handled we get it passed through to us. In those cases
            // we need to be able to ignore it (looking at e->Handled isn't sufficient
            // because that always returns true).
            // The onlyOnce flag is used to indicate whether we should only ignore the
            // next escape, or keep ignoring until you explicitly HonorEscape.
            public static void IgnoreEscape(bool onlyOnce)
            {
                // Writer lock for the static maps
                lock (s_keyboardShortcutMapLockMutex)
                {
                    int viewId = ViewModelUtilities.GetWindowId();

                    if (s_ignoreNextEscape.ContainsKey(viewId))
                    {
                        s_ignoreNextEscape[viewId] = true;
                    }

                    if (s_keepIgnoringEscape.ContainsKey(viewId))
                    {
                        s_keepIgnoringEscape[viewId] = !onlyOnce;
                    }
                }

            }

            public static void HonorEscape()
            {
                // Writer lock for the static maps
                lock (s_keyboardShortcutMapLockMutex)
                {
                    int viewId = ViewModelUtilities.GetWindowId();

                    if (s_ignoreNextEscape.ContainsKey(viewId))
                    {
                        s_ignoreNextEscape[viewId] = false;
                    }

                    if (s_keepIgnoringEscape.ContainsKey(viewId))
                    {
                        s_keepIgnoringEscape[viewId] = false;
                    }
                }
            }

            public static void HonorShortcuts(bool allow)
            {
                // Writer lock for the static maps
                lock (s_keyboardShortcutMapLockMutex)
                {
                    int viewId = ViewModelUtilities.GetWindowId();

                    if (s_fHonorShortcuts.ContainsKey(viewId))
                    {
                        if (s_fDisableShortcuts.ContainsKey(viewId))
                        {
                            if (s_fDisableShortcuts[viewId])
                            {
                                s_fHonorShortcuts[viewId] = false;
                                return;
                            }
                        }

                        s_fHonorShortcuts[viewId] = allow;
                    }
                }
            }

            public static void DisableShortcuts(bool disable)
            {
                int viewId = ViewModelUtilities.GetWindowId();

                if (s_fDisableShortcuts.ContainsKey(viewId))
                {
                    s_fDisableShortcuts[viewId] = disable;
                }

                HonorShortcuts(!disable);
            }

            public static void UpdateDropDownState(bool isOpen)
            {
                int viewId = ViewModelUtilities.GetWindowId();

                if (s_IsDropDownOpen.ContainsKey(viewId))
                {
                    s_IsDropDownOpen[viewId] = isOpen;
                }
            }

            public static void RegisterNewAppViewId()
            {
                // Writer lock for the static maps
                lock (s_keyboardShortcutMapLockMutex)
                {
                    int appViewId = ViewModelUtilities.GetWindowId();

                    // Check if the View Id has already been registered
                    if (!s_characterForButtons.ContainsKey(appViewId))
                    {
                        s_characterForButtons.Add(appViewId, new SortedDictionary<char, List<WeakReference>>());
                    }

                    if (!s_virtualKey.ContainsKey(appViewId))
                    {
                        s_virtualKey.Add(appViewId, new SortedDictionary<char, List<WeakReference>>());
                    }

                    if (!s_VirtualKeyControlChordsForButtons.ContainsKey(appViewId))
                    {
                        s_VirtualKeyControlChordsForButtons.Add(appViewId, new SortedDictionary<char, List<WeakReference>>());
                    }

                    if (!s_VirtualKeyShiftChordsForButtons.ContainsKey(appViewId))
                    {
                        s_VirtualKeyShiftChordsForButtons.Add(appViewId, new SortedDictionary<char, List<WeakReference>>());
                    }

                    if (!s_VirtualKeyAltChordsForButtons.ContainsKey(appViewId))
                    {
                        s_VirtualKeyAltChordsForButtons.Add(appViewId, new SortedDictionary<char, List<WeakReference>>());
                    }

                    if (!s_VirtualKeyControlShiftChordsForButtons.ContainsKey(appViewId))
                    {
                        s_VirtualKeyControlShiftChordsForButtons.Add(appViewId, new SortedDictionary<char, List<WeakReference>>());
                    }

                    s_IsDropDownOpen[appViewId] = false;
                    s_ignoreNextEscape[appViewId] = false;
                    s_keepIgnoringEscape[appViewId] = false;
                    s_fHonorShortcuts[appViewId] = true;
                    s_fDisableShortcuts[appViewId] = false;
                }
            }

            public static void OnWindowClosed(int viewId)
            {
                // Writer lock for the static maps
                lock (s_keyboardShortcutMapLockMutex)
                {
                    s_characterForButtons.Remove(viewId);

                    s_virtualKey.Remove(viewId);
                    s_VirtualKeyControlChordsForButtons.Remove(viewId);
                    s_VirtualKeyShiftChordsForButtons.Remove(viewId);
                    s_VirtualKeyAltChordsForButtons.Remove(viewId);
                    s_VirtualKeyControlShiftChordsForButtons.Remove(viewId);

                    s_IsDropDownOpen.Remove(viewId);
                    s_ignoreNextEscape.Remove(viewId);
                    s_keepIgnoringEscape.Remove(viewId);
                    s_fHonorShortcuts.Remove(viewId);
                    s_fDisableShortcuts.Remove(viewId);
                }
            }

            private static void OnCharacterPropertyChanged(DependencyObject target, String oldValue, String newValue)
            {

                // Writer lock for the static maps
                lock (s_keyboardShortcutMapLockMutex)
                {

                    var button = (target as ButtonBase);

                    int viewId = ViewModelUtilities.GetWindowId();
                    if (s_characterForButtons.TryGetValue(viewId, out var iterViewMap))
                    {
                        if (!string.IsNullOrEmpty(oldValue))
                        {
                            iterViewMap.Remove(oldValue[0]);
                        }

                        if (!string.IsNullOrEmpty(newValue))
                        {
                            if (newValue == ".")
                            {
                                char decSep = LocalizationSettings.GetInstance().GetDecimalSeparator();
                                iterViewMap.Add(decSep, new List<WeakReference>() { new WeakReference(button) });
                            }
                            else
                            {
                                iterViewMap.Add(newValue[0], new List<WeakReference>() { new WeakReference(button) });
                            }
                        }
                    }
                    else
                    {
                        s_characterForButtons.Add(viewId, new SortedDictionary<char, List<WeakReference>>());

                        if (newValue == ".")
                        {
                            char decSep = LocalizationSettings.GetInstance().GetDecimalSeparator();
                            s_characterForButtons[viewId].Add(decSep, new List<WeakReference>() { new WeakReference(button) });
                        }
                        else
                        {
                            s_characterForButtons[viewId].Add(newValue[0], new List<WeakReference>() { new WeakReference(button) });
                        }
                    }
                }
            }

            private static void OnVirtualKeyPropertyChanged(DependencyObject target, MyVirtualKey oldValue, MyVirtualKey newValue)
            {

                // Writer lock for the static maps
                lock (s_keyboardShortcutMapLockMutex)
                {

                    var button = ((ButtonBase)target);

                    int viewId = ViewModelUtilities.GetWindowId();

                    // Check if the View Id has already been registered
                    if (s_virtualKey.TryGetValue(viewId, out var iterViewMap))
                    {
                        iterViewMap.Add((char)newValue, new List<WeakReference>() {new WeakReference(button) });
                    }
                    else
                    {
                        // If the View Id is not already registered, then register it and make the entry
                        s_virtualKey.Add(viewId, new SortedDictionary<char, List<WeakReference>>());
                        s_virtualKey[viewId].Add((char)newValue, new List<WeakReference>() { new WeakReference(button) });
                    }
                }
            }

            private static void OnVirtualKeyControlChordPropertyChanged(DependencyObject target, MyVirtualKey oldValue, MyVirtualKey newValue)
            {

                // Writer lock for the static maps
                lock (s_keyboardShortcutMapLockMutex)
                {
                    Control control = (target as ButtonBase);

                    if (control == null)
                    {
                        // Handling Ctrl+E shortcut for Date Calc, target would be NavigationView^ in that case
                        control = (target as MUXC.NavigationView);
                    }

                    int viewId = ViewModelUtilities.GetWindowId();

                    // Check if the View Id has already been registered
                    if (s_VirtualKeyControlChordsForButtons.TryGetValue(viewId, out var iterViewMap))
                    {
                        iterViewMap.Add((char)newValue, new List<WeakReference>() { new WeakReference(control) });
                    }
                    else
                    {
                        // If the View Id is not already registered, then register it and make the entry
                        s_VirtualKeyControlChordsForButtons.Add(viewId, new SortedDictionary<char, List<WeakReference>>());
                        s_VirtualKeyControlChordsForButtons[viewId].Add((char)newValue, new List<WeakReference>() { new WeakReference(control) });
                    }
                }
            }

            private static void OnVirtualKeyShiftChordPropertyChanged(DependencyObject target, MyVirtualKey oldValue, MyVirtualKey newValue)
            {
                // Writer lock for the static maps
                lock (s_keyboardShortcutMapLockMutex)
                {
                    var button = (target as ButtonBase);

                    int viewId = ViewModelUtilities.GetWindowId();

                    // Check if the View Id has already been registered
                    if (s_VirtualKeyShiftChordsForButtons.TryGetValue(viewId, out var iterViewMap))
                    {
                        iterViewMap.Add((char)newValue, new List<WeakReference>() { new WeakReference(button) });
                    }
                    else
                    {
                        // If the View Id is not already registered, then register it and make the entry
                        s_VirtualKeyShiftChordsForButtons.Add(viewId, new SortedDictionary<char, List<WeakReference>>());
                        s_VirtualKeyShiftChordsForButtons[viewId].Add((char)newValue, new List<WeakReference>() { new WeakReference(button) });
                    }
                }
            }

            private static void OnVirtualKeyAltChordPropertyChanged(DependencyObject target, MyVirtualKey oldValue, MyVirtualKey newValue)
            {

                // Writer lock for the static maps
                lock (s_keyboardShortcutMapLockMutex)
                {
                    MUXC.NavigationView navView = (target as MUXC.NavigationView);

                    int viewId = ViewModelUtilities.GetWindowId();

                    // Check if the View Id has already been registered
                    if (s_VirtualKeyAltChordsForButtons.TryGetValue(viewId, out var iterViewMap))
                    {
                        iterViewMap.Add((char)newValue, new List<WeakReference>() { new WeakReference(navView) });
                    }
                    else
                    {
                        // If the View Id is not already registered, then register it and make the entry
                        s_VirtualKeyAltChordsForButtons.Add(viewId, new SortedDictionary<char, List<WeakReference>>());
                        s_VirtualKeyAltChordsForButtons[viewId].Add((char)newValue, new List<WeakReference>() { new WeakReference(navView) });
                    }
                }
            }

            private static void OnVirtualKeyControlShiftChordPropertyChanged(DependencyObject target, MyVirtualKey oldValue, MyVirtualKey newValue)
            {
                // Writer lock for the static maps
                lock (s_keyboardShortcutMapLockMutex)
                {
                    var button = (target as ButtonBase);

                    int viewId = ViewModelUtilities.GetWindowId();

                    // Check if the View Id has already been registered
                    if (s_VirtualKeyControlShiftChordsForButtons.TryGetValue(viewId, out var iterViewMap))
                    {
                        iterViewMap.Add((char)newValue, new List<WeakReference>() { new WeakReference(button) });
                    }
                    else
                    {
                        // If the View Id is not already registered, then register it and make the entry
                        s_VirtualKeyControlShiftChordsForButtons.Add(viewId, new SortedDictionary<char, List<WeakReference>>());
                        s_VirtualKeyControlShiftChordsForButtons[viewId].Add((char)newValue, new List<WeakReference>() { new WeakReference(button) });
                    }
                }
            }

            // In the three event handlers below we will not mark the event as handled
            // because this is a supplemental operation and we don't want to interfere with
            // the normal keyboard handling.
            private static void OnCharacterReceivedHandler(CoreWindow sender, CharacterReceivedEventArgs args)
            {
                int viewId = ViewModelUtilities.GetWindowId();
                bool hit = s_fHonorShortcuts.TryGetValue(viewId, out var currentHonorShortcuts);

                if (!hit || currentHonorShortcuts)
                {
                    char character = ((char)args.KeyCode);
                    var buttons = s_characterForButtons[viewId][character];
                    KeyboardShortcutManagerLocals.RunFirstEnabledButtonCommand(buttons);

                    KeyboardShortcutManagerLocals.LightUpButtons(buttons);
                }
            }

            private static void OnKeyDownHandler(CoreWindow sender, KeyEventArgs args)
            {
                if (args.Handled)
                {
                    return;
                }

                var key = args.VirtualKey;
                int viewId = ViewModelUtilities.GetWindowId();

                bool isControlKeyPressed = (Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.Control) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
                bool isShiftKeyPressed = (Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.Shift) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
                bool isAltKeyPressed = (Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.Menu) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;

                // Handle Ctrl + E for DateCalculator
                if ((key == Windows.System.VirtualKey.E) && isControlKeyPressed && !isShiftKeyPressed && !isAltKeyPressed)
                {
                    var lookupMap = GetCurrentKeyDictionary(isControlKeyPressed, isShiftKeyPressed, false);
                    if (lookupMap == null)
                    {
                        return;
                    }

                    var buttons = lookupMap[(char)key];
                    var navView = buttons[0].Target as MUXC.NavigationView; // CSHARP_MIGRATION: TODO: double check if button[0] exists
                    var appViewModel = (navView.DataContext as ApplicationViewModel);
                    appViewModel.Mode = ViewMode.Date;
                    var categoryName = AppResourceProvider.GetInstance().GetResourceString("DateCalculationModeText");
                    appViewModel.CategoryName = categoryName;

                    var menuItems = ((IObservableVector<object>)navView.MenuItemsSource);
                    var flatIndex = NavCategory.GetFlatIndex(ViewMode.Date);
                    navView.SelectedItem = menuItems[flatIndex];
                    return;
                }

                if (s_ignoreNextEscape.TryGetValue(viewId, out var currentIgnoreNextEscape))
                {
                    if (currentIgnoreNextEscape && key == Windows.System.VirtualKey.Escape)
                    {
                        if (s_keepIgnoringEscape.TryGetValue(viewId, out var currentKeepIgnoringEscape))
                        {
                            if (!currentKeepIgnoringEscape)
                            {
                                HonorEscape();
                            }
                            return;
                        }
                    }
                }

                if (s_fHonorShortcuts.TryGetValue(viewId, out var currentHonorShortcuts))
                {
                    if (currentHonorShortcuts)
                    {
                        var myVirtualKey = (char)key;
                        var lookupMap = GetCurrentKeyDictionary(isControlKeyPressed, isShiftKeyPressed, isAltKeyPressed);
                        if (lookupMap == null)
                        {
                            return;
                        }

                        var buttons = lookupMap[myVirtualKey];
                        if(buttons.Count <= 0) // CSHARP_MIGRATION: TODO: double check if this is equivalent to `if (buttons.first == buttons.second)`
                        {
                            return;
                        }

                        KeyboardShortcutManagerLocals.RunFirstEnabledButtonCommand(buttons);

                        // Ctrl+C and Ctrl+V shifts focus to some button because of which enter doesn't work after copy/paste. So don't shift focus if Ctrl+C or Ctrl+V
                        // is pressed. When drop down is open, pressing escape shifts focus to clear button. So dont's shift focus if drop down is open. Ctrl+Insert is
                        // equivalent to Ctrl+C and Shift+Insert is equivalent to Ctrl+V
                        //var currentIsDropDownOpen = s_IsDropDownOpen.find(viewId);
                        if(!s_IsDropDownOpen.TryGetValue(viewId, out var currentIsDropDownOpen) || !currentIsDropDownOpen)
                        {
                            // Do not Light Up Buttons when Ctrl+C, Ctrl+V, Ctrl+Insert or Shift+Insert is pressed
                            if (!(isControlKeyPressed && (key == Windows.System.VirtualKey.C || key == Windows.System.VirtualKey.V || key == Windows.System.VirtualKey.Insert))
                                & !(isShiftKeyPressed && (key == Windows.System.VirtualKey.Insert)))
                            {
                                KeyboardShortcutManagerLocals.LightUpButtons(buttons);
                            }
                        }
                    }
                }

            }

            private static void OnAcceleratorKeyActivated(CoreDispatcher dispatcher, AcceleratorKeyEventArgs args)
            {
                if (args.KeyStatus.IsKeyReleased)
                {
                    var key = args.VirtualKey;
                    bool altPressed = args.KeyStatus.IsMenuKeyDown;

                    // If the Alt/Menu key is not pressed then we don't care about the key anymore
                    if (!altPressed)
                    {
                        return;
                    }

                    bool controlKeyPressed = (Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.Control) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
                    // Ctrl is pressed in addition to alt, this means Alt Gr is intended.  do not navigate.
                    if (controlKeyPressed)
                    {
                        return;
                    }

                    bool shiftKeyPressed = (Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.Shift) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
                    var lookupMap = GetCurrentKeyDictionary(controlKeyPressed, shiftKeyPressed, altPressed);
                    if (lookupMap != null)
                    {
                        var listItems = lookupMap[(char)key];
                        foreach(var itemRef in listItems)
                        {
                            var item = itemRef.Target as MUXC.NavigationView;
                            if(item != null)
                            {
                                var navView = item as MUXC.NavigationView; // CSHARP_MIGRATION: TODO: check if this line is still needed

                                var menuItems = ((IObservableVector<Object>)navView.MenuItemsSource);
                                if (menuItems != null)
                                {
                                    var vm = (navView.DataContext as ApplicationViewModel);
                                    if (null != vm)
                                    {
                                        ViewMode toMode = NavCategory.GetViewModeForVirtualKey(((MyVirtualKey)key));
                                        var nvi = (menuItems[NavCategory.GetFlatIndex(toMode)] as MUXC.NavigationViewItem);
                                        if (nvi != null && nvi.IsEnabled && NavCategory.IsValidViewMode(toMode))
                                        {
                                            vm.Mode = toMode;
                                            navView.SelectedItem = nvi;
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                }

            }

            // CSHARP_MIGRATION: TODO: reinterpreted SortedDictionary<MyVirtualKey, List<WeakReference>> to SortedDictionary<char, List<WeakReference>>
            // double check this is equivalent before and after migration
            private static SortedDictionary<char, List<WeakReference>> GetCurrentKeyDictionary(bool controlKeyPressed, bool shiftKeyPressed, bool altPressed)
            {
                int viewId = ViewModelUtilities.GetWindowId();

                if (controlKeyPressed)
                {
                    if (altPressed)
                    {
                        return null;
                    }
                    else
                    {
                        if (shiftKeyPressed)
                        {
                            return s_VirtualKeyControlShiftChordsForButtons[viewId];
                        }
                        else
                        {
                            return s_VirtualKeyControlChordsForButtons[viewId];
                        }
                    }
                }
                else
                {
                    if (altPressed)
                    {
                        if (!shiftKeyPressed)
                        {
                            return s_VirtualKeyAltChordsForButtons[viewId];
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        if (shiftKeyPressed)
                        {
                            return s_VirtualKeyShiftChordsForButtons[viewId];
                        }
                        else
                        {
                            return s_virtualKey[viewId];
                        }
                    }
                }
            }

            private static SortedDictionary<int, SortedDictionary<char, List<WeakReference>>> s_characterForButtons = new SortedDictionary<int, SortedDictionary<char, List<WeakReference>>>();
            private static SortedDictionary<int, SortedDictionary<char, List<WeakReference>>> s_virtualKey = new SortedDictionary<int, SortedDictionary<char, List<WeakReference>>>();
            private static SortedDictionary<int, SortedDictionary<char, List<WeakReference>>> s_VirtualKeyControlChordsForButtons = new SortedDictionary<int, SortedDictionary<char, List<WeakReference>>>();
            private static SortedDictionary<int, SortedDictionary<char, List<WeakReference>>> s_VirtualKeyShiftChordsForButtons = new SortedDictionary<int, SortedDictionary<char, List<WeakReference>>>();
            private static SortedDictionary<int, SortedDictionary<char, List<WeakReference>>> s_VirtualKeyAltChordsForButtons = new SortedDictionary<int, SortedDictionary<char, List<WeakReference>>>();
            private static SortedDictionary<int, SortedDictionary<char, List<WeakReference>>> s_VirtualKeyControlShiftChordsForButtons = new SortedDictionary<int, SortedDictionary<char, List<WeakReference>>>();


            private static SortedDictionary<int, bool> s_IsDropDownOpen = new SortedDictionary<int, bool>();
            private static SortedDictionary<int, bool> s_ignoreNextEscape = new SortedDictionary<int, bool>();
            private static SortedDictionary<int, bool> s_keepIgnoringEscape = new SortedDictionary<int, bool>();
            private static SortedDictionary<int, bool> s_fHonorShortcuts = new SortedDictionary<int, bool>();
            private static SortedDictionary<int, bool> s_fDisableShortcuts = new SortedDictionary<int, bool>();

            //private static Concurrency.reader_writer_lock s_keyboardShortcutMapLock;
            private static readonly object s_keyboardShortcutMapLockMutex = new object();
        }
    }
}

#if False //Code from .cpp not merged
//--------------------------------------------------------------------------------

// Info: Parsed in .cpp but declaration not found in .h
void LightUpButtons(T buttons)
{

var iterator = buttons.first;
for (; iterator != buttons.second; ++iterator)
{
var button = iterator.second.Resolve<ButtonBase>();
if (button && button.IsEnabled)
{
LightUpButton(button);
}
}

}

//--------------------------------------------------------------------------------
// Info: Parsed in .cpp but declaration not found in .h
void LightUpButton(ButtonBase button)
{

// If the button is a toggle button then we don't need
// to change the UI of the button
if ((button as ToggleButton ))
{
return;
}

// The button will go into the visual Pressed state with this call
VisualStateManager.GoToState(button, "Pressed", true);

// This timer will fire after lightUpTime and make the button
// go back to the normal state.
// This timer will only fire once after which it will be destroyed
var timer = new DispatcherTimer();
TimeSpan lightUpTime = new TimeSpan();
lightUpTime.Duration = 500000L; // Half second (in 100-ns units)
timer.Interval = lightUpTime;

WeakReference timerWeakReference(timer);
WeakReference buttonWeakReference(button);
timer.Tick += new EventHandler<Object >((Object , Object ) => {
var button = buttonWeakReference.Resolve<ButtonBase>();
if (button)
{
VisualStateManager.GoToState(button, "Normal", true);
}

// Cancel the timer after we're done so it only fires once
var timer = timerWeakReference.Resolve<DispatcherTimer>();
if (timer)
{
timer.Stop();
}
});
timer.Start();

}

//--------------------------------------------------------------------------------
// Info: Parsed in .cpp but declaration not found in .h
void RunFirstEnabledButtonCommand(T buttons)
{

var buttonIterator = buttons.first;
for (; buttonIterator != buttons.second; ++buttonIterator)
{
var button = buttonIterator.second.Resolve<ButtonBase>();
if (button && button.IsEnabled)
{
RunButtonCommand(button);
break;
}
}

}

//--------------------------------------------------------------------------------
// Info: Parsed in .cpp but declaration not found in .h
void RunButtonCommand(ButtonBase button)
{

if (button.IsEnabled)
{
var command = button.Command;
var parameter = button.CommandParameter;
if (command && command.CanExecute(parameter))
{
command.Execute(parameter);
}

var radio = (button as RadioButton );
if (radio)
{
radio.IsChecked = true;
return;
}

var toggle = (button as ToggleButton );
if (toggle)
{
toggle.IsChecked = !(toggle.IsChecked != null && toggle.IsChecked.Value);
return;
}
}

}

//--------------------------------------------------------------------------------
// Info: Code not recognized in .cpp

namespace MUXC = Microsoft.UI.Xaml.Controls;

DEPENDENCY_PROPERTY_INITIALIZATION(KeyboardShortcutManager, Character);
DEPENDENCY_PROPERTY_INITIALIZATION(KeyboardShortcutManager, VirtualKey);
DEPENDENCY_PROPERTY_INITIALIZATION(KeyboardShortcutManager, VirtualKeyControlChord);
DEPENDENCY_PROPERTY_INITIALIZATION(KeyboardShortcutManager, VirtualKeyShiftChord);
DEPENDENCY_PROPERTY_INITIALIZATION(KeyboardShortcutManager, VirtualKeyAltChord);
DEPENDENCY_PROPERTY_INITIALIZATION(KeyboardShortcutManager, VirtualKeyControlShiftChord);

SortedDictionary<int, multimap<char, WeakReference>> KeyboardShortcutManager.s_characterForButtons;
SortedDictionary<int, multimap<MyVirtualKey, WeakReference>> KeyboardShortcutManager.s_virtualKey;
SortedDictionary<int, multimap<MyVirtualKey, WeakReference>> KeyboardShortcutManager.s_VirtualKeyControlChordsForButtons;
SortedDictionary<int, multimap<MyVirtualKey, WeakReference>> KeyboardShortcutManager.s_VirtualKeyShiftChordsForButtons;
SortedDictionary<int, multimap<MyVirtualKey, WeakReference>> KeyboardShortcutManager.s_VirtualKeyAltChordsForButtons;
SortedDictionary<int, multimap<MyVirtualKey, WeakReference>> KeyboardShortcutManager.s_VirtualKeyControlShiftChordsForButtons;

SortedDictionary<int, bool> KeyboardShortcutManager.s_IsDropDownOpen;
SortedDictionary<int, bool> KeyboardShortcutManager.s_ignoreNextEscape;
SortedDictionary<int, bool> KeyboardShortcutManager.s_keepIgnoringEscape;
SortedDictionary<int, bool> KeyboardShortcutManager.s_fHonorShortcuts;
SortedDictionary<int, bool> KeyboardShortcutManager.s_fDisableShortcuts;
reader_writer_lock KeyboardShortcutManager.s_keyboardShortcutMapLock;

namespace CalculatorApp
{
namespace Common
{

template <typename T>

//--------------------------------------------------------------------------------
// Info: Code not recognized in .cpp

template <typename T>

//--------------------------------------------------------------------------------
// Info: Code not recognized in .cpp

const multimap<MyVirtualKey, WeakReference> KeyboardShortcutManager.GetCurrentKeyDictionary(bool controlKeyPressed, bool shiftKeyPressed, bool altPressed)
{
int viewId = Utils.GetWindowId();

if (controlKeyPressed)
{
if (altPressed)
{
return null;
}
else
{
if (shiftKeyPressed)
{
return s_VirtualKeyControlShiftChordsForButtons.find(viewId).second;
}
else
{
return s_VirtualKeyControlChordsForButtons.find(viewId).second;
}
}
}
else
{
if (altPressed)
{
if (!shiftKeyPressed)
{
return s_VirtualKeyAltChordsForButtons.find(viewId).second;
}
else
{
return null;
}
}
else
{
if (shiftKeyPressed)
{
return s_VirtualKeyShiftChordsForButtons.find(viewId).second;
}
else
{
return s_virtualKey.find(viewId).second;
}
}
}
}

//--------------------------------------------------------------------------------
#endif