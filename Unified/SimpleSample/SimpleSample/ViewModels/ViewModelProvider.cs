// Copyright 2019 Scandit AG
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using SimpleSample.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleSample.ViewModels
{
    public static class ViewModelProvider
    {
        #region Events
        // public static event Action<string> PropertyChanged;
        #endregion

        #region Fields
        // static bool _isBusy;
        static List<BaseViewModel> _viewModels = new List<BaseViewModel>();
        #endregion

        #region Methods
        /// <summary>
        /// Searches for specified viewmodel
        /// retuns new instance of this type if nothing found
        /// </summary>
        /// <typeparam name="T">ViewModel type</typeparam>
        /// <returns></returns>
        public static T GetViewModel<T>() where T : BaseViewModel
        {
            T vm = (T)_viewModels.FirstOrDefault(f => f is T);
            if (vm == null)
            {
                vm = (T)Activator.CreateInstance(typeof(T));
                _viewModels.Add(vm);
            }
            return vm;
        }


        /// <summary>
        /// Force to create new instanse of  ViewModel
        /// </summary>
        /// <typeparam name="T">ViewModel type</typeparam>
        /// <param name="removeOld">if true (default) removes old instace of this type</param>
        /// <returns></returns>
        public static T GetNewViewModel<T>(bool removeOld = true) where T : BaseViewModel
        {
            T vm;
            if (removeOld)
            {
                vm = (T)_viewModels.FirstOrDefault(f => f is T);

                if (vm != null)
                {
                    _viewModels.Remove(vm);
                    vm = null;
                }
            }
            vm = (T)Activator.CreateInstance(typeof(T));
            _viewModels.Add(vm);
            return vm;
        }

        /// <summary>
        /// Checks if at least one instance of this viewmodel type presents
        /// </summary>
        /// <typeparam name="T">ViewModelType</typeparam>
        /// <returns></returns>
        public static bool HasViewModel<T>() where T : BaseViewModel
        {
            T vm = (T)_viewModels.FirstOrDefault(f => f is T);
            return (vm != null);
        }

        public static void AddViewModel<T>(T viewModel) where T : BaseViewModel
        {
            _viewModels.Add(viewModel);
        }

        static public void RemoveViewModel<T>() where T : BaseViewModel
        {
            T vm = (T)_viewModels.FirstOrDefault(f => f is T);
            if (vm != null)
            {
                _viewModels.Remove(vm);
                vm = null;
            }
        }

        static public void Clear()
        {
            for (int i = 0; i < _viewModels.Count; i++)
                _viewModels[i] = null;
            _viewModels.Clear();
        }
        #endregion

        #region Properties
        #endregion
    }
}
